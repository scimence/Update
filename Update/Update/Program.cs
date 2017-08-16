using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Update
{
    // Update.exe调用示例：

    //// "%~dp0Update.exe" "[CONFIG]https://git.oschina.net/scimence/apkTool/raw/master/files/updateFiles.txt" "E:\tmp2\Update_Files\\" "渠道计费包\0000001\\"
    ///// <summary>
    ///// 调用Update.exe，更新以perfix为前缀的配置文件
    ///// </summary>
    //public static void updateFiles(string perfix)
    //{
    //    string update_EXE = curDir() + "tools\\" + "Update.exe";
    //    string url = "[CONFIG]" + "https://git.oschina.net/scimence/apkTool/raw/master/files/updateFiles.txt";
    //    string path = ToolSetting.Instance().serverRoot;

    //    url = AddQuotation(url);
    //    path = AddQuotation(path);
    //    perfix = AddQuotation(perfix);
    //    update_EXE = AddQuotation(update_EXE);

    //    // 调用更新插件执行软件更新逻辑
    //    String arg = url + " "  + path + " " + perfix;
    //    System.Diagnostics.Process.Start(update_EXE, arg);
    //}

    ///// <summary>
    ///// 为arg添加引号
    ///// </summary>
    //private static string AddQuotation(string arg)
    //{
    //    if (arg.EndsWith("\\") && !arg.EndsWith("\\\\")) arg += "\\";
    //    arg = "\"" + arg + "\"";

    //    return arg;
    //}

    static class Program
    {
        // 调用示例：
        // https://git.oschina.net/scimence/apkTool/raw/master/files/updateFiles.txt E:\tmp\Update_Files\updateFiles.txt 645fe1bc2a99785460ac121d3885f2ba
        // [CONFIG]https://git.oschina.net/scimence/apkTool/raw/master/files/updateFiles.txt E:\tmp\Update_Files\ 渠道计费包\0000001\

        // cmd调用，如：
        // 更新指定的文件： "%~dp0Update.exe" https://git.oschina.net/scimence/apkTool/raw/master/files/updateFiles.txt E:\tmp2\Update_Files\updateFiles.txt 645fe1bc2a99785460ac121d3885f2ba
        // 更新目录下文件："%~dp0Update.exe" [CONFIG]https://git.oschina.net/scimence/apkTool/raw/master/files/updateFiles.txt E:\tmp2\Update_Files\ 渠道计费包\0000001\

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            //string str1 = MD5.fileToString(@"D:\sci\Visual Studio 2008\Projects\Update\Update\bin\Debug\updateFiles.txt");
            //string str2 = MD5.fileToString(@"E:\tmp\apkTool\files\updateFiles.txt");

            string url = "", path = "", arg3 = "", arg4 = "";
            if (args.Length > 0) url = args[0].Trim().Trim('"');
            if (args.Length > 1) path = args[1].Trim().Trim('"');
            if (args.Length > 2) arg3 = args[2].Trim().Trim('"');
            if (args.Length > 3) arg4 = args[3].Trim().Trim('"');

            //url = "[CONFIG]https://git.oschina.net/scimence/apkTool/raw/master/files/updateFiles.txt"; path = @"E:\tmp2\Update_Files\"; arg3 = @"渠道计费包\0000843\->渠道计费包\10000843\";
            //url = "[CONFIG]https://git.oschina.net/joymeng/empty/raw/master/MD5.txt"; path = @"E:\tmp2\Update_Files2\"; arg3 = @"files\";

            if (url.StartsWith("[CONFIG]"))
            {
                url = url.Substring("[CONFIG]".Length);
                updateInnerFiles(url, path, arg3, !arg4.Equals("false"));
            }
            else updateFile(url, path, arg3);   // 更新指定的文件

            //updateFile("https://git.oschina.net/scimence/apkTool/raw/master/files/updateFiles.txt", path, md5);
            //Application.Run();              // 在当前线程上运行应用程序消息循环
        }

        // 更新url指定的文件到filePath，存在MD5值（则判断变动后再更新）
        public static void updateFile(string url, string filePath = "", string md5 = ""/*, bool autoExit = true*/)
        {
            try
            {
                if (!url.Equals(""))
                {
                    if (filePath.Equals("")) filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Update_Files\\" + Path.GetFileName(url);

                    // 删除路径下的原有文件，若文件内容变动了则删除
                    if (File.Exists(filePath))
                    {
                        string fileMD5 = MD5.FileMD5(filePath);
                        if (!fileMD5.Equals(md5)) File.Delete(filePath);    // 文件内容变动，删除现有文件
                        else return;                                        // 文件内容相同，不需要更新
                    }
                    else checkDirectory(filePath);

                    // 下载文件
                    WebClient client = new WebClient();

                    if (filePath.EndsWith(".txt"))
                    {
                        String data = client.DownloadString(url);
                        if (data.Contains("\n") && !data.Contains("\r\n")) // 对于txt文件，进行替换换行符特殊处理
                            data = data.Replace("\n", "\r\n");

                        SaveFile(data, filePath);
                    }
                    else client.DownloadFile(url, filePath);
                }
            }
            catch { }

            //if (autoExit) System.Environment.Exit(0);
        }

        // 判断filePath对应的路径是否存在，不存在则创建
        private static void checkDirectory(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);               // 文件目录
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir); // 文件所在目录不存在，则创建
        }

        /// <summary>  
        /// 保存数据data到文件处理过程，返回值为保存的文件名  
        /// </summary>  
        public static String SaveFile(String data, String filePath)
        {
            System.IO.StreamWriter file1 = new System.IO.StreamWriter(filePath, false, Encoding.UTF8);     //文件已覆盖方式添加内容  

            file1.Write(data);                                                              //保存数据到文件  

            file1.Close();                                                                  //关闭文件  
            file1.Dispose();                                                                //释放对象  

            return filePath;
        }


        //服务器资源目录(https://git.oschina.net/scimence/apkTool/raw/master/files/APK_Base/)
        //本地资源目录(D:\sci\网游打包工具\APK_Tool\APK_Base\)

        //待更新文件(

        //渠道计费包/APK_Tool配置说明.txt(d6b7f448246c96b3f9a72b59ad59725a)
        //渠道计费包/通用配置.txt(d42a2cb1154f9982c7f5085891242cd6)
        //渠道计费包/0000001/ltsdk_30_sdk_platform_jmpay_v1.1.apk(4bf0ccf6be9389d5ab0a4dbff8244545)
        //渠道计费包/0000001/ltsdk_30_sdk_platform_jmpay_v1.1.txt(9696abad6152017e2fd610bdfb82fc7b)
        //渠道计费包/0000066/ltsdk_23_sdk_platform_uc_3.5.3.1.apk(aa95ac50ea11a0aeb0dc243030d22b0d)

        //)待更新文件

        // 更新configUrl中以perfix为前缀的所有文件，到dirPath下
        public static void updateInnerFiles(string configUrl, string dirPath = "", string perfix = "", bool deletOrthers = true)
        {
            if (configUrl.Equals("")) return;
            if (dirPath.Equals("")) dirPath = System.AppDomain.CurrentDomain.BaseDirectory + "Update_Files\\";

            string perfixLocal = "";                                            // 本地存储路径前缀
            if (perfix.Contains("->"))
            {
                int index = perfix.IndexOf("->");
                perfixLocal = perfix.Substring(index + "->".Length);
                perfix = perfix.Substring(0, index);
            }

            // 获取更新配置信息 示例：scimence( Name1(6JSO-F2CM-4LQJ-JN8P) )scimence
            string configInfo = WebSettings.getWebData(configUrl);
            if (configInfo.Equals("")) return;

            string serverDir = WebSettings.getNodeData(configInfo, "服务器资源目录", true).Trim();
            string filesInfo = WebSettings.getNodeData(configInfo, "待更新文件", false).Trim();

            string[] files = filesInfo.Replace("\r\n", "\n").Split('\n');
            List<string> updateFiles = new List<string>();                      // 用于记录更新的文件完整路径信息
            foreach (string file0 in files)
            {
                string file = file0.Trim().TrimEnd(')');
                if (file.Equals("")) continue;
                if (perfix.Contains("\\")) perfix = perfix.Replace("\\", "/");  // 替换为网址分隔符
                if (!perfix.Equals("") && !file.StartsWith(perfix)) continue;   // 不是要更新的文件


                string md5 = "";
                if (file.Contains("(")) // 获取相对文件名和md5值，如：渠道计费包/APK_Tool配置说明.txt(d6b7f448246c96b3f9a72b59ad59725a)
                {
                    int index = file.LastIndexOf("(");
                    md5 = file.Substring(index + 1);
                    file = file.Substring(0, index);
                }
                
                // 更新指定url对应的本地文件
                string localFileName = dirPath + file.Replace("/", "\\");
                if (!perfixLocal.Equals("")) localFileName = dirPath + file.Replace(perfix, perfixLocal).Replace("/", "\\");                     // 替换在线文件路径为本地路径

                updateFile(serverDir + file, localFileName, md5/*, false*/);
                if (deletOrthers && !updateFiles.Contains(localFileName)) updateFiles.Add(localFileName);   // 记录更新的文件
            }

            if (deletOrthers)   // 删除其它非更新文件
            {
                // 删除同步更新目录下，仅存在于本地的文件
                string localPath = dirPath + perfix.Replace("/", "\\");
                if (!perfixLocal.Equals("")) localPath = dirPath + perfixLocal.Replace("/", "\\"); ;                                 // 替换在线文件路径为本地路径
                string localFiles = getAllFiles(localPath);             // 获取本地目录下所有文件
                string[] A = localFiles.Split(';');
                foreach (string F in A)
                {
                    try
                    {
                        if (!updateFiles.Contains(F))
                            File.Delete(F);   // 删除目录下非更新的文件
                    }
                    catch { }
                }
            }

            System.Environment.Exit(0);
        }


        /// <summary>
        /// 关闭名称为processName的所有进程
        /// </summary>
        public static void KillProcess(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);

            foreach (Process process in processes)
            {
                if (process.MainModule.FileName == processName)
                {
                    process.Kill();
                }
            }
        }

        /// <summary>  
        /// 获取目录path下所有子文件名  
        /// </summary>  
        public static string getAllFiles(String path)
        {
            StringBuilder str = new StringBuilder("");
            if (System.IO.Directory.Exists(path))
            {
                //所有子文件名  
                string[] files = System.IO.Directory.GetFiles(path);
                foreach (string file in files)
                    str.Append((str.Length == 0 ? "" : ";") + file);

                //所有子目录名  
                string[] Dirs = System.IO.Directory.GetDirectories(path);
                foreach (string dir in Dirs)
                {
                    string tmp = getAllFiles(dir);  //子目录下所有子文件名  
                    if (!tmp.Equals("")) str.Append((str.Length == 0 ? "" : ";") + tmp);
                }
            }
            return str.ToString();
        }
    }
}
