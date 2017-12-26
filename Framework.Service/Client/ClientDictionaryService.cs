using Framework.Common;
using Framework.Model;
using Framework.Model.Google;
using Framework.Repository.RepositorySpace;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service.Client
{
    public interface IClientDictionaryService
    {
        Task<OxfordDict> startCrawlerOxford(string voca);
        Task<ExampleTraCau> startCrawlerTraCau(string voca);
        Task<GoogleTrans> startGoogleTrans(string voca);
        Task<GoogleTrans> startGoogleDetailTrans(string voca);
        //void GoogleTranslator(string voca);
    }
    class ClientDictionaryService : IClientDictionaryService
    {
        OxfordDict m_oxfordDict;
        public ClientDictionaryService(
            )
        {
            m_oxfordDict = new OxfordDict();
        }
        public async Task<OxfordDict> startCrawlerOxford(string voca="welcome")
        {

            //
            m_oxfordDict.m_Voca = voca;
            var url = "https://www.oxfordlearnersdictionaries.com/definition/english/"+voca;
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            GetPronAndSoundOfOxford(htmlDocument.DocumentNode);
            var temps = htmlDocument.DocumentNode.Descendants("span").Where(node => node.GetAttributeValue("class", "").Equals("sn-gs")).ToList();
            foreach (var tmp in temps)
            {
                GetExampleListOxFord(tmp);
            }
            return m_oxfordDict;
        }
        private string GetPronAndSoundOfOxford(HtmlNode htmlNode)
        {
            try
            {
                m_oxfordDict.m_Pron = htmlNode.Descendants("span").Where(node => node.GetAttributeValue("class", "").Equals("phon")).FirstOrDefault().InnerText;
                m_oxfordDict.m_Type = htmlNode.Descendants("span").Where(node => node.GetAttributeValue("class", "").Equals("pos")).FirstOrDefault().InnerText;
                m_oxfordDict.m_SoundUrl = htmlNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Equals("sound audio_play_button pron-us icon-audio")).FirstOrDefault().ChildAttributes("data-src-mp3").FirstOrDefault().Value;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return null;
        }
        public void GetExampleListOxFord(HtmlNode Node)
        {
            Explanation explanation = new Explanation();
            try
            {
                var temp = Node.Descendants("li").Where(node => node.GetAttributeValue("class", "").Equals("sn-g")).ToList().FirstOrDefault();
                var exampleList = temp.Descendants("span").Where(node => node.GetAttributeValue("class", "").Equals("def")).FirstOrDefault();
                //Set value for usecase
                explanation.m_UseCase = StringHelper.FirstLetterUpper(exampleList.InnerText);
                //Get a example for usecase
                List<HtmlNode> exampleList2 = temp.Descendants("span").Where(node => node.GetAttributeValue("class", "").Equals("x-g")).ToList();
                foreach (var exam in exampleList2)
                {
                    explanation.m_ListExample.Add(exam.InnerText);
                }
                m_oxfordDict.m_Explanation.Add(explanation);
            }
            catch (Exception e)
            {
                return;
            }
            //public 
        }
        //Tracau.vn
        public async Task<ExampleTraCau> startCrawlerTraCau(string voca)
        {
            string tempTracau = "/ev%7Cdi%7Ccn%7Caa/en";
            var url = "https://api.tracau.vn/WBBcwnwQpV89/s/" + voca + tempTracau;
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            ExampleTraCau exTracau = JsonConvert.DeserializeObject<ExampleTraCau>(html);
            for(int idTracau = 0;idTracau< exTracau.sentences.Count;idTracau++)
            {
                exTracau.sentences[idTracau].fields.en = exTracau.sentences[idTracau].fields.en;
                exTracau.sentences[idTracau].fields.en = exTracau.sentences[idTracau].fields.en;
            }

            return exTracau;
        }
        //Google translator
        public async Task<GoogleTrans> startGoogleTrans(string voca)
        {
            string result = "";
            var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=vi&hl=en-US&dt=t&dt=bd&dj=1&source=bubble&tk=908123.908123&q="+ voca;
            var webRequest = WebRequest.Create(url);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                 result = reader.ReadToEnd();
            }
            GoogleTrans googleTransJson = JsonConvert.DeserializeObject<GoogleTrans>(result);
            return googleTransJson;
        }
        //Google detail translator 
        //Google translator
        public async Task<GoogleTrans> startGoogleDetailTrans(string voca)
        {
            string result = "";
            var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=vi&hl=vi&dt=t&dt=bd&dj=1&source=icon&tk=827698.827698&q=" + voca;
            var webRequest = WebRequest.Create(url);
            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                result = reader.ReadToEnd();
            }
            GoogleTrans googleTransJson = JsonConvert.DeserializeObject<GoogleTrans>(result);
            return googleTransJson;
        }
    }
}
