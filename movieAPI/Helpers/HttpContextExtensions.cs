using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParammetersPaginationsInHeader<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            //In case no arguments..throw an exception 
            if(httpContext==null) { throw new ArgumentNullException(nameof(httpContext)); }
            //Counting the amount of records in the table (whether those records are actors, movies and so on
            double count = await queryable.CountAsync();
            httpContext.Response.Headers.Add("totalAmountOfRecords", count.ToString());
             
        }
    }
}
