using Autofac;
using InterviewTestMid.DependencyInjection;
using InterviewTestMid.LoggerFiles;
using Microsoft.Extensions.Configuration;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
namespace InterviewTestMid
{

    public class Program
    {
        private readonly ILogger _logger;
        private readonly IJsonTask _jsonTask;
        private readonly IConfiguration _configuration;

        public Program(ILogger logger, IJsonTask jsonTask, IConfiguration configuration)
        {
            _logger = logger;
            _jsonTask = jsonTask;
            _configuration = configuration;
            
        }

        public static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<Registry>();
            containerBuilder.RegisterType<Program>();
            var container = containerBuilder.Build();
  
            var logger = container.Resolve<ILogger>();
            var jsontask = container.Resolve<IJsonTask>();
            var configuration = container.Resolve<IConfiguration>();
            
            var program = new Program(logger, jsontask, configuration);
            string jsonFilePath = configuration["ValidJsonFilePath"];
            string foilFile = configuration["FoilFileName"];
            jsontask.DoWork(jsonFilePath, foilFile);
                                  
        }
    }
}