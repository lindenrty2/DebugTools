using System;

namespace DebugTools.Common
{
	/// <summary>
	/// ObjectConvert の概要の説明です。
	/// </summary>
	public class ObjectConvert
	{
		public static DateTime ToDateTime(object aoValue)
		{
			return ToDateTime(ToString(aoValue)); 
		}

		public static DateTime ToDateTime(string asValue)
		{
			if(asValue.Length == 8)
			{
				asValue = asValue.Insert(6,"/").Insert(4,"/");
			}
			if(asValue.Length == 10)
			{
				try
				{
                    DateTime result ;
                    if (DateTime.TryParse(asValue, out result))
                        return result;
                    else
                        return DateTime.MinValue;   
				}
				catch
				{
					return DateTime.MinValue;
				}
			}
			return DateTime.MinValue;
		}

 
		/// <summary>
		/// Object -> string(エラー、""は0) 
		/// </summary>
		/// <param name="defaultStr">デフォルトstring</param>
		/// <returns>string文字列</returns>
		public static string ToString(object aoObject)
		{ 
			try
			{	
				return aoObject.ToString(); 
			}
			catch  
			{	
				return string.Empty; 
			} 
		}

		public static double ToDouble(object aoValue)
		{
			return ToDouble(aoValue.ToString());
		}

		public static double ToDouble(string asValue)
		{
			try
			{
                double result = 0.0;
                if (double.TryParse(asValue, out result))
                    return result;
                else
                    return 0.0;  
			}
			catch
			{
				return 0.0;
			}
		}

        public static float ToFloat(object aoValue)
        {

            return ToFloat(aoValue.ToString());
        }

        public static float ToFloat(string asValue)
        {
            try
            {
                float result = 0;
                if (float.TryParse(asValue, out result))
                    return result;
                else
                    return 0;  
            }
            catch
            {
                return 0f;
            }
        }

		public static int ToInt(string asValue)
		{
			return ToInt(asValue,0);
		}

		public static int ToInt(string asValue,int aiDefualt)
		{
			try
			{
                int result = 0;
                if (int.TryParse(asValue,out result))
                    return result;
                else
                    return 0;

			}
			catch
			{
				return aiDefualt;
			}
		}

		public static int ToInt(object aoValue)
		{
			return ToInt(ToString (aoValue ));
		}

		public static int ToInt(double aoValue)
		{
			return ToInt(ToString (Math.Floor( aoValue).ToString() ));
		}

		public static int ToInt(float aoValue)
		{
			return ToInt(ToString (Math.Floor((double)aoValue).ToString() ));
		}

		public static decimal ToDecimal (string asValue)
		{
			try
			{
                decimal result = 0;
                if (decimal.TryParse(asValue, out result))
                    return result;
                else
                    return 0; 
			}
			catch
			{
				return 0;
			}
		}

		public static decimal ToDecimal (object aoValue)
		{
			return ToDecimal(ToString (aoValue ));
		}

		public static bool ToBool(string asValue)
		{
			if(asValue == null)
			{
				return false;
			}
			return asValue.ToUpper() == "TRUE";
		}

		public static bool ToBool(object aoValue)
		{
			return ToBool(ToString(aoValue));
		}

	



	}
}
