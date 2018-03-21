﻿using System;
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
            RegisterHotKey(this.Handle, 0, (int)KeyModifier.Control | (int)KeyModifier.Alt, Keys.C.GetHashCode());

            ListViewItem item1 = new ListViewItem(new[] { "Test", "English" });
            listExampleSentences.Items.Add(item1);

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            MessageBox.Show("Press CTRL+C to copy text to clipboard.\nPress CTRL+ALT+C to show dictionary entry foc the copied text.", "How to use");
        }

        /// <summary>
        /// Fills dictionary with translation and reading (translit).
        /// </summary>
        private void FillDictionary()
        {
            bool textIsANSI = true;
            foreach(char c in SearchText)
            {
                if(c >= 0x00ff)
                {
                    textIsANSI = false;
                    break;
                }
            }

            string selectedLanguageCode = SelectedLanguage == Language.Japanese ? "ja" : "cn";
            string languageSearchCode = textIsANSI ? "en|" + selectedLanguageCode : selectedLanguageCode + "|en";

            string googleTranslateURL = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", SearchText, languageSearchCode);

            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            if (SelectedLanguage == Language.Chinese)
            {
                // Google translate uses iso-8859-1 for Chinese pinyin encoding.
                encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            }
            else
            {
                encoding = System.Text.Encoding.GetEncoding("Shift_JIS");
            }

            WebClient webClient = new WebClient();
            webClient.Encoding = encoding;

            string googleResult;
            try
            {
                byte[] htmlData = webClient.DownloadData(googleTranslateURL);
                googleResult = encoding.GetString(htmlData);
            }
            catch (Exception ex)
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

        /// <summary>
        /// Fills example-tab with list of examples.
        /// </summary>
        private async void FillExamples()
        {
            listExampleSentences.Items.Clear();

            if (SelectedLanguage == Language.Japanese)
            {
                listExampleSentences.Columns[0].Text = "Japanese";
                listExampleSentences.Columns[1].Text = "English";
            }
            else if (SelectedLanguage == Language.Chinese)
            {
                listExampleSentences.Columns[0].Text = "中文";
                listExampleSentences.Columns[1].Text = "日本語";
            }

            List<string> sourceTexts = new List<string>();
            List<string> translatedTexts = new List<string>();

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
                    sourceTexts.Add(str);
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
                        str = Regex.Replace(str, "&(.*?);", "");
                        startIndex = str.IndexOf('<');
                    }
                    translatedTexts.Add(str);
                }
            });

            for (int i = 0; i < sourceTexts.Count() && i < translatedTexts.Count(); i++)
            {
                ListViewItem item1 = new ListViewItem(new[] { sourceTexts[i], translatedTexts[i] });
                listExampleSentences.Items.Add(item1);
            }
        }

        private async void FillKanjiPage()
        {
            dataViewKanji.Rows.Clear();

            if (SelectedLanguage == Language.Chinese)
            {
                dataViewKanji.Columns[0].Width = 250;
            }

            List<Image> images = new List<Image>();
            List<String> texts = new List<string>();

            await Task.Run(() =>
            {
                foreach (char c in SearchText)
                {
                    string kanjiURL;
                    if (SelectedLanguage == Language.Japanese)
                        kanjiURL = String.Format("http://kanji.nihongo.cz/image.php?text={0}&font=sod.ttf&fontsize=300", c);
                    else
                        kanjiURL = String.Format("http://cdn.yabla.com/chinese_static/strokes/{0}-bw.png", c);

                    Uri uriResult;
                    bool isValidURL = Uri.TryCreate(kanjiURL, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
                    if (isValidURL)
                    {
                        byte[] imageData = (new WebClient()).DownloadData(uriResult);
                        MemoryStream stream = new MemoryStream(imageData);
                        Image img = Image.FromStream(stream);

                        images.Add(img);
                        texts.Add("TODO");
                    }
                }
            });

            for (int i = 0; i < images.Count; i++)
                dataViewKanji.Rows.Add();

            for(int i = 0; i < images.Count; i++)
            {
                dataViewKanji.Rows[i].Cells[0].Value = images[i];
                dataViewKanji.Rows[i].Cells[1].Value = texts[i];       
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312) // WM_HOTKEY
            {
                SearchText =  Clipboard.GetText();
                
                if(SearchText != "")
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
                    FillKanjiPage();
                }
            }
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
                FillKanjiPage();
            }
        }

        private void btnLanguageCN_Click(object sender, EventArgs e)
        {
            SelectedLanguage = Language.Chinese;
            if(SearchText != "")
            {
                FillDictionary();
                FillExamples();
                FillKanjiPage();
            }
        }

        private void btnStrokeOrder_Click(object sender, EventArgs e)
        {
            string strokeOrderURL;
            if (SelectedLanguage == Language.Japanese)
            {
                strokeOrderURL = String.Format("http://jisho.org/search/%23kanji {0}", SearchText);
            }
            else
            {
                strokeOrderURL = String.Format("https://www.yellowbridge.com/chinese/character-stroke-order.php?word={0}", SearchText);
            }
            System.Diagnostics.Process.Start(strokeOrderURL);
        }
    }
}
