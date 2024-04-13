using CsvHelper;
using IPLogsFilter.Comparers;
using IPLogsFilter.Entity;
using IPLogsFilter.Repository;
using IPLogsFilter.Services;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Net;

namespace IPLogsFilter
{

    internal class Program : CMDArgs
    {
        public Filter Filter { get; private set; }

        public IConfiguration Сonfiguration { get; private set; }

        static void Main(string[] args) =>
            CommandLineApplication.ExecuteAsync<Program>(args);
            
        public async Task OnExecute()
        {
            try
            {
                var configBuilder = new ConfigurationBuilder();

                if (!string.IsNullOrWhiteSpace(ConfigFile)) //Аргументы из файла конфигурации
                {
                    if(!File.Exists(ConfigFile))
                        throw new FileNotFoundException(ConfigFile);
                    Сonfiguration = configBuilder.AddJsonFile(ConfigFile, optional: false, reloadOnChange: true)
                        .Build();

                    LogFilePath = Сonfiguration.GetRequiredSection("configuration:file-log").Value;
                    ResultFilePath =  Сonfiguration.GetRequiredSection("configuration:file-output").Value;

                    ValidateConfig(LogFilePath, ResultFilePath);
                    
                    StartDate = Сonfiguration.GetRequiredSection("configuration:filter:time-start").Value;
                    EndDate = Сonfiguration.GetRequiredSection("configuration:filter:time-end").Value;
                    StartIP = Сonfiguration.GetSection("configuration:filter:address-start").Value;
                    Mask = Сonfiguration.GetSection("configuration:filter:address-mask").Value;
                    
                }
                else //Аргументы из командной строки
                {
                    ValidateConfig(LogFilePath, ResultFilePath);

                }
                var serviceCollection = new ServiceCollection()
                    .AddSingleton<BaseRepository<IPLog>, IPLogsFileRepository>()
                    .AddSingleton<ILogService<IPLog>, IPLogService>()
                    .AddSingleton<IComparer<IPAddress>, IPAddressComparer>();
                if (Сonfiguration != null)
                    serviceCollection.AddSingleton(Сonfiguration);
                else
                    serviceCollection.AddSingleton<IConfiguration>(new ConfigurationBuilder().Build());
                var serviceProvider = serviceCollection.BuildServiceProvider();
                
                var logService = serviceProvider.GetRequiredService<ILogService<IPLog>>();
                
                var numberLogsByIP = logService.GetByFilters(StartDate, EndDate, StartIP, Mask);

                if(numberLogsByIP.Count()==0)
                {
                    await Console.Out.WriteLineAsync("No logs were found based on the installed filters");
                    return;
                }
                await logService.CreateAsync(numberLogsByIP);

                await Console.Out.WriteLineAsync($"Filtered ip logs loaded along the path {ResultFilePath}");
    
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong...");
                Console.WriteLine(ex.Message);
            }

        }

        static void ValidateConfig(string? fileLog, string? fileOutput)
        {
            if (fileLog == null)
                throw new ArgumentNullException("IP logs file path not found in configuration");

            if (!File.Exists(fileLog))
                throw new FileNotFoundException($"IP logs file by the path {fileLog} not found");

            if (fileOutput == null)
                throw new ArgumentNullException("IP logs file path not found in configuration");
           
            if (!File.Exists(fileOutput))
            {
                using (var sw = new StreamWriter(fileOutput, append: true))
                using (var csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<LogCount>();
                }
                Console.WriteLine($"File not found, new file created along the path {fileOutput}");
            }
        }

    }
}
