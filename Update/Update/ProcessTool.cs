using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Update
{
    public class ProcessTool
    {
        public static void test()
        {
            String processName = "ChargeModule_V4";
            killProcess(processName);
        }

        /// <summary>
        /// 结束指定Exe对应的进程
        /// </summary>
        public static void killProcessExe(String exeFile)
        {
            if (File.Exists(exeFile) && exeFile.ToLower().EndsWith(".exe"))
            {
                String name = Path.GetFileNameWithoutExtension(exeFile);
                killProcess(name);
            }
        }

        /// <summary>
        /// 结束指定名称的进程
        /// </summary>
        public static void killProcess(String processName)
        {
            //Process current = Process.GetCurrentProcess();    // 当前进程
            //current.ProcessName                               // 当前进程名

            Process[] processes = Process.GetProcessesByName(processName); // 获取指定名称的进程
            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch (Exception ex) { }
            }
        }

        /// <summary>
        /// 延时Milliseconds毫秒
        /// </summary>
        public static void delay(double Milliseconds)
        {
            long preTime = DateTime.Now.AddMilliseconds(Milliseconds).Ticks;
            while (DateTime.Now.Ticks < preTime) ;
        }

        /// <summary>
        /// 判断指定名称的进程是否正在运行
        /// </summary>
        public static bool ProcessIsRunning(String processName)
        {
            if (File.Exists(processName)) processName = Path.GetFileNameWithoutExtension(processName);

            Process[] processes = Process.GetProcessesByName(processName); // 获取指定名称的进程
            return (processes != null && processes.Length > 0);
        }
    }
}
