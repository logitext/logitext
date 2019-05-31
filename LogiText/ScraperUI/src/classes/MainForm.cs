using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using LogiText.Data;
using Scraper;
using ScraperUI.src;
using System.Threading;

namespace ScraperUI
{ 
    public partial class MainForm : Form
    {
        private bool moreInfo { get; set; }

        private List<string> ISBNs = new List<string>();

        private Size smallSize = new Size(548, 579);
        private Size expandedSize = new Size(763, 579);

        private Thread batch;
        private bool executing = false;

        // Constructor
        public MainForm()
        {
            InitializeComponent();
        }

        // Show an Error message box
        private void showError(string msg)
        {
            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Build a book object from a DataGridRow
        private Book bookFromRow(DataGridViewRow row)
        {
            Book r = new Book();

            try
            {
                r.name = (string)row.Cells[0].Value;
                r.ISBN10 = (string)row.Cells[1].Value;
                //r.price = (float)row.Cells[2].Value;
                //r.url = (string)row.Cells[3].Value;
                r.imgURL = (string)row.Cells[4].Value;
            }
            catch { }

            return r;
        }

        // When the form loads
        private void Form1_Load(object sender, EventArgs e)
        {
            Size = smallSize;

            // Populate table with field names
            for (int i = 0; i < Book.fieldNames.Length; i++)
                data.Columns.Add(Book.fieldNames[i], Book.fieldNames[i]);

            // User cannot change the table
            data.ReadOnly = true;
        }

        // When the open page button is clicked
        private void openButton_Click(object sender, EventArgs e)
        {
            // Check if user selected rows
            /**/ if (data.SelectedRows.Count == 0)
            {
                showError("Must select row!");
                return;
            }

            // Check if each row has data in it
            for (int i = 0; i < data.SelectedRows.Count; i++)
                if ((string)data.SelectedRows[i].Cells[0].Value == null)
                {
                    showError("Row must be populated!");
                    return;
                }

            for (int i = 0; i < data.SelectedRows.Count; i++)
            {
                DataGridViewRow row = data.SelectedRows[i];

                string name = (string)row.Cells[0].Value;

                bool found = false;
                for (int j = 0; j < tabControl1.TabCount; j++)
                    if (tabControl1.TabPages[j].Text == name)
                        found = true;

                if (found) continue;

                // Open a new page
                tabControl1.TabPages.Add(name);
                Page page = new Page(bookFromRow(row), tabControl1.TabPages[tabControl1.TabCount - 1]);
            }
        }

        // When the search button is clicked
        private void searchButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 10) return;
            string url = "https://www.amazon.com/Clash-Kings-Song-Fire-Book/dp/" + textBox1.Text + "/ref=tmm_hrd_swatch_0?_encoding=UTF8&qid=1559015792&sr=8-2";

            AmazonScraper scraper = new AmazonScraper();

            // Restart progress bar and update label
            { 
                progress.Value = 0;
                updateLabel.Text = "Downloading webpage...";
                Update();
            }

            // Retrieve the page
            string page = scraper.getPage(url);

            if (page == null)
            {
                showError("ISBN not valid!");

                updateLabel.Text = "Click search to scrape for an ISBN";
                progress.Value = 0;

                return;
            }

            // Update progress bar and update label
            { 
                progress.Value = 50;
                updateLabel.Text = "Finding information...";
                Update();
            }

            Book book = scraper.getBook(textBox1.Text, page);

            progress.Value = 100;

            if (book == null)
            {
                showError("ISBN not valid!");
                return;
            }

            pushBookToTable(book);

            updateLabel.Text = "Click search to scrape for an ISBN";
        }

        private void pushBookToTable(Book book)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(data,
                book.name,
                book.ISBN10,
                book.price,
                book.imgURL,
                book.imgURL
            );

            this.Invoke((MethodInvoker)delegate ()
            {
                data.Rows.Add(row);
            });
        }

        private void batchScrape_Click(object sender, EventArgs e)
        {
            string filepath;

            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.InitialDirectory = "c:\\";
                fileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                fileDialog.FilterIndex = 2;
                fileDialog.RestoreDirectory = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    filepath = fileDialog.FileName;
                    var fileStream = fileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        while (true)
                        {
                            string line = reader.ReadLine();
                            if (line == null) break;
                            ISBNs.Add(line);

                            isbnListBox.Text += line + Environment.NewLine;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (moreInfo)
            {
                moreInfo = false;
                button1.Text = "More Info >>";
                Size = smallSize;
            }
            else
            {
                moreInfo = true;
                button1.Text = "<< Less Info";
                Size = expandedSize;
            }
        }

        // Threaded function
        private void scrapeBook(string ISBN)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                updateLabel.Text = ISBN;
                detailsListBox.Text += "Scraping " + ISBN + "...  ";
            });

            AmazonScraper amazon = new AmazonScraper();

            try
            {
                string url = "https://www.amazon.com/Clash-Kings-Song-Fire-Book/dp/" + ISBN + "/ref=tmm_hrd_swatch_0?_encoding=UTF8&qid=1559015792&sr=8-2";
                string page = amazon.getPage(url);

                Book book = amazon.getBook(textBox1.Text, page);
                pushBookToTable(book);
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    detailsListBox.Text += "error: " + ex.Message + "!";
                    errorListText.Text += ISBN + Environment.NewLine;
                });
            }

            this.Invoke((MethodInvoker)delegate ()
            {
                progress.PerformStep();
                detailsListBox.Text += Environment.NewLine;
            });
        }

        private void scrapeList(List<string> isbns)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                errorListText.Text = "";
                progress.Maximum = ISBNs.Count;
            });

            foreach (string isbn in isbns)
            {
                scrapeBook(isbn);
            }

            this.Invoke((MethodInvoker)delegate ()
            {
                progress.Value = 0;
                progress.Maximum = 100;
                updateLabel.Text = "Click search to scrape for an ISBN";
            });
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            executing = (executing) ? false : true;

            if (executing)
            {
                executeButton.Text = "Cancel";

                batch = new Thread(i => { this.scrapeList((List<string>)i); });
                batch.Start(ISBNs);
            }
            else
            {
                executeButton.Text = "Execute";
                batch.Abort();

                progress.Value = 0;
                progress.Maximum = 100;
                updateLabel.Text = "Click search to scrape for an ISBN";
            }
        }
    }
}
