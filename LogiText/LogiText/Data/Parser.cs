using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using LogiText.Data;

namespace LogiText.Data
{
    public class DTAreader
    {
        public List<int> delimeters = new List<int>();

        private string[] column_names;

        private FileStream stream;
        private StreamReader reader;

        public DTAreader(string filename)
        {
            column_names = new string[] {
                "book #", "?", "author", "title", "edition",
                "binding", "?", "publisher", "ISBN13", "Check Digit",
                "List/Net Price", "List/Net Flag", "?", "?", "?", "?", "?", "?", "?", "?",
                "Wholesale Price", "Book Status", "?", "?", "?"
            };

            stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            reader = new StreamReader(stream);

            fileDelimeters(reader.ReadLine());

            //column_names = "| book # | ? | author | title | edition | binding | - | publisher | ISBN13 | Check Digit | List/Net Price | List/Net flag | ? | ? | ? | ? | ? | ? | ? | ? | Wholesale Price | Book Status | ? | ? | ? |".Split('|');
        }

        private Book processLine(string line)
        {
            Book book = new Book();
            Dictionary<string, string> row = new Dictionary<string, string>();

            string current_cell = "";
            int last = 0;
            for (int c = 0; c < line.Length; c++)
            {
                if (c == delimeters[last])
                {
                    last++;

                    if (current_cell != "" && last < column_names.Length)
                        row[column_names[last - 2]] = current_cell;

                    current_cell = "";
                    continue;
                }

                current_cell += line[c];
            }

            // Converting list/net price
            {
                // Convert to int, then float, then divide by 100, then back to string
                int price = 0;
                float LNPrice = 0.0f;
                Int32.TryParse(row["List/Net Price"], out price);
                LNPrice = (float)price / 100.0f;
                row["List/Net Price"] = LNPrice.ToString();

                // Check the flag
                if (row["List/Net Flag"] == "Y") book.list_price = LNPrice.ToString();
                else book.net_price = LNPrice.ToString();
            }

            // Converting wholesale price
            {
                // Convert to int, then float, then divide by 100, then back to string
                int price = 0;
                float WPrice = 0.0f;
                Int32.TryParse(row["Wholesale Price"], out price);
                WPrice = (float)price / 100.0f;
                book.wholesale_price = WPrice.ToString();
            }

            book.data["author"] = row["author"];
            book.data["name"] = row["title"];
            book.data["edition"] = row["edition"];
            book.data["binding"] = row["binding"];
            book.data["publisher"] = row["publisher"];
            book.data["book_status"] = row["Book Status"];
            book.data["ISBN13"] = row["ISBN13"];
            book.ISBN10 = IsbnConverter.ConvertTo10(book.ISBN13);

            return book;
        }

        private void fileDelimeters(string firstline)
        {
            for (int i = 0; i < firstline.Length; i++)
            {
                if (firstline[i] == '@') delimeters.Add(i);
            }
        }

        public List<Book> read(int first_index, int last_index)
        {
            List<Book> books = new List<Book>();
            reader = new StreamReader(stream);

            for (int i = 0; i < last_index; i++)
            {
                string line = reader.ReadLine();
                if (i < first_index) continue;

                books.Add(processLine(line));
            }

            return books;
        }

        //public List<Book> read(string filepath, StreamReader reader, int first_index = 0, int last_index = 9999999)
        //{
        //    List<Book> books = new List<Book>();
        //
        //    for (int i = first_index; i < last_index; i++)
        //    {
        //        Book book = new Book();
        //        Dictionary<string, string> row = new Dictionary<string, string>();
        //
        //        string line = reader.ReadLine();
        //        if (line == null) break;
        //
        //        if (i == 0) { fileDelimeters(line); continue; }
        //
        //        
        //
        //        books.Add(book);
        //    }
        //
        //    return books;
        //}
        //
        //public List<Book> read(string filepath, int first_index = 0, int last_index = 9999999)
        //{
        //    FileStream fileStream = new FileStream(filepath, FileMode.Open);
        //    StreamReader reader = new StreamReader(fileStream);
        //
        //    return read(filepath, reader, first_index, last_index);
        //}
    }

   public class Parser
   {
        //public static List<Book> readDTA(string filepath, int first_index = 0, int last_index = 9999999)
        //{
        //    return new DTAreader().read(filepath, first_index, last_index);
        //}
        //
        //public static List<Book> readDTA(string filepath, StreamReader reader, int first_index = 0, int last_index = 9999999)
        //{
        //    return new DTAreader().read(filepath, first_index, last_index);
        //}

        public static int getColumnCount(string filepath)
        {
            if (filepath.Split('.')[filepath.Split('.').Length - 1].ToLower() == "dta")
            {
                DTAreader reader = new DTAreader(filepath);
                return reader.delimeters.Count - 1;
            }

            return 0;
        }
    }
}
