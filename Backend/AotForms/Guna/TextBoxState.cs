using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AimBotConquer.Guna {

    [Description("")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DebuggerStepThrough]
    public class TextBoxState {

        public TextBoxState() {
        }

        [Browsable(false)]
        public TextBoxBase Parent {
            [CompilerGenerated]
            get {
                return this.textBox_0;
            }
            [CompilerGenerated]
            set {
                this.textBox_0 = value;
            }
        }

        public override string ToString() {
            return string.Empty;
        }

        private void method_0() {
            if (this.Parent != null) {
                this.Parent.Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(typeof(Color), "")]
        [Description("The textbox ForeColor")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public Color ForeColor {
            get {
                return this.color_0;
            }
            set {
                this.color_0 = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("The textbox Placeholder ForeColor")]
        [DefaultValue(typeof(Color), "")]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        public Color PlaceholderForeColor {
            get {
                return this.color_1;
            }
            set {
                this.color_1 = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(typeof(Color), "")]
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("The textbox fill color")]
        public Color FillColor {
            get {
                return this.color_2;
            }
            set {
                this.color_2 = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [NotifyParentProperty(true)]
        [Browsable(true)]
        [Description("The textbox border color")]
        [DefaultValue(typeof(Color), "")]
        public Color BorderColor {
            get {
                return this.color_3;
            }
            set {
                this.color_3 = value;
            }
        }

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TextBoxBase textBox_0;

        private Color color_0;

        private Color color_1;

        private Color color_2;

        private Color color_3;
    }
}