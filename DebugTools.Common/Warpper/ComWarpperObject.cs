using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DebugTools.Common.Warpper
{
    public abstract class ComWarpperObject : WarpperObject 
    { 

        protected bool AutoDisposeCom
        {
            get { return false; }
        }

        public ComWarpperObject(string progId)
            : base(CreateComObject(progId))
        {
            try
            {
                _objectType = Type.GetTypeFromProgID(progId); 
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        private static object CreateComObject(string progId)
        {
            try
            {
                Type objectType = Type.GetTypeFromProgID(progId);
                return Activator.CreateInstance(objectType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ComWarpperObject(object obj) : base(obj)
        { 
        }

        ~ComWarpperObject()
        {
            if (!_isDisposed)
            {
                Dispose();
            }
        }

        private string GetProgId()
        {
            object[] attributes = this.GetType().GetCustomAttributes(typeof(ComProgIdAttribute), true);
            if (attributes.Length > 0)
            {
                return ((ComProgIdAttribute)attributes[0]).ProgId;
            }
            return null;
        }
 
        public override void Dispose()
        {
            if (AutoDisposeCom)
            {
                DisposeCom();
            } 
            base.Dispose(); 
        }

        protected void DisposeCom()
        {
            while (Marshal.ReleaseComObject(_object) > 0) { }
        }
 
    }
}
