namespace ScraperUI
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.data = new System.Windows.Forms.DataGridView();
            this.openButton = new System.Windows.Forms.Button();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.updateLabel = new System.Windows.Forms.Label();
            this.fIlewwToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchScrape = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.isbnListBox = new System.Windows.Forms.TextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.detailsListBox = new System.Windows.Forms.TextBox();
            this.executeButton = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.errorListText = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIlewwToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(747, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.searchButton);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(508, 67);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ISBN Search";
            // 
            // searchButton
            // 
            this.searchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.Location = new System.Drawing.Point(403, 25);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(99, 26);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(6, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(391, 24);
            this.textBox1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 155);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(508, 326);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.data);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(500, 298);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Results";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // data
            // 
            this.data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data.Location = new System.Drawing.Point(3, 3);
            this.data.Name = "data";
            this.data.Size = new System.Drawing.Size(494, 292);
            this.data.TabIndex = 0;
            // 
            // openButton
            // 
            this.openButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openButton.Location = new System.Drawing.Point(12, 487);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(508, 42);
            this.openButton.TabIndex = 3;
            this.openButton.Text = "Open Book";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(12, 100);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(508, 26);
            this.progress.Step = 1;
            this.progress.TabIndex = 4;
            // 
            // updateLabel
            // 
            this.updateLabel.Location = new System.Drawing.Point(9, 129);
            this.updateLabel.Name = "updateLabel";
            this.updateLabel.Size = new System.Drawing.Size(400, 23);
            this.updateLabel.TabIndex = 5;
            this.updateLabel.Text = "Click search to scrape for an ISBN";
            this.updateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fIlewwToolStripMenuItem
            // 
            this.fIlewwToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.batchScrape});
            this.fIlewwToolStripMenuItem.Name = "fIlewwToolStripMenuItem";
            this.fIlewwToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fIlewwToolStripMenuItem.Text = "File";
            // 
            // batchScrape
            // 
            this.batchScrape.Name = "batchScrape";
            this.batchScrape.Size = new System.Drawing.Size(180, 22);
            this.batchScrape.Text = "Batch Scrape";
            this.batchScrape.Click += new System.EventHandler(this.batchScrape_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(415, 129);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "More Info >>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // isbnListBox
            // 
            this.isbnListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.isbnListBox.Location = new System.Drawing.Point(3, 3);
            this.isbnListBox.Multiline = true;
            this.isbnListBox.Name = "isbnListBox";
            this.isbnListBox.ReadOnly = true;
            this.isbnListBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.isbnListBox.Size = new System.Drawing.Size(186, 422);
            this.isbnListBox.TabIndex = 7;
            this.isbnListBox.WordWrap = false;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(535, 27);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(200, 454);
            this.tabControl2.TabIndex = 8;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.detailsListBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 428);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Details";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.isbnListBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(192, 428);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "ISBN List";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // detailsListBox
            // 
            this.detailsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailsListBox.Location = new System.Drawing.Point(3, 3);
            this.detailsListBox.Multiline = true;
            this.detailsListBox.Name = "detailsListBox";
            this.detailsListBox.ReadOnly = true;
            this.detailsListBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.detailsListBox.Size = new System.Drawing.Size(186, 422);
            this.detailsListBox.TabIndex = 8;
            // 
            // executeButton
            // 
            this.executeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.executeButton.Location = new System.Drawing.Point(535, 487);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(200, 42);
            this.executeButton.TabIndex = 9;
            this.executeButton.Text = "Execute";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.errorListText);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(192, 428);
            this.tabPage4.TabIndex = 2;
            this.tabPage4.Text = "Error List";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // errorListText
            // 
            this.errorListText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorListText.Location = new System.Drawing.Point(0, 0);
            this.errorListText.Multiline = true;
            this.errorListText.Name = "errorListText";
            this.errorListText.ReadOnly = true;
            this.errorListText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorListText.Size = new System.Drawing.Size(192, 428);
            this.errorListText.TabIndex = 8;
            this.errorListText.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 541);
            this.Controls.Add(this.executeButton);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.updateLabel);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Scraper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView data;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label updateLabel;
        private System.Windows.Forms.ToolStripMenuItem fIlewwToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchScrape;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox isbnListBox;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox detailsListBox;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox errorListText;
    }
}

