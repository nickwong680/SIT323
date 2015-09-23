namespace Class_10
{
    partial class Form1
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
            this.shallowCopyButton = new System.Windows.Forms.Button();
            this.deepCopyButton = new System.Windows.Forms.Button();
            this.parametersButton = new System.Windows.Forms.Button();
            this.parametersShallowCopyButton = new System.Windows.Forms.Button();
            this.parametersDeepCopyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // shallowCopyButton
            // 
            this.shallowCopyButton.Location = new System.Drawing.Point(12, 12);
            this.shallowCopyButton.Name = "shallowCopyButton";
            this.shallowCopyButton.Size = new System.Drawing.Size(199, 29);
            this.shallowCopyButton.TabIndex = 0;
            this.shallowCopyButton.Text = "Shallow Copy";
            this.shallowCopyButton.UseVisualStyleBackColor = true;
            this.shallowCopyButton.Click += new System.EventHandler(this.shallowCopyButton_Click);
            // 
            // deepCopyButton
            // 
            this.deepCopyButton.Location = new System.Drawing.Point(12, 47);
            this.deepCopyButton.Name = "deepCopyButton";
            this.deepCopyButton.Size = new System.Drawing.Size(199, 29);
            this.deepCopyButton.TabIndex = 1;
            this.deepCopyButton.Text = "Deep Copy";
            this.deepCopyButton.UseVisualStyleBackColor = true;
            this.deepCopyButton.Click += new System.EventHandler(this.deepCopyButton_Click);
            // 
            // parametersButton
            // 
            this.parametersButton.Location = new System.Drawing.Point(12, 82);
            this.parametersButton.Name = "parametersButton";
            this.parametersButton.Size = new System.Drawing.Size(199, 29);
            this.parametersButton.TabIndex = 2;
            this.parametersButton.Text = "Reference Parameters";
            this.parametersButton.UseVisualStyleBackColor = true;
            this.parametersButton.Click += new System.EventHandler(this.parametersButton_Click);
            // 
            // parametersShallowCopyButton
            // 
            this.parametersShallowCopyButton.Location = new System.Drawing.Point(12, 117);
            this.parametersShallowCopyButton.Name = "parametersShallowCopyButton";
            this.parametersShallowCopyButton.Size = new System.Drawing.Size(199, 29);
            this.parametersShallowCopyButton.TabIndex = 3;
            this.parametersShallowCopyButton.Text = "Reference Parameters - Shallow Copy";
            this.parametersShallowCopyButton.UseVisualStyleBackColor = true;
            this.parametersShallowCopyButton.Click += new System.EventHandler(this.parametersShallowCopyButton_Click);
            // 
            // parametersDeepCopyButton
            // 
            this.parametersDeepCopyButton.Location = new System.Drawing.Point(12, 152);
            this.parametersDeepCopyButton.Name = "parametersDeepCopyButton";
            this.parametersDeepCopyButton.Size = new System.Drawing.Size(199, 29);
            this.parametersDeepCopyButton.TabIndex = 4;
            this.parametersDeepCopyButton.Text = "Reference Parameters - Deep Copy";
            this.parametersDeepCopyButton.UseVisualStyleBackColor = true;
            this.parametersDeepCopyButton.Click += new System.EventHandler(this.parametersDeepCopyButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 262);
            this.Controls.Add(this.parametersDeepCopyButton);
            this.Controls.Add(this.parametersShallowCopyButton);
            this.Controls.Add(this.parametersButton);
            this.Controls.Add(this.deepCopyButton);
            this.Controls.Add(this.shallowCopyButton);
            this.Name = "Form1";
            this.Text = "Class 10 Code";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button shallowCopyButton;
        private System.Windows.Forms.Button deepCopyButton;
        private System.Windows.Forms.Button parametersButton;
        private System.Windows.Forms.Button parametersShallowCopyButton;
        private System.Windows.Forms.Button parametersDeepCopyButton;
    }
}

