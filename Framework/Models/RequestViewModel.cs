using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Framework.Controllers;

namespace Framework.ViewModels
{
    public class RequestViewModel : IRef<RequestController>
    {
        int m_iStatus { set; get; }
        string m_UserRequest  { set; get; }
    }
}