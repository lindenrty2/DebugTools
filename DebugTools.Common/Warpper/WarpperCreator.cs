using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using DebugTools;

namespace DebugTools.Common.Warpper
{

    public class WarpperCreator
    {
        private Type _type;
        public WarpperCreator(Type type)
        {
            _type = type;
        }


        public MemberInfo[] GetMemberInfos()
        {
            return _type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        }
        
        public CodeInfo Create()
        {
            return Create("DebugTools.Warpper");
        }

        public CodeInfo Create(string ns)
        {
            if (_type.IsClass || _type.IsInterface )
            {
                if ((_type.IsSubclassOf(typeof(Delegate))))
                {
                    return CreateDelegate(ns);
                }
                else { 
                    return CreateClass(ns);
                }
            }
            else if (_type.IsEnum)
            {
                return CreateEnum(ns);
            }
            else if (_type.IsValueType)
            {
                return CreateClass(ns);
            }
            else
            {
                return new CodeInfo(string.Empty);
            } 
        }

        public const string DELEGATE_TEMPLATE = @"
using System;
using System.Collections.Generic; 
using System.Text;
using System.ComponentModel;
using DebugTools.Common;
using DebugTools.Common.Warpper;

namespace {5}
{{

    [WarpperTargetType(""{0}"",""{1}"")]
    public delegate {2} {3}({4});

}}";
        public CodeInfo CreateDelegate(string ns)
        {
            string value = string.Empty;
            MethodInfo mi= _type.GetMethod("Invoke");
            string returnType = TypeHelper.GetTypeName(mi.ReturnType,true);
            string parameterDefine = string.Empty;
            foreach (ParameterInfo parameterInfo in mi.GetParameters())
            {
                string typeCode = TypeHelper.GetTypeName(parameterInfo.ParameterType,true);
                parameterDefine += string.Format("{0} {1},", typeCode, parameterInfo.Name);
            }
            parameterDefine = parameterDefine.Trim(',');
            string className = TypeHelper.GetTypeName(_type, true );
            string fullClassName = TypeHelper.GetTypeFullName(_type,false);
            value = string.Format(DELEGATE_TEMPLATE, _type.Assembly.GetName(false).Name, fullClassName, returnType, className, parameterDefine, ns);
            CodeInfo codeInfo = new CodeInfo(className, value, this.GetMemberInfos());
            codeInfo.NameSpace = ns;
            return codeInfo; 
        }

        public const string ENUM_TEMPLATE = @"
using System;
using System.Collections.Generic; 
using System.Text;
using System.ComponentModel;
using DebugTools.Common;
using DebugTools.Common.Warpper;

namespace {4}
{{

    [WarpperTargetType(""{0}"",""{1}"")]
    public enum {2} 
    {{
        {3}
    }}

}}";

        public CodeInfo CreateEnum(string ns)
        {
            string value = string.Empty;
            string[] names = _type.GetEnumNames();
            Array values = _type.GetEnumValues();
            int count = _type.GetEnumNames().Length;
            for (int i = 0; i < count; i++)
            {
                value += string.Format("{0} = {1} ,\r\n",names[i],(int)values.GetValue(i));
            }
            string warpperClassName = TypeHelper.GetTypeWarpperName(_type,true);
            string fullClassName = TypeHelper.GetTypeFullName(_type,false);
            value = string.Format(ENUM_TEMPLATE, _type.Assembly.GetName(false).Name, fullClassName, warpperClassName, value, ns);
            CodeInfo codeInfo = new CodeInfo(warpperClassName, value, this.GetMemberInfos());
            codeInfo.NameSpace = ns;
            return codeInfo;
        }

