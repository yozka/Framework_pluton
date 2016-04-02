using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Pluton.GUI
{








    
     ///=========================================================================================
    ///
    /// <summary>
    /// Схема текстур и размерность виджетов
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class ATheme
    {

        //клематис clematis seeds


        /*
         * Панельки 
         *
         */
        public const uint   panel_A_spriteID        = sprite.gui_window_panel_a;
        public const int    panel_A_width           = 448;
        public const int    panel_A_height          = 544;
        public const int    panel_A_marginLeft      = 20;
        public const int    panel_A_marginRight     = 20;
        public const int    panel_A_marginTop       = 40;
        public const int    panel_A_marginBottom    = 20;


        public const uint   panel_B_spriteID        = sprite.gui_window_panel_b;
        public const int    panel_B_width           = 352;
        public const int    panel_B_height          = 352;
        public const int    panel_B_marginLeft      = 20;
        public const int    panel_B_marginRight     = 20;
        public const int    panel_B_marginTop       = 40;
        public const int    panel_B_marginBottom    = 20;


        public const uint   panel_C_spriteID        = sprite.gui_window_panel_c;
        public const int    panel_C_width           = 448;
        public const int    panel_C_height          = 416;
        public const int    panel_C_marginLeft      = 20;
        public const int    panel_C_marginRight     = 20;
        public const int    panel_C_marginTop       = 40;
        public const int    panel_C_marginBottom    = 20;


        public const uint   panel_D_spriteID        = sprite.gui_window_panel_d;
        public const int    panel_D_width           = 448;
        public const int    panel_D_height          = 256;
        public const int    panel_D_marginLeft      = 20;
        public const int    panel_D_marginRight     = 20;
        public const int    panel_D_marginTop       = 40;
        public const int    panel_D_marginBottom    = 20;


        public const uint   panel_E_spriteID        = sprite.gui_window_panel_e;
        public const int    panel_E_width           = 384;
        public const int    panel_E_height          = 224;
        public const int    panel_E_marginLeft      = 20;
        public const int    panel_E_marginRight     = 20;
        public const int    panel_E_marginTop       = 20;
        public const int    panel_E_marginBottom    = 20;


        public const uint   panel_F_spriteID        = sprite.gui_window_panel_f;
        public const int    panel_F_width           = 352;
        public const int    panel_F_height          = 128;
        public const int    panel_F_marginLeft      = 20;
        public const int    panel_F_marginRight     = 20;
        public const int    panel_F_marginTop       = 20;
        public const int    panel_F_marginBottom    = 20;
        //////







        /*
         * Dockwidget
         * окно с именем и кнопками
         * 
         */ 
        public const uint   dockwidget_title_A_spriteID         = sprite.gui_window_panel_title_a;
        public const int    dockwidget_title_A_width            = 352;
        public const int    dockwidget_title_A_height           = 128;
        public const int    dockwidget_title_A_captionWidth     = 300;
        public const int    dockwidget_title_A_captionHeight    = 100;
        public static readonly Point dockwidget_title_A_origin  = new Point(0, -52);
        public static readonly Vector2 dockwidget_title_A_shift = new Vector2(0, 0);
  


        public const uint   dockwidget_title_B_spriteID         = sprite.gui_window_panel_title_b;
        public const int    dockwidget_title_B_width            = 256;
        public const int    dockwidget_title_B_height           = 128;
        public const int    dockwidget_title_B_captionWidth     = 200;
        public const int    dockwidget_title_B_captionHeight    = 100;
        public static readonly Point dockwidget_title_B_origin  = new Point(0, -60);
        public static readonly Vector2 dockwidget_title_B_shift = new Vector2(0, -8);


        public const uint   dockwidget_title_C_spriteID         = sprite.gui_window_panel_title_c;
        public const int    dockwidget_title_C_width            = 344;
        public const int    dockwidget_title_C_height           = 128;
        public const int    dockwidget_title_C_captionWidth     = 260;
        public const int    dockwidget_title_C_captionHeight    = 100;
        public static readonly Point dockwidget_title_C_origin  = new Point(0, -52);
        public static readonly Vector2 dockwidget_title_C_shift = new Vector2(0, 10);
        
        
        public static readonly Vector2  dockwidget_titleShift               = new Vector2(0, -60);
        public static readonly Color    dockwidget_colorCaptionTextShadow   = new Color(177, 36, 52);
        public static readonly Color    dockwidget_colorCaptionText         = new Color(255, 233, 236);

        //системная кнопка виджета
        //закрытие диалога
        public const uint   dockwidget_cancelSpriteID = sprite.gui_button_icon;
        public const int    dockwidget_sysTop  = -10;
        public const int    dockwidget_sysLeft = -30;
        ///////////////////////




        /*
         * Кнопки 
         *
         */
        public const uint   buttonText_spriteID = sprite.gui_button_text;
        public const int    buttonText_width        = 260;
        public const int    buttonText_height       = 110;

        public const int    buttonText_imgWidth     = 256;
        public const int    buttonText_imgHeight    = 96;

        public static readonly Vector2 buttonText_shift     = new Vector2(0, 0);
        public static readonly Color buttonText_color       = Color.White;
        public static readonly Color buttonText_colorCheck  = new Color(255, 108, 0);




        /*
         * 
         * Пагинатор
         * 
         * 
         */
        public const uint   pagination_activeSpriteID   = sprite.gui_pagination_active;
        public const uint   pagination_spriteID         = sprite.gui_pagination;

        public const int    pagination_size             = 32;
        public const int    pagination_marginBottom     = 60;
        public const int    pagination_widthPoint       = 40;






        /*
         * 
         * виджет показа времени
         * 
         * 
         */
        public const uint   displayTime_spriteID = sprite.gui_data_time;
        public const int    displayTime_width   = 256;
        public const int    displayTime_height  = 96;
        public static readonly Vector2 displayTime_shift = new Vector2(95, 20);






        /*
         * 
         * виджет показа очков
         * 
         * 
         */
        public const uint   displayScore_spriteID = 0;
        public const int    displayScore_width = 256;
        public const int    displayScore_height = 96;
        public static readonly Vector2 displayScore_shift = new Vector2(95, 20);









        /*
         * таб вкладки 
         *
         */
        public const uint   tabs_spriteID   = sprite.gui_tab;
        public const int    tabs_imgWidth   = 96;
        public const int    tabs_imgHeight  = 64;
        public const int    tabs_sizeIcon   = 64;

        public static readonly Vector2 tabs_shift = new Vector2(0, 5);
        public static readonly Vector2 tabs_shiftIcon = new Vector2(96 / 2, 37);

 





        /// <summary>
        /// Цвет текста описания юнита
        /// </summary>
        public static readonly Color colorTextDescription = new Color(127, 64, 39);






        /*
         * МЕНЮ
         * 
         * Цвет текста в меню
         * 
        */
        public const uint            dockmenu_spriteID  = sprite.gui_icon_popup_menu;
        public static readonly Color dockmenu_shadow    = new Color(153, 56, 82); //тень
        public static readonly Color dockmenu_text      = new Color(242, 242, 242); //основной текст




    }
    ///------------------------------------------------------------------------------------------
   

}
