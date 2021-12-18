namespace AwesomeBlazor.Models.Test;

internal static class TestFixture
{
    public static string GetContentsForTest()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample.md");
        var contents = File.ReadAllText(path);
        return contents;
    }
}
