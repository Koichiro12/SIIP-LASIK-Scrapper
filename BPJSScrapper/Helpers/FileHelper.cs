using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPJSScrapper.Helpers
{
    public class FileHelper
    {

        public FileHelper()
        {

        }

        public string GetFilePath(string filter)
        {
            string result = null;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                //Initial Path
                ofd.InitialDirectory = "c:\\";

                // Filtering File As Extensions
                ofd.Filter = filter;
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    result = ofd.FileName;
                    return result;
                }
            }


            return result;
        }

        public void CreateFolderIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string StringCatcher(string source, string kanan = null, string kiri = null)
        {
            string res = string.Empty;
            if (kanan == null)
            {
                kanan = string.Empty;
            }
            if (kiri == null)
            {
                kiri = string.Empty;
            }
            if (source.Contains(kanan) && source.Contains(kiri))
            {
                int start, end, strEnd;
                start = source.IndexOf(kanan, 0) + kanan.Length;
                end = source.IndexOf(kiri, start);
                strEnd = source.Length - 1;
                string token = source.Substring(start, end - start);
                res = token;
            }
            else
            {
                res = "No data Available";
            }
            return res;
        }

    }
}
