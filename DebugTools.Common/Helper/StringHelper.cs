using System;
using System.Collections;

namespace DebugTools.Common
{
	/// <summary>
	/// StringHelper �̊T�v�̐����ł��B
	/// </summary>
	public class StringHelper
	{

		public static bool CheckRange(string asValue,string asFrom,string asTo)
		{ 
			return !(String.Compare(asFrom, asValue) > 0 || String.Compare(asValue, asTo) > 0);  
		}

		public static bool CheckRange(byte abValue,byte abFrom,byte abTo)
		{ 
			return abFrom <= abValue && abValue <= abTo; 
		}

		public static bool CheckRange(char acValue,char acFrom,char acTo)
		{ 
			return acFrom <= acValue && acValue <= acTo; 
		}

 
	}
}
