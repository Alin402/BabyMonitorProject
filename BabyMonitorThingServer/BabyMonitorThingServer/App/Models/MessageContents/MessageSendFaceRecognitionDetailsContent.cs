using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Models.MessageContents
{
    public class MessageSendFaceRecognitionDetailsContent
    {
        public List<string>? Emotions { get; set; }
        public float[]? BoundingBox { get; set; }
        public bool? Awake { get; set; }
    }
}
