namespace InterviewTestMid.SerializationModels
{
    public class Materials
    {
        public Look Material { get; set; }
        public decimal Percentage { get; set; }
        public bool MatrIsBarrier { get; set; } // Optional properties for Material
        public bool MatrIsDensifier { get; set; }
        public bool MatrIsOpacifier { get; set; }
    }
}
