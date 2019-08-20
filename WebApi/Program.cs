using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    internal class CurrentDirectoryHelpers
    {
        /* This is for SQLite databases to work in development mode.
         * There's an issue with aspnet core that sets the base directory path to that of IIS.
         * This class detects the issue and fixes the path to correctly point to the app directory.
         * See more at https://github.com/aspnet/AspNetCore.Docs/issues/9865
         */

        internal const string AspNetCoreModuleDll = "aspnetcorev2_inprocess.dll";
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [System.Runtime.InteropServices.DllImport(AspNetCoreModuleDll)]
        private static extern int http_get_application_properties(ref IISConfigurationData iiConfigData);
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct IISConfigurationData
        {
            public IntPtr pNativeApplication;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.BStr)]
            public string pwzFullApplicationPath;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.BStr)]
            public string pwzVirtualApplicationPath;
            public bool fWindowsAuthEnabled;
            public bool fBasicAuthEnabled;
            public bool fAnonymousAuthEnable;
        }
        public static void SetCurrentDirectory()
        {
            try
            {
                // Check if physical path was provided by ANCM
                var sitePhysicalPath = Environment.GetEnvironmentVariable("ASPNETCORE_IIS_PHYSICAL_PATH");
                if (string.IsNullOrEmpty(sitePhysicalPath))
                {
                    // Skip if not running ANCM InProcess
                    if (GetModuleHandle(AspNetCoreModuleDll) == IntPtr.Zero)
                    {
                        return;
                    }
                    IISConfigurationData configurationData = default(IISConfigurationData);
                    if (http_get_application_properties(ref configurationData) != 0)
                    {
                        return;
                    }
                    sitePhysicalPath = configurationData.pwzFullApplicationPath;
                }
                Environment.CurrentDirectory = sitePhysicalPath;
            }
            catch
            {
                // ignore
            }
        }
    }
}
