using Framework.Model;
using Framework.Service.Admin;
using Framework.ViewData.Admin.GetData;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

namespace Framework.Schedule
{
    public class StudyDictJob :IJob
    {
        
        public async void Execute(IJobExecutionContext context)
        {
            string paramChatfuel = "";
           // DetailOurWord detailOut = detailOutWord.GetById(1);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response2 = await client.PostAsync("http://localhost:20000/Dictionary/notifyMessenger", null);
            if (response2.Content != null)
            {
                // Error Here
                var responseContent = await response2.Content.ReadAsStringAsync();
                ListUserNofity listNofity = JsonConvert.DeserializeObject<ListUserNofity>(responseContent);
                foreach(var userNoti in listNofity.reminduser)
                {
                    paramChatfuel = "https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/"+ userNoti.IdMess+"/"+ "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=59a43f64e4b03a25b73c0f23";
                    int i = 1;
                    foreach (var vocaID in userNoti.vocainfo)
                    {
                        paramChatfuel += "&word" + i.ToString() + "=" + vocaID.voca;
                        paramChatfuel += "&pron" + i.ToString() + "=" + vocaID.pron;
                        paramChatfuel += "&meanvn" + i.ToString() + "=" + vocaID.meanVN;
                        paramChatfuel += "&meanen" + i.ToString() + "=" + vocaID.usecase;
                        i++;
                    }
                }
            }
            //ListUserNofity listNofity = JsonConvert.DeserializeObject<ListUserNofity>(response2.ToString());
            var response = await client.PostAsync(paramChatfuel, null);
        }
    }
}