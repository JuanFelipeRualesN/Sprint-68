using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Lessons
{
    // This is a model to consturct the dynamic JavaScript models only
    public class ObjectDynamicsJavascriptModel
    {
        public string AcademicWord { get; set; }
        public string AcademicExplanation { get; set; }
        public string AcademicAudioMp3 { get; set; }
        public string AcademicAudioOgg { get; set; }
        public string AcademicImgUrl { get; set; }
        public string AcademicImgFrames { get; set; }
        public string AcademicImgWidth { get; set; }
        public string ObjectTouchImgLoop { get; set; }
        public string ObjectTouchImg { get; set; }
        public string ObjectTouchAudioMp3 { get; set; }
        public string ObjectTouchAudioOgg { get; set; }
        public string ObjectDropImgLoop { get; set; }
        public string ObjectDropImgUrl { get; set; }
        public string ObjectDropAudioMp3 { get; set; }
        public string ObjectDropAudioOgg { get; set; }
    }
}
