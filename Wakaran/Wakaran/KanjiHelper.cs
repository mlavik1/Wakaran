using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wakaran
{
    class KanjiSearchResult
    {
        public Image mStrokeOrderImage = null;
        public string mDescription = "";
    }

    class KanjiInfo
    {
        public string mOnYomi = "";
        public string mKunYomi = "";
    }

    class KanjiHelper
    {
        private Dictionary<string, KanjiInfo> mKanjiInfos = new Dictionary<string, KanjiInfo>();

        public async void LoadKanjiInfo()
        {
            await Task.Run(() =>
            {
                var assm = Assembly.GetExecutingAssembly();
                using (var stream = assm.GetManifestResourceStream("Wakaran.Resources.KanjiInfo.txt"))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(';');
                            if (parts.Count() >= 20)
                            {
                                KanjiInfo kanjiInfo = new KanjiInfo();
                                kanjiInfo.mOnYomi = parts[14];
                                kanjiInfo.mKunYomi = parts[19];
                                mKanjiInfos[parts[1]] = kanjiInfo;
                            }
                        }
                    }
                }
            });
        }

        public KanjiSearchResult GetKanjiInfo(char inKanji, Language inLanguage)
        {
            KanjiSearchResult kanjiSearchResult = new KanjiSearchResult();

            string kanjiURL;
            if (inLanguage == Language.Japanese)
                kanjiURL = String.Format("http://kanji.nihongo.cz/image.php?text={0}&font=sod.ttf&fontsize=300", inKanji);
            else
                kanjiURL = String.Format("http://cdn.yabla.com/chinese_static/strokes/{0}-bw.png", inKanji);

            Uri uriResult;
            bool isValidURL = Uri.TryCreate(kanjiURL, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
            if (isValidURL)
            {
                try
                {
                    byte[] imageData = (new WebClient()).DownloadData(uriResult);
                    MemoryStream stream = new MemoryStream(imageData);
                    kanjiSearchResult.mStrokeOrderImage = Image.FromStream(stream);
                }
                catch (System.Net.WebException ex)
                {
                    return null;
                }
            }

            if(inLanguage == Language.Japanese)
            {
                if (mKanjiInfos.ContainsKey(inKanji.ToString()))
                {
                    KanjiInfo kanjiInfo = mKanjiInfos[inKanji.ToString()];
                    kanjiSearchResult.mDescription = "On yomi: " + kanjiInfo.mOnYomi + "\nKun yomi: " + kanjiInfo.mKunYomi;
                }
            }

            return kanjiSearchResult;
        }
    }
}
