//using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;

namespace ApliuFormsApp
{
    public partial class Main : Form
    {
        private WebBrowser webBrowser = new WebBrowser();
        System.Windows.Forms.Timer timeDown = new System.Windows.Forms.Timer();
        Uri uri = new Uri("https://www.baidu.com/s?wd=1");//("https://news.baidu.com");
        List<String> arryImgUrl = new List<String>() { };//所有图片URL
        int current = 0;//用于记录浏览器滚动条往下翻页的数据
        int page = 1;//获取多少页内容
        String Next_Page_Id = "";//下一页的Id 如不匹配则为空
        String nextpageClass = "n";//下一页的class 如不匹配则为空
        String nextpageContent = "下一页>";//下一页的文本内容 如不匹配则为空


        public Main()
        {
            InitializeComponent();
            this.Load += Main_Load;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
            this.textUrl.Text = uri.ToString();
            tbnextclass.Text = nextpageClass;
            tbnextid.Text = Next_Page_Id;
            tbnexttext.Text = nextpageContent;
            tbpagecount.Text = page.ToString();
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (jxload)
            {
                jxload = false;
                GetImgUrl();
            }
            if (timeDown != null)
            {
                timeDown.Interval = 100;
                timeDown.Tick += new EventHandler(timeDown_Tick);
                timeDown.Start();
            }
        }

