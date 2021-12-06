namespace Pamaxie.ImageScanning
{
    public class OutputProperties
    {
        public enum ImagePredictionResult
        {
            Gore,
            None,
            Pornographic,
            Racy,
            Violence
        }

        public ImagePredictionResult PredictedLabel { get; set; }

        public float PornographicLikelihood { get; set; }
        public float RacyLikelihood { get; set; }
        public float ViolenceLikelihood { get; set; }
        public float GoreLikelihood { get; set; }
        public float NoneLikelihood { get; set; }
    }
}