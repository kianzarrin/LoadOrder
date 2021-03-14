namespace LoadOrderTool.Util {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    public static class FormHelpers {
        public static void SetItems<T>(this ComboBox combo) where T : Enum {
            combo.Items.Clear();
            combo.Items.AddRange(typeof(T).GetEnumNames());
        }
        public static T GetSelectedItem<T>(this ComboBox combo) where T : Enum {
            T[] values = typeof(T).GetEnumValues() as T[];
            return values[combo.SelectedIndex];
        }
    }
}
