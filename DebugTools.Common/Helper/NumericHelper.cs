using System;

namespace DebugTools.Common
{
	/// <summary>
	/// NumericHelper ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class NumericHelper
	{
   
		public static bool CheckRange(string asValue,string asFrom,string asTo)
		{
			return CheckRange(
				System.Convert.ToDecimal(asValue),
				System.Convert.ToDecimal(asFrom),
				System.Convert.ToDecimal(asTo)
				); 
		}

		public static bool CheckRange(int aiValue,int aiFrom,int aiTo)
		{
			return aiFrom <= aiValue && aiValue <= aiTo;
		} 

		public static bool CheckRange(float aiValue,float aiFrom,float aiTo)
		{
			return aiFrom <= aiValue && aiValue <= aiTo;
		}

		public static bool CheckRange(double aiValue,double aiForm,double aiTo)
		{ 
			return aiForm <= aiValue && aiValue <= aiTo;
		}

		public static bool CheckRange(decimal aiValue,decimal aiFrom,decimal aiTo)
		{
			return aiFrom <= aiValue && aiValue <= aiTo;
		}
 
		public static bool CheckRange(object asValue,object asFrom,object asTo)
		{
			return CheckRange(asValue.ToString(),asFrom.ToString(),asTo.ToString());
		}

        public byte[] IntToByte(int i)
        {
            byte[] abyte0 = new byte[4];
            abyte0[0] = (byte)(0xff & i);
            abyte0[1] = (byte)((0xff00 & i) >> 8);
            abyte0[2] = (byte)((0xff0000 & i) >> 16);
            abyte0[3] = (byte)((0xff000000 & i) >> 24);
            return abyte0;
        }
         
	}
}
