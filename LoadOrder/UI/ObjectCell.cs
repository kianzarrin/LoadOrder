namespace LoadOrderTool.UI {
    using CO;
    using CO.Packaging;
    using CO.Plugins;
    using CO.PlatformServices;
    using LoadOrderTool.Util;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using System.Diagnostics;
    using LoadOrderTool.Data;
    using System.Threading.Tasks;
    using System.ComponentModel;
    using System.Windows.Forms.Design;

    class ObjectCell : DataGridViewCell {
        object value_;
        protected override object GetValue(int rowIndex) => value_;
        protected override bool SetValue(int rowIndex, object value) {
            value_ = value;
            return true;
        }
        public override bool Visible => false;

        public override object Clone() => new ObjectCell { value_ = value_ };
    }
}
