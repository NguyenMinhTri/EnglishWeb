using Framework.Model;
using Framework.Model.TableJoin;
using Framework.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Framework.Repository.RepositorySpace
{
    public interface IApplicationUserRoleRepository : IRepository<ApplicationUserRole>
    {
        IEnumerable<ApplicationUserRoleGetAll> GetAllWithDisplay();
        IEnumerable<ApplicationUserRoleGetAll> GetAllWithDisplayByUserId(String userId);
        IEnumerable<ApplicationRole> GetRolesExceptInUserIdCombobox(String userId);
        List<String> GetRolesOfUser(String userId);
    }
    public class ApplicationUserRoleRepository : BaseRepository<ApplicationUserRole>, IApplicationUserRoleRepository
    {
        public ApplicationUserRoleRepository(IDbFactory dbFactory)
            : base(dbFactory)
        {
        }

        public IEnumerable<ApplicationUserRoleGetAll> GetAllWithDisplay()
        {
            return from aUR in dataContext.Set<ApplicationUserRole>()
                   join aR in dataContext.Set<ApplicationRole>() on aUR.RoleId equals aR.Id
                   join aU in dataContext.Set<ApplicationUser>() on aUR.UserId equals aU.Id
                   select new ApplicationUserRoleGetAll()
                   {
                       Id = aUR.Id,
                       ApplicationRole_Id = aUR.RoleId,
                       ApplicationUser_Id = aUR.UserId,
                       RoleDisplay = aR.Name,
                       UserDisplay = aU.UserName,
                       CreatedDate = aUR.CreatedDate,
                       CreatedBy = aUR.CreatedBy,
                       UpdatedDate = aUR.UpdatedDate,
                       UpdatedBy = aUR.UpdatedBy,
                       Status = aUR.Status
                   };
        }


        public IEnumerable<ApplicationUserRoleGetAll> GetAllWithDisplayByUserId(string userId)
        {
            return from aUR in dataContext.Set<ApplicationUserRole>()
                   join aR in dataContext.Set<ApplicationRole>() on aUR.RoleId equals aR.Id
                   where aUR.UserId == userId
                   select new ApplicationUserRoleGetAll()
                   {
                       Id = aUR.Id,
                       ApplicationRole_Id = aUR.RoleId,
                       ApplicationUser_Id = userId,
                       RoleDisplay = aR.Name,
                       CreatedDate = aUR.CreatedDate,
                       CreatedBy = aUR.CreatedBy,
                       UpdatedDate = aUR.UpdatedDate,
                       UpdatedBy = aUR.UpdatedBy,
                       Status = aUR.Status
                   };
        }


        public IEnumerable<ApplicationRole> GetRolesExceptInUserIdCombobox(string userId)
        {
            var aURTable = dataContext.Set<ApplicationUserRole>();
            var aRTable = dataContext.Set<ApplicationRole>() ;
            return from aR in aRTable
                   where !(from aUR in aURTable where aUR.UserId == userId select aUR.RoleId).Distinct().
                   Contains(aR.Id)
                   select aR;
        }


        public List<string> GetRolesOfUser(string userId)
        {
            return GetAll().Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
        }
    }
}
