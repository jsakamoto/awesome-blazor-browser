using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AwesomeBlazorBrowser.PreRenderer
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var content = await RenderComponentAsync(typeof(App));
            RewriteHtmlFile(targetHtmlFilePath: args[0], content);
        }

        public static async Task<IHtmlContent> RenderComponentAsync(
            Type componentType,
            Action<IServiceCollection> configureServices = null,
            IDictionary<string, object> parameters = null,
            RenderMode renderMode = RenderMode.Static)
        {
            var diContainer = new ServiceCollection();
            diContainer.AddLogging();
            diContainer.AddRazorPages();
            configureServices?.Invoke(diContainer);
            diContainer.TryAddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000/") });

            using var serviceProvider = diContainer.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();

            var featureCollection = new FeatureCollection();
            featureCollection.Set<IHttpRequestFeature>(new HttpRequestFeature
            {
                Protocol = "HTTP/2",
                Scheme = "http",
                Method = "GET",
                PathBase = "",
                Path = "/",
                QueryString = "",
                RawTarget = "/",
                Headers = { { "Host", "localhost:5000" } }
            });
            featureCollection.Set<IHttpResponseFeature>(new HttpResponseFeature());

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
                ComponentType = componentType,
                RenderMode = renderMode,
                Parameters = parameters,
                ViewContext = new ViewContext { HttpContext = httpContext }
            };

            await componentTagHelper.ProcessAsync(tagHelperContext, tagHelperOutput);

            httpContextFactory.Dispose(httpContext);

            return tagHelperOutput.Content;
        }

        private static string GetContent(this IHtmlContent htmlContent)
        {
            using var stringWriter = new StringWriter();
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }

        enum RewritingHtmlState
        {
            BeforeMarker,
            InsideMarkers,
            AfterMarker
        }

        private static void RewriteHtmlFile(string targetHtmlFilePath, IHtmlContent htmlContent)
        {
            var sourceHtmlLines = File.ReadAllLines(targetHtmlFilePath);

            var content = htmlContent.GetContent();
            content = Regex.Replace(content, " +__internal_[^ />]+", "");

            var state = RewritingHtmlState.BeforeMarker;
            using var targetHtmlFileWriter = File.CreateText(targetHtmlFilePath);
            foreach (var sourceHtmlLine in sourceHtmlLines)
            {
                state = sourceHtmlLine.EndsWith("<!-- END PRERENDERING -->") ? RewritingHtmlState.AfterMarker : state;

                if (state != RewritingHtmlState.InsideMarkers)
                    targetHtmlFileWriter.WriteLine(sourceHtmlLine);

                if (sourceHtmlLine.EndsWith("<!-- BEGIN PRERENDERING -->"))
                {
                    state = RewritingHtmlState.InsideMarkers;

                    targetHtmlFileWriter.Write(content);
                    targetHtmlFileWriter.WriteLine();
                }
            }
        }
    }
}
