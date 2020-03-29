using System;
using System.Runtime.InteropServices;

namespace DebugTools.DBO
{
    public class ConvertHelper
    {
        public static byte[] StructToBytes(object target)
        {
            int size = Marshal.SizeOf(target);
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(target, structPtr, false);
            byte[] bytes = new byte[size - 1 + 1];
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }

        public static T BytesToStruct<T>(byte[] bytes)
        {
            Type type = typeof(T);
            int size = Marshal.SizeOf(type);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return (T)Marshal.PtrToStructure(buffer, type);
            }
            catch (Exception ex)
            {
                return default(T);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}
