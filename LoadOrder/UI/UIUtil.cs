namespace LoadOrderTool.UI {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    public class TextAttribute : Attribute {
        public string Text { get; set; }
        public TextAttribute(string text) => Text = text;
    }

    public static class UIUtil {
        public static bool DesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        public static void U32TextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        public static void U32TextBox_Submit(object? sender, EventArgs e) {
            if (sender is TextBox tb) {
                if (tb.Text == "")
                    tb.Text = "0";
                else
                    tb.Text = UInt32.Parse(tb.Text).ToString();
            }
        }

        public static string GetMemberDisplayText(this MemberInfo member) {
            var att = member.GetCustomAttribute<TextAttribute>();
            return att?.Text ?? member.Name;
        }

        public static void SetItems<T>(this ComboBox combo) where T : Enum {
            var items = typeof(T).GetEnumNames()
                .Select(item => typeof(T).GetMember(item)[0])
                .Select(item => item.GetMemberDisplayText());

            combo.Items.Clear();
            combo.Items.AddRange(items.ToArray());
            combo.AutoSize();
        }

        public static void AutoSize(this ComboBox combo) {
            combo.Width = combo.MeasureMaxItemWidth() + SystemInformation.VerticalScrollBarWidth;
            combo.DropDownWidth = combo.MeasureMaxItemWidth();
            if (combo.Items.Count > combo.MaxDropDownItems)
                combo.DropDownWidth += SystemInformation.VerticalScrollBarWidth;
        }

        public static int MeasureMaxItemWidth(this ComboBox combo) {
            int maxWidth = 0;
            foreach (var obj in combo.Items) {
                int w = TextRenderer.MeasureText(obj.ToString(), combo.Font).Width;
                maxWidth = Math.Max(w, maxWidth);
            }
            return maxWidth;
        }

        public static T GetSelectedItem<T>(this ComboBox combo) where T : Enum {
            T[] values = typeof(T).GetEnumValues() as T[];
            if (combo.SelectedIndex < 0)
                throw new IndexOutOfRangeException("combo.SelectedIndex=" + combo.SelectedIndex);
            try {
                return values[combo.SelectedIndex];
            } catch(Exception ex) {
                Log.Exception(ex, $"SelectedIndex={combo?.SelectedIndex} values={string.Join(", ", values)}");
                throw ex;
            }
        }

        public static IEnumerable<Type> GetAll<Type>(this Control control) where Type : Control {
            var controls = control.Controls.Cast<Control>();
            return controls
                .SelectMany(ctrl => GetAll<Type>(ctrl))
                .Concat(controls)
                .OfType<Type>();
        }

        //public static ToolTip SetTooltip(this Control control, string tooltip) {
        //    ToolTip tp = new ToolTip {
        //        IsBalloon = true,
        //        InitialDelay = 1,
        //        ReshowDelay = 1,
        //        AutomaticDelay = 3600 * 1000,
        //        UseAnimation = false,
        //        UseFading = false,
        //        ShowAlways = true,
        //    };
        //    tp.SetToolTip(control, tooltip);
        //    return tp;
        //}

        public static ToolTip SetTooltip(this Control control, string tooltip) {
            ToolTip tp = new ToolTip {
                IsBalloon = true,
                InitialDelay = 1,
                ReshowDelay = 1,
                AutomaticDelay = 3600 * 1000,
                UseAnimation = false,
                UseFading = false,
                ShowAlways = true,
            };
            tp.SetToolTip(control, tooltip);
            control.MouseEnter += (_, __) => {
                var pnt = control.PointToClient(Cursor.Position);
                pnt.X += 10;
                pnt.Y += 10;
                tp.Show(tooltip, control, pnt);
            };
            control.MouseLeave += (_, __) => tp.Hide(control);
            return tp;
        }

        public static void AutoResizeFirstColumn(this DataGridView dgv) {
            dgv.AutoResizeColumn(columnIndex: 0);
            var col0 = dgv.Columns[0];
            col0.Width = Math.Max(col0.Width, col0.HeaderCell.Size.Width + SystemInformation.Border3DSize.Width);

        }
    }
}
