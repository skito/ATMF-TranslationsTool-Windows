using System.Diagnostics;

namespace ATMF_TranslationsTool
{
    class Utilities
    {
        public static void OpenWebURL(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}
