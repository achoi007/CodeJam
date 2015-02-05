using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeJam.AllYourBase
{
    class Reader
    {
        public string[] Read(string pathname)
        {
            using (var reader = File.OpenText(pathname))
            {
                return Read(reader);
            }
        }

        public string[] Read(TextReader reader)
        {
            int n = int.Parse(reader.ReadLine());
            string[] inputs = new string[n];
            for (int i = 0; i < n; i++)
            {
                inputs[i] = reader.ReadLine();
            }
            return inputs;
        }
    }
}
