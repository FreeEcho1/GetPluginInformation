using System.IO;
using System;
using System.Collections.Generic;
using Swindom.IPluginSwindom;

namespace GetPluginInformation;

internal class Processing
{
    /// <summary>
    /// ディレクトリ内のファイルパスを取得
    /// </summary>
    /// <returns>ディレクトリ内のファイルパス</returns>
    private static List<string> GetFilesPath()
    {
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        List<string> path = new();

        for (int index = 1; index < commandLineArgs.Length; index++)
        {
            path.AddRange(Directory.GetFiles(commandLineArgs[index], "*.dll"));
        }
        return path;
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
            List<string> filesPath = GetFilesPath();     // ディレクトリ内のファイルパス

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
                            Console.WriteLine(iPlugin.IsWindowExist.ToString());
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
