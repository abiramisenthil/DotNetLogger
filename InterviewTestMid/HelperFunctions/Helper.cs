using InterviewTestMid.SerializationModels;
using InterviewTestMid.LoggerFiles;
using System.Text.Json;


namespace InterviewTestMid.HelperFunctions
{
    public class Helper :IHelper
    {
        private readonly ILogger _logger;


        public Helper(ILogger logger)
        {
            _logger = logger;
        }

        public List<Part> LoadJsonFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.WriteErrorMessage(new FileNotFoundException());
                    return new List<Part>();
                }
                string jsonContent;
                using (var streamReader = new StreamReader(filePath))
                {

                    jsonContent = streamReader.ReadToEnd();
                }


                if (string.IsNullOrEmpty(jsonContent))
                {
                    _logger.WriteErrorMessage(new NullReferenceException());
                    return new List<Part>();
                }

                // Deserialization using JsonSerializer
                return JsonSerializer.Deserialize<List<Part>>(jsonContent);

            }
            catch (Exception ex)
            {
                _logger.WriteErrorMessage(ex);
                return new List<Part>();
            }
        }


        public List<Part> GetMaterialDescriptionsForPart(List<Part> parts, string partDesc)
        {
            if (parts == null) return null;
            return parts.FindAll(part => part.PartDesc == partDesc);
        }

        public string SerializeObjectToJson(List<Part> parts)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(parts, options);
        }

        public void WriteJsonToFile(string content, string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Create directory if it doesn't exist
            File.WriteAllText(filePath, content);
        }
    }
}
