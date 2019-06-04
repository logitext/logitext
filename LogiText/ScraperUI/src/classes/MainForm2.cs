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

using ScraperUI.src;
using System.Threading;

namespace ScraperUI.src.classes
{
    public partial class MainForm2 : Form
    {
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
            fileList.Columns.Add("", -2);

            previewBox.Text = "Select file to preview...";

            checkThreads();

            genInfoLabels();
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

                    if (!supported_types.Contains(filepath.Split('.')[filepath.Split('.').Length - 1].ToLower()))
                    {
                        fileList.Items.Add(filepath).BackColor = Color.OrangeRed;
                    }
                    else
                    {
                        fileList.Items.Add(filepath);
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
    }
}
