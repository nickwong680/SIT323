namespace Monday29
{
    partial class assertionsForm
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
            this.anyButtonEG = new System.Windows.Forms.Button();
            this.customEG = new System.Windows.Forms.Button();
            this.okEG = new System.Windows.Forms.Button();
            this.simpleEG = new System.Windows.Forms.Button();
            this.debugEG = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // anyButtonEG
            // 
            this.anyButtonEG.Location = new System.Drawing.Point(12, 192);
            this.anyButtonEG.Name = "anyButtonEG";
            this.anyButtonEG.Size = new System.Drawing.Size(154, 39);
            this.anyButtonEG.TabIndex = 9;
            this.anyButtonEG.Text = "Responding to any MessageBox button";
            this.anyButtonEG.UseVisualStyleBackColor = true;
            this.anyButtonEG.Click += new System.EventHandler(this.anyButtonEG_Click);
            // 
            // customEG
            // 
            this.customEG.Location = new System.Drawing.Point(12, 147);
            this.customEG.Name = "customEG";
            this.customEG.Size = new System.Drawing.Size(154, 39);
            this.customEG.TabIndex = 8;
            this.customEG.Text = "Customising a MessageBox";
            this.customEG.UseVisualStyleBackColor = true;
            this.customEG.Click += new System.EventHandler(this.customEG_Click);
            // 
            // okEG
            // 
            this.okEG.Location = new System.Drawing.Point(12, 102);
            this.okEG.Name = "okEG";
            this.okEG.Size = new System.Drawing.Size(154, 39);
            this.okEG.TabIndex = 7;
            this.okEG.Text = "Responding to clicking OK";
            this.okEG.UseVisualStyleBackColor = true;
            this.okEG.Click += new System.EventHandler(this.okEG_Click);
            // 
            // simpleEG
            // 
            this.simpleEG.Location = new System.Drawing.Point(12, 57);
            this.simpleEG.Name = "simpleEG";
            this.simpleEG.Size = new System.Drawing.Size(154, 39);
            this.simpleEG.TabIndex = 6;
            this.simpleEG.Text = "Simple MessageBox";
            this.simpleEG.UseVisualStyleBackColor = true;
            this.simpleEG.Click += new System.EventHandler(this.simpleEG_Click);
            // 
            // debugEG
            // 
            this.debugEG.Location = new System.Drawing.Point(12, 12);
            this.debugEG.Name = "debugEG";
            this.debugEG.Size = new System.Drawing.Size(154, 39);
            this.debugEG.TabIndex = 5;
            this.debugEG.Text = "Debug";
            this.debugEG.UseVisualStyleBackColor = true;
            this.debugEG.Click += new System.EventHandler(this.debugEG_Click);
            // 
            // assertionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 262);
            this.Controls.Add(this.anyButtonEG);
            this.Controls.Add(this.customEG);
            this.Controls.Add(this.okEG);
            this.Controls.Add(this.simpleEG);
            this.Controls.Add(this.debugEG);
            this.Name = "assertionsForm";
            this.Text = "Class 9 - Assertions";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button anyButtonEG;
        private System.Windows.Forms.Button customEG;
        private System.Windows.Forms.Button okEG;
        private System.Windows.Forms.Button simpleEG;
        private System.Windows.Forms.Button debugEG;
    }
}

