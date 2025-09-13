namespace BPJSScrapper
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
            this.label1 = new System.Windows.Forms.Label();
            this.btn_bpjs = new System.Windows.Forms.Button();
            this.btn_lasik = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "BPJS LASIK Scrapper";
            // 
            // btn_bpjs
            // 
            this.btn_bpjs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_bpjs.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_bpjs.Location = new System.Drawing.Point(6, 59);
            this.btn_bpjs.Name = "btn_bpjs";
            this.btn_bpjs.Size = new System.Drawing.Size(238, 46);
            this.btn_bpjs.TabIndex = 1;
            this.btn_bpjs.Text = "SIIP BPJS";
            this.btn_bpjs.UseVisualStyleBackColor = true;
            this.btn_bpjs.Click += new System.EventHandler(this.btn_bpjs_Click);
            // 
            // btn_lasik
            // 
            this.btn_lasik.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_lasik.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_lasik.Location = new System.Drawing.Point(6, 111);
            this.btn_lasik.Name = "btn_lasik";
            this.btn_lasik.Size = new System.Drawing.Size(238, 46);
            this.btn_lasik.TabIndex = 2;
            this.btn_lasik.Text = "LASIK";
            this.btn_lasik.UseVisualStyleBackColor = true;
            this.btn_lasik.Click += new System.EventHandler(this.btn_lasik_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 178);
            this.Controls.Add(this.btn_lasik);
            this.Controls.Add(this.btn_bpjs);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "BPJS LASIK Scrapper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_bpjs;
        private System.Windows.Forms.Button btn_lasik;
    }
}

