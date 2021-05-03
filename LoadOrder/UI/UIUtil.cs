namespace LoadOrderTool.UI {
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    public class TextAttribute : Attribute {
        public string Text { get; set; }
        public TextAttribute(string text) => Text = text;
    }

    public static class UIUtil {
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
    }
}
