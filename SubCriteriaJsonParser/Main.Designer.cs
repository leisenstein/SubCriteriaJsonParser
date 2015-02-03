namespace SubCriteriaJsonParser
{
    partial class Main
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
            this.textInput = new System.Windows.Forms.TextBox();
            this.textLog = new System.Windows.Forms.TextBox();
            this.lblInput = new System.Windows.Forms.Label();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnParse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textInput
            // 
            this.textInput.Location = new System.Drawing.Point(12, 54);
            this.textInput.Multiline = true;
            this.textInput.Name = "textInput";
            this.textInput.Size = new System.Drawing.Size(490, 264);
            this.textInput.TabIndex = 0;
            // 
            // textLog
            // 
            this.textLog.Location = new System.Drawing.Point(12, 398);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.Size = new System.Drawing.Size(490, 264);
            this.textLog.TabIndex = 1;
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(13, 35);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(31, 13);
            this.lblInput.TabIndex = 2;
            this.lblInput.Text = "Input";
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(16, 379);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(45, 13);
            this.lblLog.TabIndex = 3;
            this.lblLog.Text = "Logging";
            // 
            // btnParse
            // 
            this.btnParse.Location = new System.Drawing.Point(529, 54);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(102, 40);
            this.btnParse.TabIndex = 4;
            this.btnParse.Text = "Parse";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 674);
            this.Controls.Add(this.btnParse);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.textInput);
            this.Name = "Main";
            this.Text = "SubCriteria Parser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textInput;
        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnParse;
    }
}

