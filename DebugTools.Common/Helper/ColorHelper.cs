using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DebugTools.Common
{
    public class ColorHelper
    {
        public static Color GetColor(string colorInfo){
            if(string.IsNullOrEmpty(colorInfo)){
                return Color.Empty;
            }
            if(colorInfo.IndexOf(",") > 0){
                string[] v = colorInfo.Split(',');
                if(v.Length != 3){
                    return Color.Empty;
                }
                return Color.FromArgb(
                    ObjectConvert.ToInt(v[0]), 
                    ObjectConvert.ToInt(v[1]), 
                    ObjectConvert.ToInt(v[2])
                    );
            }
            return Color.FromName(colorInfo);

        }

    }
}
