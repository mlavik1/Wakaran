namespace Wakaran
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.txtSearchText = new System.Windows.Forms.RichTextBox();
            this.txtRomaji = new System.Windows.Forms.RichTextBox();
            this.txtEnglish = new System.Windows.Forms.RichTextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage1MainPanel = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listExampleSentences = new System.Windows.Forms.ListView();
            this.sourceText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.translation = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnLanguageCN = new System.Windows.Forms.Button();
            this.btnLanguageJP = new System.Windows.Forms.Button();
            this.btnLinkGoogleTrans = new System.Windows.Forms.Button();
            this.btnLinkWeblio = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchText
            // 
            this.txtSearchText.BackColor = System.Drawing.Color.White;
            this.txtSearchText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSearchText.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchText.ForeColor = System.Drawing.Color.Maroon;
            this.txtSearchText.Location = new System.Drawing.Point(18, 18);
            this.txtSearchText.Name = "txtSearchText";
            this.txtSearchText.ReadOnly = true;
            this.txtSearchText.Size = new System.Drawing.Size(613, 105);
            this.txtSearchText.TabIndex = 1;
            this.txtSearchText.Text = "入力テスト";
            // 
            // txtRomaji
            // 
            this.txtRomaji.BackColor = System.Drawing.Color.White;
            this.txtRomaji.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRomaji.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRomaji.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.txtRomaji.Location = new System.Drawing.Point(18, 143);
            this.txtRomaji.Name = "txtRomaji";
            this.txtRomaji.ReadOnly = true;
            this.txtRomaji.Size = new System.Drawing.Size(613, 102);
            this.txtRomaji.TabIndex = 2;
            this.txtRomaji.Text = "入力テスト";
            // 
            // txtEnglish
            // 
            this.txtEnglish.BackColor = System.Drawing.Color.White;
            this.txtEnglish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEnglish.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEnglish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtEnglish.Location = new System.Drawing.Point(18, 251);
            this.txtEnglish.Name = "txtEnglish";
            this.txtEnglish.ReadOnly = true;
            this.txtEnglish.Size = new System.Drawing.Size(613, 106);
            this.txtEnglish.TabIndex = 3;
            this.txtEnglish.Text = "入力テスト";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(712, 380);
            this.tabControl.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabPage1MainPanel);
            this.tabPage1.Controls.Add(this.txtSearchText);
            this.tabPage1.Controls.Add(this.txtEnglish);
            this.tabPage1.Controls.Add(this.txtRomaji);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(704, 351);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Translation";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage1MainPanel
            // 
            this.tabPage1MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPage1MainPanel.AutoSize = true;
            this.tabPage1MainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tabPage1MainPanel.Location = new System.Drawing.Point(18, 6);
            this.tabPage1MainPanel.Name = "tabPage1MainPanel";
            this.tabPage1MainPanel.Size = new System.Drawing.Size(0, 0);
            this.tabPage1MainPanel.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listExampleSentences);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(704, 351);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Examples";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listExampleSentences
            // 
            this.listExampleSentences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listExampleSentences.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.sourceText,
            this.translation});
            this.listExampleSentences.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listExampleSentences.Location = new System.Drawing.Point(6, 6);
            this.listExampleSentences.Name = "listExampleSentences";
            this.listExampleSentences.Size = new System.Drawing.Size(684, 442);
            this.listExampleSentences.TabIndex = 0;
            this.listExampleSentences.UseCompatibleStateImageBehavior = false;
            this.listExampleSentences.View = System.Windows.Forms.View.Details;
            this.listExampleSentences.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listExampleSentences_KeyUp);
            // 
            // sourceText
            // 
            this.sourceText.Width = 320;
            // 
            // translation
            // 
            this.translation.Width = 320;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnLanguageCN
            // 
            this.btnLanguageCN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLanguageCN.BackgroundImage = global::Wakaran.Properties.Resources.flag_cn;
            this.btnLanguageCN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLanguageCN.Location = new System.Drawing.Point(645, 398);
            this.btnLanguageCN.Name = "btnLanguageCN";
            this.btnLanguageCN.Size = new System.Drawing.Size(75, 75);
            this.btnLanguageCN.TabIndex = 7;
            this.btnLanguageCN.UseVisualStyleBackColor = true;
            this.btnLanguageCN.Click += new System.EventHandler(this.btnLanguageCN_Click);
            // 
            // btnLanguageJP
            // 
            this.btnLanguageJP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLanguageJP.BackgroundImage = global::Wakaran.Properties.Resources.flag_jp;
            this.btnLanguageJP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLanguageJP.Location = new System.Drawing.Point(564, 397);
            this.btnLanguageJP.Name = "btnLanguageJP";
            this.btnLanguageJP.Size = new System.Drawing.Size(75, 75);
            this.btnLanguageJP.TabIndex = 6;
            this.btnLanguageJP.UseVisualStyleBackColor = true;
            this.btnLanguageJP.Click += new System.EventHandler(this.btnLanguageJP_Click);
            // 
            // btnLinkGoogleTrans
            // 
            this.btnLinkGoogleTrans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLinkGoogleTrans.BackgroundImage = global::Wakaran.Properties.Resources.Google_Translate_Icon;
            this.btnLinkGoogleTrans.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLinkGoogleTrans.Location = new System.Drawing.Point(12, 394);
            this.btnLinkGoogleTrans.Name = "btnLinkGoogleTrans";
            this.btnLinkGoogleTrans.Size = new System.Drawing.Size(88, 84);
            this.btnLinkGoogleTrans.TabIndex = 5;
            this.btnLinkGoogleTrans.UseVisualStyleBackColor = true;
            this.btnLinkGoogleTrans.Click += new System.EventHandler(this.btnLinkGoogleTrans_Click);
            // 
            // btnLinkWeblio
            // 
            this.btnLinkWeblio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLinkWeblio.BackgroundImage = global::Wakaran.Properties.Resources.Weblio;
            this.btnLinkWeblio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLinkWeblio.Location = new System.Drawing.Point(106, 394);
            this.btnLinkWeblio.Name = "btnLinkWeblio";
            this.btnLinkWeblio.Size = new System.Drawing.Size(88, 84);
            this.btnLinkWeblio.TabIndex = 4;
            this.btnLinkWeblio.UseVisualStyleBackColor = true;
            this.btnLinkWeblio.Click += new System.EventHandler(this.btnLinkWeblio_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 476);
            this.ControlBox = false;
            this.Controls.Add(this.btnLanguageCN);
            this.Controls.Add(this.btnLanguageJP);
            this.Controls.Add(this.btnLinkGoogleTrans);
            this.Controls.Add(this.btnLinkWeblio);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.Text = "Wakaran";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtSearchText;
        private System.Windows.Forms.RichTextBox txtRomaji;
        private System.Windows.Forms.RichTextBox txtEnglish;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel tabPage1MainPanel;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView listExampleSentences;
        private System.Windows.Forms.ColumnHeader sourceText;
        private System.Windows.Forms.ColumnHeader translation;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnLinkGoogleTrans;
        private System.Windows.Forms.Button btnLinkWeblio;
        private System.Windows.Forms.Button btnLanguageJP;
        private System.Windows.Forms.Button btnLanguageCN;
    }
}