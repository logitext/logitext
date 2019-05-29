using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LogiText.Data;
using Scraper;
using ScraperUI.src;

namespace ScraperUI
{ 
    public partial class MainForm : Form
    {
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
                r.ISBN = (string)row.Cells[1].Value;
                r.price = (float)row.Cells[2].Value;
                r.url = (string)row.Cells[3].Value;
                r.imgURL = (string)row.Cells[4].Value;
            }
            catch { }

            return r;
        }

        // When the form loads
        private void Form1_Load(object sender, EventArgs e)
        {
            // Data fields
            string[] columnNames = {
                "Title",
                "ISBN",
                "Price",
                "URL",
                "Image URL"
            };

            // Populate table with field names
            for (int i = 0; i < columnNames.Length; i++)
                data.Columns.Add(columnNames[i], columnNames[i]);

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

            // Restart progress bar and update label
            { 
                progress.Value = 0;
                updateLabel.Text = "Downloading webpage...";
                Update();
            }

            // Retrieve the page
            string page = Amazon.getPage(url);

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

            Book book = Amazon.scrapeISBN(textBox1.Text, page);

            progress.Value = 100;

            if (book == null)
            {
                showError("ISBN not valid!");
                return;
            }

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(data, 
                book.name,
                book.ISBN,
                book.price,
                book.url,
                book.imgURL
            );

            data.Rows.Add(row);

            updateLabel.Text = "Click search to scrape for an ISBN";
        }

        private void batchScrape_Click(object sender, EventArgs e)
        {

        }
    }
}
