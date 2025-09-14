using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPJSScrapper.Helpers
{
    class RichTextboxLogger
    {

        private RichTextBox _rtb;

        public RichTextboxLogger(RichTextBox rtb)
        {
            _rtb = rtb;
        }

        public void Append(string value)
        {
            if (this._rtb.InvokeRequired)
            {
                this._rtb.Invoke(new Action<string>(Append), new object[] { value });
                return;
            }
            this._rtb.Text += value + Environment.NewLine;
            this._rtb.SelectionStart = this._rtb.Text.Length;
            this._rtb.ScrollToCaret();
        }

        public void In(string value)
        {
            this.Append(  "[ "+DateTime.Now+" ] << "+value);
        }

        public void Out(string value)
        {
            this.Append("[ " + DateTime.Now + " ] >> " + value);
        }

        public void Process(string value)
        {
            this.Append("[ " + DateTime.Now + " ] >< " + value);
        }

    }
}
