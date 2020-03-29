using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugTools.Common
{
    public class TypeHelper
    {
        public static string GetTypeName(Type type, bool includeGenericInfo)
        {
            string typeName = type.Name;
            int index = typeName.IndexOf("`");
            if (index >= 0) typeName = typeName.Substring(0,index); 
            if (type.IsGenericType && includeGenericInfo)
            { 
                Type[] genericArgs = type.GetGenericArguments();
                string genericArgTypeCode = string.Empty;
                foreach (Type genericArgType in genericArgs)
                {
                    genericArgTypeCode += "," + GetTypeName(genericArgType, includeGenericInfo);
                }
                genericArgTypeCode = genericArgTypeCode.Trim(',');
                typeName += "<" + genericArgTypeCode + ">";
            }
            return typeName;
        }

        public static string GetTypeFullName(Type type, bool includeGenericInfo)
        {
            string typeName = type.FullName ;
            int index = typeName.IndexOf("`");
            if (index >= 0) typeName = typeName.Substring(0,index); 
            if (type.IsGenericType && includeGenericInfo)
            { 
                Type[] genericArgs = type.GetGenericArguments();
                string genericArgTypeCode = string.Empty;
                foreach (Type genericArgType in genericArgs)
                {
                    genericArgTypeCode += "," + GetTypeName(genericArgType, includeGenericInfo);
                }
                genericArgTypeCode = genericArgTypeCode.Trim(',');
                typeName+="<" + genericArgTypeCode + ">";
            }
            return typeName;
        }

        internal static string GetTypeWarpperName(Type type, bool includeGenericInfo)
        {
            bool isArray = type.IsArray;
            if (isArray)
            {
                type = type.GetElementType();
            }

            string typeName = type.Name;
            int index = typeName.IndexOf("`");
            if (index >= 0) typeName = typeName.Substring(0,index);

   
            if (type.IsGenericType && includeGenericInfo)
            {
                Type[] genericArgs = type.GetGenericArguments();
                string genericArgTypeCode = string.Empty;
                foreach (Type genericArgType in genericArgs)
                {
                    genericArgTypeCode += "," + GetTypeWarpperName(genericArgType, includeGenericInfo);
                }
                if (type.Namespace.StartsWith("Nec.") || genericArgTypeCode.IndexOf("Warpper") >= 0)
                {
                    typeName = string.Format("{0}Warpper", typeName);
                }
                genericArgTypeCode = genericArgTypeCode.Trim(',');
                typeName += "<" + genericArgTypeCode + ">";
            }
            else
            {
                if (!type.IsGenericParameter && type.Namespace.StartsWith("Nec."))
                {
                    typeName = string.Format("{0}Warpper", typeName);
                }
            }
            if (isArray)
            {
                typeName += "[]";
            }
            return typeName;
        }

        public static string GetTypeSafeName(Type type)
        {
            return type.Name; 
        }

        public static Type[] GetTypes<T>()
        {
            return new Type[] { typeof(T) };
        }

        public static Type[] GetTypes<T1,T2>()
        {
            return new Type[] { typeof(T1), typeof(T2) };
        }
         
    }
}
