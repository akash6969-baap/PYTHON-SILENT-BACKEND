using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace AimBotConquer.Guna
{

    [ToolboxItem(true)]
    [Description("A ToggleSwitch Control")]
    [DebuggerStepThrough]
    public class Guna2ToggleSwitch : ToggleSwitch
    {

        public Guna2ToggleSwitch()
        {
            base.DefaultCheckedState.Parent = this;
            base.DefaultCheckedState.BorderColor = color_19;
            base.DefaultCheckedState.FillColor = color_19;
            base.DefaultCheckedState.InnerBorderColor = Color.White;
            base.DefaultCheckedState.InnerColor = Color.White;
            base.DefaultUncheckedState.Parent = this;
            base.DefaultUncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            base.DefaultUncheckedState.FillColor = Color.FromArgb(125, 137, 149);
            base.DefaultUncheckedState.InnerBorderColor = Color.White;
            base.DefaultUncheckedState.InnerColor = Color.White;
            base.Size = new Size(35, 20);
        }

        internal static Color color_19 = Color.FromArgb(94, 148, 255);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("The properties that will be applied when the control is in a checked state")]
        [Browsable(true)]
        public ToggleSwitchState CheckedState
        {
            get
            {
                return base.DefaultCheckedState;
            }
            set
            {
                base.DefaultCheckedState = value;
            }
        }

        [Description("The properties that will be applied when the control is in an unchecked state")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        public ToggleSwitchState UncheckedState
        {
            get
            {
                return base.DefaultUncheckedState;
            }
            set
            {
                base.DefaultUncheckedState = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Description("The toggle switch's font style")]
        [Browsable(false)]
        public new Font Font
        {
            [CompilerGenerated]
            get
            {
                return this.font_0;
            }
            [CompilerGenerated]
            set
            {
                this.font_0 = value;
            }
        }

        [Browsable(false)]
        [Description("The toggle switch's text")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string Text
        {
            [CompilerGenerated]
            get
            {
                return this.string_0;
            }
            [CompilerGenerated]
            set
            {
                this.string_0 = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [Description("The toggle switch's ForeColor")]
        public new string ForeColor
        {
            [CompilerGenerated]
            get
            {
                return this.string_1;
            }
            [CompilerGenerated]
            set
            {
                this.string_1 = value;
            }
        }

        [DefaultValue(true)]
        [Browsable(true)]
        [Description("If true, the control will be animated while interacting with the mouse")]
        public bool Animated
        {
            get
            {
                return base.DefaultAnimated;
            }
            set
            {
                base.DefaultAnimated = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(DashStyle.Solid)]
        [Description("The css-like style of the border. You can customize the border to meet your design needs")]
        public DashStyle BorderStyle
        {
            get
            {
                return base.DefaultBorderStyle;
            }
            set
            {
                base.DefaultBorderStyle = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Shadow property of the control to add and customize a control's shadow")]
        [Browsable(true)]
        public ShadowDecoration ShadowDecoration
        {
            get
            {
                return base.DefaultShadowDecoration;
            }
            set
            {
                base.DefaultShadowDecoration = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [Description("If true, the background will allow a transparent color")]
        public bool UseTransparentBackground
        {
            get
            {
                return base.DefaultUseTransparentBackground;
            }
            set
            {
                base.DefaultUseTransparentBackground = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(false)]
        [Description("The properties that will be applied when the control is checked")]
        public bool Checked
        {
            get
            {
                return base.DefaultChecked;
            }
            set
            {
                base.DefaultChecked = value;
            }
        }

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Font font_0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        private string string_0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        private string string_1;
    }
}