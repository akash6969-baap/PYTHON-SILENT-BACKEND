namespace AotForms
{
    partial class ServerADB
    {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            SuspendLayout();

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(10, 10);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ServerADB";
            Text = "ServerADB";
            Load += ServerADB_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}