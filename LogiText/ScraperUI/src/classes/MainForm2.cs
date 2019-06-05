using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Data;
using ScraperUI.src;
using System.Threading;

namespace ScraperUI.src.classes
{
    public partial class MainForm2 : Form
    {
        private Data.MySql database = null;

        private string workingDirectory;
        private string projectDirectory;

        public Settings settings = new Settings();

        private List<Thread> threads = new List<Thread>();

        private List<string> supported_types = new List<string> {
            "txt", "dta"
        };

        private Dictionary<string, string> DatabaseInfo; 

        public MainForm2()
        {
            InitializeComponent();

            workingDirectory = (Environment.CurrentDirectory).Replace('\\', '/');
            projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName.Replace('\\', '/');

            resetInfo();
        }

        private void resetInfo()
        {
            DatabaseInfo = new Dictionary<string, string> {
                { "Total Row Count:", "0" },
                { "Table Count:", "0" }
            };
        }

        private string formatNumber(int number)
        {
            string str = number.ToString();
            string r = "";

            for (int i = str.Length - 1; i >= 0; i--)
            {
                if ((-i - 1 + str.Length) % 3 == 0 && i != str.Length - 1)
                    r += ",";

                r += str[i];
            }

            char[] charArray = r.ToCharArray();
            Array.Reverse(charArray);

            return new string(charArray);
        }

        private void showError(string msg)
        {
            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MainForm2_Load(object sender, EventArgs e)
        {
            fileList.View = View.Details;
            fileList.HeaderStyle = ColumnHeaderStyle.None;
            fileList.FullRowSelect = true;
            fileList.Columns.Add("Name", 400);
            fileList.Columns.Add("Size", 500);

            fileColumns.View = View.Details;
            fileColumns.HeaderStyle = ColumnHeaderStyle.None;
            fileColumns.FullRowSelect = true;
            fileColumns.Columns.Add("Name", -2);

            tableColumns.View = View.Details;
            tableColumns.HeaderStyle = ColumnHeaderStyle.None;
            tableColumns.FullRowSelect = true;
            tableColumns.Columns.Add("Name", -2);

            previewBox.Text = "Select file to preview...";

            checkThreads();

            genInfoLabels();

        }

        private void updateInfoLabels()
        {
            foreach (KeyValuePair<string, string> stat in DatabaseInfo)
            { 
                foreach (Control control in dataPanel.Controls)
                {
                    if (control.Name == stat.Key)
                    {
                        int number;
                        if (Int32.TryParse(stat.Value, out number))
                        {
                            control.Text = formatNumber(number);
                        }
                        else
                        {
                            control.Text = stat.Value;
                        }

                        break;
                    }
                }
            }
        }

        private void genInfoLabels()
        {
            infoLabel.Visible = false;
            dataLabel.Visible = false;

            int padding = 2;
            int i = 0;
            foreach (KeyValuePair<string, string> stat in DatabaseInfo)
            {
                Label info = new Label()
                {
                    AutoSize = false,
                    Size = infoLabel.Size,
                    Parent = infoLabel.Parent,
                    Location = new Point(infoLabel.Location.X, infoLabel.Location.Y + (infoLabel.Size.Height + padding) * i),
                };

                Label data = new Label()
                {
                    Name = stat.Key,
                    AutoSize = false,
                    Size = dataLabel.Size,
                    Parent = dataLabel.Parent,
                    Location = new Point(dataLabel.Location.X, dataLabel.Location.Y + (dataLabel.Size.Height + padding) * i),
                };
                
                info.Text = stat.Key;

                int number;
                if (Int32.TryParse(stat.Value, out number))
                {
                    data.Text = formatNumber(number);
                }
                else 
                    data.Text = stat.Value;

                i++;
            }
        }

        private void addFile_Click(object sender, EventArgs e)
        {
            string filepath;

            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.InitialDirectory = "c:\\";
                fileDialog.Filter = "All files (*.*)|*.*";
                fileDialog.FilterIndex = 2;
                fileDialog.RestoreDirectory = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    filepath = fileDialog.FileName;
                    FileInfo info = new FileInfo(filepath);
                    int file_size = (int)((float)info.Length / 1000.0f);

                    string[] row = { filepath, formatNumber(file_size) + " KB" };
                    ListViewItem item = new ListViewItem(row);

                    if (!supported_types.Contains(filepath.Split('.')[filepath.Split('.').Length - 1].ToLower()))
                    {
                        fileList.Items.Add(item).BackColor = Color.OrangeRed;
                    }
                    else
                    {
                        fileList.Items.Add(item);
                    }

                }
            }
        }

        private void removeFile_Click(object sender, EventArgs e)
        {
            if (fileList.SelectedItems.Count == 0)
            {
                showError("Must select file to remove!");
                return;
            }

            fileList.Items[fileList.SelectedIndices[0]].Remove();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFile_Click(sender, e);
        }

