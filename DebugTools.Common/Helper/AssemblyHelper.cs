using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DebugTools.Common.Warpper;

namespace DebugTools.Common
{
    public class AssemblyHelper
    {
        public static Assembly GetAssembly(string name)
        { 
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly targetAssembly = null;
            foreach (Assembly assembly in assemblies)
            {
                if (assembly.GetName(false).Name.Equals(name))
                {
                    targetAssembly = assembly;
                }
            }
            return targetAssembly;
        }

        public static Type GetTargetType(string assemblyName,string targetName)
        {
            Assembly targetAssembly = GetAssembly(assemblyName);
            if (targetAssembly == null) { return null; }
            Type type = targetAssembly.GetType(targetName);
            return type;
        }

        public static Type GetTargetType(string assemblyName, string targetName, params Type[] genericTypes)
        {
            Assembly targetAssembly = GetAssembly(assemblyName);
            if (targetAssembly == null) { return null; }
            return targetAssembly.GetType(targetName).MakeGenericType(genericTypes);
        }

        public static T CreateInstance<T>(Type type,params object[] parameters)
        {
            object o = Activator.CreateInstance(type, parameters);
            return WarpperObject.CreateWarpperObject<T>(o);
        }


    }
}
