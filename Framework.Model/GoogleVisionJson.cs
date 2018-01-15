using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class Image
    {
        public string content { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
    }

    public class RequestVision
    {
        public RequestVision()
        {
            image = new Image();
            features = new List<Feature>();
        }
        public Image image { get; set; }
        public List<Feature> features { get; set; }
    }

    public class GoogleVisionJson
    {
        public GoogleVisionJson()
        {
            requests = new List<RequestVision>();
        }
        public List<RequestVision> requests { get; set; }
    }
}
