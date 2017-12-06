using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.Model
{
    [Table("AspNetUsers")]
    public class ApplicationUser : 
        IdentityUser <string, IdentityUserLogin, ApplicationUserRole, IdentityUserClaim>, IUser
        , IAuditable
    {
        //TODO 1_ThemBang: st1. Tao 1 lop tuong tu nhu lop nay
        public DateTime? CreatedDate { set; get; }
        [MaxLength(256)]
        public string CreatedBy { set; get; }
        public DateTime? UpdatedDate { set; get; }
        [MaxLength(256)]
        public string UpdatedBy { set; get; }
        [DefaultValue(true)]
        public bool Status { set; get; }
        public bool Protected { get; set; }

        //TODO 2_CSCot: st1. Sua 1 truong trong lop can thay doi csdl
        public String Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public bool Logined { get; set; }
        public String About { get; set; }
        public String Level { get; set; }
        public DateTime? Birthday { get; set; }
        public String Address { get; set; }
        public String Career { get; set; }
        public String Organization { get; set; }
        public bool Sex { get; set; }
        public bool? UserStatus { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Facebook { get; set; }
        public String Google { get; set; }
        public String Hobby { get; set; }
        public String FavoriteShow { get; set; }
        public String FavoriteFilm { get; set; }
        public String FavoriteGame { get; set; }
        public String FavoriteArtist { get; set; }
        public String FavoriteBook { get; set; }
        public String FavoriteAuthor { get; set; }
        public String Avatar { get; set; }
        public float Score { get; set; }
        public String Id_Messenger { get; set; }
        public String Degree { get; set; }

        /// <summary>
        ///     Constructor which creates a new Guid for the Id
        /// </summary>
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        ///     Constructor that takes a userName
        /// </summary>
        /// <param name="userName"></param>
        public ApplicationUser(string userName)
            : this()
        {
            UserName = userName;
        }
    }
}
