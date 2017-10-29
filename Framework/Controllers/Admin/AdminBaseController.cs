using Framework.Common;
using Framework.Model;
using Framework.Service.Admin;
using Framework.Service;
using Framework.ViewData;
using Framework.ViewData.Admin;
using Framework.ViewData.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Framework.Controllers
{
    public class AdminBaseController<T, VM> : Controller
        where T : class, IAuditable, new()
        where VM : BaseViewData<T>, new()
    {
        protected IQlService<T> _service;
        protected ILayoutAdminService _layoutService;
        public AdminBaseController(IQlService<T> service,
            ILayoutAdminService layoutService
            )
        {
            _service = service;
            _layoutService = layoutService;
        }
        public void CreateLayoutAdminView()
        {
            String controllerName = this.GetType().Name;
            controllerName = controllerName.Substring(0, controllerName.Length - 10);
            ViewBag.ControllerName = controllerName;

            var userId = User.Identity.GetUserId();
            //if (userId == null || userId == "")
            //    return;

            ViewBag.RenderNav = 1;

            //var user = _layoutService.GetUserById(userId);
            //if (!user.Logined)
            //{
            //    HttpContext.GetOwinContext().Authentication.SignOut();
            //    return;
            //}
            //ViewBag.User = user;

        }
        [HttpGet]
        public virtual JsonResult GetAll()
        {
            return Json(ViewDataMapper<VM, T>.MapObjects(_service.GetAll()), JsonRequestBehavior.AllowGet);
        }
        public virtual void ChangeCache(object data)
        {

        }
        [HttpPost]
        public virtual JsonResult Add(VM entity)
        {
            var insertObject = PropertyCopy.Copy<T, VM>(entity);
            if (User.Identity != null)
            {
                insertObject.CreatedBy = _layoutService.GetUserById(User.Identity.GetUserId()).Name;
            }
            else
                insertObject.CreatedBy = "";
            insertObject.CreatedDate = DateTime.Now;
            var item = _service.Add(insertObject);
            _service.Save();
            ChangeCache(item);

            //var value = insertObject.GetType().GetProperty("Id").GetValue(insertObject, null);
            //PropertyInfo propertyInfo = entity.GetType().GetProperty("Id");
            //propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyInfo.PropertyType), null);
            return Json(insertObject);
        }
        public virtual JsonResult DoUpdate(T obj)
        {
            if (User.Identity != null)
                obj.UpdatedBy = _layoutService.GetUserById(User.Identity.GetUserId()).Name;
            else
                obj.UpdatedBy = "";

            obj.UpdatedDate = DateTime.Now;
            _service.Update(obj);
            _service.Save();

            return Json(new
            {
                result = "success",
                data = obj
            });
        }

        [HttpPost]
        public virtual JsonResult Update(VM entity)
        {
            #region old
            //dynamic getId = entity;
            //var old = _service.GetById(getId.Id);
            //var newObj = PropertyCopy.Copy<T, VM>(entity);

            //List<ControlViewData> controls = new AutoForm<VM>().GetControls();

            //foreach(ControlViewData control in controls)
            //{
            //    if(!control.NotChangeWhenUpdate)
            //    {
            //        String propertyString = control.FieldName;
            //        if (newObj.GetType().GetProperty(propertyString) == null)
            //            continue;
            //        var value = newObj.GetType().GetProperty(propertyString).GetValue(newObj, null);
            //        PropertyInfo propertyInfo = old.GetType().GetProperty(propertyString);
            //        propertyInfo.SetValue(old, Convert.ChangeType(value, propertyInfo.PropertyType), null);

            //    }
            //}

            //if (User.Identity != null)
            //    old.UpdatedBy = User.Identity.GetUserName();
            //else
            //    old.UpdatedBy = "";

            //old.UpdatedDate = DateTime.Now;

            //_service.Update(old);
            //_service.Save();
            //return Json(new
            //{
            //    result = "success",
            //    data = entity
            //}); 
            #endregion

            int id = (int)entity.GetType().GetProperty("Id").GetValue(entity, null);
            var oldObj = _service.GetById(id);

            if (oldObj.Protected)
            {
                return Json(new
                {
                    result = "failed",
                    message = "this record can't update"
                });
            }

            List<ControlViewData> controls = new AutoForm<VM>().GetControls();
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
            ChangeCache(oldObj);
            return DoUpdate(oldObj);
        }
        [HttpPost]
        public virtual JsonResult Delete(DeleteViewData obj)
        {
            var entity = _service.GetById(obj.Id);
            if (entity.Protected)
            {
                return Json(new
                {
                    result = "failed",
                    message = "this record can't delete"
                });
            }
            entity.Status = false;
            _service.Update(entity);
            _service.Save();
            ChangeCache(entity);
            return Json(new
            {
                result = "success"
            });
        }
    }
}