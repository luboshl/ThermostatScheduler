using System.Text.RegularExpressions;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Routing;

namespace Scheduler.App
{
    public class CustomRouteStrategy : DefaultRouteStrategy
    {
        public CustomRouteStrategy(DotvvmConfiguration configuration, string viewsFolder, string pattern = "*.dothtml")
            : base(configuration, viewsFolder, pattern)
        {
        }

        protected override string GetRouteName(RouteStrategyMarkupFileInfo file)
        {
            return GetFileRelativePathWithoutFileName(file)
                .Replace('/', '_');
        }

        protected override string GetRouteUrl(RouteStrategyMarkupFileInfo file)
        {
            var fileRelativePathWithoutFileName = GetFileRelativePathWithoutFileName(file);
            return SplitWordsByCamelCaseNotation(fileRelativePathWithoutFileName)
                .ToLowerInvariant();
        }

        private static string GetFileRelativePathWithoutFileName(RouteStrategyMarkupFileInfo file)
        {
            var lastSlashIndex = file.ViewsFolderRelativePath.LastIndexOf('/');
            var fileRelativePathWithoutFileName = file.ViewsFolderRelativePath
                .Substring(0, lastSlashIndex);
            return fileRelativePathWithoutFileName;
        }

        private static string SplitWordsByCamelCaseNotation(string fileRelativePathWithoutFileName)
        {
            // Replaces /Events/EventList/SomethingXYZ with /Events/Event-List/Something-XYZ
            return Regex.Replace(fileRelativePathWithoutFileName, "(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])", "-$1");
        }
    }
}