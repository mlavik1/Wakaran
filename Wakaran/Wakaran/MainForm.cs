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

        TranslationHelper mTranslationHelper = new TranslationHelper();
        ExampleSentenceHelper mExampleSentenceHelper = new ExampleSentenceHelper();

        string mSearchText = "";

        TimeSpan mTimeAtActivation = DateTime.Now.TimeOfDay;

        Language mSelectedLanguage = Language.Japanese;

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
            TranslationRresult translationResult = mTranslationHelper.GetTranslation(mSearchText, mSelectedLanguage);

            this.txtSearchText.Text = translationResult.mSourceText;
            this.txtRomaji.Text = translationResult.mTranslitText;
            this.txtEnglish.Text = translationResult.mTranslatedText;
        }

        /// <summary>
        /// Fills example-tab with list of examples.
        /// </summary>
        private async void FillExamples()
        {
            listExampleSentences.Items.Clear();

            if (mSelectedLanguage == Language.Japanese)
            {
                listExampleSentences.Columns[0].Text = "Japanese";
                listExampleSentences.Columns[1].Text = "English";
            }
            else if (mSelectedLanguage == Language.Chinese)
            {
                listExampleSentences.Columns[0].Text = "中文";
                listExampleSentences.Columns[1].Text = "日本語";
            }

            ExampleSearchResult exampleSearchResult = null;
            await Task.Run(() =>
            {
                exampleSearchResult = mExampleSentenceHelper.GetExampleSentences(mSearchText, mSelectedLanguage);
            });

            if(exampleSearchResult != null)
            {
                foreach(ExampleSentence exampleSentence in exampleSearchResult.mExampleSentences)
                {
                    ListViewItem item1 = new ListViewItem(new[] { exampleSentence.mSourceText, exampleSentence.mTargetText });
                    listExampleSentences.Items.Add(item1);
                }
            }
        }

        private async void FillKanjiPage()
        {
            dataViewKanji.Rows.Clear();

            if (mSelectedLanguage == Language.Chinese)
            {
                dataViewKanji.Columns[0].Width = 250;
            }
            else
            {
                dataViewKanji.Columns[0].Width = 180;
            }

            List<Image> images = new List<Image>();
            List<String> texts = new List<string>();

            await Task.Run(() =>
            {
                foreach (char c in mSearchText)
                {
                    string kanjiURL;
                    if (mSelectedLanguage == Language.Japanese)
                        kanjiURL = String.Format("http://kanji.nihongo.cz/image.php?text={0}&font=sod.ttf&fontsize=300", c);
                    else
                        kanjiURL = String.Format("http://cdn.yabla.com/chinese_static/strokes/{0}-bw.png", c);

                    Uri uriResult;
                    bool isValidURL = Uri.TryCreate(kanjiURL, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
                    if (isValidURL)
                    {
                        try
                        {
                            byte[] imageData = (new WebClient()).DownloadData(uriResult);
                            MemoryStream stream = new MemoryStream(imageData);
                            Image img = Image.FromStream(stream);

                            images.Add(img);
                            texts.Add("TODO");
                        }
                        catch (System.Net.WebException ex)
                        {
                        }
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
                mSearchText =  Clipboard.GetText();
                
                if(mSearchText != "")
                {
                    mTimeAtActivation = DateTime.Now.TimeOfDay;
                    this.Visible = true;
                    this.WindowState = FormWindowState.Normal;
                    //this.TopMost = true;
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
            if (this.Visible && (DateTime.Now.TimeOfDay - mTimeAtActivation).Seconds > 1.0f && GetForegroundWindow() != this.Handle)
            {
                this.Visible = false;
            }
        }

        private void btnLinkWeblio_Click(object sender, EventArgs e)
        {
            string weblioURL = String.Format("https://{0}.weblio.jp/sentence/content/{1}", mSelectedLanguage == Language.Japanese ? "ejje" : "cjjc", mSearchText);
            System.Diagnostics.Process.Start(weblioURL);
        }

        private void btnLinkGoogleTrans_Click(object sender, EventArgs e)
        {
            string googleTranslateURL = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", mSearchText, mSelectedLanguage == Language.Japanese ? "ja|en" : "cn|en");
            System.Diagnostics.Process.Start(googleTranslateURL);
        }

        private void btnLanguageJP_Click(object sender, EventArgs e)
        {
            mSelectedLanguage = Language.Japanese;
            if (mSearchText != "")
            {
                FillDictionary();
                FillExamples();
                FillKanjiPage();
            }
        }

        private void btnLanguageCN_Click(object sender, EventArgs e)
        {
            mSelectedLanguage = Language.Chinese;
            if(mSearchText != "")
            {
                FillDictionary();
                FillExamples();
                FillKanjiPage();
            }
        }

        private void btnStrokeOrder_Click(object sender, EventArgs e)
        {
            string strokeOrderURL;
            if (mSelectedLanguage == Language.Japanese)
            {
                strokeOrderURL = String.Format("http://jisho.org/search/%23kanji {0}", mSearchText);
            }
            else
            {
                strokeOrderURL = String.Format("https://www.yellowbridge.com/chinese/character-stroke-order.php?word={0}", mSearchText);
            }
            System.Diagnostics.Process.Start(strokeOrderURL);
        }

        private void listExampleSentences_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            SetExampleToolTip(e.Item);
        }

        private async void SetExampleToolTip(ListViewItem item)
        {
            TranslationRresult translationResult = null;
            await Task.Run(() =>
            {
                translationResult = mTranslationHelper.GetTranslation(item.Text, mSelectedLanguage);
            });

            if(translationResult != null && translationResult.mTranslitText != "")
            {
                item.ToolTipText = translationResult.mTranslitText;
            }
        }

    }
}
