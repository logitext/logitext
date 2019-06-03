using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperUI.src.classes
{
    public class Settings
    {
        public Dictionary<string, string> data { get; set; }

        public Settings()
        {
            data = new Dictionary<string, string>
            {
                 {"Preview Line Count", "100"}
            };
        }
    }
}
