using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AimBotConquer.Guna
{

    [Description("A textbox control")]
    [DebuggerStepThrough]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Windows.Forms.TextBox))]
    public class Guna2TextBox : TextBoxBase
    {

        public Guna2TextBox()
        {
            base.DefaultDisabledState.Parent = this;
            base.DefaultDisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            base.DefaultDisabledState.FillColor = Color.FromArgb(226, 226, 226);
            base.DefaultDisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            base.DefaultDisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            base.DefaultHoveredState.Parent = this;
            base.DefaultHoveredState.BorderColor = color_19;
            base.DefaultFocusedState.Parent = this;
            base.DefaultFocusedState.BorderColor = color_19;
        }

        internal static Color color_19 = Color.FromArgb(94, 148, 255);

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("The properties that will be applied when the cursor is over the control")]
        public TextBoxState HoveredState
        {
            get
            {
                return base.DefaultHoveredState;
            }
            set
            {
                base.DefaultHoveredState = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("The properties that will be applied when the control is focused")]
        public TextBoxState FocusedState
        {
            get
            {
                return base.DefaultFocusedState;
            }
            set
            {
                base.DefaultFocusedState = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        [Description("The properties that will be applied when the cursor is in a disabled state")]
        public TextBoxState DisabledState
        {
            get
            {
                return base.DefaultDisabledState;
            }
            set
            {
                base.DefaultDisabledState = value;
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

        [DefaultValue(typeof(Point), "0, 0")]
        [Description("The control's text position")]
        public Point TextOffset
        {
            get
            {
                return base.DefaultTextOffset;
            }
            set
            {
                base.DefaultTextOffset = value;
            }
        }

        [Browsable(true)]
        [Category("Options")]
        [Description("The control's placeholder text")]
        public string PlaceholderText
        {
            get
            {
                return base.DefaultPlaceholderText;
            }
            set
            {
                base.DefaultPlaceholderText = value;
            }
        }

        [Browsable(true)]
        [Description("The control's placeholder text ForeColor")]
        [DefaultValue(typeof(Color), "193, 200, 207")]
        public Color PlaceholderForeColor
        {
            get
            {
                return base.DefaultPlaceholderForeColor;
            }
            set
            {
                base.DefaultPlaceholderForeColor = value;
            }
        }

        [Description("Sets the TextBox's border radius.")]
        [DefaultValue(0)]
        [Browsable(true)]
        public int BorderRadius
        {
            get
            {
                return base.DefaultBorderRadius;
            }
            set
            {
                base.DefaultBorderRadius = value;
            }
        }

        [Description("The control's css-like border style")]
        [DefaultValue(DashStyle.Solid)]
        [Browsable(true)]
        public new DashStyle BorderStyle
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

        [Description("Gets or sets the control border color.")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "213, 218, 223")]
        public Color BorderColor
        {
            get
            {
                return base.DefaultBorderColor;
            }
            set
            {
                base.DefaultBorderColor = value;
            }
        }

        [DefaultValue(1)]
        [Browsable(true)]
        [Description("Gets or sets the control border size.")]
        public int BorderThickness
        {
            get
            {
                return base.DefaultBorderThickness;
            }
            set
            {
                base.DefaultBorderThickness = value;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Shadow property of the control to add and customize a control's shadow")]
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
        [Description("Sets the TextBox's left icon.")]
        [DefaultValue(typeof(Image), "")]
        public Image IconLeft
        {
            get
            {
                return base.DefaultIconLeft;
            }
            set
            {
                base.DefaultIconLeft = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(typeof(Size), "20, 20")]
        [Description("Sets TextBox's left icon size.")]
        public Size IconLeftSize
        {
            get
            {
                return base.DefaultIconLeftSize;
            }
            set
            {
                base.DefaultIconLeftSize = value;
            }
        }

        [Description("Sets TextBox's left icon cursor.")]
        [Browsable(true)]
        [DefaultValue(typeof(Cursor), "Default")]
        public Cursor IconLeftCursor
        {
            get
            {
                return base.DefaultIconLeftCursor;
            }
            set
            {
                base.DefaultIconLeftCursor = value;
            }
        }

        [Browsable(true)]
        [Description("Sets TextBox's left icon offset (Point).")]
        [DefaultValue(typeof(Point), "0, 0")]
        public Point IconLeftOffset
        {
            get
            {
                return base.DefaultIconLeftOffset;
            }
            set
            {
                base.DefaultIconLeftOffset = value;
            }
        }

        [DefaultValue(typeof(Image), "")]
        [Description("Sets the TextBox's right icon.")]
        [Browsable(true)]
        public Image IconRight
        {
            get
            {
                return base.DefaultIconRight;
            }
            set
            {
                base.DefaultIconRight = value;
            }
        }

        [Description("Sets TextBox's right icon size.")]
        [DefaultValue(typeof(Size), "20, 20")]
        [Browsable(true)]
        public Size IconRightSize
        {
            get
            {
                return base.DefaultIconRightSize;
            }
            set
            {
                base.DefaultIconRightSize = value;
            }
        }

        [Description("Sets TextBox's right icon cursor.")]
        [Browsable(true)]
        [DefaultValue(typeof(Cursor), "Default")]
        public Cursor IconRightCursor
        {
            get
            {
                return base.DefaultIconRightCursor;
            }
            set
            {
                base.DefaultIconRightCursor = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(typeof(Point), "0, 0")]
        [Description("Sets TextBox's right icon offset (Point).")]
        public Point IconRightOffset
        {
            get
            {
                return base.DefaultIconRightOffset;
            }
            set
            {
                base.DefaultIconRightOffset = value;
            }
        }

        [Browsable(true)]
        [Description("Sets the TextBox's fill color or inner-background color.")]
        [DefaultValue(typeof(Color), "White")]
        public Color FillColor
        {
            get
            {
                return base.DefaultFillColor;
            }
            set
            {
                base.DefaultFillColor = value;
            }
        }
    }
}