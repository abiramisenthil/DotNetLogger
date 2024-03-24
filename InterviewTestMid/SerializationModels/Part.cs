namespace InterviewTestMid.SerializationModels
{
    public class Part
    {
        public int PartId { get; set; }
        public string PartNbr { get; set; }
        public string PartDesc { get; set; }
        public PartMeta Meta { get; set; }
        public UnitOfMeasure PartWeight { get; set; }
        public bool ConversionsApplied { get; set; }
        public List<Materials> Materials { get; set; }
    }
}
