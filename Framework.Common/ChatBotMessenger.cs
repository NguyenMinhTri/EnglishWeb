using Framework.Model.Google;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common
{
    public class ChatBotMessenger
    {
        public static string getVocaNull()
        {
            return "&word1=&pron1=&meanvn1=&meanen1=&word2=&pron2=&meanvn2=&meanen2=&word3=&pron3=&meanvn3=&meanen3=&word4=&pron4=&meanvn4=&meanen4=&word5=&pron5=&meanvn5=&meanen5=";
        }
        public static string getNotiNull()
        {
            return "&title=&url=";
        }
        public static void sendTextMeg(string idMess, string contain)
        {
            MessJson messExplaintionDict = new MessJson();
            messExplaintionDict.text = contain;
            JsonMessengerText jsonTextMessenger = new JsonMessengerText();
            jsonTextMessenger.recipient.id = idMess;
            jsonTextMessenger.message = messExplaintionDict;
            sendRequest(JsonConvert.SerializeObject(jsonTextMessenger));
        }
        public static string sendRequest(string dataBody,bool IsMenuChatBot = false)
        {
            try
            {
                HttpWebRequest request;
                if(IsMenuChatBot)
                    request = (HttpWebRequest)WebRequest.Create("https://graph.facebook.com/v2.6/me/messenger_profile?access_token=EAACn86pGAioBAAJ8gqV2eRJPN5Yznq3rXG9az1IpesyWJTem3HlchCQNEfSfxQmDxMtlvBpyclx2CvLDf9Im2ZCUPVgzty3IURuxNJ2STjUZBvTVGprkNs7NjnGKLMbuu0ZAwr99cFtcSxHTSfpblqkiLYtkKbWUZBZBBMGDSGZBYcpUxxo3rp");

                else
                    request = (HttpWebRequest)WebRequest.Create("https://graph.facebook.com/v2.6/me/messages?access_token=EAACn86pGAioBAAJ8gqV2eRJPN5Yznq3rXG9az1IpesyWJTem3HlchCQNEfSfxQmDxMtlvBpyclx2CvLDf9Im2ZCUPVgzty3IURuxNJ2STjUZBvTVGprkNs7NjnGKLMbuu0ZAwr99cFtcSxHTSfpblqkiLYtkKbWUZBZBBMGDSGZBYcpUxxo3rp");
                request.ContentType = "application/json";
                request.Method = "POST";
                using (var requestWriter = new StreamWriter(request.GetRequestStream()))
                {
                    requestWriter.Write(dataBody);
                    requestWriter.Flush();
                    requestWriter.Close();
                }
                var response = (HttpWebResponse)request.GetResponse();
                if (response == null)
                    throw new InvalidOperationException("GetResponse returns null");

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
