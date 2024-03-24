using Autofac;
using InterviewTestMid.HelperFunctions;
using InterviewTestMid.LoggerFiles;
using Microsoft.Extensions.Configuration;


namespace InterviewTestMid.DependencyInjection
{
    public class Registry : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Log4netLogger>()
                   .As<ILogger>()
                   .WithParameter("loggerName", "InterviewTestMidLogger")
                   .SingleInstance();
            builder.RegisterType<Jsontask>()
                   .As<IJsonTask>()
                   .SingleInstance();
            builder.RegisterType<Helper>()
                   .As<IHelper>()
                   .SingleInstance();
            builder.Register<IConfiguration>(context =>
            {
                var configBuilder = new ConfigurationBuilder();
                configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                return configBuilder.Build();
            });
            
        }
    }
}
