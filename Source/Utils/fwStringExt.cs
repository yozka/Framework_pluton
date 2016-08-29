﻿#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion
///--------------------------------------------------------------------------------------


namespace Pluton
{

    
     ///=====================================================================================
    ///
    /// <summary>
    /// Расширение строчки
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public static class stringExt
    {


        
        public static string toParser(this string value)
        {
            int iRN = value.IndexOf("\\n\\");
            if (iRN >= 0)
            {
                return value.Replace("\\n\\", "\n");
            }

            iRN = value.IndexOf("/n/");
            if (iRN >= 0)
            {
                return value.Replace("/n/", "\n");
            }

            return value;
        }


        public static string toSplit(this string value, int length)
        {
            string res = string.Empty;
            int ln = 0;
            bool nextSpace = false;
            var list = value.Split(' ');
            foreach(string s in list)
            {
                if (nextSpace)
                {
                    res += " ";
                }

                res += s;
                ln += s.Length;
                if (ln > length)
                {
                    ln = 0;
                    res += "\n";
                    nextSpace = false;
                }
                else
                {
                    nextSpace = true;
                }
            }

            return res;
        }





        //объеденить массив строк в одну строку
        public static string join(this List<string> value, string prefix)
        {
            string res = string.Empty;
            bool nextSpace = false;
            foreach (string s in value)
            {
                if (nextSpace)
                {
                    res += prefix;
                }

                res += s;
                nextSpace = true;
            }
            return res;
        }

    }

}
