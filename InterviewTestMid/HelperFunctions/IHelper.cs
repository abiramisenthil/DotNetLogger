using InterviewTestMid.SerializationModels;
namespace InterviewTestMid.HelperFunctions
{
    public interface IHelper
    {
        public List<Part> LoadJsonFromFile(string filePath);
        public List<Part> GetMaterialDescriptionsForPart(List<Part> parts, string partDesc);
        public string SerializeObjectToJson(List<Part> parts);
        public void WriteJsonToFile(string content, string filePath);
    }
}
