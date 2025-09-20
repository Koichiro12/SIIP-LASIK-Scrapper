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
            this.btn_bpjs = new System.Windows.Forms.Button();
            this.btn_lasik = new System.Windows.Forms.Button();
            this.btn_dpt = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_register_user = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_bpjs
            // 
            this.btn_bpjs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_bpjs.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_bpjs.Location = new System.Drawing.Point(12, 102);
            this.btn_bpjs.Name = "btn_bpjs";
            this.btn_bpjs.Size = new System.Drawing.Size(350, 46);
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
            this.btn_lasik.Location = new System.Drawing.Point(12, 154);
            this.btn_lasik.Name = "btn_lasik";
            this.btn_lasik.Size = new System.Drawing.Size(350, 46);
            this.btn_lasik.TabIndex = 2;
            this.btn_lasik.Text = "LASIK";
            this.btn_lasik.UseVisualStyleBackColor = true;
            this.btn_lasik.Click += new System.EventHandler(this.btn_lasik_Click);
            // 
            // btn_dpt
            // 
            this.btn_dpt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_dpt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_dpt.Location = new System.Drawing.Point(12, 206);
            this.btn_dpt.Name = "btn_dpt";
            this.btn_dpt.Size = new System.Drawing.Size(350, 46);
            this.btn_dpt.TabIndex = 3;
            this.btn_dpt.Text = "CEK DPT";
            this.btn_dpt.UseVisualStyleBackColor = true;
            this.btn_dpt.Click += new System.EventHandler(this.btn_dpt_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 14);
            this.label2.TabIndex = 6;
            this.label2.Text = "SIIP BPJS , LASIK Dan DPT Online";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 45);
            this.label1.TabIndex = 5;
            this.label1.Text = "Auto Checker";
            // 
            // btn_register_user
            // 
            this.btn_register_user.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_register_user.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_register_user.Location = new System.Drawing.Point(13, 258);
            this.btn_register_user.Name = "btn_register_user";
            this.btn_register_user.Size = new System.Drawing.Size(350, 46);
            this.btn_register_user.TabIndex = 7;
            this.btn_register_user.Text = "REGISTER USER";
            this.btn_register_user.UseVisualStyleBackColor = true;
            this.btn_register_user.Click += new System.EventHandler(this.btn_register_user_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 322);
            this.Controls.Add(this.btn_register_user);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_dpt);
            this.Controls.Add(this.btn_lasik);
            this.Controls.Add(this.btn_bpjs);
            this.Name = "Form1";
            this.Text = "BPJS LASIK Scrapper";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_bpjs;
        private System.Windows.Forms.Button btn_lasik;
        private System.Windows.Forms.Button btn_dpt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_register_user;
    }
}

