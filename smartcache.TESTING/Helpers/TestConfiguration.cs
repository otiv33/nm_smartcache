using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartcache.TESTING.Helpers
{
    using Microsoft.Extensions.Configuration;

    public static class TestConfiguration
    {
        public static IConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("JWT-secret", "xasdasdasfadgdsfgadgasdfgdhgasfdfasfdsafeassdfasddfda"),
                    new KeyValuePair<string, string>("JWT-issuer", "x"),
                    new KeyValuePair<string, string>("JJWT-audience", "x")
                })
                .Build();

            return configuration;
        }
    }
}
