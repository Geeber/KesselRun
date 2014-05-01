namespace KesselRun
{
    partial class RulesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RulesTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // RulesTextBox
            // 
            this.RulesTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.RulesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RulesTextBox.Location = new System.Drawing.Point(2, 2);
            this.RulesTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.RulesTextBox.Name = "RulesTextBox";
            this.RulesTextBox.ReadOnly = true;
            this.RulesTextBox.Size = new System.Drawing.Size(538, 492);
            this.RulesTextBox.TabIndex = 0;
            this.RulesTextBox.Text = "";
            // 
            // RulesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 496);
            this.Controls.Add(this.RulesTextBox);
            this.Name = "RulesForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Text = "Kessel Run Rules";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox RulesTextBox;
    }
}