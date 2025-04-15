using System.Diagnostics;
using System.Text;
using CSV.Diff.Service.Domain.Entities;

namespace CSV.Diff.Service.Domain.Logics;

public static class Extensions
{
    public static string ToCsv(this string?[] values)
    {
        return string.Join(",", values.Select(value => $"\"{value?.Replace("\"", "\"\"")}\""));
    }

    public static void Open(this SavedFile file)
    {
        if (File.Exists(file.Fullname))
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = file.Fullname;
            psi.UseShellExecute = true;
            psi.WorkingDirectory = Environment.CurrentDirectory;
            Process.Start(psi);
        }
    }

    public static void Write(this SavedFile file, string contents)
    {
        var encoder = CodePagesEncodingProvider.Instance.GetEncoding("shift_jis");
        if(encoder == null)
        {
            throw new Exception("Encoder is null!");
        }
        Directory.CreateDirectory(file.Directory);
        File.WriteAllText(file.Fullname, contents, encoder);
    }
}
