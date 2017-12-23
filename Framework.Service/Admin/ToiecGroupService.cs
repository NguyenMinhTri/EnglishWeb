using Framework.Model;
using Framework.Repository.RepositorySpace;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Model.Feeds;
using Newtonsoft.Json;
using System.Net.Http;
using Framework.Model.Likes;
using Framework.Common;

namespace Framework.Service.Admin
{
    public interface IToiecGroupService : IQlService<ToiecGroup>
    {
        Task<ListPostFB> GetListFeedsOfGroup(string id = "1446400445645839");
        Task<ImageFB> GetImageByPost(string id);
        Task<ListCommentOfPost> GetCommentByPost(string id);
        Task<Likes> GetLikesByID(string id);
        Task<string> DetectedAnswOfPost(string postid);
        Task<ListPostFB> GetListFeedTextOfGroup(string id = "1446400445645839");
        Task<ImageFB> PostingToGroupFB(string content);
        ListPostFB GetToiecExamList();
    }
    public class ToiecGroupService : QlService<ToiecGroup>, IToiecGroupService
    {
        string[] nameDapAn = new string[4];
        int[] dapAn = new int[4];
        IToiecGroupRepository _toiecGroupRepository;
        public ToiecGroupService(IToiecGroupRepository toiecGroupRepository, IUnitOfWork unitOfWork) : base(toiecGroupRepository, unitOfWork) 
        {
            this._repository = toiecGroupRepository;
            _toiecGroupRepository = toiecGroupRepository;
            //
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
            string json = "";
                while(true)
                {
                    try
                    {
                        json = httpClient.GetStringAsync(url).Result;
                        break;
                    }
                    catch (Exception e)
                    {

                    }
                };
            return JsonConvert.DeserializeObject<ImageFB>(json);
        }

        public async Task<ListPostFB> GetListFeedsOfGroup(string id = "1446400445645839")
        {
            var url = "https://graph.facebook.com/v2.11/" + id + "/feed?limit=20&access_token=" + Token.Facebook;
            var httpClient = new HttpClient();
            string json = "";
            while (true)
            {
                try
                {
                    json = httpClient.GetStringAsync(url).Result;
                    break;
                }
                catch (Exception e)
                {

                }
            };
            return JsonConvert.DeserializeObject<ListPostFB>(json);
        }

        public async Task<ListCommentOfPost> GetCommentByPost(string id)
        {
            string json = "";
            try
            {
                var url = "https://graph.facebook.com/v2.11/" + id + "/comments?access_token=" + Token.Facebook;
                var httpClient = new HttpClient();
                while (true)
                {
                    try
                    {
                        json = httpClient.GetStringAsync(url).Result;
                        break;
                    }
                    catch (Exception e)
                    {

                    }
                };
            }
            catch(Exception e)
            {

            }
            return JsonConvert.DeserializeObject<ListCommentOfPost>(json);
        }
        //Xac dinh so like cua comment de truy ra giai thich nghia
        public async Task<Likes> GetLikesByID(string id)
        {
            string json = "";
            try
            {
                var url = "https://graph.facebook.com/v2.11/" + id + "/likes?access_token=" + Token.Facebook;
                var httpClient = new HttpClient();
                while (true)
                {
                    try
                    {
                        json = httpClient.GetStringAsync(url).Result;
                        break;
                    }
                    catch (Exception e)
                    {

                    }
                };
            }
            catch(Exception e)
            {

            }
            return JsonConvert.DeserializeObject<Likes>(json);
        }
        //Xac dinh comment like cao nhat trong list 

        //Xac dinh dap an cho post
        public async Task<string> DetectedAnswOfPost(string postid)
        {
            // chỉ check đúng 10 cmt đầu
            int numberOfCmt = 0;
            //
            string giaiThich = "";
            int maxLike = 0;
            ListCommentOfPost listComment = await GetCommentByPost(postid);
            foreach (var cmt in listComment.data)
            {
                if (cmt.message == "a" || cmt.message == "A" || cmt.message.IndexOf("A") == 0 || cmt.message.IndexOf("a") == 0 || cmt.message.IndexOf("A.") == 0 || cmt.message.IndexOf("a.") == 0)
                    dapAn[0]++;
                else if (cmt.message == "b" || cmt.message == "B" || cmt.message.IndexOf("B") == 0 || cmt.message.IndexOf("b") == 0 || cmt.message.IndexOf("B.") == 0 || cmt.message.IndexOf("b.") == 0)
                    dapAn[1]++;
                else if (cmt.message == "c" || cmt.message == "C" || cmt.message.IndexOf("C") == 0 || cmt.message.IndexOf("c") == 0 || cmt.message.IndexOf("C.") == 0 || cmt.message.IndexOf("c.") == 0)
                    dapAn[2]++;
                else if (cmt.message == "d" || cmt.message == "D" || cmt.message.IndexOf("D") == 0 || cmt.message.IndexOf("d") == 0 || cmt.message.IndexOf("D.") == 0 || cmt.message.IndexOf("d.") == 0)
                    dapAn[3]++;
                //xac dinh max like of comment khi có đáp án
                if (dapAn[0] != 0 || dapAn[1] != 0 || dapAn[2] != 0 || dapAn[3] != 0)
                {
                    try
                    {
                        var likes = await GetLikesByID(cmt.id);
                        if (likes.data.Count() > maxLike)
                        {
                            maxLike = likes.data.Count();
                            giaiThich = cmt.message;
                        }
                    }
                    catch(Exception e)
                    {

                    }
                }
                numberOfCmt++;
                if (numberOfCmt > 10)
                {
                    break;
                }
            }
            //Nếu số cmt lớn hơn 5 ko lien qua đáp án thì bỏ qua bài post này
            if((numberOfCmt > 5 ) && (dapAn[0] == 0 && dapAn[1] == 0 && dapAn[2] == 0 && dapAn[3] == 0))
            {
                ToiecGroup item = new ToiecGroup();
                item.Id_Post = postid;
                Add(item);
                Save();
            }
            //Xac dinh so lon nhat
            int maxNum = dapAn[0];
            string DapAn = nameDapAn[0];
            //
            if (dapAn[0] != 0 || dapAn[1] != 0 || dapAn[2] != 0 || dapAn[3] != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (dapAn[i] > maxNum)
                    {
                        maxNum = dapAn[i];
                        DapAn = nameDapAn[i];
                    }
                }
            }
            return maxNum > 2 ? (DapAn + "#" + giaiThich) : "";
        }

