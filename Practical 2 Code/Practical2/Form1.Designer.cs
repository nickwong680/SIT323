namespace Practical2
{
    partial class Practical2Week3
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
            this.readCharacters = new System.Windows.Forms.Button();
            this.validate = new System.Windows.Forms.Button();
            this.readLines = new System.Windows.Forms.Button();
            this.readValues = new System.Windows.Forms.Button();
            this.obtainFilename = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // readCharacters
            // 
            this.readCharacters.Location = new System.Drawing.Point(12, 58);
            this.readCharacters.Name = "readCharacters";
            this.readCharacters.Size = new System.Drawing.Size(138, 40);
            this.readCharacters.TabIndex = 0;
            this.readCharacters.Text = "read characters";
            this.readCharacters.UseVisualStyleBackColor = true;
            this.readCharacters.Click += new System.EventHandler(this.readCharacters_Click);
            // 
            // validate
            // 
            this.validate.Location = new System.Drawing.Point(12, 196);
            this.validate.Name = "validate";
            this.validate.Size = new System.Drawing.Size(138, 40);
            this.validate.TabIndex = 1;
            this.validate.Text = "validate";
            this.validate.UseVisualStyleBackColor = true;
            this.validate.Click += new System.EventHandler(this.validate_Click);
            // 
            // readLines
            // 
            this.readLines.Location = new System.Drawing.Point(12, 104);
            this.readLines.Name = "readLines";
            this.readLines.Size = new System.Drawing.Size(138, 40);
            this.readLines.TabIndex = 2;
            this.readLines.Text = "read lines";
            this.readLines.UseVisualStyleBackColor = true;
            this.readLines.Click += new System.EventHandler(this.readLines_Click);
            // 
            // readValues
            // 
            this.readValues.Location = new System.Drawing.Point(12, 150);
            this.readValues.Name = "readValues";
            this.readValues.Size = new System.Drawing.Size(138, 40);
            this.readValues.TabIndex = 3;
            this.readValues.Text = "read values";
            this.readValues.UseVisualStyleBackColor = true;
            this.readValues.Click += new System.EventHandler(this.readValues_Click);
            // 
            // obtainFilename
            // 
            this.obtainFilename.Location = new System.Drawing.Point(12, 12);
            this.obtainFilename.Name = "obtainFilename";
            this.obtainFilename.Size = new System.Drawing.Size(138, 40);
            this.obtainFilename.TabIndex = 4;
            this.obtainFilename.Text = "obtain filename";
            this.obtainFilename.UseVisualStyleBackColor = true;
            this.obtainFilename.Click += new System.EventHandler(this.obtainFilename_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Practical2Week3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(166, 254);
            this.Controls.Add(this.obtainFilename);
            this.Controls.Add(this.readValues);
            this.Controls.Add(this.readLines);
            this.Controls.Add(this.validate);
            this.Controls.Add(this.readCharacters);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Practical2Week3";
            this.Text = "Practical 2 - Week 3";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button readCharacters;
        private System.Windows.Forms.Button validate;
        private System.Windows.Forms.Button readLines;
        private System.Windows.Forms.Button readValues;
        private System.Windows.Forms.Button obtainFilename;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

