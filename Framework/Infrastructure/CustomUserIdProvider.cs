using Framework.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Infrastructure
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            try
            {
                ApplicationDbContext dbContext = new ApplicationDbContext();
                var lst = dbContext.Users.Where(x => x.UserName == request.User.Identity.Name).ToList();
                if (lst.Count > 0)
                {
                    return lst[0].Id;
                }
            }
            catch
            {
                return "";
            }
            return "";
        }
    }
}