        //xac dinh bai post chi co text
        public async Task<ListPostFB> GetListFeedTextOfGroup(string id = "1446400445645839")
        {
            int numberSentences = 0;
            ListPostFB result = new ListPostFB();
            //
            try
            {
                var url = "https://graph.facebook.com/v2.11/" + id + "/feed?limit=30&access_token=" + Token.Facebook;
                var httpClient = new HttpClient();
                string json = "";
                while(true)
                {
                    try
                    {
                        json = httpClient.GetStringAsync(url).Result;
                        break;
                    }
                    catch (Exception e)
                    {

                    }
                }
                var listPostFB = JsonConvert.DeserializeObject<ListPostFB>(json);
                foreach (var postFB in listPostFB.data)
                {
                    var postData = GetAll().Where(x => x.Id_Post == postFB.id ).FirstOrDefault();
                    //ko kiem tra lại dữ liệu sau khi lưu vào database
                    if (postData != null)
                    {
                        if (postData.DapAn == null || postData.DapAn == "")
                            continue;
                        Model.Feeds.Datum temp = new Model.Feeds.Datum();
                        temp.id = postData.Id_Post;
                        temp.imageURL = postData.ImageUrl;
                        temp.message = postData.Content;
                        temp.DapAn = postData.DapAn;
                        temp.GiaiThich = postData.GiaiThich;
                        result.data.Add(temp);
                        numberSentences++;
                        if (numberSentences > 10)
                        {
                            break;
                        }
                        continue;
                    }
                        

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
                            //Add to database
                            ToiecGroup item = new ToiecGroup();
                            item.Content = postFB.message;
                            item.Id_Post = postFB.id;
                            item.DapAn = postFB.DapAn.Split('#').FirstOrDefault();
                            item.GiaiThich = postFB.DapAn.Split('#').LastOrDefault();
                            //
                            postFB.DapAn = item.DapAn;
                            postFB.GiaiThich = item.GiaiThich;
                            
                            //
                            Add(item);
                            Save();
                            //
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
                            //Add to database
                            ToiecGroup item = new ToiecGroup();
                            item.Content = postFB.message;
                            item.ImageUrl = postFB.imageURL;
                            item.Id_Post = postFB.id;
                            item.DapAn = postFB.DapAn.Split('#').FirstOrDefault();
                            item.GiaiThich = postFB.DapAn.Split('#').LastOrDefault();
                            Add(item);
                            Save();
                            //
                            result.data.Add(postFB);
                            numberSentences++;
                        }
                    }

                    dapAn[0] = 0;
                    dapAn[1] = 0;
                    dapAn[2] = 0;
                    dapAn[3] = 0;
                    if (numberSentences > 9)
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
        //Post bài lên nhóm fb
        public async Task<ImageFB> PostingToGroupFB(string content)
        {
            HttpResponseMessage reponse = new HttpResponseMessage();
            var url = "https://graph.facebook.com/v2.11/"+Token.GroupID+"/feed?message=" + content + "&access_token=" + Token.Facebook;
            var httpClient = new HttpClient();
            while(true)
            {
                try
                {
                    reponse = await httpClient.PostAsync(url, null);
                    break;
                }
                catch
                {

                }
            }
            var contents = await reponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ImageFB>(contents);
        }
        //Get 10 toiec sentence
        public ListPostFB GetToiecExamList()
        {
            ListPostFB result = new ListPostFB();
            var listAllToiecExam = GetAll().Where(x => x.DapAn != null && x.DapAn !="");
            var tempToiec = listAllToiecExam.Skip(Math.Max(0, listAllToiecExam.Count() - 10)).ToList();
            foreach(var postData in tempToiec)
            {
                Model.Feeds.Datum temp = new Model.Feeds.Datum();
                temp.id = postData.Id_Post;
                temp.imageURL = postData.ImageUrl;
                temp.message = postData.Content;
                temp.DapAn = postData.DapAn;
                temp.GiaiThich = postData.GiaiThich;
                result.data.Add(temp);
            }
            return result;
        }
    }
}
