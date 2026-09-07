using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace AimBotConquer.Guna
{
    public enum MouseState
    {

        HOVER,

        DOWN,

        const_2
    }

    [ToolboxItem(false)]
    [DefaultEvent("CheckedChanged")]
    public class ToggleSwitch : Control, IControl
    {

        public ToggleSwitch()
        {
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.UserMouse | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            this.method_0();
        }

        [Browsable(false)]
        [Description("Gets a value that indicates whether the Component is currently in design mode.")]
        public bool IsDesignMode
        {
            get
            {
                return base.DesignMode;
            }
        }

        private void method_0()
        {
            this.animationManager_0 = new AnimationManager(true)
            {
                Increment = 0.079999998211860657,
                AnimationType = AnimationType.EaseOut
            };
            this.animationManager_0.OnAnimationProgress += this.method_1;
        }

        private void method_1(object object_0)
        {
            base.Invalidate();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected ToggleSwitchState DefaultCheckedState
        {
            get
            {
                if (this.toggleSwitchState_0 == null)
                {
                    this.toggleSwitchState_0 = new ToggleSwitchState
                    {
                        Parent = this
                    };
                }
                return this.toggleSwitchState_0;
            }
            set
            {
                this.toggleSwitchState_0 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected ToggleSwitchState DefaultUncheckedState
        {
            get
            {
                if (this.toggleSwitchState_1 == null)
                {
                    this.toggleSwitchState_1 = new ToggleSwitchState
                    {
                        Parent = this
                    };
                }
                return this.toggleSwitchState_1;
            }
            set
            {
                this.toggleSwitchState_1 = value;
            }
        }

        [Browsable(false)]
        protected bool DefaultAnimated
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        [Browsable(false)]
        protected DashStyle DefaultBorderStyle
        {
            get
            {
                return this.dashStyle_0;
            }
            set
            {
                this.dashStyle_0 = value;
                base.Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected ShadowDecoration DefaultShadowDecoration
        {
            get
            {
                if (this.shadowDecoration_0 == null)
                {
                    this.shadowDecoration_0 = new ShadowDecoration(this);
                }
                return this.shadowDecoration_0;
            }
            set
            {
                this.shadowDecoration_0 = value;
            }
        }

        [Browsable(false)]
        protected bool DefaultChecked
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
                this.OnCheckedChanged(EventArgs.Empty);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.bool_2 = true;
            this.mouseState_0 = MouseState.DOWN;
            if (this.bool_3)
            {
                base.Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.mouseState_0 = (this.bool_2 ? MouseState.HOVER : MouseState.const_2);
            if (this.bool_3)
            {
                base.Invalidate();
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.bool_2 = true;
            this.mouseState_0 = MouseState.HOVER;
            if (this.bool_3)
            {
                base.Invalidate();
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.bool_2 = false;
            this.mouseState_0 = MouseState.const_2;
            if (this.bool_3)
            {
                base.Invalidate();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.bool_2 = false;
            this.mouseState_0 = MouseState.const_2;
            if (this.bool_4)
            {
                base.Invalidate();
            }
            base.OnLostFocus(e);
        }

        [Browsable(false)]
        protected bool DefaultUseTransparentBackground
        {
            get
            {
                return this.bool_5;
            }
            set
            {
                this.bool_5 = value;
                base.Invalidate();
            }
        }

        private void method_2(Graphics graphics_0)
        {
            if (this.bool_5)
            {
                graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                graphics_0.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics_0.CompositingQuality = CompositingQuality.GammaCorrected;
                if (base.Parent != null)
                {
                    if (this.BackColor != Color.Transparent)
                    {
                        this.BackColor = Color.Transparent;
                    }
                    int childIndex = base.Parent.Controls.GetChildIndex(this);
                    int num = base.Parent.Controls.Count - 1;
                    int num2 = childIndex + 1;
                    for (int i = num; i >= num2; i += -1)
                    {
                        Control control = base.Parent.Controls[i];
                        if (control.Bounds.IntersectsWith(base.Bounds) && control.Visible)
                        {
                            Bitmap bitmap = new Bitmap(control.Width, control.Height, graphics_0);
                            control.DrawToBitmap(bitmap, control.ClientRectangle);
                            graphics_0.TranslateTransform((float)(control.Left - base.Left), (float)(control.Top - base.Top));
                            graphics_0.DrawImageUnscaled(bitmap, Point.Empty);
                            graphics_0.TranslateTransform((float)(base.Left - control.Left), (float)(base.Top - control.Top));
                            bitmap.Dispose();
                        }
                    }
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            this.method_2(e.Graphics);
        }

        public event EventHandler CheckedChanged
        {
            [CompilerGenerated]
            add
            {
                EventHandler eventHandler = this.eventHandler_0;
                EventHandler eventHandler2;
                do
                {
                    eventHandler2 = eventHandler;
                    EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_0, eventHandler3, eventHandler2);
                }
                while (eventHandler != eventHandler2);
            }
            [CompilerGenerated]
            remove
            {
                EventHandler eventHandler = this.eventHandler_0;
                EventHandler eventHandler2;
                do
                {
                    eventHandler2 = eventHandler;
                    EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_0, eventHandler3, eventHandler2);
                }
                while (eventHandler != eventHandler2);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (this.animationManager_0 != null && !base.DesignMode)
            {
                this.animationManager_0.StartNewAnimation(this.bool_1 ? AnimationDirection.In : AnimationDirection.Out, null);
            }
            base.Invalidate();
            if (this.eventHandler_0 != null)
            {
                this.eventHandler_0(this, EventArgs.Empty);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (!this.bool_1)
            {
                this.DefaultChecked = true;
            }
            else
            {
                this.DefaultChecked = false;
            }
            base.OnClick(e);
        }

        private void method_3(Graphics graphics_0, ToggleSwitchState toggleSwitchState_2)
        {
            int num = toggleSwitchState_2.BorderRadius;
            if (this.bool_0 && !base.DesignMode && base.Enabled)
            {
                Color color = smethod_23((int)(this.animationManager_0.GetProgress() * 100.0), this.DefaultUncheckedState.FillColor, this.DefaultCheckedState.FillColor);
                Color color2 = smethod_23((int)(this.animationManager_0.GetProgress() * 100.0), this.DefaultUncheckedState.BorderColor, this.DefaultCheckedState.BorderColor);
                if (num > 0)
                {
                    graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                    GraphicsPath graphicsPath = smethod_12(smethod_11(base.ClientRectangle), (float)(num * 2));
                    graphics_0.FillPath(new SolidBrush(color), graphicsPath);
                    if (toggleSwitchState_2.BorderThickness < 1)
                    {
                        graphics_0.DrawPath(new Pen(Color.FromArgb(180, color)), graphicsPath);
                    }
                    smethod_20(graphics_0, new SolidBrush(color2), base.ClientRectangle, num, toggleSwitchState_2.BorderThickness, this.dashStyle_0);
                }
                else
                {
                    graphics_0.SmoothingMode = SmoothingMode.Default;
                    graphics_0.FillRectangle(new SolidBrush(color), base.ClientRectangle);
                    smethod_22(graphics_0, new SolidBrush(color2), base.ClientRectangle, toggleSwitchState_2.BorderThickness, this.dashStyle_0);
                }
                int num2 = toggleSwitchState_2.InnerOffset + 8;
                int num3 = num2 / 2;
                num = toggleSwitchState_2.InnerBorderRadius;
                int num4 = Math.Max(num3, (int)(this.animationManager_0.GetProgress() * (double)(base.Width - (base.Height - num2 + num3))));
                color = smethod_23((int)(this.animationManager_0.GetProgress() * 100.0), this.DefaultUncheckedState.InnerColor, this.DefaultCheckedState.InnerColor);
                color2 = smethod_23((int)(this.animationManager_0.GetProgress() * 100.0), this.DefaultUncheckedState.InnerBorderColor, this.DefaultCheckedState.InnerBorderColor);
                Rectangle rectangle = new Rectangle(num4, num3, base.Height - num2, base.Height - num2);
                if (num > 0)
                {
                    graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                    GraphicsPath graphicsPath = smethod_12(smethod_11(rectangle), (float)(num * 2));
                    graphics_0.FillPath(new SolidBrush(color), graphicsPath);
                    if (toggleSwitchState_2.InnerBorderThickness < 1)
                    {
                        graphics_0.DrawPath(new Pen(Color.FromArgb(180, color)), graphicsPath);
                    }
                    smethod_20(graphics_0, new SolidBrush(color2), rectangle, num, toggleSwitchState_2.InnerBorderThickness, this.dashStyle_0);
                }
                else
                {
                    graphics_0.SmoothingMode = SmoothingMode.Default;
                    graphics_0.FillRectangle(new SolidBrush(color), rectangle);
                    smethod_22(graphics_0, new SolidBrush(color2), rectangle, toggleSwitchState_2.InnerBorderThickness, this.dashStyle_0);
                }
            }
            else
            {
                Color color = toggleSwitchState_2.FillColor;
                Color color2 = toggleSwitchState_2.BorderColor;
                if (num > 0)
                {
                    graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                    GraphicsPath graphicsPath = smethod_12(smethod_11(base.ClientRectangle), (float)(num * 2));
                    graphics_0.FillPath(new SolidBrush(color), graphicsPath);
                    if (toggleSwitchState_2.BorderThickness < 1)
                    {
                        graphics_0.DrawPath(new Pen(Color.FromArgb(180, color)), graphicsPath);
                    }
                    smethod_20(graphics_0, new SolidBrush(color2), base.ClientRectangle, num, toggleSwitchState_2.BorderThickness, this.dashStyle_0);
                }
                else
                {
                    graphics_0.SmoothingMode = SmoothingMode.Default;
                    graphics_0.FillRectangle(new SolidBrush(color), base.ClientRectangle);
                    smethod_22(graphics_0, new SolidBrush(color2), base.ClientRectangle, toggleSwitchState_2.BorderThickness, this.dashStyle_0);
                }
                int num5 = toggleSwitchState_2.InnerOffset + 8;
                int num6 = num5 / 2;
                num = toggleSwitchState_2.InnerBorderRadius;
                int num4 = (this.bool_1 ? (base.Width - (base.Height - num5 + num6)) : num6);
                color = toggleSwitchState_2.InnerColor;
                color2 = toggleSwitchState_2.InnerBorderColor;
                Rectangle rectangle2 = new Rectangle(num4, num6, base.Height - num5, base.Height - num5);
                if (num > 0)
                {
                    graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                    GraphicsPath graphicsPath = smethod_12(smethod_11(rectangle2), (float)(num * 2));
                    graphics_0.FillPath(new SolidBrush(color), graphicsPath);
                    if (toggleSwitchState_2.InnerBorderThickness < 1)
                    {
                        graphics_0.DrawPath(new Pen(Color.FromArgb(180, color)), graphicsPath);
                    }
                    smethod_20(graphics_0, new SolidBrush(color2), rectangle2, num, toggleSwitchState_2.InnerBorderThickness, this.dashStyle_0);
                }
                else
                {
                    graphics_0.SmoothingMode = SmoothingMode.Default;
                    graphics_0.FillRectangle(new SolidBrush(color), rectangle2);
                    smethod_22(graphics_0, new SolidBrush(color2), rectangle2, toggleSwitchState_2.InnerBorderThickness, this.dashStyle_0);
                }
            }
        }

        internal static RectangleF smethod_11(RectangleF rectangleF_0)
        {
            return new RectangleF(rectangleF_0.X, rectangleF_0.Y, rectangleF_0.Width - 1f, rectangleF_0.Height - 1f);
        }

        internal static GraphicsPath smethod_12(RectangleF rectangleF_0, float float_0)
        {
            RectangleF rectangleF = rectangleF_0;
            rectangleF.X -= 0.1f;
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(rectangleF.X, rectangleF.Y, float_0, float_0, 180f, 90f);
            graphicsPath.AddArc(rectangleF.X + rectangleF.Width - float_0, rectangleF.Y, float_0, float_0, 270f, 90f);
            graphicsPath.AddArc(rectangleF.X + rectangleF.Width - float_0, rectangleF.Y + rectangleF.Height - float_0, float_0, float_0, 0f, 90f);
            graphicsPath.AddArc(rectangleF.X, rectangleF.Y + rectangleF.Height - float_0, float_0, float_0, 90f, 90f);
            graphicsPath.CloseAllFigures();
            return graphicsPath;
        }

        internal static Color smethod_23(int int_0, Color color_1, Color color_2)
        {
            Color color = color_1;
            Color color2 = color_2;
            if (int_0 < 100)
            {
                if (color == Color.Transparent)
                {
                    color = Color.Empty;
                }
                if (color2 == Color.Transparent)
                {
                    color2 = Color.Empty;
                }
            }
            int a = (int)color.A;
            int r = (int)color.R;
            int g = (int)color.G;
            int b = (int)color.B;
            int a2 = (int)color2.A;
            int r2 = (int)color2.R;
            int g2 = (int)color2.G;
            int b2 = (int)color2.B;
            double num = Math.Round((double)a + (double)(checked((a2 - a) * int_0)) * 0.01, 0);
            double num2 = Math.Round((double)r + (double)(checked((r2 - r) * int_0)) * 0.01);
            double num3 = Math.Round((double)g + (double)(checked((g2 - g) * int_0)) * 0.01, 0);
            double num4 = Math.Round((double)b + (double)(checked((b2 - b) * int_0)) * 0.01);
            return Color.FromArgb((int)num, (int)num2, (int)num3, (int)num4);
        }

        internal static void smethod_20(Graphics graphics_0, Brush brush_0, Rectangle rectangle_1, int int_0, int int_1, DashStyle dashStyle_0 = DashStyle.Solid)
        {
            if (int_1 >= 1)
            {
                GraphicsPath graphicsPath = smethod_15(rectangle_1, (float)int_0, (float)int_1);
                using (Pen pen = new Pen(brush_0, (float)int_1))
                {
                    pen.DashStyle = dashStyle_0;
                    graphics_0.DrawPath(pen, graphicsPath);
                }
            }
        }

        internal static GraphicsPath smethod_15(Rectangle rectangle_1, float float_0, float float_1)
        {
            RectangleF rectangleF = new RectangleF((float)rectangle_1.X, (float)rectangle_1.Y, (float)rectangle_1.Width, (float)rectangle_1.Height);
            rectangleF.Width -= 1f;
            rectangleF.Height -= 1f;
            GraphicsPath graphicsPath;
            if (float_1 != 1f)
            {
                float num = float_1 / 2f;
                rectangleF.X += num;
                rectangleF.Y += num;
                rectangleF.Width -= float_1;
                rectangleF.Height -= float_1;
                graphicsPath = smethod_12(rectangleF, float_0 * 2f - 1f);
            }
            else
            {
                graphicsPath = smethod_12(rectangleF, float_0 * 2f);
            }
            return graphicsPath;
        }

        internal static void smethod_22(Graphics graphics_0, Brush brush_0, Rectangle rectangle_1, int int_0, DashStyle dashStyle_0 = DashStyle.Solid)
        {
            if (int_0 >= 1)
            {
                using (Pen pen = new Pen(brush_0, (float)int_0))
                {
                    pen.DashStyle = dashStyle_0;
                    GraphicsPath graphicsPath = new GraphicsPath();
                    RectangleF rectangleF = new RectangleF((float)rectangle_1.X, (float)rectangle_1.Y, (float)rectangle_1.Width, (float)rectangle_1.Height);
                    float num = (float)int_0 / 2f;
                    if (int_0 > 1)
                    {
                        rectangleF.X += num;
                        rectangleF.Y += num;
                    }
                    rectangleF.Width -= (float)int_0;
                    rectangleF.Height -= (float)int_0;
                    graphicsPath.AddRectangle(rectangleF);
                    graphics_0.DrawPath(pen, graphicsPath);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!base.Enabled)
            {
                Bitmap bitmap = new Bitmap(base.Width, base.Height);
                Graphics graphics = Graphics.FromImage(bitmap);
                if (this.bool_1)
                {
                    this.method_3(graphics, this.DefaultCheckedState);
                }
                else
                {
                    this.method_3(graphics, this.DefaultUncheckedState);
                }
                ControlPaint.DrawImageDisabled(e.Graphics, bitmap, 0, 0, Color.White);
            }
            else if (this.bool_1)
            {
                this.method_3(e.Graphics, this.DefaultCheckedState);
            }
            else
            {
                this.method_3(e.Graphics, this.DefaultUncheckedState);
            }
            base.OnPaint(e);
        }

        private AnimationManager animationManager_0;

        private ToggleSwitchState toggleSwitchState_0;

        private ToggleSwitchState toggleSwitchState_1;

        private bool bool_0 = true;

        private DashStyle dashStyle_0 = DashStyle.Solid;

        private ShadowDecoration shadowDecoration_0;

        private bool bool_1 = false;

        private bool bool_2 = false;

        internal bool bool_3 = false;

        internal bool bool_4 = false;

        internal MouseState mouseState_0 = MouseState.const_2;

        private bool bool_5;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        private EventHandler eventHandler_0;
    }
}