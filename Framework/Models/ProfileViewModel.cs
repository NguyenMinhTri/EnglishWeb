using Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewModels
{
    public class HeaderViewModel : LayoutViewModel, IRef<ProfileController>
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Degree { get; set; }
        public String Avatar { get; set; }
        public String LastName { get; set; }
    }

    public class NewsFeedViewModel : LayoutViewModel, IRef<ProfileController>
    {
        public String LastName { get; set; }
        public String Name { get; set; }
        public String About { get; set; }
        public String AboutToString
        {
            get
            {
                if (About == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return About;
                }
            }
        }
        public String Facebook { get; set; }
        public String Degree { get; set; }
        public String Avatar { get; set; }
    }

    public class AboutViewModel : LayoutViewModel, IRef<ProfileController>
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String PhoneToString
        {
            get
            {
                if (Phone == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return Phone;
                }
            }
        }
        public DateTime? Birthday { get; set; }
        public bool Sex { get; set; }
        public String SexToString
        {
            get
            {
                if (Sex)
                {
                    return "Nam";
                }
                else
                {
                    return "Nữ";
                }
            }
        }
        public bool? UserStatus { get; set; }
        public String UserStatusToString
        {
            get
            {
                if (UserStatus.ToString().Contains("true") || UserStatus == null)
                {
                    return "Còn FA";
                }
                else
                {
                    return "Đã có chủ";
                }
            }
        }
        public String Career { get; set; }
        public String CareerToString
        {
            get
            {
                if (Career == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return Career;
                }
            }
        }
        public String Organization { get; set; }
        public String OrganizationToString
        {
            get
            {
                if (Organization == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return Organization;
                }
            }
        }
        public String Address { get; set; }
        public String AddressToString
        {
            get
            {
                if (Address == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return Address;
                }
            }
        }
        public String About { get; set; }
        public String AboutToString
        {
            get
            {
                if (About == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return About;
                }
            }
        }
        public String Hobby { get; set; }
        public String HobbyToString
        {
            get
            {
                if (Hobby == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return Hobby;
                }
            }
        }
        public String FavoriteShow { get; set; }
        public String FavoriteShowToString
        {
            get
            {
                if (FavoriteShow == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return FavoriteShow;
                }
            }
        }
        public String FavoriteFilm { get; set; }
        public String FavoriteFilmToString
        {
            get
            {
                if (FavoriteFilm == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return FavoriteFilm;
                }
            }
        }
        public String FavoriteGame { get; set; }
        public String FavoriteGameToString
        {
            get
            {
                if (FavoriteGame == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return FavoriteGame;
                }
            }
        }
        public String FavoriteBook { get; set; }
        public String FavoriteBookToString
        {
            get
            {
                if (FavoriteBook == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return FavoriteBook;
                }
            }
        }
        public String FavoriteArtist { get; set; }
        public String FavoriteArtistToString
        {
            get
            {
                if (FavoriteArtist == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return FavoriteArtist;
                }
            }
        }
        public String FavoriteAuthor { get; set; }
        public String FavoriteAuthorToString
        {
            get
            {
                if (FavoriteAuthor == null)
                {
                    return "Chưa có thông tin";
                }
                else
                {
                    return FavoriteAuthor;
                }
            }
        }
        public String Facebook { get; set; }
        public DateTime? CreatedDate { set; get; }
    }

    public class FriendViewModel : LayoutViewModel, IRef<ProfileController>
    {
        public String LastName { get; set; }
    }

}