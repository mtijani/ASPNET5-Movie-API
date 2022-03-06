﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace movieAPI.Filters
{
    public class MyExceptionFilter: ExceptionFilterAttribute 
    {
        private readonly ILogger<MyExceptionFilter> logger; 
        public MyExceptionFilter(ILogger<MyExceptionFilter> logger )
        {
            this.logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context); 
        }
    }
}
