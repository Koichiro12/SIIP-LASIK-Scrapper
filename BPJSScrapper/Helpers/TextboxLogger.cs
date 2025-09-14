using System;
using System.Windows.Forms;

namespace BPJSScrapper.Helpers
{
    class TextboxLogger
    {

        TextBox textbox;
        public TextboxLogger() { }

        public TextboxLogger(TextBox textbox) {
            this.textbox = textbox;
        }

        public void AppendTextBox(string value)
        {
            if (this.textbox.InvokeRequired)
            {
                this.textbox.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            this.textbox.Text += value + Environment.NewLine;
            this.textbox.SelectionStart = this.textbox.Text.Length;
            this.textbox.ScrollToCaret();
        }

        public void SetTextBox(string value)
        {
            if (this.textbox.InvokeRequired)
            {
                this.textbox.Invoke(new Action<string>(SetTextBox), new object[] { value });
                return;
            }
            this.textbox.Text = value;
        }

    }
}
