using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using LogiText.Data;

namespace ScraperUI.src
{
    // Basic structure for displaying fields
    struct Info
    {
        public string title { get; set; }
        public string info  { get; set; }
    }
    
    class Page
    {
        // Function for downloading the image
        private static void download(Object sender, EventArgs e)
        {
            TabPage page = (TabPage)((Button)sender).Parent;
            
            for (int i = 0; i < page.Controls.Count; i++)
            {
                if (page.Controls[i].GetType() == typeof(PictureBox))
                {
                    string url = ((PictureBox)page.Controls[i]).ImageLocation;
                    MessageBox.Show(url);
                }
            }
        }

        // Function for closing the page
        private static void closePage(Object sender, EventArgs e)
        {
            TabPage page = (TabPage)((Button)sender).Parent;
            TabControl control = (TabControl)page.Parent;

            for (int i = 0; i < page.Controls.Count; i++)
                page.Controls[i].Dispose();

            for (int i = 0; i < control.TabPages.Count; i++)
                if (control.TabPages[i] == page)
                    control.TabPages.RemoveAt(i);
        }

        // Constructor that builds the page
        public Page(Book book, TabPage page)
        {
            // Picturebox
            { 
                // Make the picturebox for the image
                PictureBox p = new PictureBox
                {
                    Parent = page,
                    BackColor = Color.Red,
                    Size = new Size(195, 210),
                    Location = new Point(3, 3)
                };

                p.SizeMode = PictureBoxSizeMode.StretchImage;

                if (book.imgURL != "")
                    p.Load(book.imgURL);
            }

            // Label Information
            {
                Info[] controls = new Info[Book.fieldNames.Length];

                int it = 0;
                foreach (string field in Book.fieldNames)
                {
                    controls[it] = new Info { title = field + ":", info = book.data[field] };
                    it++;
                }

                float size = 18.0f;
                const float padding = 1.0f;
                for (int i = 0; i < controls.Length; i++)
                {
                    Info control = controls[i];

                    Label title = new Label()
                    { 
                        Parent = page,
                        Location = new Point(204, (int)(7 + (size + padding * 10.0f) * i)),
                        Size = new Size(100, (int)(size)),
                        Text = control.title
                    };

                    TextBox info = new TextBox()
                    {
                        Parent = page,
                        Location = new Point(304, (int)(7 + (size + padding * 10.0f) * i)),
                        Size = new Size(150, (int)(size)),
                        Text = control.info
                    };
                }
            }

            // Button to download
            {
                Button download = new Button()
                {
                    Parent = page,
                    Text = "Download",
                    Location = new Point(4, 219),
                    Size = new Size(194, 31)
                };

                download.Click += new EventHandler(Page.download);

            }

            // Button to close
            {
                Button close = new Button()
                {
                    Parent = page,
                    Text = "Close",
                    Size = new Size(494, 42),
                    Location = new Point(3, 303)
                };

                close.Click += new EventHandler(Page.closePage);
            }
        }
    }
}
