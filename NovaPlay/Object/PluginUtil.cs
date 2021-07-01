using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Utils;
using System.IO;
using System.Reflection;

namespace NovaPlay.Object
{
    public class PluginUtil
    {

        public static string GetPluginsFolder()
        {
            string pluginDirectory = Config.GetProperty("PluginDirectory", "Plugins");
            return pluginDirectory;
        }

        public static string GetNovaCore()
        {
            string dir = GetPluginsFolder() + @"\NovaCore";
            bool check = System.IO.Directory.Exists(dir);
            if (!check)
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            return dir;
        }


    }
}