        private void previewFile(string filename)
        {
            int line_count;

            if (!Int32.TryParse(settings.data["Preview Line Count"], out line_count))
            {
                showError("Error parsing settings information!");
                return;
            }

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line = "";
                    for (int i = 0; i < line_count && line != null; i++)
                    {
                        line = sr.ReadLine();

                        this.Invoke((MethodInvoker)delegate ()
                        {
                            if (i == 0) previewBox.Text = "";

                            previewBox.Text += line + Environment.NewLine;
                        });
                    }
                }
            }
            catch (IOException ex)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    previewBox.Text = "Select file to preview...";
                });

                showError("This file could not be read!\n" + ex.Message);
                return;
            }
        }

        private void previewButton_Click(object sender, EventArgs e)
        {
            if (fileList.SelectedItems.Count == 0)
            {
                showError("Must select file to read!");
                return;
            }
            
            threads.Add(new Thread(i => { this.previewFile((string)i); }));
            threads[threads.Count - 1].Start((fileList.SelectedItems[0].Text));
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            previewBox.Text = "Select file to preview...";
        }

        private void checkThreads()
        {
            for (int i = 0; i < threads.Count; i++)
                if (!threads[i].IsAlive)
                    threads.RemoveAt(i);
        }

        private void thread_checker_Tick(object sender, EventArgs e)
        {
            threads.Add(new Thread(this.checkThreads));
            threads[threads.Count - 1].Start();
        }

        private void updateDatabasePanel()
        {
            if (fileList.SelectedItems.Count != 0) insertionBox.Enabled = true;

            database_info_panel.Enabled = true;

            connectButton.Text = "Disconnect";
            connectButton.BackColor = Color.Red;

            tableBox.Items.Clear();

            // Update tables
            List<string> tables = database.ListTables();
            foreach (string table in tables)
                tableBox.Items.Add(table);

            int rowCount = 0;
            foreach (string table in tables)
                rowCount += database.RowCount(table);

            this.DatabaseInfo["Total Row Count:"] = rowCount.ToString();
            this.DatabaseInfo["Table Count:"] = tables.Count.ToString();
            updateInfoLabels();
        }

        private void connectToMySQL()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                statusLabel.Text = "Connecting to SQL Database...";
                databases.Enabled = false;
                connectButton.Text = "Connecting";
                connectButton.BackColor = Color.Yellow;
            });

            database = new Data.MySql("user id=master;password=Ortner-0210;server=sqltest.ct7pcrou5asi.us-east-2.rds.amazonaws.com;database=MySqlTest");

            if (!database.connSuccessful)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    connectButton.Text = "Connect";
                    connectButton.BackColor = Color.Green;

                    databases.Enabled = true;
                    showError("Error connecting to database");
                    statusLabel.Text = "Ready";
                    return;
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    updateDatabasePanel();
                });
            }

            statusLabel.Text = "Ready";
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (databases.SelectedItem != null && databases.SelectedItem.ToString() == "MySql")
            {
                if (connectButton.Text == "Connect")
                {
                    threads.Add(new Thread(this.connectToMySQL));
                    threads[threads.Count - 1].Start();
                }
                else
                {
                    database = null;

                    insertionBox.Enabled = false;
                    database_info_panel.Enabled = false;
                    tableBox.Items.Clear();

                    resetInfo();
                    updateInfoLabels();

                    databases.Enabled = true;
                    connectButton.Text = "Connect";
                    connectButton.BackColor = Color.Green;

                    viewingLabel.Text = "No Table Selected";
                    tablePreview.Columns.Clear();
                    tablePreview.Rows.Clear();

                    fileColumns.Items.Clear();
                    tableColumns.Items.Clear();
                }
            }
        }

        private void previewTable(string tablename)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                viewingLabel.Text = "Previewing " + tablename;
                tablePreview.Columns.Clear();
            });

            List<string> columns = database.ColumnNames(tablename);
            foreach (string column in columns)
                this.Invoke((MethodInvoker)delegate ()
                {
                    tablePreview.Columns.Add(column, column);
                });

            DataTable rows = database.ReadAll(tablename, 10);
            foreach (DataRow row in rows.Rows)
            {
                DataGridViewRow dataRow = new DataGridViewRow();
                dataRow.CreateCells(tablePreview);

                for (int i = 0; i < columns.Count; i++)
                {
                    dataRow.Cells[i].Value = row[i].ToString();
                }

                this.Invoke((MethodInvoker)delegate ()
                {
                    tablePreview.Rows.Add(dataRow);
                });
            }
        }

        private void tableBox_DoubleClick(object sender, MouseEventArgs e)
        {
            int index = tableBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                string tableName = tableBox.Items[index].ToString();
                databaseControl.SelectedIndex = 1;

                threads.Add(new Thread(i => this.previewTable((string)i)));
                threads[threads.Count - 1].Start(tableName);
            }
        }

        private void createTableButton_Click(object sender, EventArgs e)
        {
            string tablename = Prompt.ShowDialog("Table Name:", "Create Table");
            DialogResult yesOrNo = MessageBox.Show("Are you sure you want to create a table called " + tablename + "?", "Are you sure?", MessageBoxButtons.YesNo);

            if (yesOrNo == DialogResult.Yes)
            {
                string columns = Prompt.ShowDialog("Insert the column names as a comma delimited list:", "Create Table");

                if (columns != "")
                {
                    database.CreateTable(tablename, columns);
                    updateDatabasePanel();
                }
            }
        }

        private void deleteTableButton_Click(object sender, EventArgs e)
        {
            /**/ if (tableBox.SelectedItems.Count == 0) { showError("You must select a table to delete!"); return; }
            else if (tableBox.SelectedItems.Count  > 1) { showError("You must delete one table at a time!"); return; }

            DialogResult yesOrNo = MessageBox.Show("Are you sure you want to delete " + tableBox.SelectedItem.ToString() + "?", "Are you sure?", MessageBoxButtons.YesNo);

            if (yesOrNo == DialogResult.Yes)
            {
                database.DropTable(tableBox.SelectedItem.ToString());
                updateDatabasePanel();
            }
        }

        private void MainForm2_ResizeBegin(object sender, EventArgs e)
        {
            SuspendLayout();
        }

        private void MainForm2_ResizeEnd(object sender, EventArgs e)
        {
            ResumeLayout();
        }

        private void countEntries(string filename)
        {
            int linecount = File.ReadLines(filename).Count();

            this.Invoke((MethodInvoker)delegate ()
            {
                entriesLabel.Text = formatNumber(linecount);
            });

            int columncount = LogiText.Data.Parser.getColumnCount(filename);

            this.Invoke((MethodInvoker)delegate ()
            {
                columnLabel.Text = formatNumber(columncount);
            });
            return;
        }

        private void fileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fileList.SelectedItems.Count == 0)
            {
                insertionBox.Enabled = false;
                columnLabel.Text  = "0";
                entriesLabel.Text = "0";
                return;
            }

            if (database != null) insertionBox.Enabled = true;

            string filename = fileList.SelectedItems[0].Text;

            threads.Add(new Thread(i => this.countEntries((string)i)));
            threads[threads.Count - 1].Start(filename);

            threads.Add(new Thread(this.populateFileOptions));
            threads[threads.Count - 1].Start();
        }

        private void populateTableOptions()
        {
            string item = "";

            this.Invoke((MethodInvoker)delegate ()
            {
                item = tableBox.SelectedItem.ToString();
                tableColumns.Items.Clear();
            });

            List<string> databaseColumns = database.ColumnNames(item);

            foreach (string column in databaseColumns)
                this.Invoke((MethodInvoker)delegate ()
                {
                    if (!LogiText.Data.Book.fieldNames.Contains(column))
                        tableColumns.Items.Add(column).BackColor = Color.Orange;
                    else
                        tableColumns.Items.Add(column);
                });

            bool is_selected = false;

            this.Invoke((MethodInvoker)delegate ()
            {
                is_selected = fileList.SelectedItems.Count > 0;
            });

            if (is_selected)
            {
                this.populateFileOptions();
            }
        }

        private void populateFileOptions()
        {
            List<string> columns = new List<string>();
            string file_type = "";
            this.Invoke((MethodInvoker)delegate ()
            {
                string[] parts = fileList.SelectedItems[fileList.SelectedIndices[0]].Text.Split('.');
                file_type = parts[parts.Length - 1];
                fileColumns.Items.Clear();

                foreach (ListViewItem item in tableColumns.Items)
                    columns.Add(item.Text);
            });


            if (supported_types.Contains(file_type.ToLower()))
            {
                foreach (string column in LogiText.Data.Book.fieldNames)
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        if (!columns.Contains(column))
                            fileColumns.Items.Add(column).BackColor = Color.Orange;
                        else
                            fileColumns.Items.Add(column);
                    });
            }
        }

        private void tableBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tableBox.SelectedItems.Count == 0) { insertOptions.Enabled = false; return; }

            insertOptions.Enabled = true;

            threads.Add(new Thread(this.populateTableOptions));
            threads[threads.Count - 1].Start();
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            /**/ if (runButton.Text == "Run")
            {
                toolStripProgressBar1.Visible = true;
                insertionWorker.RunWorkerAsync();
                runButton.Text = "Cancel";
            }
            else if (MessageBox.Show("Are you sure you want to cancel the insertion?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes
                && insertionWorker.IsBusy)
            {
                toolStripProgressBar1.Visible = false;
                runButton.Text = "Run";
                insertionWorker.CancelAsync();
            }
        }

        private void insertionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                if (insertionWorker.CancellationPending)
                {
                    e.Cancel = true;
                    insertionWorker.ReportProgress(0);
                    return;
                }

                Thread.Sleep(100);
                insertionWorker.ReportProgress(i);
            }
        }

        private void insertionWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
            progressBar1.Value = e.ProgressPercentage;
            updateLabel.Text = progressBar1.Value.ToString() + "%";
            statusLabel.Text = "Insertion Process " + updateLabel.Text;
        }

        private void insertionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar1.Visible = false;
            statusLabel.Text = "Ready";
            updateLabel.Text = "Click run to begin insertion";
            runButton.Text = "Run";
        }
    }
}
