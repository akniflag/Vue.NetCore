using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace VOL.Core.Middleware
{
    public class HttpRequestMiddleware
    {
        public static Func<RequestDelegate, RequestDelegate> Context
        {
            get
            {
                return next =>
                    async context =>
                    {
                        //动态标识刷新token(2021.05.01)
                        context.Response.Headers["Access-Control-Expose-Headers"] = "vol_exp";
                        await next(context);
                    };
            }
        }
    }
}
