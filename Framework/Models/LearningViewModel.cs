using Framework.Controllers;
using Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.ViewModels
{
    public class LearningViewModel : LayoutViewModel, IRef<LearningController>
    {
    }
    public class WordFormViewModel : LayoutViewModel, IRef<LearningController>
    {
    }
    public class ListeningViewModel : LayoutViewModel, IRef<LearningController>
    {
    }
    public class ReadingViewModel : LayoutViewModel, IRef<LearningController>
    {
    }
    public class GrammarViewModel : LayoutViewModel, IRef<LearningController>
    {
    }
    public class MultipleChoiceViewModel : LayoutViewModel, IRef<LearningController>
    {
        public List<TracNghiem> listTracNghiem { get; set; }
    }

}