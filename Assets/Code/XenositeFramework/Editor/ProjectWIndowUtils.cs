using System.Reflection;
using UnityEngine;

namespace XenositeFramework.Editor
{
    public static class ProjectWindowUtil
    {
        public static string GetCurrentProjectFolder()
        {
            var projectBrowserType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
            var browsers = Resources.FindObjectsOfTypeAll(projectBrowserType);

            if (browsers.Length == 0) return null;

            var browser = browsers[0];
            var prop = projectBrowserType.GetProperty("currentFolderPath",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (prop != null)
            {
                return (string)prop.GetValue(browser);
            }

            return null;
        }
    }
}