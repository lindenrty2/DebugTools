using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebugTools.Common.Hook.MethodHook
{ 
    public class MethodHookHelper
    { 

        /// <summary>
        /// 函数Hook
        /// </summary>
        /// <param name="src">原函数</param>
        /// <param name="dest">替换函数</param>
        /// <param name="ori">原函数（为了方便在替换函数中调用原函数）</param>
        /// <returns></returns>
        public static bool hookMethod(MethodInfo src,MethodInfo dest,MethodInfo ori)
        {
            try
            {
                var engine = DetourFactory.CreateDetourEngine();
                engine.Patch(src, dest, ori);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        } 

    }
}
