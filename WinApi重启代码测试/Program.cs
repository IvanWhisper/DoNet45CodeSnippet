using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace WinApi重启代码测试
{
    public class Program
    {
        private delegate uint ZwShutdownSystem(int ShutdownAction);//编译
        private delegate uint RtlAdjustPrivilege(int Privilege, bool Enable, bool CurrentThread, ref int Enabled);
        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(String path);
        [DllImport("kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr lib, String funcName);
        [DllImport("kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);
        //将要执行的函数转换为委托
        private static Delegate Invoke(String APIName, Type t, IntPtr hLib)
        {
            IntPtr api = GetProcAddress(hLib, APIName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            IntPtr hLib = LoadLibrary("ntdll.dll");
            RtlAdjustPrivilege rtla = (RtlAdjustPrivilege)Invoke("RtlAdjustPrivilege", typeof(RtlAdjustPrivilege), hLib);
            ZwShutdownSystem shutdown = (ZwShutdownSystem)Invoke("ZwShutdownSystem", typeof(ZwShutdownSystem), hLib);
            int en = 0;
            uint ret = rtla(0x13, true, false, ref en);//SE_SHUTDOWN_PRIVILEGE = 0x13;     //关机权限
            ret = shutdown(0x1); // POWEROFF = 0x2 // 关机 // REBOOT = 0x1 // 重启
        }
    }
}
