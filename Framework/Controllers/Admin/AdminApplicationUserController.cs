using Framework.Model;
using Framework.ViewData.Admin.GetData;
using Framework.ViewData.Admin.Input;
using Framework.ViewData.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Framework.Service.Admin;
using Framework.Service;
using System.Reflection;

namespace Framework.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminApplicationUserController : AdminBaseController<ApplicationUser, ApplicationUserInputAdminViewData>
    {
        IApplicationUserService _applicationUserService;

        public AdminApplicationUserController(IApplicationUserService applicationUserService
            , ILayoutAdminService layoutService)
            : base(applicationUserService, layoutService)
        {
            _applicationUserService = applicationUserService;
        }
        //
        // GET: /AdminApplicationUser/
        public ActionResult Index()
        {
            CreateLayoutAdminView();
            AutoForm<ApplicationUserInputAdminViewData> applicationUsers = new AutoForm<ApplicationUserInputAdminViewData>();
            ViewBag.Forms = applicationUsers.GetControls();
            return View();
        }
        [HttpGet]
        public override JsonResult Delete(ViewData.Admin.DeleteViewData obj)
        {
            return Json(new
            {
                result = "failed",
                message = "failed"
            });
        }

        [HttpPost]
        public new JsonResult Delete(ViewData.Admin.DeleteWithStringIdViewData obj)
        {
            var entity = _layoutService.GetUserById(obj.Id);
            if (entity.Protected)
            {
                return Json(new
                {
                    result = "failed",
                    message = "this record can't delete"
                });
            }
            entity.Logined = false;
            entity.Status = false;
            _service.Update(entity);
            _service.Save();
            return Json(new
            {
                result = "success"
            });
        }

        //public override JsonResult GetAll()
        //{
        //    return Json(_applicationUserService.GetAllUsers(), JsonRequestBehavior.AllowGet);
        //}

        public override JsonResult Update(ApplicationUserInputAdminViewData entity)
        {
            String id = (String)entity.GetType().GetProperty("Id").GetValue(entity, null);
            var oldObj = _layoutService.GetUserById(id);

            if (oldObj.Protected)
            {
                return Json(new
                {
                    result = "failed",
                    message = "this record can't update"
                });
            }

            List<ControlViewData> controls = new AutoForm<ApplicationUserInputAdminViewData>().GetControls();
            foreach (ControlViewData control in controls)
            {
                if (!control.NotChangeWhenUpdate)
                {
                    String propertyString = control.FieldName;
                    if (oldObj.GetType().GetProperty(propertyString) == null)
                        continue;
                    var value = entity.GetType().GetProperty(propertyString).GetValue(entity, null);
                    PropertyInfo propertyInfo = oldObj.GetType().GetProperty(propertyString);
                    propertyInfo.SetValue(oldObj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                }
            }

            if (User.Identity != null)
                oldObj.UpdatedBy = User.Identity.GetUserName();
            else
                oldObj.UpdatedBy = "";

            oldObj.UpdatedDate = DateTime.Now;
            _service.Update(oldObj);
            _service.Save();

            return Json(new
            {
                result = "success",
                data = entity
            });

        }
    }
}
