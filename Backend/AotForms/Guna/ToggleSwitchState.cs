
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AimBotConquer.Guna
{

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DebuggerStepThrough]
    [Description("ToggleSwitchState")]
    public class ToggleSwitchState
    {

        public ToggleSwitchState()
        {
        }

        [Browsable(false)]
        public ToggleSwitch Parent
        {
            [CompilerGenerated]
            get
            {
                return this.toggleSwitch_0;
            }
            [CompilerGenerated]
            set
            {
                this.toggleSwitch_0 = value;
            }
        }

        private void method_0()
        {
            if (this.Parent != null)
            {
                this.Parent.Invalidate();
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }

        [DefaultValue(typeof(Color), "")]
        [Description("The toggle switch fill color")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [NotifyParentProperty(true)]
        public Color FillColor
        {
            get
            {
                return this.color_0;
            }
            set
            {
                this.color_0 = value;
                this.method_0();
            }
        }

        [Browsable(true)]
        [Description("The toggle switch border color")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(Color), "")]
        public Color BorderColor
        {
            get
            {
                return this.color_1;
            }
            set
            {
                this.color_1 = value;
                this.method_0();
            }
        }

        [Description("The toggle switch inner color")]
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(Color), "")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color InnerColor
        {
            get
            {
                return this.color_2;
            }
            set
            {
                this.color_2 = value;
                this.method_0();
            }
        }

        [NotifyParentProperty(true)]
        [Browsable(true)]
        [Description("The toggle switch inner border color")]
        [DefaultValue(typeof(Color), "")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color InnerBorderColor
        {
            get
            {
                return this.color_3;
            }
            set
            {
                this.color_3 = value;
                this.method_0();
            }
        }

        [Browsable(true)]
        [Description("The toggle switch border radius")]
        [DefaultValue(9)]
        [NotifyParentProperty(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public int BorderRadius
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
                this.method_0();
            }
        }

        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Description("The toggle switch border thickness")]
        [DefaultValue(0)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public int BorderThickness
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
                this.method_0();
            }
        }

        [NotifyParentProperty(true)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("The toggle switch innder border radius")]
        [DefaultValue(5)]
        public int InnerBorderRadius
        {
            get
            {
                return this.int_2;
            }
            set
            {
                this.int_2 = value;
                this.method_0();
            }
        }

        [Description("The toggle switch innder border thickness")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(0)]
        [NotifyParentProperty(true)]
        public int InnerBorderThickness
        {
            get
            {
                return this.int_3;
            }
            set
            {
                this.int_3 = value;
                this.method_0();
            }
        }

        [Browsable(true)]
        [DefaultValue(0)]
        [NotifyParentProperty(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("The toggle switch inner offset")]
        public int InnerOffset
        {
            get
            {
                return this.int_4;
            }
            set
            {
                this.int_4 = value;
                this.method_0();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        private ToggleSwitch toggleSwitch_0;

        private Color color_0;

        private Color color_1;

        private Color color_2;

        private Color color_3;

        private int int_0 = 9;

        private int int_1;

        private int int_2 = 5;

        private int int_3;

        private int int_4;
    }
}