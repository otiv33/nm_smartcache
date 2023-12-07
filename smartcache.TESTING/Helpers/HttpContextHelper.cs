using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcache.TESTING.Helpers
{
    public static class HttpContextHelper
    {
        public static HttpContext CreateHttpContext()
        {
            // Create a service collection and add necessary services
            var services = new ServiceCollection();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Create a service provider
            var serviceProvider = services.BuildServiceProvider();

            // Create an HttpContext instance
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider,
                // Set other properties as needed (e.g., Request, Response)
            };

            return httpContext;
        }
    }
}
