using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Wakaran
{
    public partial class MainForm : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        enum Language
        {
            Japanese,
            Chinese
        }

        static string SearchText = "";

        static TimeSpan TimeAtActivation = DateTime.Now.TimeOfDay;

        static Language SelectedLanguage = Language.Japanese;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            RegisterHotKey(this.Handle, 0, (int)KeyModifier.Control, Keys.X.GetHashCode());

            ListViewItem item1 = new ListViewItem(new[] { "Test", "English"});
            listExampleSentences.Items.Add(item1);

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //UnregisterHotKey(this.Handle, 0);
            this.Visible = false;
            //RegisterHotKey(this.Handle, 0, (int)KeyModifier.Control, Keys.X.GetHashCode());
            e.Cancel = true;
        }

        private void FillDictionary()
        {
            string googleTranslateURL = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", SearchText, SelectedLanguage == Language.Japanese ? "ja|en" : "cn|en");

            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            if (SelectedLanguage == Language.Chinese)
            {
                encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            }

            WebClient webClient = new WebClient();
            webClient.Encoding = encoding;

            string googleResult;
            try
            {
                var htmlData = webClient.DownloadData(googleTranslateURL);
                googleResult = encoding.GetString(htmlData);
            }
            catch(Exception ex)
            {
                // TODO: Show message
                return;
            }

            mshtml.HTMLDocument doc = new mshtml.HTMLDocument();
            mshtml.IHTMLDocument2 doc2 = (mshtml.IHTMLDocument2)doc;
            doc2.write(googleResult);

            mshtml.IHTMLElement elemResultBox = doc.getElementById("result_box");
            mshtml.IHTMLElement elemTranslit = doc.getElementById("src-translit");

            string txtResultBox = elemResultBox.innerText;
            string txtTranslit = elemTranslit.innerText;

            this.txtSearchText.Text = SearchText;
            this.txtRomaji.Text = txtTranslit;
            this.txtEnglish.Text = txtResultBox;
        }

        private async void FillExamples()
        {
            listExampleSentences.Items.Clear();

            List<string> japaneseTexts = new List<string>();
            List<string> englishTexts = new List<string>();

            await Task.Run(() =>
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;

                string weblioURL = String.Format("https://{0}.weblio.jp/sentence/content/{1}", SelectedLanguage == Language.Japanese ? "ejje" : "cjjc", SearchText);
                string weblioResult;

                try
                {
                    weblioResult = webClient.DownloadString(weblioURL);

                }
                catch (Exception ex)
                {
                    // TODO: show message
                    return;
                }

                string sourceTextClassName = SelectedLanguage == Language.Japanese ? "CJJ" : "CC";
                string targetTextClassName = SelectedLanguage == Language.Japanese ? "CJE" : "CJ";

                Regex r = new Regex(Regex.Escape(String.Format("<p class=qot{0}>", sourceTextClassName)) + "(.*?)" + Regex.Escape("</p>"));
                MatchCollection matches = r.Matches(weblioResult);
                foreach (Match match in matches)
                {
                    string str = match.Groups[1].Value;
                    int iSpan = str.IndexOf("<span");
                    if (iSpan != -1)
                    {
                        str = str.Substring(0, iSpan);
                    }
                    str = str.Replace("<b>", "").Replace("</b>", "");
                    japaneseTexts.Add(str);
                }

                r = new Regex(Regex.Escape(String.Format("<p class=qot{0}>", targetTextClassName)) + "(.*?)" + Regex.Escape("</p>"));
                matches = r.Matches(weblioResult);
                foreach (Match match in matches)
                {
                    string str = match.Groups[1].Value;
                    int startIndex = str.IndexOf('<');
                    int i;
                    while (startIndex != -1)
                    {
                        int bracketCount = 0;
                        for (i = startIndex + 1; i < str.Length; i++)
                        {
                            if (str[i] == '<')
                                bracketCount++;
                            else if (str[i] == '>')
                            {
                                if (bracketCount == 0)
                                    break;
                                bracketCount--;
                            }
                        }
                        str = str.Remove(startIndex, i - startIndex + 1);
                        str = Regex.Replace(str, "<span>(.*?)</span>", "");
                        startIndex = str.IndexOf('<');
                    }
                    englishTexts.Add(str);
                }
            });

            for (int i = 0; i < japaneseTexts.Count() && i < englishTexts.Count(); i++)
            {
                ListViewItem item1 = new ListViewItem(new[] { japaneseTexts[i], englishTexts[i] });
                listExampleSentences.Items.Add(item1);
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                SearchText =  Clipboard.GetText();
                
                if(SearchText != "")
                {
                    {
                        TimeAtActivation = DateTime.Now.TimeOfDay;
                        this.Visible = true;
                        this.WindowState = FormWindowState.Normal;
                        this.TopMost = true;
                        this.BringToFront();
                        SetForegroundWindow(this.Handle);

                        tabControl.SelectTab(0);

                        FillDictionary();
                        FillExamples();
                    }
                }
            }
        }

        private void ExampleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
        }

        private void listExampleSentences_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Control &&e.KeyCode == Keys.C && listExampleSentences.SelectedItems.Count > 0)
            {
                Clipboard.SetText(listExampleSentences.SelectedItems[0].Text);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Visible && (DateTime.Now.TimeOfDay - TimeAtActivation).Seconds > 1.0f && GetForegroundWindow() != this.Handle)
            {
                this.Visible = false;
            }
        }

        private void btnLinkWeblio_Click(object sender, EventArgs e)
        {
            string weblioURL = String.Format("https://{0}.weblio.jp/sentence/content/{1}", SelectedLanguage == Language.Japanese ? "ejje" : "cjjc", SearchText);
            System.Diagnostics.Process.Start(weblioURL);
        }

        private void btnLinkGoogleTrans_Click(object sender, EventArgs e)
        {
            string googleTranslateURL = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", SearchText, SelectedLanguage == Language.Japanese ? "ja|en" : "cn|en");
            System.Diagnostics.Process.Start(googleTranslateURL);
        }

        private void btnLanguageJP_Click(object sender, EventArgs e)
        {
            SelectedLanguage = Language.Japanese;
            if (SearchText != "")
            {
                FillDictionary();
                FillExamples();
            }
        }

        private void btnLanguageCN_Click(object sender, EventArgs e)
        {
            SelectedLanguage = Language.Chinese;
            if(SearchText != "")
            {
                FillDictionary();
                FillExamples();
            }
        }
    }
}
