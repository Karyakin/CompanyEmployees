using Conrtacts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployees.Filters.TestsFlters
{
    public class LoggerResourceFilter : Attribute, IResourceFilter
    {
        ILogger _logger;
        public ILoggerManager _logger1 { get; }
        public LoggerResourceFilter(ILoggerFactory loggerFactory, ILoggerManager loggerManager)
        {
            _logger = loggerFactory.CreateLogger("SimpleResourceFilter");
            _logger1 = loggerManager;
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _logger.LogInformation($"OnResourceExecuted - {DateTime.Now}");
            _logger1.LogInfo($"лог из фильтра после { DateTime.Now}");

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _logger.LogInformation($"OnResourceExecuting - {DateTime.Now}");
            _logger1.LogInfo($"лог из фильтра до { DateTime.Now}");

        }
    }
}
