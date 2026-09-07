using AimBotConquer.Guna;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AimBotConquer.Guna {

    [DefaultEvent("TextChanged")]
    [ToolboxItem(false)]
    public class TextBoxBase : UserControl, IControl {

        public TextBoxBase() {
            base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.UserMouse | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            this.method_0();
            this.InitializeComponent();
        }

        [Description("Gets a value that indicates whether the Component is currently in design mode.")]
        [Browsable(false)]
        public bool IsDesignMode {
            get {
                return base.DesignMode;
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing && this.icontainer_0 != null) {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.Owner = new Class8();
            base.SuspendLayout();
            this.Owner.BackColor = Color.White;
            this.Owner.BorderStyle = BorderStyle.None;
            this.Owner.Font = new Font("Segoe UI", 9f);
            this.Owner.ForeColor = Color.FromArgb(125, 137, 149);
            this.Owner.Location = new Point(9, 10);
            this.Owner.Name = "Owner";
            this.Owner.Boolean_0 = false;
            this.Owner.String_0 = "";
            this.Owner.Size = new Size(182, 16);
            this.Owner.TabIndex = 0;
            this.Owner.Event_0 += this.method_9;
            this.Owner.Event_1 += this.method_10;
            this.Owner.TextChanged += this.Owner_TextChanged;
            this.Owner.KeyDown += this.Owner_KeyDown;
            this.Owner.KeyPress += this.Owner_KeyPress;
            this.Owner.KeyUp += this.Owner_KeyUp;
            this.Owner.MouseDown += this.Owner_MouseDown;
            this.Owner.MouseEnter += this.Owner_MouseEnter;
            this.Owner.MouseLeave += this.Owner_MouseLeave;
            this.Owner.MouseUp += this.Owner_MouseUp;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.Owner);
            this.Cursor = Cursors.IBeam;
            this.ForeColor = Color.FromArgb(125, 137, 149);
            base.Name = "TextBoxWithIcon";
            base.Size = new Size(200, 36);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void method_0() {
            this.animationManager_0 = new AnimationManager(true) {
                Increment = 0.070000000298023224,
                AnimationType = AnimationType.Linear
            };
            this.animationManager_0.OnAnimationProgress += this.method_2;
            this.animationManager_1 = new AnimationManager(true) {
                Increment = 0.070000000298023224,
                AnimationType = AnimationType.Linear
            };
            this.animationManager_1.OnAnimationProgress += this.method_1;
        }

        private void method_1(object object_0) {
            base.Invalidate();
        }

        private void method_2(object object_0) {
            base.Invalidate();
        }

        [Browsable(true)]
        [Description("Occurs when the property 'Text' changes.")]
        public new event EventHandler TextChanged {
            [CompilerGenerated]
            add {
                EventHandler eventHandler = this.eventHandler_0;
                EventHandler eventHandler2;
                do {
                    eventHandler2 = eventHandler;
                    EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_0, eventHandler3, eventHandler2);
                }
                while (eventHandler != eventHandler2);
            }
            [CompilerGenerated]
            remove {
                EventHandler eventHandler = this.eventHandler_0;
                EventHandler eventHandler2;
                do {
                    eventHandler2 = eventHandler;
                    EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_0, eventHandler3, eventHandler2);
                }
                while (eventHandler != eventHandler2);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected new virtual void OnTextChanged(EventArgs e) {
            if (this.eventHandler_0 != null) {
                this.eventHandler_0(this, EventArgs.Empty);
            }
        }

        public event EventHandler IconLeftClick {
            [CompilerGenerated]
            add {
                EventHandler eventHandler = this.eventHandler_1;
                EventHandler eventHandler2;
                do {
                    eventHandler2 = eventHandler;
                    EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_1, eventHandler3, eventHandler2);
                }
                while (eventHandler != eventHandler2);
            }
            [CompilerGenerated]
            remove {
                EventHandler eventHandler = this.eventHandler_1;
                EventHandler eventHandler2;
                do {
                    eventHandler2 = eventHandler;
                    EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_1, eventHandler3, eventHandler2);
                }
                while (eventHandler != eventHandler2);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnIconLeftClick(EventArgs e) {
            if (this.eventHandler_1 != null) {
                this.eventHandler_1(this, EventArgs.Empty);
            }
        }

        public event EventHandler IconRightClick {
            [CompilerGenerated]
            add {
                EventHandler eventHandler = this.eventHandler_2;
                EventHandler eventHandler2;
                do {
                    eventHandler2 = eventHandler;
                    EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_2, eventHandler3, eventHandler2);
                }
                while (eventHandler != eventHandler2);
            }
            [CompilerGenerated]
            remove {
                EventHandler eventHandler = this.eventHandler_2;
                EventHandler eventHandler2;
                do {
                    eventHandler2 = eventHandler;
                    EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
                    eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_2, eventHandler3, eventHandler2);
                }
                while (eventHandler != eventHandler2);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnIconRightClick(EventArgs e) {
            if (this.eventHandler_2 != null) {
                this.eventHandler_2(this, EventArgs.Empty);
            }
        }

        protected override void OnClick(EventArgs e) {
            if (this.image_0 != null && this.method_3(this.point_3)) {
                this.OnIconLeftClick(EventArgs.Empty);
            }
            if (this.image_1 != null && this.method_4(this.point_3)) {
                this.OnIconRightClick(EventArgs.Empty);
            }
            base.OnClick(e);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected TextBoxState DefaultHoveredState {
            get {
                if (this.textBoxState_0 == null) {
                    this.textBoxState_0 = new TextBoxState {
                        Parent = this
                    };
                }
                return this.textBoxState_0;
            }
            set {
                this.textBoxState_0 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected TextBoxState DefaultFocusedState {
            get {
                if (this.textBoxState_1 == null) {
                    this.textBoxState_1 = new TextBoxState {
                        Parent = this
                    };
                }
                return this.textBoxState_1;
            }
            set {
                this.textBoxState_1 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        protected TextBoxState DefaultDisabledState {
            get {
                if (this.textBoxState_2 == null) {
                    this.textBoxState_2 = new TextBoxState {
                        Parent = this
                    };
                }
                return this.textBoxState_2;
            }
            set {
                this.textBoxState_2 = value;
            }
        }

        [Browsable(false)]
        protected bool DefaultAnimated {
            get {
                return this.bool_0;
            }
            set {
                this.bool_0 = value;
            }
        }

        [Browsable(false)]
        protected TextBoxStyle DefaultStyle {
            get {
                return this.textBoxStyle_0;
            }
            set {
                this.textBoxStyle_0 = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Browsable(false)]
        protected Point DefaultTextOffset {
            get {
                return this.point_0;
            }
            set {
                this.point_0 = value;
                this.method_7();
                base.Invalidate();
            }
        }

        [Browsable(false)]
        protected string DefaultPlaceholderText {
            get {
                return this.Owner.String_0;
            }
            set {
                this.Owner.String_0 = value;
            }
        }

        [Browsable(false)]
        protected Color DefaultPlaceholderForeColor {
            get {
                return this.color_0;
            }
            set {
                this.color_0 = value;
                this.method_9(this.Owner.Boolean_0);
            }
        }

        [Browsable(false)]
        protected int DefaultBorderRadius {
            get {
                return this.int_0;
            }
            set {
                this.int_0 = ((value < 0) ? 0 : value);
                base.Invalidate();
            }
        }

        [Browsable(false)]
        protected DashStyle DefaultBorderStyle {
            get {
                return this.dashStyle_0;
            }
            set {
                this.dashStyle_0 = value;
                base.Invalidate();
            }
        }

        [Browsable(false)]
        protected Color DefaultBorderColor {
            get {
                return this.color_1;
            }
            set {
                this.color_1 = value;
                base.Invalidate();
            }
        }

        [Browsable(false)]
        protected int DefaultBorderThickness {
            get {
                return this.int_1;
            }
            set {
                this.int_1 = value;
                base.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        protected ShadowDecoration DefaultShadowDecoration {
            get {
                if (this.shadowDecoration_0 == null) {
                    this.shadowDecoration_0 = new ShadowDecoration(this);
                }
                return this.shadowDecoration_0;
            }
            set {
                this.shadowDecoration_0 = value;
            }
        }

        [Browsable(false)]
        protected Image DefaultIconLeft {
            get {
                return this.image_0;
            }
            set {
                this.image_0 = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Browsable(false)]
        protected Size DefaultIconLeftSize {
            get {
                return this.size_0;
            }
            set {
                this.size_0 = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Browsable(false)]
        protected Cursor DefaultIconLeftCursor {
            get {
                return this.cursor_0;
            }
            set {
                this.cursor_0 = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Browsable(false)]
        protected Point DefaultIconLeftOffset {
            get {
                return this.point_1;
            }
            set {
                this.point_1 = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Browsable(false)]
        protected Image DefaultIconRight {
            get {
                return this.image_1;
            }
            set {
                this.image_1 = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Browsable(false)]
        protected Size DefaultIconRightSize {
            get {
                return this.size_1;
            }
            set {
                this.size_1 = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Browsable(false)]
        protected Cursor DefaultIconRightCursor {
            get {
                return this.cursor_1;
            }
            set {
                this.cursor_1 = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Browsable(false)]
        protected Point DefaultIconRightOffset {
            get {
                return this.point_2;
            }
            set {
                this.point_2 = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Browsable(false)]
        protected Color DefaultFillColor {
            get {
                return this.color_2;
            }
            set {
                this.color_2 = value;
                base.Invalidate();
            }
        }

        [Browsable(false)]
        [Description("Gets or sets the text associated with this control.")]
        public virtual string Text {
            get {
                return this.Owner.Text;
            }
            set {
                this.Owner.Text = value;
            }
        }

        [DisplayName("Text")]
        [Description("Sets the text input.")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Browsable(true)]
        public virtual string DefaultText {
            get {
                return this.Owner.Text;
            }
            set {
                this.Owner.Text = value;
            }
        }

        private bool method_3(Point point_4) {
            return point_4.X < 5 + this.point_1.X + this.size_0.Width;
        }

        private bool method_4(Point point_4) {
            return point_4.X > base.Width - (5 + this.point_2.X + this.size_1.Width);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            this.point_3 = e.Location;
            if (this.method_3(e.Location) && this.image_0 != null) {
                if (this.Cursor != this.cursor_0) {
                    this.Cursor = this.cursor_0;
                }
            } else if (this.method_4(e.Location) && this.image_1 != null) {
                if (this.Cursor != this.cursor_1) {
                    this.Cursor = this.cursor_1;
                }
            } else if (this.Cursor != Cursors.IBeam) {
                this.Cursor = Cursors.IBeam;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            this.bool_1 = true;
            this.mouseState_0 = MouseState.DOWN;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            this.mouseState_0 = (this.bool_1 ? MouseState.HOVER : MouseState.const_2);
            base.OnMouseUp(e);
            this.method_5(this.mouseState_0, this.Owner.mouseState_0);
        }

        private void method_5(MouseState mouseState_1, MouseState mouseState_2) {
            if ((mouseState_1 == MouseState.HOVER) | (mouseState_2 == MouseState.HOVER)) {
                if (!this.bool_0) {
                    base.Invalidate();
                } else if (!base.DesignMode) {
                    this.animationManager_0.StartNewAnimation(AnimationDirection.In, null);
                }
            }
            if (mouseState_1 == MouseState.const_2 && mouseState_2 == MouseState.const_2) {
                if (!this.bool_0) {
                    base.Invalidate();
                } else if (!base.DesignMode) {
                    this.animationManager_0.StartNewAnimation(AnimationDirection.Out, null);
                }
            }
        }

        private void method_6(bool bool_2, bool bool_3) {
            if (bool_2 || bool_3) {
                if (!this.bool_0) {
                    base.Invalidate();
                } else if (!base.DesignMode) {
                    this.animationManager_1.StartNewAnimation(AnimationDirection.In, null);
                }
            }
            if (!bool_2 && !bool_3) {
                if (!this.bool_0) {
                    base.Invalidate();
                } else if (!base.DesignMode) {
                    this.animationManager_1.StartNewAnimation(AnimationDirection.Out, null);
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e) {
            this.bool_1 = true;
            this.mouseState_0 = MouseState.HOVER;
            base.OnMouseEnter(e);
            this.method_5(this.mouseState_0, this.Owner.mouseState_0);
        }

        protected override void OnMouseLeave(EventArgs e) {
            this.bool_1 = false;
            this.mouseState_0 = MouseState.const_2;
            base.OnMouseLeave(e);
            this.method_5(this.mouseState_0, this.Owner.mouseState_0);
        }

        protected override void OnLostFocus(EventArgs e) {
            this.bool_1 = false;
            this.mouseState_0 = MouseState.const_2;
            base.OnLostFocus(e);
            this.method_6(base.Focused, this.Owner.Focused);
        }

        protected override void OnGotFocus(EventArgs e) {
            this.Owner.Focus();
            this.method_5(this.mouseState_0, this.Owner.mouseState_0);
            base.OnGotFocus(e);
            this.method_6(base.Focused, this.Owner.Focused);
        }

        public void AppendText(string text) {
            this.Owner.AppendText(text);
        }

        public void Clear() {
            this.Owner.Clear();
        }

        public void ClearUndo() {
            this.Owner.ClearUndo();
        }

        public void Cut() {
            this.Owner.Cut();
        }

        public void DeselectAll() {
            this.Owner.DeselectAll();
        }

        public void SelectAll() {
            this.Owner.SelectAll();
        }

        public void Paste() {
            this.Owner.Paste();
        }

        public new void Focus() {
            this.Owner.Focus();
        }

        public new void Select() {
            this.Owner.Select();
        }

        public override void ResetText() {
            this.Owner.ResetText();
        }

        public void ScrollToCaret() {
            this.Owner.ScrollToCaret();
        }

        public void Select(int start, int length) {
            this.Owner.Select(start, length);
        }

        public override string ToString() {
            return this.Owner.ToString();
        }

        public void Undo() {
            this.Owner.Undo();
        }

        [Category("Behavior")]
        [Description("Gets or sets a value indicating whether pressing ENTER in a multiline TextBox control creates a new line of text in the control or activates the default button for the form.")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool AcceptsReturn {
            get {
                return this.Owner.AcceptsReturn;
            }
            set {
                this.Owner.AcceptsReturn = value;
            }
        }

        [Category("Behavior")]
        [Description("Gets or sets a value indicating whether pressing the TAB key in a multiline text box control types a TAB character in the control instead of moving the focus to the next control in the tab order.")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool AcceptsTab {
            get {
                return this.Owner.AcceptsTab;
            }
            set {
                this.Owner.AcceptsTab = value;
            }
        }

        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Gets or sets a custom System.Collections.Specialized.StringCollection to use when the AutoCompleteSource property is set to CustomSource.")]
        public AutoCompleteStringCollection AutoCompleteCustomSource {
            get {
                return this.Owner.AutoCompleteCustomSource;
            }
            set {
                this.Owner.AutoCompleteCustomSource = value;
            }
        }

        [Description("Gets or sets an option that controls how automatic completion works for the TextBox.")]
        [Browsable(true)]
        [DefaultValue(AutoCompleteMode.None)]
        public AutoCompleteMode AutoCompleteMode {
            get {
                return this.Owner.AutoCompleteMode;
            }
            set {
                this.Owner.AutoCompleteMode = value;
            }
        }

        [Browsable(true)]
        [Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
        [DefaultValue(AutoCompleteSource.None)]
        public AutoCompleteSource AutoCompleteSource {
            get {
                return this.Owner.AutoCompleteSource;
            }
            set {
                this.Owner.AutoCompleteSource = value;
            }
        }

        [Browsable(false)]
        [Description("Gets a value indicating whether the user can undo the previous operation in a text box control.")]
        public bool CanUndo {
            get {
                return this.Owner.CanUndo;
            }
        }

        [Category("Options")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "125, 137, 149")]
        [Description("Sets the TextBox's foreground color.")]
        public new Color ForeColor {
            get {
                return base.ForeColor;
            }
            set {
                base.ForeColor = value;
                this.method_9(this.Owner.Boolean_0);
            }
        }

        [DefaultValue(CharacterCasing.Normal)]
        [Category("Behavior")]
        [Description("Gets or sets whether the TextBox control modifies the case of characters as they are typed.")]
        [Browsable(true)]
        public CharacterCasing CharacterCasing {
            get {
                return this.Owner.CharacterCasing;
            }
            set {
                this.Owner.CharacterCasing = value;
            }
        }

        [Description("Gets or sets the font of the text displayed by the control.")]
        public new Font Font {
            get {
                return this.Owner.Font;
            }
            set {
                this.Owner.Font = value;
                this.UpdateBaseTextBoxPosition();
            }
        }

        [Description("Gets a value indicating whether the control has input focus")]
        public new bool Focused {
            get {
                return this.Owner.Focused | base.Focused;
            }
        }

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.")]
        [Browsable(true)]
        public bool HideSelection {
            get {
                return this.Owner.HideSelection;
            }
            set {
                this.Owner.HideSelection = value;
            }
        }

        [Category("Appearance")]
        [MergableProperty(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(true)]
        [Localizable(true)]
        [Description("Gets or sets the lines of text in a text box control.")]
        [Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string[] Lines {
            get {
                return this.Owner.Lines;
            }
            set {
                this.Owner.Lines = value;
            }
        }

        [Description("Gets or sets the maximum number of characters the user can type or paste into the text box control.")]
        [Category("Behavior")]
        [Browsable(true)]
        [DefaultValue(32767)]
        public int MaxLength {
            get {
                return this.Owner.MaxLength;
            }
            set {
                this.Owner.MaxLength = value;
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        [Browsable(true)]
        [Description("Gets or sets a value that indicates that the text box control has been modified by the user since the control was created or its contents were last set.")]
        public bool Modified {
            get {
                return this.Owner.Modified;
            }
            set {
                this.Owner.Modified = value;
            }
        }

        [Description("Gets or sets a value indicating whether this is a multiline TextBox control.")]
        [DefaultValue(false)]
        [Browsable(true)]
        [Category("Behavior")]
        public bool Multiline {
            get {
                return this.Owner.Multiline;
            }
            set {
                this.Owner.Multiline = value;
                this.method_7();
            }
        }

        [Description("Gets or sets the character used to mask characters of a password in a single-line TextBox control.")]
        [Category("Behavior")]
        [Browsable(true)]
        public char PasswordChar {
            get {
                return this.Owner.Char_0;
            }
            set {
                this.Owner.Char_0 = value;
            }
        }

        [Browsable(false)]
        [Description("Gets the preferred height for a text box.")]
        [Category("Behavior")]
        public int PreferredHeight {
            get {
                return this.Owner.PreferredHeight;
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Gets or sets a value indicating whether text in the text box is read-only.")]
        [Browsable(true)]
        public bool ReadOnly {
            get {
                return this.Owner.ReadOnly;
            }
            set {
                this.Owner.ReadOnly = value;
            }
        }

        [Browsable(true)]
        [Description("Gets or sets a value indicating the currently selected text in the control.")]
        [Category("Behavior")]
        public string SelectedText {
            get {
                return this.Owner.SelectedText;
            }
            set {
                this.Owner.SelectedText = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(0)]
        [Category("Behavior")]
        [Description("Gets or sets the number of characters selected in the text box.")]
        public int SelectionLength {
            get {
                return this.Owner.SelectionLength;
            }
            set {
                this.Owner.SelectionLength = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(0)]
        [Description("Gets or sets the starting point of text selected in the text box.")]
        [Category("Behavior")]
        public int SelectionStart {
            get {
                return this.Owner.SelectionStart;
            }
            set {
                this.Owner.SelectionStart = value;
            }
        }

        [DefaultValue(true)]
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Gets or sets a value indicating whether the defined shortcuts are enabled.")]
        public bool ShortcutsEnabled {
            get {
                return this.Owner.ShortcutsEnabled;
            }
            set {
                this.Owner.ShortcutsEnabled = value;
            }
        }

        [Browsable(true)]
        [Description("Gets or sets how text is aligned in a TextBox control.")]
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign {
            get {
                return this.Owner.TextAlign;
            }
            set {
                this.Owner.TextAlign = value;
            }
        }

        [Category("Behavior")]
        [Description("Gets the length of text in the control.")]
        [Browsable(false)]
        public int TextLength {
            get {
                return this.Owner.TextLength;
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        [Browsable(true)]
        [Description("Gets or sets a value indicating whether the text in the TextBox control should appear as the default password character.")]
        public bool UseSystemPasswordChar {
            get {
                return this.Owner.UseSystemPasswordChar;
            }
            set {
                this.Owner.UseSystemPasswordChar = value;
            }
        }

        [DefaultValue(true)]
        [Description("Indicates whether a multiline text box control automatically wraps words to the beginning of the next line when necessary.")]
        [Browsable(true)]
        [Category("Behavior")]
        public bool WordWrap {
            get {
                return this.Owner.WordWrap;
            }
            set {
                this.Owner.WordWrap = value;
            }
        }

        protected override void OnResize(EventArgs e) {
            this.method_7();
            base.Invalidate();
            base.OnResize(e);
        }

        protected void UpdateBaseTextBoxPosition() {
            this.method_7();
            base.Invalidate();
        }

        private void method_7() {
            if (this.Owner != null) {
                Padding padding = this.padding_0;
                padding.Left += this.point_0.X;
                padding.Right += this.point_0.X;
                Rectangle clientRectangle = base.ClientRectangle;
                Rectangle clientRectangle2 = this.Owner.ClientRectangle;
                clientRectangle2.X = 0;
                clientRectangle2.Y = 0;
                if (!this.Owner.Multiline) {
                    clientRectangle2.Y = base.Height / 2 - clientRectangle2.Height / 2;
                    clientRectangle2.Y += this.point_0.Y;
                } else {
                    clientRectangle2.Y = padding.Top;
                    clientRectangle2.Y += this.point_0.Y;
                }
                if (this.image_0 != null) {
                    clientRectangle2.X = padding.Left + this.point_1.X + this.size_0.Width;
                    clientRectangle.Width -= clientRectangle2.X + padding.Right;
                } else {
                    clientRectangle2.X = padding.Left;
                    clientRectangle.Width -= padding.Left + padding.Right;
                }
                if (this.image_1 != null) {
                    clientRectangle.Width -= this.size_1.Width + this.point_2.X;
                }
                if (this.Owner.Multiline) {
                    clientRectangle2.Height = clientRectangle.Height - (padding.Bottom + padding.Top);
                    if (this.Owner.Height != clientRectangle2.Width) {
                        this.Owner.Height = clientRectangle2.Height;
                    }
                }
                if (this.Owner.Width != clientRectangle.Width) {
                    this.Owner.Width = clientRectangle.Width;
                }
                if (this.Owner.Left != clientRectangle2.Left) {
                    this.Owner.Left = clientRectangle2.Left;
                }
                if (this.Owner.Top != clientRectangle2.Top) {
                    this.Owner.Top = clientRectangle2.Top;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            this.method_8(e.Graphics);
            base.OnPaint(e);
        }

        private void method_8(Graphics graphics_0) {
            Color color = this.color_1;
            Color color2 = this.ForeColor;
            Color color3 = this.color_2;
            Color color4 = this.color_0;
            int num = 0;
            int num2 = 0;
            Image image = this.image_0;
            Image image2 = this.image_1;
            if (base.Enabled) {
                if (this.bool_0 && !base.DesignMode) {
                    num = (int)(this.animationManager_0.GetProgress() * 100.0);
                    num2 = (int)(this.animationManager_1.GetProgress() * 100.0);
                } else if (this.Owner.Focused | base.Focused) {
                    num2 = 100;
                } else if ((this.mouseState_0 == MouseState.HOVER) | (this.Owner.mouseState_0 == MouseState.HOVER) | (this.mouseState_0 == MouseState.DOWN) | (this.Owner.mouseState_0 == MouseState.DOWN)) {
                    num = 100;
                }
            } else {
                color = smethod_24(100, color, this.DefaultDisabledState.BorderColor);
                color2 = smethod_24(100, this.ForeColor, this.DefaultDisabledState.ForeColor);
                color3 = smethod_24(100, this.color_2, this.DefaultDisabledState.FillColor);
                color4 = smethod_24(100, this.color_0, this.DefaultDisabledState.PlaceholderForeColor);
            }
            color = smethod_24(num, color, this.DefaultHoveredState.BorderColor);
            color2 = smethod_24(num, color2, this.DefaultHoveredState.ForeColor);
            color3 = smethod_24(num, color3, this.DefaultHoveredState.FillColor);
            color4 = smethod_23(num, color4, this.DefaultHoveredState.PlaceholderForeColor);
            if (this.DefaultStyle == TextBoxStyle.Default) {
                color = smethod_24(num2, color, this.DefaultFocusedState.BorderColor);
            }
            color2 = smethod_24(num2, color2, this.DefaultFocusedState.ForeColor);
            color3 = smethod_24(num2, color3, this.DefaultFocusedState.FillColor);
            color4 = smethod_23(num2, color4, this.DefaultFocusedState.PlaceholderForeColor);
            if (this.DefaultStyle == TextBoxStyle.Default) {
                if (this.int_0 > 0) {
                    graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
                    GraphicsPath graphicsPath = smethod_12(smethod_11(base.ClientRectangle), (float)(checked(this.int_0 * 2)));
                    graphics_0.FillPath(new SolidBrush(color3), graphicsPath);
                    graphics_0.SmoothingMode = SmoothingMode.HighQuality;
                    if (this.int_1 > 0) {
                        smethod_20(graphics_0, new SolidBrush(color), base.ClientRectangle, this.int_0, this.int_1, this.dashStyle_0);
                    }
                } else {
                    graphics_0.FillRectangle(new SolidBrush(color3), base.ClientRectangle);
                    smethod_22(graphics_0, new SolidBrush(color), base.ClientRectangle, this.int_1, this.dashStyle_0);
                }
            } else {
                int num3 = this.int_1;
                if (num3 < 1) {
                    num3 = 1;
                }
                num3 = ((this.Owner.Focused || this.Focused) ? (num3 + 1) : num3);
                graphics_0.FillRectangle(new SolidBrush(color3), base.ClientRectangle);
                Color color5 = ((this.DefaultFocusedState.BorderColor == Color.Empty) ? ((this.DefaultHoveredState.BorderColor == Color.Empty) ? this.color_1 : this.DefaultHoveredState.BorderColor) : this.DefaultFocusedState.BorderColor);
                if (!this.animationManager_1.IsAnimating()) {
                    graphics_0.FillRectangle(this.Owner.Focused ? new SolidBrush(color5) : new SolidBrush(color), 0, base.Height - num3, base.Width, num3);
                } else {
                    int num4 = (int)((double)base.Width * this.animationManager_1.GetProgress());
                    int num5 = num4 / 2;
                    int num6 = base.Width / 2;
                    int num7 = base.Height - this.int_1;
                    graphics_0.FillRectangle(new SolidBrush(color), 0, num7, base.Width, this.int_1);
                    num7 = base.Height - num3;
                    graphics_0.FillRectangle(new SolidBrush(color5), num6 - num5, num7, num4, num3);
                }
            }
            if (image != null) {
                graphics_0.DrawImage(image, new Rectangle(5 + this.point_1.X, base.Height / 2 - this.size_0.Height / 2 + this.point_1.Y, this.size_0.Width, this.size_0.Height));
            }
            if (image2 != null) {
                graphics_0.DrawImage(image2, new Rectangle(base.Width - (this.size_1.Width + 5 + this.point_2.X), base.Height / 2 - this.size_0.Height / 2 + this.point_2.Y, this.size_1.Width, this.size_1.Height));
            }
            if (this.Owner.BackColor != color3) {
                this.Owner.BackColor = color3;
            }
            if (this.Owner.Boolean_0) {
                if (this.Owner.ForeColor != this.color_0) {
                    this.Owner.ForeColor = this.color_0;
                }
            } else if (this.Owner.ForeColor != color2) {
                this.Owner.ForeColor = color2;
            }
        }

        internal static void smethod_20(Graphics graphics_0, Brush brush_0, Rectangle rectangle_1, int int_0, int int_1, DashStyle dashStyle_0 = DashStyle.Solid) {
            if (int_1 >= 1) {
                GraphicsPath graphicsPath = smethod_15(rectangle_1, (float)int_0, (float)int_1);
                using (Pen pen = new Pen(brush_0, (float)int_1)) {
                    pen.DashStyle = dashStyle_0;
                    graphics_0.DrawPath(pen, graphicsPath);
                }
            }
        }

        internal static GraphicsPath smethod_15(Rectangle rectangle_1, float float_0, float float_1) {
            RectangleF rectangleF = new RectangleF((float)rectangle_1.X, (float)rectangle_1.Y, (float)rectangle_1.Width, (float)rectangle_1.Height);
            rectangleF.Width -= 1f;
            rectangleF.Height -= 1f;
            GraphicsPath graphicsPath;
            if (float_1 != 1f) {
                float num = float_1 / 2f;
                rectangleF.X += num;
                rectangleF.Y += num;
                rectangleF.Width -= float_1;
                rectangleF.Height -= float_1;
                graphicsPath = smethod_12(rectangleF, float_0 * 2f - 1f);
            } else {
                graphicsPath = smethod_12(rectangleF, float_0 * 2f);
            }
            return graphicsPath;
        }

        internal static RectangleF smethod_11(RectangleF rectangleF_0) {
            return new RectangleF(rectangleF_0.X, rectangleF_0.Y, rectangleF_0.Width - 1f, rectangleF_0.Height - 1f);
        }

        internal static GraphicsPath smethod_12(RectangleF rectangleF_0, float float_0) {
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

        internal static void smethod_22(Graphics graphics_0, Brush brush_0, Rectangle rectangle_1, int int_0, DashStyle dashStyle_0 = DashStyle.Solid) {
            if (int_0 >= 1) {
                using (Pen pen = new Pen(brush_0, (float)int_0)) {
                    pen.DashStyle = dashStyle_0;
                    GraphicsPath graphicsPath = new GraphicsPath();
                    RectangleF rectangleF = new RectangleF((float)rectangle_1.X, (float)rectangle_1.Y, (float)rectangle_1.Width, (float)rectangle_1.Height);
                    float num = (float)int_0 / 2f;
                    if (int_0 > 1) {
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
        internal static Color smethod_23(int int_0, Color color_1, Color color_2) {
            Color color = color_1;
            Color color2 = color_2;
            if (int_0 < 100) {
                if (color == Color.Transparent) {
                    color = Color.Empty;
                }
                if (color2 == Color.Transparent) {
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

        internal static Color smethod_24(int int_0, Color color_1, Color color_2) {
            Color color = color_1;
            Color color2 = color_2;
            if ((color == Color.Transparent) | (color == Color.Empty)) {
                color = Color.Black;
            }
            if ((color2 == Color.Transparent) | (color2 == Color.Empty)) {
                color2 = color;
            }
            Color color3;
            if (color == color2) {
                color3 = color;
            } else {
                int r = (int)color.R;
                int g = (int)color.G;
                int b = (int)color.B;
                int r2 = (int)color2.R;
                int g2 = (int)color2.G;
                int b2 = (int)color2.B;
                double num = Math.Round((double)r + (double)(checked((r2 - r) * int_0)) * 0.01, 0);
                double num2 = Math.Round((double)g + (double)(checked((g2 - g) * int_0)) * 0.01, 0);
                double num3 = Math.Round((double)b + (double)(checked((b2 - b) * int_0)) * 0.01, 0);
                color3 = Color.FromArgb(255, (int)num, (int)num2, (int)num3);
            }
            return color3;
        }

        private void method_9(bool bool_2) {
            if (bool_2) {
                this.Owner.ForeColor = this.color_0;
            }
        }

        private void Owner_TextChanged(object sender, EventArgs e) {
            base.Invalidate();
            this.OnTextChanged(e);
        }

        private void Owner_KeyDown(object sender, KeyEventArgs e) {
            base.OnKeyDown(e);
        }

        private void Owner_KeyPress(object sender, KeyPressEventArgs e) {
            base.OnKeyPress(e);
        }

        private void Owner_KeyUp(object sender, KeyEventArgs e) {
            base.OnKeyUp(e);
        }

        private void Owner_MouseLeave(object sender, EventArgs e) {
            this.method_5(this.mouseState_0, this.Owner.mouseState_0);
        }

        private void Owner_MouseEnter(object sender, EventArgs e) {
            this.method_5(this.mouseState_0, this.Owner.mouseState_0);
        }

        private void Owner_MouseDown(object sender, MouseEventArgs e) {
        }

        private void Owner_MouseUp(object sender, MouseEventArgs e) {
            this.method_5(this.mouseState_0, this.Owner.mouseState_0);
        }

        private void method_10(bool bool_2) {
            this.method_6(base.Focused, bool_2);
        }

        private IContainer icontainer_0 = null;

        private Class8 Owner;

        private AnimationManager animationManager_0;

        private AnimationManager animationManager_1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        private EventHandler eventHandler_0;

        [CompilerGenerated]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler eventHandler_1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [CompilerGenerated]
        private EventHandler eventHandler_2;

        private TextBoxState textBoxState_0;

        private TextBoxState textBoxState_1;

        private TextBoxState textBoxState_2;

        private bool bool_0 = true;

        private TextBoxStyle textBoxStyle_0 = TextBoxStyle.Default;

        private Point point_0;

        private Color color_0 = Color.FromArgb(193, 200, 207);

        private int int_0 = 0;

        private DashStyle dashStyle_0 = DashStyle.Solid;

        private Color color_1 = Color.FromArgb(213, 218, 223);

        private int int_1 = 1;

        private ShadowDecoration shadowDecoration_0;

        private Image image_0;

        private Size size_0 = new Size(20, 20);

        private Cursor cursor_0 = Cursors.Default;

        private Point point_1;

        private Image image_1;

        private Size size_1 = new Size(20, 20);

        private Cursor cursor_1 = Cursors.Default;

        private Point point_2;

        private Color color_2 = Color.White;

        private Point point_3;

        private bool bool_1 = false;

        internal MouseState mouseState_0 = MouseState.const_2;

        private Padding padding_0 = new Padding(9, 9, 9, 9);
    }

    public enum TextBoxStyle {

        Default,

        Material
    }
}