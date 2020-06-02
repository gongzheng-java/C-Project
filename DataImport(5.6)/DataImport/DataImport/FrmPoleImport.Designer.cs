namespace DataImport
{
    partial class FrmPoleImport
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtXLBH = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboxYWBM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboxYWDW = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboxXLMC = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbtnMulti = new System.Windows.Forms.RadioButton();
            this.rbtnSingle = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cboxDH = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboxDH);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtXLBH);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboxYWBM);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboxYWDW);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboxXLMC);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(768, 144);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "塔杆基本信息";
            // 
            // txtXLBH
            // 
            this.txtXLBH.Location = new System.Drawing.Point(500, 39);
            this.txtXLBH.Name = "txtXLBH";
            this.txtXLBH.ReadOnly = true;
            this.txtXLBH.Size = new System.Drawing.Size(84, 28);
            this.txtXLBH.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(414, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 18);
            this.label5.TabIndex = 6;
            this.label5.Text = "线路编号";
            // 
            // cboxYWBM
            // 
            this.cboxYWBM.FormattingEnabled = true;
            this.cboxYWBM.Location = new System.Drawing.Point(500, 97);
            this.cboxYWBM.Name = "cboxYWBM";
            this.cboxYWBM.Size = new System.Drawing.Size(236, 26);
            this.cboxYWBM.TabIndex = 5;
            this.cboxYWBM.SelectedIndexChanged += new System.EventHandler(this.cboxYWBM_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(417, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "运维部门";
            // 
            // cboxYWDW
            // 
            this.cboxYWDW.FormattingEnabled = true;
            this.cboxYWDW.Location = new System.Drawing.Point(114, 92);
            this.cboxYWDW.Name = "cboxYWDW";
            this.cboxYWDW.Size = new System.Drawing.Size(240, 26);
            this.cboxYWDW.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "运维单位";
            // 
            // cboxXLMC
            // 
            this.cboxXLMC.FormattingEnabled = true;
            this.cboxXLMC.Location = new System.Drawing.Point(115, 39);
            this.cboxXLMC.Name = "cboxXLMC";
            this.cboxXLMC.Size = new System.Drawing.Size(239, 26);
            this.cboxXLMC.TabIndex = 1;
            this.cboxXLMC.SelectedIndexChanged += new System.EventHandler(this.cboxXLMC_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "线路名称";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(23, 383);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(98, 18);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "导入状况：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbtnMulti);
            this.groupBox3.Controls.Add(this.rbtnSingle);
            this.groupBox3.Location = new System.Drawing.Point(12, 171);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(397, 77);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "导入方式";
            // 
            // rbtnMulti
            // 
            this.rbtnMulti.AutoSize = true;
            this.rbtnMulti.Location = new System.Drawing.Point(225, 27);
            this.rbtnMulti.Name = "rbtnMulti";
            this.rbtnMulti.Size = new System.Drawing.Size(69, 22);
            this.rbtnMulti.TabIndex = 6;
            this.rbtnMulti.TabStop = true;
            this.rbtnMulti.Text = "目录";
            this.rbtnMulti.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnMulti.UseVisualStyleBackColor = true;
            // 
            // rbtnSingle
            // 
            this.rbtnSingle.AutoSize = true;
            this.rbtnSingle.Location = new System.Drawing.Point(84, 27);
            this.rbtnSingle.Name = "rbtnSingle";
            this.rbtnSingle.Size = new System.Drawing.Size(69, 22);
            this.rbtnSingle.TabIndex = 5;
            this.rbtnSingle.TabStop = true;
            this.rbtnSingle.Text = "文件";
            this.rbtnSingle.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtFileName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Controls.Add(this.btnOpenFile);
            this.groupBox2.Location = new System.Drawing.Point(12, 268);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(747, 77);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "导入方式";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(115, 32);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(339, 28);
            this.txtFileName.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "文件名称";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(631, 27);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(105, 33);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "开始导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(491, 27);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(105, 33);
            this.btnOpenFile.TabIndex = 5;
            this.btnOpenFile.Text = "打开文件";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(601, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 18);
            this.label6.TabIndex = 7;
            this.label6.Text = "带号";
            // 
            // cboxDH
            // 
            this.cboxDH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxDH.FormattingEnabled = true;
            this.cboxDH.Items.AddRange(new object[] {
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52"});
            this.cboxDH.Location = new System.Drawing.Point(651, 40);
            this.cboxDH.Name = "cboxDH";
            this.cboxDH.Size = new System.Drawing.Size(85, 26);
            this.cboxDH.TabIndex = 8;
            // 
            // FrmPoleImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 453);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmPoleImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "导入杆塔台账信息";
            this.Load += new System.EventHandler(this.frmPoleImport_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboxYWBM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboxYWDW;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboxXLMC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtXLBH;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbtnMulti;
        private System.Windows.Forms.RadioButton rbtnSingle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.ComboBox cboxDH;
        private System.Windows.Forms.Label label6;
    }
}