        public const string CLASS_TEMPLATE = @"
using System;
using System.Collections.Generic; 
using System.Text;
using System.ComponentModel;
using DebugTools.Common;
using DebugTools.Common.Warpper;

namespace {4}
{{

    [WarpperTargetType(""{0}"",""{1}"")]
    public class {2} : WarpperObject
    {{
        public {5}(object obj)
           : base(obj)
        {{

        }}
    {3}
    }}

}}";
        public CodeInfo CreateClass(string ns)
        {
            string value = string.Empty;
            MemberInfo[] infos = GetMemberInfos();
            foreach (MemberInfo member in infos)
            {
                if (member.MemberType == MemberTypes.Field)
                {
                    value += GetFieldCode((FieldInfo)member);
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    value += GetPropertyCode((PropertyInfo)member);
                }
                else if (member.MemberType == MemberTypes.Method)
                {
                    value += GetMethodCode((MethodInfo)member);
                }
                else if (member.MemberType == MemberTypes.Event)
                {
                    value += GetEventCode((EventInfo)member);
                }
                else if (member.MemberType == MemberTypes.Constructor)
                {
                    value += GetConstructorCode((ConstructorInfo)member);
                }
                else if (member.MemberType == MemberTypes.TypeInfo)
                { 
                }
            }

            string className = TypeHelper.GetTypeWarpperName(_type, false);
            string warpperClassName = TypeHelper.GetTypeWarpperName(_type,true);
            string fullClassName = TypeHelper.GetTypeFullName(_type,false);
            value = string.Format(CLASS_TEMPLATE, _type.Assembly.GetName(false).Name, fullClassName, warpperClassName, value, ns, className);
            CodeInfo codeInfo = new CodeInfo(warpperClassName, value, infos);
            codeInfo.NameSpace = ns;
            return codeInfo;
        }

        public const string FIELD_TEMPLATE = @"
    public {0} {1}
    {{
        get {{ 
            return this.GetField<{0}>(""{1}""); 
        }}
        set {{ 
            this.SetField(""{1}"" , value); 
        }}
    }}";

        public const string STATIC_FIELD_TEMPLATE = @"
    public static {0} {1}
    {{
        get {{ 
            return WarpperObject.GetStaticField<{2},{0}>(""{1}""); 
        }}
        set {{ 
            WarpperObject.SetStaticField<{2}>(""{1}"" , value); 
        }}
    }}"; 

        public string GetFieldCode(FieldInfo field)
        {
            string staticCode = string.Empty;
            string staticMethod = string.Empty;
            string warpperName = TypeHelper.GetTypeWarpperName(_type, true);
            string returnWarpperName = TypeHelper.GetTypeWarpperName(field.FieldType, true);
            string template = FIELD_TEMPLATE;
            if (field.IsStatic)
            {
                template = STATIC_FIELD_TEMPLATE;
            }
            return string.Format(template, returnWarpperName, field.Name, warpperName);
        }

        public const string PROPERTY_TEMPLATE = @"
    public {0} {1}
    {{
        {2}
        {3}
    }}";

