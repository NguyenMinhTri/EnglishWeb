using Framework.Common;
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
            try
            {
                string paramChatfuel = "";
                // DetailOurWord detailOut = detailOutWord.GetById(1);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response2 = await client.PostAsync("http://olympusenglish.azurewebsites.net/Dictionary/notifyMessenger", null);
                if (JobScheduler.ToiecUpdatedTime == 0)
                    await client.PostAsync("http://olympusenglish.azurewebsites.net/Learning/autoGeneratorToiecExam", null);
                if (response2.Content != null)
                {
                    // Error Here
                    var responseContent = await response2.Content.ReadAsStringAsync();
                    ListUserNofity listNofity = JsonConvert.DeserializeObject<ListUserNofity>(responseContent);
                    foreach (var userNoti in listNofity.reminduser)
                    {
                        paramChatfuel = "https://api.chatfuel.com/bots/59a43f64e4b03a25b73c0ebd/users/" + userNoti.IdMess + "/" + "send?chatfuel_token=vnbqX6cpvXUXFcOKr5RHJ7psSpHDRzO1hXBY8dkvn50ZkZyWML3YdtoCnKH7FSjC&chatfuel_block_id=5a28393be4b0d0e6fdab76b3";
                        for (int i = 1; i <= 5; i++)
                        {
                            paramChatfuel += "&word" + i.ToString() + "=" + ((i <= userNoti.vocainfo.Count) ? userNoti.vocainfo[i - 1].voca : "");
                            paramChatfuel += "&pron" + i.ToString() + "=" + ((i <= userNoti.vocainfo.Count) ? userNoti.vocainfo[i - 1].pron : "");
                            paramChatfuel += "&meanvn" + i.ToString() + "=" + ((i <= userNoti.vocainfo.Count) ? userNoti.vocainfo[i - 1].meanVN : "");
                            paramChatfuel += "&meanen" + i.ToString() + "=" + ((i <= userNoti.vocainfo.Count) ? userNoti.vocainfo[i - 1].usecase : "");
                        }
                        paramChatfuel += ChatBotMessenger.getNotiNull();
                        var response = await client.PostAsync(paramChatfuel, null);
                    }
                }
                //ListUserNofity listNofity = JsonConvert.DeserializeObject<ListUserNofity>(response2.ToString());
                  
                await client.PostAsync("http://olympusenglish.azurewebsites.net/Home/checkTimeOfPost", null);
                await client.PostAsync("http://olympusenglish.azurewebsites.net/Post/CheckAnswerOnFB", null);
                if (JobScheduler.ToiecUpdatedTime >= 15)
                {
                    JobScheduler.ToiecUpdatedTime = 0;
                }
                JobScheduler.ToiecUpdatedTime++;
            }
            catch
            {

            }

        }
    }
}