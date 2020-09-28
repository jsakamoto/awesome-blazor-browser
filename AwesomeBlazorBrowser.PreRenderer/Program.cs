using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AwesomeBlazorBrowser.PreRenderer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddRazorPages();
                services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000/") });
            }).Build();

            using var scope = host.Services.CreateScope();

            var httpRequestFeature = new HttpRequestFeature
            {
                Protocol = "HTTP/2",
                Scheme = "http",
                Method = "GET",
                PathBase = "",
                Path = "/",
                QueryString = "",
                RawTarget = "/",
                Headers = { { "Host", "localhost:5000" } }
            };
            var httpResponseFeature = new HttpResponseFeature();
            var featureCollection = new FeatureCollection();
            featureCollection.Set<IHttpRequestFeature>(httpRequestFeature);
            featureCollection.Set<IHttpResponseFeature>(httpResponseFeature);

            var httpContextFactory = new DefaultHttpContextFactory(scope.ServiceProvider);
            var httpContext = httpContextFactory.Create(featureCollection);

            var attributes = new TagHelperAttributeList();

            var tagHelperContext = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString("N"));

            var tagHelperOutput = new TagHelperOutput(
               tagName: string.Empty,
               attributes,
               getChildContentAsync: (_, _) => Task.FromResult(new DefaultTagHelperContent() as TagHelperContent));

            var componentTagHelper = new ComponentTagHelper
            {
                ComponentType = typeof(App),
                RenderMode = RenderMode.Static,
                ViewContext = new ViewContext { HttpContext = httpContext }
            };

            await componentTagHelper.ProcessAsync(tagHelperContext, tagHelperOutput);

            var content = tagHelperOutput.Content.GetContent();
            content = Regex.Replace(content, " +__internal_[^ />]+", "");

            httpContextFactory.Dispose(httpContext);
            Console.WriteLine(content);
            File.WriteAllText(@"c:\work\content.html", content);
        }
    }
}
