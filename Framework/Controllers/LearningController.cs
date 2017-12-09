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
using Framework.Model;
using Microsoft.AspNet.Identity;

namespace Framework.Controllers
{

    public class LearningController : LayoutController
    {
        IClientLearningService _clientLearningService;
        IOurWordService _ourWordService;
        IDetailOurWordService _detailOutWordService;
        IDictCacheService _dictCache;
        public LearningController(ILayoutService layoutService,
            IClientLearningService clientLearningService,
                    IDetailOurWordService detailOutWordService,
            IOurWordService ourWordService,
            IDictCacheService dictCache
            )
            : base(layoutService)
        {
            _clientLearningService = clientLearningService;
            _detailOutWordService = detailOutWordService;
            _ourWordService = ourWordService;
            _dictCache = dictCache;
        }

        MultipleChoiceViewModel MultipleChoiceViewModel
        {
            get
            {
                return (MultipleChoiceViewModel)_viewModel;
            }
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
                    case "multiplechoice":
                        {
                            _viewModel = new MultipleChoiceViewModel();
                            CreateLayoutView("Trắc nghiệm");
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

        protected TracNghiem RandomTracNghiemChoVoca(int idWord)
        {
            TracNghiem cauTracNghiem = new TracNghiem();
            var vocaCanHoc = _ourWordService.GetById(idWord);
            Random rnd = new Random();
            var listDict = _dictCache.GetAll();
            int pos = rnd.Next(0, listDict.Count);
            var ramdomPo = randomPosition();
            cauTracNghiem.Question = vocaCanHoc.MeanEn;
            for (int i = 0; i < 4; i++)
            {
                DapAn dapAn = new DapAn();
                if (i == 0)
                {
                    dapAn.Checked = true;
                    dapAn.Contain = vocaCanHoc.Word;
                    cauTracNghiem.ABCD[ramdomPo[i]] = dapAn;
                }
                else
                {
                    int posDictCache = rnd.Next(0, listDict.Count);
                    dapAn.Contain = listDict.ElementAt(posDictCache).VocaID;
                    cauTracNghiem.ABCD[ramdomPo[i]] = dapAn;
                }

            }
            return cauTracNghiem;
        }
        protected List<int> randomPosition()
        {
            List<int> posResult = new List<int>();

            Random rnd = new Random();
            int count = 0;
            while (true)
            {
                int pos = rnd.Next(0, 4);
                if (posResult.Where(x => x == pos).ToList().Count == 0)
                {
                    posResult.Add(pos);
                    count++;
                    if (count > 3)
                    {
                        break;
                    }
                }
            }
            return posResult;
        }

        [AllowAnonymous]
        public ActionResult MultipleChoice()
        {
            _viewModel = new MultipleChoiceViewModel();
            List<int> randDomPost = randomPosition();
            List<TracNghiem> dataTracNghiem = new List<TracNghiem>();

            List<int> listIDWord = new List<int>();
            var currentUser = _service.GetUserById(User.Identity.GetUserId());
            listIDWord = _detailOutWordService.listIdOutWord(currentUser.Id, -1);
            foreach (var id in listIDWord)
            {
                dataTracNghiem.Add(RandomTracNghiemChoVoca(id));
            }
            MultipleChoiceViewModel.listTracNghiem = dataTracNghiem;
            return PartialView("_MultipleChoice", MultipleChoiceViewModel);
        }
    }
}
