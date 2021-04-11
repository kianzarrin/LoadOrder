namespace LoadOrderTool.Util {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Linq;
    using System.Reflection;

    public class TextAttribute : Attribute {
        public string Text { get; set; }
        public TextAttribute(string text) => Text = text;
    }

    public static class FormHelpers {
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
            return values[combo.SelectedIndex];
        }
    }
}
