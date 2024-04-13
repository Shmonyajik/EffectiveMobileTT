using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPLogsFilter.Entity
{
    [Command(Description = "Programm desc", Name = "Name")]
    [HelpOption("-?|--help")]
    internal class CMDArgs//Клиентская валидация
    { 
        [Option("-c|--file-config <FILE_PATH>", CommandOptionType.SingleValue, Description = "Set configuration file")]
        protected string? ConfigFile { get; set; }

        [Option("--file-log <FILE_PATH>", CommandOptionType.SingleValue, Description = "Log file path")]
        protected string? LogFilePath { get; set; }

        [Option("--file-output <FILE_PATH>", CommandOptionType.SingleValue, Description = "Result file path")]
        protected string? ResultFilePath { get; set; }

        [Option("--address-start=<ADDRESS>", CommandOptionType.SingleOrNoValue, Description = "Start ip of Diapazon")]
        protected string? StartIP { get; set; }

        [Option("--address-mask=<MASK>", CommandOptionType.SingleOrNoValue, Description = "Sunbet mask for Diapazon end")]
        protected string? Mask { get; set; } = "-1";

        [Option("--time-start <DATE>", CommandOptionType.SingleValue, Description = "Diapazon start date")]
        protected string? StartDate { get; set; }

        [Option("--time-end <DATE>", CommandOptionType.SingleValue, Description = "Diapazon end date")]
        protected string? EndDate { get; set; }
        
    }
}