        public const string PROPERTY_GET_TEMPLATE = @"get {{ 
        return this.GetProperty<{0}>(""{1}""); 
    }}";

        public const string PROPERTY_SET_TEMPLATE = @"set {{ 
        this.SetProperty(""{0}"",value);
    }}";

        public string GetPropertyCode(PropertyInfo property)
        {
            string propertyName = property.Name;
            string propertyTypeCode = TypeHelper.GetTypeWarpperName(property.PropertyType, true); 

            string getCode = string.Empty;
            string setCode = string.Empty;
            if (property.CanRead) { getCode = string.Format(PROPERTY_GET_TEMPLATE, propertyTypeCode, propertyName); }
            if (property.CanWrite) { setCode = string.Format(PROPERTY_SET_TEMPLATE, propertyName); }
            return string.Format(PROPERTY_TEMPLATE, propertyTypeCode, property.Name, getCode, setCode);
        }

        public const string METHOD_TEMPLATE = @"
    public {0} {1}({2})
    {{
        return ({0})this.InvokeMethod<{0}>(""{1}"" {3});
    }}";

        public const string VOID_METHOD_TEMPLATE = @"
    public void {1}({2})
    {{
        this.InvokeMethod(""{1}"" {3});
    }}";

        public const string STATIC_METHOD_TEMPLATE = @"
    public static {0} {1}({2})
    {{
        return ({0})WarpperObject.InvokeStaticMethod<{4},{0}>(""{1}"" {3});
    }}";

        public const string STATIC_VOID_METHOD_TEMPLATE = @"
    public static void {1}({2})
    {{
        WarpperObject.InvokeStaticMethod<{4}>(""{1}"" {3});
    }}"; 

        public string GetMethodCode(MethodInfo methodInfo)
        {
            if (methodInfo.Name.StartsWith("get_") || methodInfo.Name.StartsWith("set_")) return string.Empty;

            string parameterDefine = string.Empty;
            string parameter = string.Empty;
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                string typeCode = TypeHelper.GetTypeWarpperName(parameterInfo.ParameterType,true);
                parameter += "," + parameterInfo.Name;
                parameterDefine += string.Format("{0} {1},", typeCode, parameterInfo.Name);
            }
            parameter = parameter.TrimEnd(',');
            parameterDefine = parameterDefine.Trim(',');
            string template = methodInfo.IsStatic ? STATIC_METHOD_TEMPLATE : METHOD_TEMPLATE;
            string warpperName = TypeHelper.GetTypeWarpperName(_type, true); 

            if (methodInfo.ReturnType == typeof(void))
            {
                template = methodInfo.IsStatic ? STATIC_VOID_METHOD_TEMPLATE : VOID_METHOD_TEMPLATE;
            }
            string returnTypeCode = TypeHelper.GetTypeWarpperName(methodInfo.ReturnType, true);
            string staticCode = string.Empty;
            string staticMethodCode = string.Empty;
            string methodGenericArgs = string.Empty;
            if (methodInfo.IsGenericMethod)
            {
                Type[] types = methodInfo.GetGenericArguments();
                foreach (Type type in types)
                {
                    methodGenericArgs += string.Format("{0},",TypeHelper.GetTypeWarpperName(type,true));
                }
                methodGenericArgs = string.Format("<{0}>", methodGenericArgs.Trim(','));
            }
            return string.Format(template, returnTypeCode, methodInfo.Name + methodGenericArgs, parameterDefine, parameter, warpperName);
    
        }

        public const string CONSTRUCTOR_TEMPLATE = @"
    public static {0} CreateInstance({1})
    {{
        return ({0})WarpperObject.CreateInstance<{0}>({2});
    }}";

        public string GetConstructorCode(ConstructorInfo methodInfo)
        {
            if (methodInfo.Name.Equals(ConstructorInfo.TypeConstructorName)) return string.Empty;
            string parameterDefine = string.Empty;
            string parameter = string.Empty;
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                string typeCode = TypeHelper.GetTypeWarpperName(parameterInfo.ParameterType, true);
                parameter += "," + parameterInfo.Name;
                parameterDefine += string.Format("{0} {1},", typeCode, parameterInfo.Name);
            }
            parameter = parameter.Trim(',');
            parameterDefine = parameterDefine.Trim(',');
            return string.Format(CONSTRUCTOR_TEMPLATE, TypeHelper.GetTypeWarpperName(_type,true), parameterDefine, parameter);
        }

        public const string EVENT_ADD_TEMPLATE = @"
    public bool Add{0}Event ( Delegate eventDelegate)
    {{
        return this.AddEvent(""{0}"" ,eventDelegate);
    }}
    public bool Remove{0}Event ( Delegate eventDelegate)
    {{
        return this.RemoveEvent(""{0}"" ,eventDelegate);
    }}
";

        public string GetEventCode(EventInfo eventInfo)
        {
            return string.Format(EVENT_ADD_TEMPLATE, eventInfo.Name);
        }

    }
}
