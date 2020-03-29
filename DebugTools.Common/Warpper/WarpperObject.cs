using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DebugTools.Common.Warpper
{
    [Serializable]
    public class WarpperObject
    {
        private static BindingFlags COMMON_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.ExactBinding | BindingFlags.FlattenHierarchy ;
        protected Type _objectType = null;
        protected object _object = null;
        public object Object
        {
            get { return _object; }
        }

        protected bool _isDisposed = false;

        protected bool AutoDispose
        {
            get { return false; }
        } 

        public WarpperObject(object obj)
        {
            string progId = GetProgId(); 
            _objectType = obj.GetType();  
            _object = obj;
        }

        //~WarpperObject()
        //{
        //    if (!_isDisposed)
        //    {
        //        Dispose();
        //    }
        //}

        private string GetProgId()
        {
            object[] attributes = this.GetType().GetCustomAttributes(typeof(ComProgIdAttribute), true);
            if (attributes.Length > 0)
            {
                return ((ComProgIdAttribute)attributes[0]).ProgId;
            }
            return null;
        }

        public static Type GetWarpperType<T>(){
            object[] attributes = typeof(T).GetCustomAttributes(typeof(WarpperTargetTypeAttribute), true);
            if (attributes.Length == 0)
            {
                return null;
            }
            WarpperTargetTypeAttribute attribute = (WarpperTargetTypeAttribute)attributes[0];
            if (typeof(T).IsGenericType)
            {
                return attribute.GetWarpperType(GetRealTypes( typeof(T).GetGenericArguments()));
            }
            else
            {
                return attribute.Type;
            }
        }

        public Type GetWarpperType()
        {
            object[] attributes = this.GetType().GetCustomAttributes(typeof(WarpperTargetTypeAttribute), true);
            if (attributes.Length == 0)
            {
                return null;
            }
            WarpperTargetTypeAttribute attribute = (WarpperTargetTypeAttribute)attributes[0];
            return attribute.Type;
        }
      
        public static T CreateInstance<T>(params object[] args) where T : WarpperObject
        {
            Type targetType = typeof(T);
            object[] attributes = targetType.GetCustomAttributes(typeof(WarpperTargetTypeAttribute), true);
            if (attributes.Length == 0)
            {
                return null;
            }
            WarpperTargetTypeAttribute attribute = (WarpperTargetTypeAttribute)attributes[0];
            Type type = attribute.Type;
            if (targetType.IsGenericType)
            {
                type = attribute.GetWarpperType(GetRealTypes(targetType.GetGenericArguments()));
            }
            object o = Activator.CreateInstance(type, args);
            return WarpperObject.CreateWarpperObject<T>(o);
        }

        public object GetField(string field)
        {
            FieldInfo fieldInfo = _objectType.GetField(field, BindingFlags.GetField | COMMON_BINDING_FLAGS);
            return fieldInfo.GetValue(_object);
        }

        public T GetField<T>(string field)
        {
            object obj = this.GetField(field);
            return WarpperObject.CreateWarpperObject<T>(obj);
        }

        public void SetField(string field,object value)
        {
            FieldInfo fieldInfo = _objectType.GetField(field, BindingFlags.SetField | COMMON_BINDING_FLAGS);
            fieldInfo.SetValue(_object, GetRealValue(value));
        }

        public static object GetStaticField<T>(string field)
        {
            Type type = GetWarpperType<T>();
            if (type == null) return null;
            FieldInfo fieldInfo = type.GetField(field, BindingFlags.Static | BindingFlags.GetField | COMMON_BINDING_FLAGS);
            return fieldInfo.GetValue(null);
        }

        public static V GetStaticField<T,V>(string field)
        {

            object obj = GetStaticField<T>(field);
            return WarpperObject.CreateWarpperObject<V>(obj);
        }

        public static void SetStaticField<T>(string field, object value)
        {
            Type type = GetWarpperType<T>();
            if (type == null) return; 
            FieldInfo fieldInfo = type.GetField(field, BindingFlags.Static | BindingFlags.SetField | COMMON_BINDING_FLAGS);
            fieldInfo.SetValue(fieldInfo, GetRealValue(value));
        }

        public object GetProperty(string propertyName)
        {
            return GetProperty(_objectType,propertyName );
        }

        public object GetProperty(Type type,string propertyName)
        {
            object result = null;
            try
            {
                result = type.GetProperty(propertyName, BindingFlags.GetProperty | COMMON_BINDING_FLAGS).GetValue(_object, null);
            }
            catch (Exception e)
            {
                result = e;
            }
            return WarpperObject.CreateWarpperObject<object>(result);
        }

        public object GetProperty(string propertyName, params object[] parameters)
        {
            return this.GetProperty(this._objectType, propertyName, parameters);
        }

        public object GetProperty(Type type, string propertyName, params object[] parameters)
        {
            //object result = type.InvokeMember(propertyName,
            //    BindingFlags.GetProperty | COMMON_BINDING_FLAGS, null, _object, GetRealValues( parameters));
            object result = null;
            try
            {
                if (parameters == null || parameters.Length == 0)
                {
                    result = type.GetProperty(propertyName, BindingFlags.GetProperty | COMMON_BINDING_FLAGS).GetValue(_object, GetRealValues(parameters));
                }
                else
                {
                    result = type.InvokeMember("get_" + propertyName, BindingFlags.InvokeMethod | COMMON_BINDING_FLAGS, null, this._object, parameters);
                }
            }
            catch(AmbiguousMatchException)
            {
                result = type.GetProperty(propertyName, BindingFlags.GetProperty | COMMON_BINDING_FLAGS | BindingFlags.DeclaredOnly).GetValue(_object, GetRealValues(parameters));
            }
            return WarpperObject.CreateWarpperObject<object>(result);
        }

        public T GetProperty<T>(string propertyName)  
        {
            return this.GetProperty<T>(_objectType, propertyName, null);
        }

        public T GetProperty<T>(string propertyName, int index) 
        {
            return this.GetProperty<T>(_objectType, propertyName, index);
        } 

        public T GetProperty<T>(string propertyName, params object[] parameter)  
        {
            return this.GetProperty<T>(_objectType, propertyName, parameter);
        }

        public T GetProperty<T>(Type type,string propertyName, params object[] parameter)  
        {
            object obj = this.GetProperty(type,propertyName, GetRealValues(parameter));
            if (obj == null)
            {
                return default(T);
            }
            return WarpperObject.CreateWarpperObject<T>(obj);
        }

        public T GetProperty<T, P>(string propertyName, P parent)  
        { 
            return this.GetProperty<T, P>(_objectType, propertyName, parent, null);
        }

        public T GetProperty<T, P>(Type type, string propertyName, P parent) 
        {
            return this.GetProperty<T, P>(type, propertyName, parent,null);
        }

        public T GetProperty<T, P>(string propertyName, P parent, int index) 
        {
            return this.GetProperty<T, P>(_objectType, propertyName, parent, index);
        }

        public T GetProperty<T, P>(string propertyName, P parent, params object[] parameter) 
        {
            return this.GetProperty<T, P>(_objectType, propertyName, parent, parameter);
        }

        public T GetProperty<T, P>(Type type,string propertyName, P parent, params object[] parameter)  
        {
            object obj = this.GetProperty(type,propertyName, parameter);
            if (obj == null)
            {
                return default(T);
            }
            return WarpperObject.CreateWarpperObject<T>(obj, parent);
        }

        public void SetProperty(string propertyName,object value)
        {
            this.SetProperty(_objectType, propertyName, value); 
        }

        public void SetProperty(Type type, string propertyName, object value)
        {
            this.SetProperty(_objectType, propertyName, value,null);
        }

        public void SetProperty(Type type,string propertyName, object value,object[] index)
        {
            //type.InvokeMember(propertyName, COMMON_BINDING_FLAGS | BindingFlags.SetProperty, null, _object, new object[] { GetRealValue(value) });
            //type.GetProperty(propertyName, COMMON_BINDING_FLAGS).GetSetMethod().Invoke(_object, new object[] { GetRealValue(value) });
            type.GetProperty(propertyName, COMMON_BINDING_FLAGS).SetValue(_object, GetRealValue(value), index);
        }

        public static void SetStaticProperty<T>(string propertyName, object value, params object[] parameter)
        {
            Type type = GetWarpperType<T>();
            type.GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Static | COMMON_BINDING_FLAGS).SetValue(null, GetRealValue(value),GetRealValues(parameter));
        }

        public static MethodInfo GetStaticMethodInfo<T>(string name)
        {
            Type type = GetWarpperType<T>();
            return type.GetMethod(name, BindingFlags.Static | COMMON_BINDING_FLAGS);
        }

        public static object GetStaticProperty(Type type,string propertyName, params object[] parameter)
        { 
            object result = type.GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Static | COMMON_BINDING_FLAGS).GetValue(null,GetRealValues( parameter));
            if (result == DBNull.Value)
            {
                return null;
            }
            return result;
        }

        public static object GetStaticProperty<T>(string propertyName, params object[] parameter)
        {
            Type type = GetWarpperType<T>();
            object result = type.GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Static | COMMON_BINDING_FLAGS).GetValue(null, GetRealValues(parameter));
            return result;
        }

        public static V GetStaticProperty<T,V>(string propertyName, params object[] parameter) 
        {
            object obj = GetStaticProperty<T>(propertyName, GetRealValues(parameter)); 
            return WarpperObject.CreateWarpperObject<V>(obj);
        } 

        public object InvokeMethod(string methodName)
        {
            return this.InvokeMethod(_objectType, methodName, null, null);
        }

        public object InvokeMethod(Type type, string methodName)
        {
            return this.InvokeMethod(type, methodName, null, null);
        }

        public object InvokeMethod(string methodName, params object[] args)
        {
            return this.InvokeMethod(_objectType, methodName, args, null);
        }

        public object InvokeMethod(Type type,string methodName, params object[] args)
        {
            return this.InvokeMethod(type, methodName, args, null);
        }

        public object InvokeMethod(string methodName, object[] args, ParameterModifier[] modifiers)
        {
            return this.InvokeMethod(_objectType, methodName, args, modifiers);
        }

        public object InvokeMethod(Type type,string methodName, object[] args, ParameterModifier[] modifiers)
        {
            return this.InvokeMethod(_objectType,null, methodName, args, modifiers);
        }

        public object InvokeMethod(Type type, Type[] genericTypes, string methodName, object[] args, ParameterModifier[] modifiers)
        {
            if (genericTypes == null || genericTypes.Length == 0)
            {
                return type.InvokeMember(methodName,
                    BindingFlags.InvokeMethod | COMMON_BINDING_FLAGS, null, _object, GetRealValues(args), modifiers, null, null);
            }
            else
            {
                MethodInfo methodInfo = type.GetMethod(methodName,BindingFlags.InvokeMethod | COMMON_BINDING_FLAGS,null,CallingConventions.Any,genericTypes,modifiers);
                return methodInfo.Invoke(_object, args);
            }
        }

        public T InvokeMethod<T>(string methodName, Type[] genericTypes)
        {
            return this.InvokeMethod<T>(_objectType, methodName, genericTypes, null);
        }

        public T InvokeMethod<T>(string methodName, params object[] args)
        {
            return this.InvokeMethod<T>(_objectType, methodName,args);
        }

        public T InvokeMethod<T>(string methodName, Type[] genericTypes, params object[] args)
        {
            return this.InvokeMethod<T>(_objectType, methodName, genericTypes,args);
        }

        public T InvokeMethod<T>(Type type, string methodName, params object[] args)
        {
            return this.InvokeMethod<T>(type, methodName,null, args);
        }

        public T InvokeMethod<T>(Type type, string methodName, Type[] genericTypes, params object[] args)
        {
            object obj = this.InvokeMethod(type, genericTypes, methodName, GetRealValues(args),null);
            if (obj == null)
            {
                return default(T);
            }
            return WarpperObject.CreateWarpperObject<T>(obj);
        }

        public T InvokeMethod<T,P>(string methodName,P parent, params object[] args) 
        {
            return this.InvokeMethod<T, P>(_objectType ,methodName,parent,args);
        }

        public T InvokeMethod<T, P>(Type type,string methodName, P parent, params object[] args)
        {
            object obj = this.InvokeMethod(methodName, GetRealValues(args));
            if (obj == null)
            {
                return default(T);
            }
            return WarpperObject.CreateWarpperObject<T>(obj, parent);
        }


        public static object InvokeStaticMethod<T>(string methodName,Type[] genericTypes, params object[] args)
        {
            Type type = GetWarpperType<T>();
            if (type == null) return null;
            //return type.InvokeMember(methodName, System.Reflection.BindingFlags.InvokeMethod | BindingFlags.Static | COMMON_BINDING_FLAGS, null, null, GetRealValues(args));

            if (genericTypes == null || genericTypes.Length == 0)
            {
                return type.InvokeMember(methodName,
                    BindingFlags.Static | BindingFlags.InvokeMethod | COMMON_BINDING_FLAGS, null, null, GetRealValues(args), null, null, null);
            }
            else
            {
                MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.InvokeMethod | COMMON_BINDING_FLAGS, null, CallingConventions.Any, genericTypes, null);
                return methodInfo.Invoke(null, args);
            }
        }

        public static object InvokeStaticMethod<T>(string methodName, params object[] args)
        {
            return WarpperObject.InvokeStaticMethod<T>(methodName, null, args);
        }

        public static object InvokeStaticMethod<T>(string methodName)
        {
            return WarpperObject.InvokeStaticMethod<T>(methodName, null);
        }

        public static V InvokeStaticMethod<V>(MethodInfo info, object parent, params object[] args) 
        {
            object obj = info.Invoke(null, GetRealValues(args));
            if (obj == null)
            {
                return default(V);
            }
            return WarpperObject.CreateWarpperObject<V>(obj, parent);
        }

        public static V InvokeStaticMethod<T, V>(string methodName, params object[] args) 
        {
            object obj = WarpperObject.InvokeStaticMethod<T>(methodName, GetRealValues(args));
            if (obj == null)
            {
                return default(V);
            }
            return WarpperObject.CreateWarpperObject<V>(obj);
        }

        public static V InvokeStaticMethod<T, V>(string methodName, object parent, params object[] args)  
        {
            object obj = WarpperObject.InvokeStaticMethod<T>(methodName, GetRealValues(args));
            if (obj == null)
            {
                return default(V);
            }
            return WarpperObject.CreateWarpperObject<V>(obj, parent);
        }

        public static T CreateWarpperObject<T>(object obj)
        {
            return WarpperObject.CreateWarpperObject<T>(obj, null);
        }

        public static T CreateWarpperObject<T>(object obj, object parent)
        {
            if (obj == null || obj == DBNull.Value )
            {
                return default(T);
            }
            if (typeof(T).IsSubclassOf (typeof(WarpperObject)))
            {
                if (parent == null)
                    return (T)Activator.CreateInstance(typeof(T), obj);
                else
                    return (T)Activator.CreateInstance(typeof(T), obj, parent);
            }
            return (T)obj;
        }

        public MemberInfo[] GetMemberInfo<T>(string name)
        {
            return _object.GetType().GetMember(name, COMMON_BINDING_FLAGS);
        }

        public MemberInfo[] GetStaticMemberInfos()
        {
            return _objectType.GetMembers( BindingFlags.Static | COMMON_BINDING_FLAGS );
        }

        public static MemberInfo[] GetMemberInfoS<T>(string name)
        {
            return WarpperObject.GetRealType(typeof(T)).GetMember(name, COMMON_BINDING_FLAGS);
        }

        public static MemberInfo[] GetMemberInfos<T>()
        {
            return WarpperObject.GetRealType(typeof(T)).GetMembers(COMMON_BINDING_FLAGS);
        }

        public MethodInfo GetMethodInfo<T>(string name)
        {
            return _object.GetType().GetMethod(name, COMMON_BINDING_FLAGS);
        }

        public MethodInfo[] GetMethodInfos()
        {
            return this._object.GetType().GetMethods(COMMON_BINDING_FLAGS);
        }

        public static MethodInfo GetMethodInfoS<T>(string name)
        {
            return WarpperObject.GetRealType(typeof(T)).GetMethod(name,COMMON_BINDING_FLAGS);
        }

        public static MethodInfo[] GetMethodInfos<T>()  
        {
            return WarpperObject.GetRealType(typeof(T)).GetMethods(COMMON_BINDING_FLAGS);
        }

        public PropertyInfo[] GetPropertyInfos()
        {
            return this._object.GetType().GetProperties(COMMON_BINDING_FLAGS);
        }

        public static Type GetRealType(Type t)
        {
            if (t == null) return null;
            object[] attributes = t.GetCustomAttributes(typeof(WarpperTargetTypeAttribute), true);
            if (attributes.Length == 0)
            {
                return t;
            }
            WarpperTargetTypeAttribute attribute = (WarpperTargetTypeAttribute)attributes[0];
            if (t.IsGenericType)
            {
                return attribute.GetWarpperType(t.GetGenericArguments());
            }
            else
            {
                return attribute.Type;
            }  
        }

        public static Type[] GetRealTypes(Type[] ts)
        {
            if (ts == null) return null;
            Type[] results = new Type[ts.Length];
            for (int i = 0; i < ts.Length; i++)
            {
                results[i] = GetRealType(ts[i]);
            } 
            return results;
        }

        public static object GetRealValue(object obj)
        {
            if (obj == null) return null;
            if (obj is WarpperObject)
            {
                return ((WarpperObject)obj).Object;
            }
            if (obj.GetType().IsEnum)
            {
                return (int)obj;
            }
            return obj;
        }

        public static object[] GetRealValues(object[] obj)
        {
            if (obj == null) { return null; }
            object[] results = new object[obj.Length ];
            for (int i = 0; i < obj.Length; i++)
            {
                results[i] = GetRealValue(obj[i]);
            }
            return results;
        }

        public EventInfo GetEvent(string asName)
        {
            return this._object.GetType().GetEvent(asName);
        }

        public bool AddEvent(string asName, Delegate aoDelegate)
        {
            try
            {
                GetEvent(asName).AddEventHandler(Object, aoDelegate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public bool RemoveEvent(string asName, Delegate aoDelegate)
        {
            try
            {
                GetEvent(asName).RemoveEventHandler(Object, aoDelegate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        { 
            if (_object == null && obj == null) return true;
            if (_object == null || obj == null) return false;
            if (_object == obj) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        } 

        public virtual void Dispose()
        {
            if (_isDisposed)
            {
                return;
            } 
            //_object = null;
            _isDisposed = true;
        }
         
 
    }
}
