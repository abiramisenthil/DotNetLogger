using InterviewTestMid;
using Moq;
using InterviewTestMid.LoggerFiles;
using InterviewTestMid.SerializationModels;
using Newtonsoft.Json;
using InterviewTestMid.HelperFunctions;
using Microsoft.Extensions.Configuration;
using InterviewTestMid.Constant;


namespace InterviewTestMidTest
{
    public class Tests
    {
        Mock<ILogger> loggerMock = new Mock<ILogger>();
        Mock<IHelper> helperMock = new Mock<IHelper>();
        Mock<IConfiguration> configurationMock = new Mock<IConfiguration>();

        [SetUp]
        public void Setup()
        {
            loggerMock = new Mock<ILogger>();
            helperMock = new Mock<IHelper>();
            configurationMock= new Mock<IConfiguration>();
        }

        [Test]
        public void DoWork_Success_LogsSuccessfulMessages()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "SampleData.json");
            string foilFileName = "FoilData.json";
            List<Part> expectedParts = GetExpectedParts("FOIL");
            Mock<IConfigurationSection> foilFileNameSectionMock = new Mock<IConfigurationSection>();
            foilFileNameSectionMock.Setup(s => s.Value).Returns("FoilData");
            //configurationMock.Setup(config => config.GetSection("FoilFileName")).Returns(foilFileNameSectionMock.object);


            helperMock.Setup(helper => helper.LoadJsonFromFile(It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.GetMaterialDescriptionsForPart(It.IsAny<List<Part>>(), It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.SerializeObjectToJson(It.IsAny<List<Part>>())).Returns(It.IsAny<string>());
            helperMock.Setup(helper => helper.WriteJsonToFile(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(MockWriteJsonToFile);
                    


            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath, foilFileName);

            // Assert 
            loggerMock.Verify(logger => logger.WriteLogMessage("Doing some JSON tasks..."), Times.Once);
            loggerMock.Verify(logger => logger.WriteLogMessage("Successfully modified and saved data in a new json file 'FoilData.json'"), Times.Once);
            loggerMock.Verify(logger => logger.WriteLogMessage("Finished doing some JSON tasks."), Times.Once);
        }

        [Test]
        public void DoWork_LogsException_When_File_Is_Not_Found()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "NoFilePath.json");
            string foilFileName = "FoilData.json";
            helperMock.Setup(helper => helper.LoadJsonFromFile(It.IsAny<string>())).Throws<FileNotFoundException>();

            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath, foilFileName);

