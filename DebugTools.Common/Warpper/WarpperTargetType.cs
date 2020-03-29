using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DebugTools.Common.Warpper
{
    public class WarpperTargetTypeAttribute : Attribute 
    {
        private string _targetType = null;
        public string TargetType
        {
            get { return _targetType; }
        }

        private string _assemblyName = null;
        public string AssemblyName
        {
            get { return _assemblyName; }
        } 

        private Type _type = null;
        public Type Type
        {
            get {
                if (_type == null)
                {
                    _type = GetWarpperType();
                }
                return _type; 
            }
        }

        public WarpperTargetTypeAttribute(string assemblyName,string targetType)
        {
            _assemblyName = assemblyName;
            _targetType = targetType;
        } 

        public Type GetWarpperType()
        {
            return AssemblyHelper.GetTargetType(this.AssemblyName, this.TargetType);
        }

        public Type GetWarpperType(params Type[] genericTypes)
        { 
            return AssemblyHelper.GetTargetType(this.AssemblyName, this.TargetType, genericTypes);
            //if (genericTypes != null)
            //{
            //    string genericName = string.Empty;
            //    foreach (Type genericType in genericTypes)
            //    {
            //        genericName += genericType.Name + ",";
            //    }
            //    typeName = string.Format("{0}[{1}]", typeName,genericName.Trim(','));
            //}
            //return targetAssembly.GetType(typeName).MakeGenericType(genericTypes);
        }

        public Assembly GetAssembly()
        {
            return AssemblyHelper.GetAssembly(this.AssemblyName);
        }
         
    }
}
