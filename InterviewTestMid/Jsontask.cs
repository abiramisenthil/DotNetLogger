using InterviewTestMid.HelperFunctions;
using InterviewTestMid.LoggerFiles;
using InterviewTestMid.SerializationModels;
using Microsoft.Extensions.Configuration;

using InterviewTestMid.Constant;


namespace InterviewTestMid
{
    public class Jsontask : IJsonTask
    {
        private readonly ILogger _logger;
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;

        public Jsontask(ILogger logger, IHelper helper, IConfiguration configuration)
        {
            _logger = logger;
            _helper = helper;
            _configuration = configuration;
        }
        public void DoWork(string jsonFilePath, string foilFile)
        {
            _logger.WriteLogMessage("Doing some JSON tasks...");

            string newJsonFilePath = Path.Combine(Path.GetDirectoryName(jsonFilePath), foilFile);

            try
            {
                //JSON TASK START
                // Load JSON file into class structure
                List<Part> parts = _helper.LoadJsonFromFile(jsonFilePath);

                if (parts.Count != 0)
                {
                    // Get material descriptions for "FOIL" part using LINQ
                    List<Part> foilMaterialDescriptions = _helper.GetMaterialDescriptionsForPart(parts, ProjectConstants.PartDesc);

                                      
                    if (foilMaterialDescriptions.Count != 0)
                    {
                        // Write material descriptions part details to CSV
                        
                        string csvString = string.Join(
                            Environment.NewLine, 
                            foilMaterialDescriptions.Select(part =>
                                string.Join(",", part.PartId, part.PartNbr, part.PartDesc, part.ConversionsApplied))
                        );

                        _logger.StringListWriteInCsv(new List<string>() { csvString });

                        _logger.WriteLogMessage("Details of Part with FOIL as Material Description is written in csv file.");
                        _logger.WriteLogMessage("To number of Material Descriptions with 'FOIL' part is:" + foilMaterialDescriptions);

                        //Change the PartWeight value of a part of your choice.
                        foreach (Part part in foilMaterialDescriptions)
                        {                            
                            part.PartWeight.UoM = 5;
                            part.PartWeight.Value = 100.000000m;
                            _logger.WriteLogMessage("Material Descriptions for "+ part.PartId + "is " +"'FOIL'" + "so part weight is assigned with 5 and 100.000000m");
                        }

                        //Serialise the edited object back to a new JSON file.
                        string newJsonContent = _helper.SerializeObjectToJson(foilMaterialDescriptions);


                        // Write serialized JSON to a new file
                        _helper.WriteJsonToFile(newJsonContent, newJsonFilePath);

                        _logger.WriteLogMessage("Successfully modified and saved data in a new json file 'FoilData.json'");
                    }
                    else
                    {
                        _logger.WriteLogMessage("Can't find 'FOIL' in Material Description");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.WriteErrorMessage( ex);
            }

            _logger.WriteLogMessage("Finished doing some JSON tasks.");   
        }
       
    }
}