            // Assert 
            loggerMock.Verify(logger => logger.WriteErrorMessage(It.IsAny<FileNotFoundException>()), Times.Once);

        }

        [Test]
        public void CheckMetaObjects()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "SampleData.json");
            string foilFileName = "FoilData.json";

            // Act
            string jsonContent = File.ReadAllText(filePath);
            List<Part> parts = JsonConvert.DeserializeObject<List<Part>>(jsonContent);

            // Assert
            foreach (Part part in parts)
            {
                Assert.That(part.Meta, Is.Not.Null);
            }

        }

        [Test]
        public void DoWork_LogsExceptionWhen_No_JsonData()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "Exception.json");
            string foilFileName = "FoilData.json";

            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath, foilFileName);

            // Assert 
            loggerMock.Verify(logger => logger.WriteErrorMessage(It.IsAny<NullReferenceException>()), Times.Once);
            loggerMock.Verify(logger => logger.WriteLogMessage("Finished doing some JSON tasks."), Times.Once);
        }

        [Test]
        public void DoWork_Not_Creating_New_Json_When_No_Foil_PartDesc()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "NoFoil.json");
            string foilFileName = "FoilData.json";

            List<Part> expectedParts = GetExpectedParts("NotFOIL");

            helperMock.Setup(helper => helper.LoadJsonFromFile(It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.GetMaterialDescriptionsForPart(It.IsAny<List<Part>>(), It.IsAny<string>())).Returns(new List<Part>());

            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath, foilFileName);

            // Assert 
            loggerMock.Verify(logger => logger.WriteLogMessage("Can't find 'FOIL' in Material Description"), Times.Once);
            loggerMock.Verify(logger => logger.WriteLogMessage("Successfully modified and saved data in a new json file 'FoilData.json'"), Times.Never);
        }


        [Test]
        public void DoWork_Works_When_Olny_Part_Is_Provided()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "OlnyParts.json");
            string foilFileName = "FoilData.json";

            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath, foilFileName);

            // Assert 
            loggerMock.Verify(logger => logger.WriteLogMessage("Doing some JSON tasks..."), Times.Once);
            loggerMock.Verify(logger => logger.WriteLogMessage("Successfully modified and saved data in a new json file 'FoilData.json'"), Times.Never);
            loggerMock.Verify(logger => logger.WriteLogMessage("Finished doing some JSON tasks."), Times.Once);
        }

        [Test]
        public void DoWork_Creates_CSV_When_Foil_Found()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "SampleData.json");
            string foilFileName = "FoilData.json";

            List<Part> expectedParts = GetExpectedParts(ProjectConstants.PartDesc);

            helperMock.Setup(helper => helper.LoadJsonFromFile(It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.GetMaterialDescriptionsForPart(It.IsAny<List<Part>>(), It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.SerializeObjectToJson(It.IsAny<List<Part>>())).Returns(It.IsAny<string>());
            helperMock.Setup(helper => helper.WriteJsonToFile(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(MockWriteJsonToFile);


            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath, foilFileName);

            // Assert 
            loggerMock.Verify(logger => logger.WriteLogMessage("Details of Part with FOIL as Material Description is written in csv file."), Times.Once);
            
        }

        [Test]
        public void DoWork_Modify_Part_Weight_For_Foil_Records()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "SampleData.json");
            string foilFileName = "FoilData.json";

            List<Part> expectedParts = GetExpectedParts(ProjectConstants.PartDesc);

            helperMock.Setup(helper => helper.LoadJsonFromFile(It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.GetMaterialDescriptionsForPart(It.IsAny<List<Part>>(), It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.SerializeObjectToJson(It.IsAny<List<Part>>())).Returns(It.IsAny<string>());
            helperMock.Setup(helper => helper.WriteJsonToFile(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(MockWriteJsonToFile);


            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath, foilFileName);

            // Assert 
            loggerMock.Verify(logger => logger.WriteLogMessage("Material Descriptions for 11170is 'FOIL'so part weight is assigned with 5 and 100.000000m"), Times.AtLeastOnce);

        }

        [Test]
        public void DoWork_Calls_LoadJsonFromFile()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "SampleData.json");
            string foilFileName = "FoilData.json";

            List<Part> expectedParts = GetExpectedParts("FOIL");

            helperMock.Setup(helper => helper.LoadJsonFromFile(It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.GetMaterialDescriptionsForPart(It.IsAny<List<Part>>(), It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.SerializeObjectToJson(It.IsAny<List<Part>>())).Returns(It.IsAny<string>());
            helperMock.Setup(helper => helper.WriteJsonToFile(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(MockWriteJsonToFile);


            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath, foilFileName);

            // Assert 
            helperMock.Verify(helper => helper.LoadJsonFromFile(filePath), Times.Once);

        }

        [Test]
        public void DoWork_Calls_GetMaterialDescriptionsForPart_When_Parts_Count_Is_Not_Zero()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "SampleData.json");
            string foilFileName = "FoilData.json";

            List<Part> expectedParts = GetExpectedParts("FOIL");

            helperMock.Setup(helper => helper.LoadJsonFromFile(It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.GetMaterialDescriptionsForPart(It.IsAny<List<Part>>(), It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.SerializeObjectToJson(It.IsAny<List<Part>>())).Returns(It.IsAny<string>());
            helperMock.Setup(helper => helper.WriteJsonToFile(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(MockWriteJsonToFile);


            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath, foilFileName);

            // Assert 
            helperMock.Verify(helper => helper.GetMaterialDescriptionsForPart(expectedParts, "FOIL"), Times.Once);
            helperMock.Verify(helper => helper.SerializeObjectToJson(expectedParts), Times.Once);
        }

        [Test]
        public void DoWork_Not_Calling_GetMaterialDescriptions_When_Parts_Count_Is_Zero()
        {
            // Arrange
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "TestJsonFiles", "SampleData.json");
            string foilFileName = "FoilData.json";

            List<Part> expectedParts = GetExpectedParts("FOIL");

            helperMock.Setup(helper => helper.LoadJsonFromFile(It.IsAny<string>())).Returns(new List<Part>());
            helperMock.Setup(helper => helper.GetMaterialDescriptionsForPart(It.IsAny<List<Part>>(), It.IsAny<string>())).Returns(expectedParts);
            helperMock.Setup(helper => helper.SerializeObjectToJson(It.IsAny<List<Part>>())).Returns(It.IsAny<string>());
            helperMock.Setup(helper => helper.WriteJsonToFile(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>(MockWriteJsonToFile);


            // Act 
            var jsontask = new Jsontask(loggerMock.Object, helperMock.Object, configurationMock.Object);
            jsontask.DoWork(filePath , foilFileName);

            // Assert 
            helperMock.Verify(helper => helper.GetMaterialDescriptionsForPart(expectedParts, "FOIL"), Times.Never);
            helperMock.Verify(helper => helper.SerializeObjectToJson(expectedParts), Times.Never);
        }



        private List<Part> GetExpectedParts(string partDesc)
        {
            var partList = new List<Part>()
                    {
                        new Part()
                        {
                            PartId = 11170,
                            PartNbr = "101687",
                            PartDesc = partDesc,
                            Meta = new PartMeta() 
                            {
                               
                            },
                            PartWeight = new UnitOfMeasure() 
                            {
                                UoM = 10,
                                Value = 0.50000000m
                            },
                            ConversionsApplied = false,
                            Materials = new List<Materials>()
                            {
                            
                            }
                        },
                   
                     };
            return partList;
        }
        private void MockWriteJsonToFile(string content, string filePath)
        {
            
            Console.WriteLine($"Mocked writing content: {content} to file: {filePath}");
        }
    }
}