        bool download = false;
        bool jxload = false;
        int getpage = 1;
        private void timeDown_Tick(object sender, EventArgs e)
        {
            System.Windows.Forms.HtmlDocument doc = webBrowser.Document;
            int height = webBrowser.Document.Body.ScrollRectangle.Height;
            current += height / 10;
            if (current >= height)
            {
                current = height;
                RunCrawler();
                if (timeDown != null)
                {
                    timeDown.Stop();
                    timeDown.Dispose();
                    timeDown = null;
                    btnGet.Enabled = true;
                    btnDownload.Enabled = true;

                    //开始获取下一页内容
                    if (int.TryParse(tbpagecount.Text.ToString(), out int a) && getpage < a)
                    {
                        if (!String.IsNullOrEmpty(tbnextid.Text.Trim()))
                        {
                            HtmlElement temp = webBrowser.Document.GetElementById(tbnextid.Text);
                            if (temp != null)
                            {
                                temp.InvokeMember("Click");
                                jxload = true;
                                getpage++;
                            }
                            else WriteLine($"找不到下一页的按钮，{nameof(Next_Page_Id)}：{tbnextid.Text}");
                            return;
                        }
                        else
                        {
                            foreach (HtmlElement temp in webBrowser.Document.All)
                            {
                                String attrclass = temp.GetAttribute("className");
                                String content = temp.InnerText;
                                if (!String.IsNullOrEmpty(tbnextclass.Text.Trim()) && !String.IsNullOrEmpty(tbnexttext.Text.Trim()))
                                {
                                    if (attrclass == tbnextclass.Text.Trim() && content == tbnexttext.Text.Trim())
                                    {
                                        temp.InvokeMember("Click");
                                        jxload = true;
                                        getpage++;
                                        return;
                                    }
                                }
                                else if (!String.IsNullOrEmpty(tbnextclass.Text.Trim()))
                                {
                                    if (attrclass == tbnextclass.Text.Trim())
                                    {
                                        temp.InvokeMember("Click");
                                        jxload = true;
                                        getpage++;
                                        return;
                                    }
                                }
                                else if (!String.IsNullOrEmpty(tbnexttext.Text.Trim()))
                                {
                                    if (content == tbnexttext.Text.Trim())
                                    {
                                        temp.InvokeMember("Click");
                                        jxload = true;
                                        getpage++;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (download)
                        {
                            download = false;
                            DownloadImg();
                        }
                    }
                }
            }
            doc.Window.ScrollTo(0, current);
        }

        private void RunCrawler()
        {
            HtmlElementCollection htmlElementCollection = webBrowser.Document.Images;

            //String htmlContent = webBrowser.Document.Body.InnerHtml;
            //HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            //htmlDocument.LoadHtml(htmlContent);

            //HtmlNode IDNode = doc.DocumentNode.SelectSingleNode("//div[@id='header']/div[@id='blogTitle']/h1");
            //HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//img[@src!='']");
            //WriteLine($"共找到{htmlNodeCollection.Count}张图片");
            WriteLine("--------------------------------------------------------");

            //foreach (var temp in htmlNodeCollection)
            foreach (HtmlElement temp in htmlElementCollection)
            {
                String imgUrl = String.Empty;
                String tempUrl = temp.GetAttribute("src");
                if (String.IsNullOrEmpty(tempUrl)) continue;
                if (tempUrl.ToUpper().IndexOf("HTTP") == 0 || tempUrl.ToUpper().IndexOf("HTTPS") == 0)
                {
                    imgUrl = tempUrl;
                }
                else if (tempUrl.ToUpper().IndexOf(":") == 0)
                {
                    imgUrl = uri.Scheme + tempUrl;
                }
                else if (tempUrl.ToUpper().IndexOf("//") == 0)
                {
                    imgUrl = uri.Scheme + ":" + tempUrl;
                }
                else if (tempUrl.ToUpper().IndexOf("/") == 0)
                {
                    imgUrl = uri.AbsoluteUri.Substring(0, uri.AbsoluteUri.Length - 1) + tempUrl;
                }
                else
                {
                    imgUrl = uri.AbsoluteUri + tempUrl;
                }
                arryImgUrl.Add(imgUrl);
                WriteLine(imgUrl);
            }
            WriteLine("--------------------------------------------------------");
            WriteLine($"共找到{htmlElementCollection.Count}张图片");
            WriteLine("");
        }

        private void WriteLine(String text)
        {
            this.Invoke(new Action(delegate
            {
                textBox.AppendText(text + System.Environment.NewLine);
            }));
        }

        private void GetImgUrl()
        {
            WriteLine($"开始获取第{(getpage > 0 ? getpage : 1)}页图片 {webBrowser.Url?.ToString()}");
            current = 0;
            timeDown = new System.Windows.Forms.Timer();
            btnGet.Enabled = false;
            btnDownload.Enabled = false;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textUrl.Text))
            {
                getpage = 1;
                arryImgUrl.Clear();
                webBrowser.Url = uri;
                GetImgUrl();
                webBrowser.Navigate(uri);
            }
        }

        private void DownloadImg()
        {
            string filePath = String.Empty;
            if (arryImgUrl.Count == 0)
            {
                WriteLine($"请先获取图片");
                return;
            }
            Assembly assem = Assembly.GetExecutingAssembly();
            string assemDir = Path.GetDirectoryName(assem.Location);
            filePath = Path.Combine(assemDir, @"imgs\");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath.ToString());
            }

            btnDownload.Enabled = false;
            btnGet.Enabled = false;
            WriteLine($"开始下载图片，共{arryImgUrl.Count}张图片");
            WriteLine("--------------------------------------------------------");
            HttpClient httpClient = new HttpClient();
            Int32 intDown = 0;
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    foreach (String imgUrl in arryImgUrl)
                    {
                        try
                        {
                            Stream stream = await httpClient.GetStreamAsync(imgUrl);
                            Image image = Image.FromStream(stream);
                            if (image.Width > 0)
                            {
                                String imgName = Guid.NewGuid().ToString().ToLower() + ".jpg";
                                image.Save(filePath + imgName);
                                WriteLine($"开始下载：{imgUrl}");
                                intDown++;
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLine($"下载失败：{imgUrl}，详情：" + ex.Message);
                        }

                        if (imgUrl == arryImgUrl[arryImgUrl.Count - 1])
                        {
                            WriteLine("--------------------------------------------------------");
                            WriteLine($"图片下载完成，共{intDown}张图片");
                            WriteLine($"保存路径：{filePath}");
                            WriteLine("");
                            this.Invoke(new Action(delegate
                            {
                                btnDownload.Enabled = true;
                                btnGet.Enabled = true;
                            }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLine($"发生错误：{ex.Message}");
                }
            });
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            download = true;
            btnGet_Click(null, null);
        }
    }
}
