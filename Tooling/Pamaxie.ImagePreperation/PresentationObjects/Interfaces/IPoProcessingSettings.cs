using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.ImagePreparation.PresentationObjects.Interfaces
{
    interface IPoProcessingSettings
    {
        bool ColorChange { get; set; }
        bool MirrorImages { get; set; }
        bool StopOnError { get; set; }
        string Width { get; set; }
        string Height { get; set; }
    }
}
