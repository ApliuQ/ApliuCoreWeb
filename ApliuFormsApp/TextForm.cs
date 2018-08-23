using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApliuFormsApp
{
    public partial class TextForm : Form
    {
        public TextForm()
        {
            InitializeComponent();
            this.Load += TextForm_Load;
        }

        private void TextForm_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://www.baidu.com/s?wd=1");
            webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            MessageBox.Show("完成");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //webBrowser1.Document.GetElementsByTagName("n")[0].InvokeMember("Click");
            foreach (HtmlElement temp in webBrowser1.Document.Links)
            {
                String attrclass = temp.GetAttribute("className");
                String content = temp.InnerText;
                if (attrclass == "n" && content == "下一页>")
                {
                    temp.InvokeMember("Click");
                    break;
                }
            }
        }
    }
}
