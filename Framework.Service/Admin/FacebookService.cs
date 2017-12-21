using Framework.Common;
using Framework.Model;
using Framework.Model.Feeds;
using Framework.Model.Likes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service.Admin
{
    public class FacebookService
    {
        string[] nameDapAn = new string[4];
        int[] dapAn = new int[4];
        public FacebookService()
        {
            nameDapAn[0] = "A";
            nameDapAn[1] = "B";
            nameDapAn[2] = "C";
            nameDapAn[3] = "D";
            //
            dapAn[0] = 0;
            dapAn[1] = 0;
            dapAn[2] = 0;
            dapAn[3] = 0;
        }   
         
        public async Task<ImageFB> GetImageByPost(string id)
        {
            var url = "https://graph.facebook.com/v2.11/" + id + "?fields=full_picture&access_token=" + Token.Facebook;
            var httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<ImageFB>(json);
        }

        public async Task<ListPostFB> GetListFeedsOfGroup(string id = "1446400445645839")
        {
            var url = "https://graph.facebook.com/v2.11/" + id + "/feed?limit=20&access_token=" + Token.Facebook;
            var httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<ListPostFB>(json);
        }

        public async Task<ListCommentOfPost> GetCommentByPost(string id)
        {
            var url = "https://graph.facebook.com/v2.11/" + id + "/comments?access_token=" + Token.Facebook;
            var httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<ListCommentOfPost>(json);
        }
        //Xac dinh so like cua comment de truy ra giai thich nghia
        async Task<Likes> GetLikesByID(string id)
        {
            var url = "https://graph.facebook.com/v2.11/" + id + "/likes?access_token=" + Token.Facebook;
            var httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<Likes>(json);
        }
        //Xac dinh comment like cao nhat trong list 

        //Xac dinh dap an cho post
        public async Task<string> DetectedAnswOfPost(string postid)
        {
            string giaiThich = "";
            int maxLike = 0;         
            ListCommentOfPost listComment = await GetCommentByPost(postid);
            foreach (var cmt in listComment.data)
            {
                if (cmt.message == "a" || cmt.message == "A")
                    dapAn[0]++;
                else if (cmt.message == "b" || cmt.message == "B")
                    dapAn[1]++;
                else if (cmt.message == "c" || cmt.message == "C")
                    dapAn[2]++;
                else if (cmt.message == "d" || cmt.message == "D")
                    dapAn[3]++;
                //xac dinh max like of comment
                var likes = await GetLikesByID(cmt.id);
                if(likes.data.Count() > maxLike)
                {
                    maxLike = likes.data.Count();
                    giaiThich = cmt.message;
                }
            }
            //Xac dinh so lon nhat
            int maxNum = dapAn[0];
            string DapAn = nameDapAn[0];
            for (int i = 0; i < 4; i++)
            {
                if (dapAn[i] > maxNum)
                {
                    maxNum = dapAn[i];
                    DapAn = nameDapAn[i];
                }
            }

            return maxNum > 2 ? (DapAn +"#"+ giaiThich ): "";
        }

        //xac dinh bai post chi co text
        public async Task<ListPostFB> GetListFeedTextOfGroup(string id = "1446400445645839")
        {
            int numberSentences = 0;
            ListPostFB result = new ListPostFB();

            try
            {
                var url = "https://graph.facebook.com/v2.11/" + id + "/feed?limit=100&access_token=" + Token.Facebook;
                var httpClient = new HttpClient();
                string json = await httpClient.GetStringAsync(url);
                var listPostFB = JsonConvert.DeserializeObject<ListPostFB>(json);
                foreach (var postFB in listPostFB.data)
                {
                    var imageOfPost = await GetImageByPost(postFB.id);
                    //Bài post phải là text
                    if (imageOfPost.full_picture == null)
                    {
                        postFB.DapAn = await DetectedAnswOfPost(postFB.id);
                        int countDapAn = postFB.DapAn.Count();
                        if (countDapAn == 0)
                        {

                        }
                        else
                        {
                            result.data.Add(postFB);
                            numberSentences++;

                        }
                    }
                    //câu hỏi dạng hình ảnh
                    else
                    {
                        postFB.DapAn = await DetectedAnswOfPost(postFB.id);
                        int countDapAn = postFB.DapAn.Count();
                        postFB.imageURL = imageOfPost.full_picture;
                        if (countDapAn == 0)
                        {

                        }
                        else
                        {
                            result.data.Add(postFB);
                            numberSentences++;
                        }
                    }

                    dapAn[0] = 0;
                    dapAn[1] = 0;
                    dapAn[2] = 0;
                    dapAn[3] = 0;
                    if(numberSentences > 10)
                    {
                        break;
                    }
                }

            }
            catch (Exception e)
            {

            }
            return result;
        }
    }
}
