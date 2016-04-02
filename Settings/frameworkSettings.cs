#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
#endregion

namespace Pluton
{
    ///------------------------------------------------------------------------------------










     ///=====================================================================================
    ///
    /// <summary>
    /// Настройка
    /// </summary>
    /// 
    /// -------------------------------------------------------------------------------------
    class AFrameworkSettings
    {
        static public readonly string version = "1.0.0.12"; //версия игры


        static public readonly int spriteSize = 118; //Размерность спрайтов


        //static public readonly Color background = new Color(133, 212, 135); //цвет подложки
        static public readonly Color background = new Color(255, 244, 206); //цвет подложки






        //звукая схема для кнопок
        static public readonly string sound_buttonText  = "click_into_place";
        static public readonly string sound_buttonIcon  = "click_into_place";
        static public readonly string sound_tabs        = "click_into_place"; //нажали на табулятор вкладку


        static public readonly string[] sounds = { "rewind", "reverse_woodpecker", "log_triple", "egg_shaker_quick", "triple_click", "warp_speed", "fuzz_out", "playful_closing", "woosh_in_boots", "pop_top", "click_into_place", "idm_ring" };


        //музыка
        static public readonly string[] musics = { "olga_scotland_mouse_dirigible", "olga_scotland_japanese"};

    }
    ///------------------------------------------------------------------------------------------






     ///=========================================================================================
    ///
    /// <summary>
    /// Цветовая схема
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AThemeColor
    {
        /// <summary>
        /// Цвет заголовка окна
        /// </summary>
        public static readonly Color colorTextUserName = Color.BlueViolet;


        /// <summary>
        /// Цвет заголовка не активной вкладки
        /// </summary>
        public static readonly Color colorRate = Color.BlueViolet;




        /// <summary>
        /// текс иконки выбора уровня
        /// </summary>
        public static readonly Color colorViewIconText = Color.BlueViolet;



        /// <summary>
        /// 
        /// </summary>
        public static readonly Color colorActionMessageBoxTextShadow = new Color(104, 65, 79);



        /// <summary>
        /// 
        /// </summary>
        public static readonly Color colorActionMessageBoxText = new Color(255, 238, 244);








        /// <summary>
        /// Цвет объектов синие
        /// </summary>
        public static readonly Color colorBlue = new Color(93, 89, 165);



        /// <summary>
        /// Цвет объектов зеленые
        /// </summary>
        public static readonly Color colorGreen = new Color(144, 181, 23);



        /// <summary>
        /// Цвет объектов оранживые
        /// </summary>
        public static readonly Color colorOrange = new Color(244, 166, 40);



        /// <summary>
        /// Цвет объектов розовыые
        /// </summary>
        public static readonly Color colorPink = new Color(141, 79, 154);



        /// <summary>
        /// Цвет объектов красные
        /// </summary>
        public static readonly Color colorRed = new Color(222, 23, 16);



        /// <summary>
        /// Цвет объектов желтые
        /// </summary>
        public static readonly Color colorYellow = new Color(254, 237, 0);




        /// <summary>
        /// Цвет шрифта названия игры
        /// </summary>
        public static readonly Color colorTitleHUD = Color.HotPink;



 


        /// <summary>
        /// Цвет заголовка текста достижений
        /// </summary>
        public static readonly Color colorAchivCaption = new Color(255, 138, 0);



        /// <summary>
        /// Цвет оснвого текста достижений
        /// </summary>
        public static readonly Color colorAchivText = Color.White;



        /// <summary>
        /// Цвет текса кнопки экиперовки
        /// </summary>
        public static readonly Color colorTextBuyPremium = new Color(222, 23, 16);






        /// <summary>
        /// Цвет заголовка
        /// </summary>
        public static readonly Color colorTextTitle = new Color(255, 243, 238);


        /// <summary>
        /// Цвет заголовка
        /// </summary>
        public static readonly Color colorTextTitleShadow = new Color(228, 93, 39);



        /// <summary>
        /// цвет надписи о проигрыше в игре
        /// </summary>
        public static readonly Color colorTextFail = new Color(104, 65, 79);




        /// <summary>
        /// цвет текста описание покупки 
        /// </summary>
        public static readonly Color purchaseDescription = new Color(255, 239, 151);



        /// <summary>
        /// цвет текста цены покупки 
        /// </summary>
        public static readonly Color purchaseRate = new Color(187, 232, 164);


        /// <summary>
        /// Цвет заголовка
        /// </summary>
        public static readonly Color purchaseShadow = new Color(228, 93, 39);

    }
    ///------------------------------------------------------------------------------------------
      


}
