using System.IO;
using System;
using Swindom.IPluginSwindom;

namespace GetPluginInformation;

internal class Processing
{
    /// <summary>
    /// ディレクトリ内のファイルパスを取得
    /// </summary>
    /// <returns>ディレクトリ内のファイルパス</returns>
    private static string[] GetFilesPath()
    {
        return Directory.GetFiles(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "Plugins", "*.dll");
    }

    /// <summary>
    /// プラグインの情報を書き込む
    /// </summary>
    public static void WritePluginsInformation()
    {
        try
        {
            string? iPluginTypeName = typeof(IPlugin).FullName;       // プラグインインターフェースの型名
            if (iPluginTypeName == null)
            {
                return;
            }
            string[] filesPath = GetFilesPath();     // ディレクトリ内のファイルパス

            foreach (string nowFilePath in filesPath)
            {
                try
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(nowFilePath);
                    foreach (Type type in assembly.GetTypes())
                    {
                        // プラグインのみ書き込む
                        if (type.IsClass && type.IsPublic
                            && type.IsAbstract == false
                            && type.GetInterface(iPluginTypeName) != null
                            && string.IsNullOrEmpty(type.FullName) == false)
                        {
                            IPlugin? iPlugin = (IPlugin?)assembly.CreateInstance(type.FullName);
                            if (iPlugin == null)
                            {
                                break;
                            }
                            string pluginName = iPlugin.PluginName;
                            if (string.IsNullOrEmpty(pluginName))
                            {
                                pluginName = Path.GetFileNameWithoutExtension(nowFilePath);
                            }
                            Console.WriteLine(nowFilePath);
                            Console.WriteLine(pluginName);
                            break;
                        }
                    }
                }
                catch
                {
                }
            }
        }
        catch
        {
        }
    }
}
