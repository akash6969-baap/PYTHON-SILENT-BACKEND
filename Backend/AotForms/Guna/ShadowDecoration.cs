using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace AimBotConquer.Guna {

    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Description("ShadowDecoration")]
    public class ShadowDecoration {

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Control Parent {
            get {
                return this.J3BnI0lbjggojq2wUZouY3aZH4f;
            }
            set {
                if (this.J3BnI0lbjggojq2wUZouY3aZH4f != value) {
                    if (this.J3BnI0lbjggojq2wUZouY3aZH4f != null) {
                        this.J3BnI0lbjggojq2wUZouY3aZH4f.VisibleChanged -= this.hV94cVaEjj2sDKgPdxobdwgXf3o;
                    }
                    this.J3BnI0lbjggojq2wUZouY3aZH4f = value;
                    if (this.J3BnI0lbjggojq2wUZouY3aZH4f != null) {
                        this.J3BnI0lbjggojq2wUZouY3aZH4f.VisibleChanged += this.hV94cVaEjj2sDKgPdxobdwgXf3o;
                    }
                }
            }
        }

        internal void Ts4yq7LqhZIiuthWW98suMWsPzg(int A_1) {
            this.DsoqPTFrGYH4iDwoVeXdSLDeXhK = A_1;
            if (this.DsoqPTFrGYH4iDwoVeXdSLDeXhK > 0 && this.Parent != null && this.OB02FP6bhYTLsbV62KURE40P9Bt && this.Parent.BackColor != Color.Transparent) {
                this.Parent.BackColor = Color.Transparent;
            }
        }

        internal void VKyccgKSRgEEE6bWxqjAzv1oFMJ(PaintValueEventArgs A_1) {
            if (this.Enabled) {
                A_1.Graphics.FillRectangle(new SolidBrush(this.KlEnha1KiANaUxccLujJt5XaGYfA), A_1.Bounds);
            }
        }

        public ShadowDecoration(IControl A_1) {
            this.KlEnha1KiANaUxccLujJt5XaGYfA = Color.Black;
            this.XhvVDouKOZ3aw5r4RVAiHzo49IB = 30;
            this.BG8RkRXrr1keSwf4gLsoWQnX95d = new Padding(5);
            this.bnNDAe51VzPIUaemAavSNKLcfqT = 6;
            this.CLvZxCPc3oehVRPwun5pd5Dnsd = false;
            this.Parent = (Control)A_1;
            this.Parent.Invalidate();
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Style the control's border sides.")]
        public virtual CustomizableEdges CustomizableEdges {
            get {
                return this.X0WGfXDFJwYbtLhO9tQAwnN8O3;
            }
            set {
                if (this.X0WGfXDFJwYbtLhO9tQAwnN8O3 != value) {
                    this.X0WGfXDFJwYbtLhO9tQAwnN8O3 = value;
                    this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
                }
            }
        }

        [Browsable(true)]
        [NotifyParentProperty(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [Description("If true, the shadow decoration will be enabled")]
        public bool Enabled {
            get {
                return this.OB02FP6bhYTLsbV62KURE40P9Bt;
            }
            set {
                if (this.OB02FP6bhYTLsbV62KURE40P9Bt != value) {
                    this.OB02FP6bhYTLsbV62KURE40P9Bt = value;
                    if (this.DsoqPTFrGYH4iDwoVeXdSLDeXhK > 0 && this.Parent != null && this.OB02FP6bhYTLsbV62KURE40P9Bt && this.Parent.BackColor != Color.Transparent) {
                        this.Parent.BackColor = Color.Transparent;
                    }
                    if (this.GjCmpVWY14rLa9hnUbVh6kd6n0f == null) {
                        new Thread(new ThreadStart(this.eADcckhTjlvygro60ghzA7tD7lq)).Start();
                    } else {
                        this.XrQVrcOAy09VyITF6fJRbHYPWSF(this.OB02FP6bhYTLsbV62KURE40P9Bt);
                    }
                }
                this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
            }
        }

        [Browsable(true)]
        [NotifyParentProperty(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(ShadowMode.Custom)]
        [Description("The shadow decoration mode")]
        public ShadowMode Mode {
            get {
                return this.nd7veED6faDh1yXqsD1kGxDZRY;
            }
            set {
                this.nd7veED6faDh1yXqsD1kGxDZRY = value;
                this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
            }
        }

        [Browsable(true)]
        [NotifyParentProperty(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(typeof(Color), "Black")]
        [Description("The shadow decoration color")]
        public Color Color {
            get {
                return this.KlEnha1KiANaUxccLujJt5XaGYfA;
            }
            set {
                this.KlEnha1KiANaUxccLujJt5XaGYfA = value;
                this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
            }
        }

        [Browsable(true)]
        [NotifyParentProperty(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(30)]
        [Description("The shadow decoration depth")]
        public int Depth {
            get {
                return this.XhvVDouKOZ3aw5r4RVAiHzo49IB;
            }
            set {
                this.XhvVDouKOZ3aw5r4RVAiHzo49IB = ((value > 255) ? 255 : ((value < 0) ? 0 : value));
                this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
            }
        }

        [Browsable(true)]
        [NotifyParentProperty(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(typeof(Padding), "5, 5, 5, 5")]
        [Description("The shadow decoration shadow")]
        public Padding Shadow {
            get {
                return this.BG8RkRXrr1keSwf4gLsoWQnX95d;
            }
            set {
                this.BG8RkRXrr1keSwf4gLsoWQnX95d = value;
                this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
            }
        }

        [Browsable(true)]
        [NotifyParentProperty(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(6)]
        [Description("The shadow decoration border radius")]
        public int BorderRadius {
            get {
                return this.bnNDAe51VzPIUaemAavSNKLcfqT;
            }
            set {
                this.bnNDAe51VzPIUaemAavSNKLcfqT = ((value < 0) ? 0 : value);
                this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
            }
        }

        private void XrQVrcOAy09VyITF6fJRbHYPWSF(bool A_1) {
            if (this.lO6pp78RGPDQvzoKBF0epcdMhK()) {
                if (!A_1) {
                    this.sHNggUYK4gysOZTiSC2nXrIbRYA = 0;
                    this.GjCmpVWY14rLa9hnUbVh6kd6n0f.ControlRemoved -= this.xx9MWtQp348PaC0kBQADXURGlY5;
                    this.GjCmpVWY14rLa9hnUbVh6kd6n0f.Paint -= this.OcuOJgFb1fmz8EDaARMKtpGakBQ;
                    this.GjCmpVWY14rLa9hnUbVh6kd6n0f.Resize -= this.nxTtkik8koCCus2S6ZDff3231mw;
                    this.Parent.Resize -= this.nPZIRWrcP73ODXldSA14yj5vFc;
                } else if (this.sHNggUYK4gysOZTiSC2nXrIbRYA == 0) {
                    this.sHNggUYK4gysOZTiSC2nXrIbRYA = 1;
                    this.GjCmpVWY14rLa9hnUbVh6kd6n0f.ControlRemoved += this.xx9MWtQp348PaC0kBQADXURGlY5;
                    this.GjCmpVWY14rLa9hnUbVh6kd6n0f.Paint += this.OcuOJgFb1fmz8EDaARMKtpGakBQ;
                    this.GjCmpVWY14rLa9hnUbVh6kd6n0f.Resize += this.nxTtkik8koCCus2S6ZDff3231mw;
                    this.Parent.Resize += this.nPZIRWrcP73ODXldSA14yj5vFc;
                    return;
                }
            }
        }

        private void eADcckhTjlvygro60ghzA7tD7lq() {
            while (this.Parent.Parent == null) {
                Thread.Sleep(100);
                Application.DoEvents();
            }
            this.GjCmpVWY14rLa9hnUbVh6kd6n0f = this.Parent.Parent;
            this.XrQVrcOAy09VyITF6fJRbHYPWSF(this.OB02FP6bhYTLsbV62KURE40P9Bt);
            this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
        }

        private bool lO6pp78RGPDQvzoKBF0epcdMhK() {
            return !((IControl)this.Parent).IsDesignMode && this.GjCmpVWY14rLa9hnUbVh6kd6n0f != null;
        }

        private void B7qcvNmLyrDYeFt2FY2HDYWnPqn() {
            this.CLvZxCPc3oehVRPwun5pd5Dnsd = false;
            if (this.lO6pp78RGPDQvzoKBF0epcdMhK()) {
                this.GjCmpVWY14rLa9hnUbVh6kd6n0f.Invalidate();
            }
        }

        public override string ToString() {
            return string.Empty;
        }

        private Rectangle HD1rLT8zPLEcQoLD2CbetEsRcNJ() {
            return checked(new Rectangle(this.Parent.Location.X - this.Shadow.Left, this.Parent.Location.Y - this.Shadow.Top, this.Parent.Width + (this.Shadow.Left + this.Shadow.Right), this.Parent.Height + (this.Shadow.Top + this.Shadow.Bottom)));
        }

        private int W1UTtfZLxzAIQq3em1L0NJPZsq() {
            int num;
            if (this.Shadow.Left < this.Shadow.Right) {
                num = this.Shadow.Right;
            } else {
                num = this.Shadow.Left;
            }
            int num2;
            if (this.Shadow.Top >= this.Shadow.Bottom) {
                num2 = this.Shadow.Left;
            } else {
                num2 = this.Shadow.Right;
            }
            int num3;
            if (num < num2) {
                num3 = num2;
            } else {
                num3 = num;
            }
            return num3;
        }

        private void hV94cVaEjj2sDKgPdxobdwgXf3o(object A_1, EventArgs A_2) {
            if (this.Enabled) {
                this.XrQVrcOAy09VyITF6fJRbHYPWSF(this.Parent.Visible);
            }
            this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
        }

        private void nPZIRWrcP73ODXldSA14yj5vFc(object A_1, EventArgs A_2) {
            this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
        }

        private void xx9MWtQp348PaC0kBQADXURGlY5(object A_1, ControlEventArgs A_2) {
            if (A_2.Control == this.Parent) {
                this.XrQVrcOAy09VyITF6fJRbHYPWSF(false);
                this.B7qcvNmLyrDYeFt2FY2HDYWnPqn();
            }
        }

        private void nxTtkik8koCCus2S6ZDff3231mw(object A_1, EventArgs A_2) {
            this.GjCmpVWY14rLa9hnUbVh6kd6n0f.Invalidate();
        }

        private void OcuOJgFb1fmz8EDaARMKtpGakBQ(object A_1, PaintEventArgs A_2) {
            try {
                if (this.Parent.Visible) {
                    if (!this.CLvZxCPc3oehVRPwun5pd5Dnsd | (this.TbHC1JkALmiUimlCrkoy4SwvW5H == null)) {
                        Bitmap bitmap = new Bitmap(this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Width / 2, this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Height / 2);
                        Graphics graphics = Graphics.FromImage(bitmap);
                        if (this.nd7veED6faDh1yXqsD1kGxDZRY == ShadowMode.Custom) {
                            graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            graphics.FillPath(new SolidBrush(Color.FromArgb(this.Depth, this.Color)), GraphicsHelper.RoundRect(GraphicsHelper.o2GhHXpYDnkVMC6SZ8zjIElSiFB(new RectangleF(0f, 0f, (float)(this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Width / 2), (float)(this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Height / 2))), (float)((this.BorderRadius < 2) ? 2 : this.BorderRadius), this.CustomizableEdges));
                        } else {
                            GraphicsPath graphicsPath = new GraphicsPath();
                            graphicsPath.AddEllipse(0, 0, this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Width / 2 - 1, this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Height / 2 - 1);
                            graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            graphics.FillPath(new SolidBrush(Color.FromArgb(this.Depth, this.Color)), graphicsPath);
                        }
                        this.TbHC1JkALmiUimlCrkoy4SwvW5H = new Bitmap(this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Width, this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Height);
                        Graphics graphics2 = Graphics.FromImage(this.TbHC1JkALmiUimlCrkoy4SwvW5H);
                        graphics2.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        int num = this.W1UTtfZLxzAIQq3em1L0NJPZsq();
                        int num2 = ((num >= 10) ? 10 : num);
                        checked {
                            for (int i = 0; i <= num2; i++) {
                                graphics2.DrawImage(bitmap, new Rectangle(i, i, this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Width - i * 2, this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Height - i * 2));
                            }
                            bitmap.Dispose();
                            this.CLvZxCPc3oehVRPwun5pd5Dnsd = true;
                        }
                    }
                    A_2.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    A_2.Graphics.DrawImage(this.TbHC1JkALmiUimlCrkoy4SwvW5H, this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().X, this.HD1rLT8zPLEcQoLD2CbetEsRcNJ().Y);
                }
            } catch {
            }
        }

        private Bitmap TbHC1JkALmiUimlCrkoy4SwvW5H;

        private bool CLvZxCPc3oehVRPwun5pd5Dnsd;

        private Control J3BnI0lbjggojq2wUZouY3aZH4f;

        private int DsoqPTFrGYH4iDwoVeXdSLDeXhK;

        private Control GjCmpVWY14rLa9hnUbVh6kd6n0f;

        private CustomizableEdges X0WGfXDFJwYbtLhO9tQAwnN8O3 = new CustomizableEdges();

        private bool OB02FP6bhYTLsbV62KURE40P9Bt;

        private ShadowMode nd7veED6faDh1yXqsD1kGxDZRY;

        private Color KlEnha1KiANaUxccLujJt5XaGYfA;

        private int XhvVDouKOZ3aw5r4RVAiHzo49IB;

        private Padding BG8RkRXrr1keSwf4gLsoWQnX95d;

        private int bnNDAe51VzPIUaemAavSNKLcfqT;

        private int sHNggUYK4gysOZTiSC2nXrIbRYA;
    }

    public enum ShadowMode {

        Custom,

        Circle
    }
}