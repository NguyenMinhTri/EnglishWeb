using Framework.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Service.Admin;
using Framework.Common;
using System.Threading;
using System.Web.UI;
using System.Drawing;
using System.IO;
using System.Drawing.Text;
using Framework.ViewModels;
using Framework.Service.Client;

namespace Framework.Controllers
{

    public class LearningController : LayoutController
    {
        IClientLearningService _clientLearningService;
        public LearningController(ILayoutService layoutService,
            IClientLearningService clientLearningService
            )
            : base(layoutService)
        {
            _clientLearningService = clientLearningService;
        }


        WordFormViewModel WordFormViewModel
        {
            get
            {
                return (WordFormViewModel)_viewModel;
            }
        }

        ListeningViewModel ListeningViewModel
        {
            get
            {
                return (ListeningViewModel)_viewModel;
            }
        }

        ReadingViewModel ReadingViewModel
        {
            get
            {
                return (ReadingViewModel)_viewModel;
            }
        }

        GrammarViewModel GrammarViewModel
        {
            get
            {
                return (GrammarViewModel)_viewModel;
            }
        }

        public ActionResult Index(string option)
        {
            if (option != null)
            {
                switch (option.ToLower())
                {
                    case "wordform":
                        {
                            _viewModel = new WordFormViewModel();
                            CreateLayoutView("Từ vựng");
                            break;
                        }
                    case "listening":
                        {
                            _viewModel = new ListeningViewModel();
                            CreateLayoutView("Tập nghe");
                            break;
                        }
                    case "reading":
                        {
                            _viewModel = new ReadingViewModel();
                            CreateLayoutView("Tập đọc");
                            break;
                        }
                    case "grammar":
                        {
                            _viewModel = new GrammarViewModel();
                            CreateLayoutView("Ngữ pháp");
                            break;
                        }
                    default:
                        {
                            _viewModel = new LearningViewModel();
                            CreateLayoutView("Tự học");
                            break;
                        }
                }
            }
            else
            {
                _viewModel = new LearningViewModel();
                CreateLayoutView("Tự học");
                ViewBag.page = "Index";
                return View(_viewModel);
            }

            return View("Index", "~/Views/Layout/_LearningLayout.cshtml", _viewModel);
        }

        [HttpGet]
        public ActionResult WordForm()
        {
            _viewModel = new WordFormViewModel();
            return PartialView("_WordForm", WordFormViewModel);
        }

        [HttpGet]
        public ActionResult Listening()
        {
            _viewModel = new ListeningViewModel();
            return PartialView("_Listening", ListeningViewModel);
        }

        [HttpGet]
        public ActionResult Reading()
        {
            _viewModel = new ReadingViewModel();
            return PartialView("_Reading", ReadingViewModel);
        }

        public ActionResult Grammar()
        {
            _viewModel = new GrammarViewModel();
            return PartialView("_Grammar", GrammarViewModel);
        }
    }
}
