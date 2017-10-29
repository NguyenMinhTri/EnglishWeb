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
using Framework.Model.Google;

namespace Framework.Controllers
{

    public class DictionaryController : LayoutController
    {
        IClientDictionaryService _clientDictionaryService;
        public DictionaryController(ILayoutService layoutService,
            IClientDictionaryService clientDictionaryService
            )
            : base(layoutService)
        {
            _clientDictionaryService = clientDictionaryService;
        }


        DictionariesViewModel DictionariesViewModel
        {
            get
            {
                return (DictionariesViewModel)_viewModel;
            }
        }

        OldWordsViewModel OldWordsViewModel
        {
            get
            {
                return (OldWordsViewModel)_viewModel;
            }
        }

        public ActionResult Index(string option)
        {
            if (option != null)
            {
                switch (option.ToLower())
                {
                    case "dictionaries":
                        {
                            _viewModel = new DictionariesViewModel();
                            CreateLayoutView("Từ điển");
                            break;
                        }
                    case "oldwords":
                        {
                            _viewModel = new OldWordsViewModel();
                            CreateLayoutView("Từ đã tra");
                            break;
                        }
                    default:
                        {
                            _viewModel = new DictionariesViewModel();
                            CreateLayoutView("Từ điển");
                            break;
                        }
                }
            }
            else
            {
                _viewModel = new DictionariesViewModel();
                CreateLayoutView("Từ điển");
            }

            return View(_viewModel);
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> Dictionaries(string keyword)
        {
            keyword = keyword.Trim();
            _viewModel = new DictionariesViewModel();
            int size = keyword.Split(' ').Length;
            if (size > 1)
            {
                GoogleTransJson googleTransJson = await _clientDictionaryService.startGoogleTrans(keyword);
                DictionariesViewModel.isGoogleTrans = true;
                DictionariesViewModel.m_GoogleTrans = googleTransJson;
            }
            else
            {
                OxfordDict dict = await _clientDictionaryService.startCrawlerOxford(keyword);
                DictionariesViewModel.m_Explanation = dict.m_Explanation;
                DictionariesViewModel.m_SoundUrl = dict.m_SoundUrl;
                DictionariesViewModel.m_Type = dict.m_Type;
                DictionariesViewModel.m_Voca = dict.m_Voca;
            }
            DictionariesViewModel.m_ExaTraCau = await _clientDictionaryService.startCrawlerTraCau(keyword);
            return PartialView("_Dictionaries", DictionariesViewModel);
        }

        [HttpGet]
        public ActionResult OldWords()
        {
            _viewModel = new OldWordsViewModel();
            return PartialView("_OldWords", OldWordsViewModel);
        }
    }
}
