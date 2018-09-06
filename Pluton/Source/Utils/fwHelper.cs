using System;
using System.Collections.Generic;
using System.Text;

namespace Pluton.Helper
{
    ///--------------------------------------------------------------------------------------






     ///=====================================================================================
    ///
    /// <summary>
    /// Синглентон с базовыми вспомогательными функциями
    /// 1. случайные числа
    /// 2. время
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AHelper : ASingleton<AHelper>
    {
        public readonly Random       random = new Random();
        
        
        /// Вызовет защищенный конструктор класса Singleton
        public AHelper() 
        {
            random.Next();
        }


    }

}
