#region Using framework
using System;
using System.Reflection;
#endregion
///--------------------------------------------------------------------------------------


namespace Pluton
{


     ///=====================================================================================
    ///
    /// <summary>
    /// Расширение типа объекта
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public static class typeExt
    {




        
        public static bool IsSubclassOf(this Type value, Type compare)
        {
            if (compare == null || value == null)
            {
                return false;
            }
            return value.GetTypeInfo().IsSubclassOf(compare);
        }
   

       



    }

}
