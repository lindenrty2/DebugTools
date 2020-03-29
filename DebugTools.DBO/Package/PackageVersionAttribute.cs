using System;

namespace DebugTools.DBO
{
    public class PackageVersionAttribute : Attribute
    {
        private int _verNo;
        public int VerNo
        {
            get
            {
                return _verNo;
            }
        }

        public PackageVersionAttribute(int verNo)
        {
            _verNo = verNo;
        }
    }
}
