namespace KesselRun
{
    partial class GameBoardControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gameStatusStrip = new System.Windows.Forms.StatusStrip();
            this.gameStatusStripLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.gameStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameStatusStrip
            // 
            this.gameStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameStatusStripLabel});
            this.gameStatusStrip.Location = new System.Drawing.Point(0, 438);
            this.gameStatusStrip.Name = "gameStatusStrip";
            this.gameStatusStrip.Size = new System.Drawing.Size(614, 22);
            this.gameStatusStrip.SizingGrip = false;
            this.gameStatusStrip.TabIndex = 0;
            this.gameStatusStrip.Text = "placeholder";
            // 
            // gameStatusStripLabel
            // 
            this.gameStatusStripLabel.BackColor = System.Drawing.Color.Transparent;
            this.gameStatusStripLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.gameStatusStripLabel.Name = "gameStatusStripLabel";
            this.gameStatusStripLabel.Size = new System.Drawing.Size(69, 17);
            this.gameStatusStripLabel.Text = "placeholder";
            // 
            // GameBoardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.gameStatusStrip);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "GameBoardControl";
            this.Size = new System.Drawing.Size(614, 460);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GameBoardControl_KeyPress);
            this.gameStatusStrip.ResumeLayout(false);
            this.gameStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip gameStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel gameStatusStripLabel;
    }
}
