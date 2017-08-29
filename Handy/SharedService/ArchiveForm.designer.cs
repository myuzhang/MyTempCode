namespace SharedService
{
    partial class ArchiveForm
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
            this.Infomation = new System.Windows.Forms.Label();
            this.Archive = new System.Windows.Forms.Button();
            this.Replace = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Infomation
            // 
            this.Infomation.AutoSize = true;
            this.Infomation.Location = new System.Drawing.Point(23, 35);
            this.Infomation.Name = "Infomation";
            this.Infomation.Size = new System.Drawing.Size(56, 13);
            this.Infomation.TabIndex = 0;
            this.Infomation.Text = "Infomation";
            // 
            // Archive
            // 
            this.Archive.Location = new System.Drawing.Point(26, 140);
            this.Archive.Name = "Archive";
            this.Archive.Size = new System.Drawing.Size(75, 23);
            this.Archive.TabIndex = 1;
            this.Archive.Text = "Archive";
            this.Archive.UseVisualStyleBackColor = true;
            this.Archive.Click += new System.EventHandler(this.Archive_Click);
            // 
            // Replace
            // 
            this.Replace.Location = new System.Drawing.Point(144, 140);
            this.Replace.Name = "Replace";
            this.Replace.Size = new System.Drawing.Size(75, 23);
            this.Replace.TabIndex = 2;
            this.Replace.Text = "Replace";
            this.Replace.UseVisualStyleBackColor = true;
            this.Replace.Click += new System.EventHandler(this.Replace_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(263, 140);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ArchiveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 191);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Replace);
            this.Controls.Add(this.Archive);
            this.Controls.Add(this.Infomation);
            this.Name = "ArchiveForm";
            this.Text = "ArchiveForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Infomation;
        private System.Windows.Forms.Button Archive;
        private System.Windows.Forms.Button Replace;
        private System.Windows.Forms.Button Cancel;
    }
}