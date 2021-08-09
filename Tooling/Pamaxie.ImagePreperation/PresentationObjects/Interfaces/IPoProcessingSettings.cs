namespace Pamaxie.ImageTooling.PresentationObjects.Interfaces
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
