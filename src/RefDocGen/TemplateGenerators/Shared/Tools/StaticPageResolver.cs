using Markdig;

namespace RefDocGen.TemplateGenerators.Shared.Tools;

internal record StaticPage(string DirectoryPath, string Name, string Html);

internal class StaticPageResolver
{
    private const string staticFilesFolder = "C:\\Users\\vojta\\UK\\mgr-thesis\\refdocgen\\demo-lib\\pages";

    internal static readonly string cssFile = Path.Join("css", "styles.css");

    internal IEnumerable<StaticPage> GetStaticPages()
    {
        var staticPages = new List<StaticPage>();

        foreach (string filePath in Directory.GetFiles(staticFilesFolder, "*", SearchOption.AllDirectories))
        {
            var file = new FileInfo(filePath);
            if (file.Extension is ".md" or ".html")
            {
                string fileText = File.ReadAllText(filePath);

                if (file.Extension == ".md")
                {
                    string md = File.ReadAllText(filePath);
                    fileText = Markdown.ToHtml(md);
                }

                string pageDir = Path.GetRelativePath(staticFilesFolder, file.DirectoryName ?? staticFilesFolder);

                staticPages.Add(new(pageDir, Path.GetFileNameWithoutExtension(filePath), fileText));

                //string fileName = file.Name;

                //if (fileName == "index")
                //{
                //    staticPages.Insert(0, new("Home", "index.html"), File.ReadAllText(filePath));
                //}
                //else
                //{
                //    string pageName = fileName.Replace("-", " ").Capitalize();
                //    staticPages.Add((pageName, $"./{fileName}.html"));
                //}
            }
        }

        return staticPages;

        //foreach (string dir in Directory.GetDirectories(staticFilesFolder))
        //{
        //    foreach (string file in Directory.GetFiles(dir))
        //    {
        //        if (file.EndsWith(".md") || file.EndsWith(".html"))
        //        {
        //            string fileName = new FileInfo(file).Name.Split('.').First();

        //            if (fileName == "index")
        //            {
        //                staticPages.Insert(0, ("Home", "index.html"));
        //            }
        //            else
        //            {
        //                string pageName = fileName.Replace("-", " ").Capitalize();
        //                staticPages.Add((pageName, $"./{fileName}.html"));
        //            }
        //        }
        //    }
        //}
    }

    internal FileInfo? GetCssFile()
    {
        string cssPath = Path.Join(staticFilesFolder, cssFile);
        return File.Exists(cssPath)
            ? new FileInfo(cssPath)
            : null;
    }

    internal IEnumerable<FileInfo> GetOtherFiles()
    {
        var files = new List<FileInfo>();

        foreach (string filePath in Directory.GetFiles(staticFilesFolder, "*", SearchOption.AllDirectories))
        {
            var file = new FileInfo(filePath);
            if (file.Extension is ".js" or ".json")
            {
                files.Add(file);
            }
        }

        return files;
    }
}
