namespace SpotifyController
{
    partial class FormMain
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
            this.labelDetected = new System.Windows.Forms.Label();
            this.labelSong = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelDetected
            // 
            this.labelDetected.AutoSize = true;
            this.labelDetected.Location = new System.Drawing.Point(12, 11);
            this.labelDetected.Name = "labelDetected";
            this.labelDetected.Size = new System.Drawing.Size(67, 13);
            this.labelDetected.TabIndex = 1;
            this.labelDetected.Text = "Connecting..";
            // 
            // labelSong
            // 
            this.labelSong.AutoSize = true;
            this.labelSong.Location = new System.Drawing.Point(13, 28);
            this.labelSong.Name = "labelSong";
            this.labelSong.Size = new System.Drawing.Size(67, 13);
            this.labelSong.TabIndex = 2;
            this.labelSong.Text = "Connecting..";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 53);
            this.Controls.Add(this.labelSong);
            this.Controls.Add(this.labelDetected);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SpotifyController";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelDetected;
        private System.Windows.Forms.Label labelSong;
    }
}

