#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

namespace Enemies.MonoGame
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region Include Libraries to path

            string baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

#if DEBUG
            Environment.CurrentDirectory = new DirectoryInfo("../../../Enemies.Content/").FullName;
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
#endif

            string platform = Environment.OSVersion.Platform == PlatformID.Unix ? "linux" : "win32";
            string arch = Environment.Is64BitProcess ? "x64" : "x86";
            string libDir = Path.Combine(baseDir, "lib", platform, arch);
            Environment.SetEnvironmentVariable("PATH", libDir + ";" + Environment.GetEnvironmentVariable("PATH"));
            #endregion

            using (var game = new MainGame())
                game.Run();
        }
    }
}
