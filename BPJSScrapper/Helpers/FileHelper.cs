using System;
using System.Collections.Generic;
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

    }
}
