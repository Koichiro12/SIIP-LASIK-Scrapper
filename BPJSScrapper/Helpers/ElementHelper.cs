using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPJSScrapper.Helpers
{
    class ElementHelper
    {
        Button button;
        public ElementHelper()
        {

        }
        public ElementHelper(Button button)
        {
            this.button = button;
        }

        public void setText(string text)
        {
            if (this.button.InvokeRequired)
            {
                this.button.Invoke(new Action<string>(setText), new object[] { text });
                return;
            }
            this.button.Text = text;
        }

    }
}
