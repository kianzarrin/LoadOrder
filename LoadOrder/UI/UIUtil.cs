namespace LoadOrderTool.UI {
    using PresentationControls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class TextAttribute : Attribute {
        public string Text { get; set; }
        public TextAttribute(string text) => Text = text;
    }

    public static class UIUtil {
        public static bool DesignMode =>
            LicenseManager.UsageMode == LicenseUsageMode.Designtime || !Program.IsMain;
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
        public static T GetSelectedItem<T>(this ComboBox combo) where T : Enum {
            T[] values = typeof(T).GetEnumValues() as T[];
            if (combo.SelectedIndex < 0)
                throw new IndexOutOfRangeException("combo.SelectedIndex=" + combo.SelectedIndex);
            try {
                return values[combo.SelectedIndex];
            } catch (Exception ex) {
                Log.Exception(ex, $"SelectedIndex={combo?.SelectedIndex} values={string.Join(", ", values)}");
                throw;
            }
        }

        public static void SetItems<T>(this CheckBoxComboBox combo) where T : Enum {
            var items = typeof(T).GetEnumNames()
                .Select(item => typeof(T).GetMember(item)[0])
                .Select(item => item.GetMemberDisplayText());

            combo.Items.Clear();
            combo.Items.AddRange(items.ToArray());
            combo.AutoSize();
        }
        public static T[] GetSelectedItems<T>(this CheckBoxComboBox combo) where T : Enum {
            T[] values = typeof(T).GetEnumValues() as T[];
            try {
                List<T> ret = new List<T>();
                // skip first item which is empty.
                for(int i=1;i< combo.CheckBoxItems.Count;  ++i) {
                    if (combo.CheckBoxItems[i].Checked) {
                        ret.Add(values[i-1]);
                    }
                    
                }
                return ret.ToArray();
            } catch (Exception ex) {
                var items = combo.CheckBoxItems.Select(item => item.Text).ToArray();
                Log.Exception(ex,
                    $"items={string.Join(", ", items)}  " +
                    $"values={string.Join(", ", values)} " +
                    $"names={string.Join(", ", typeof(T).GetEnumNames())}");
                throw ex;
            }
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

        public enum WIN32Color {
            Normal = 1,
            Error = 2,
            Warning = 3,
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetColor(this ProgressBar p, WIN32Color color) {
            SendMessage(p.Handle, 1040, (IntPtr)(int)color, IntPtr.Zero);
        }

    }
}
