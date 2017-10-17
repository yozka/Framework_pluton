#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion




namespace Pluton.GUI
{
    ///------------------------------------------------------------------------------------------
    using Pluton;
    using Pluton.SystemProgram;
    using Pluton.SystemProgram.Devices;
    ///------------------------------------------------------------------------------------------






     ///=========================================================================================
    ///
    /// <summary>
    /// Виджет панелька
    /// 
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class APanel
                :
                    AFrame
    {

        ///--------------------------------------------------------------------------------------
        private EStylePanel mStyle = EStylePanel.none;
        private uint mSpriteID = 0;
        ///--------------------------------------------------------------------------------------





        ///--------------------------------------------------------------------------------------
        ///тип панели
        public enum EStylePanel
        {
            none,
            panel_A,
            panel_B,
            panel_C,
            panel_D,
            panel_E,
            panel_F
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public APanel(int left, int top, EStylePanel style)
        {
            setDefault(style);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// установка данных по умолчанию
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void setDefault(EStylePanel style)
        {
            mStyle = style;
            switch (mStyle)
            {
                case EStylePanel.panel_A:
                    {
                        setMargin(  ATheme.panel_A_marginLeft,
                                    ATheme.panel_A_marginRight,
                                    ATheme.panel_A_marginTop,
                                    ATheme.panel_A_marginBottom);

                        width       = ATheme.panel_A_width;
                        height      = ATheme.panel_A_height;
                        mSpriteID   = ATheme.panel_A_spriteID;
                        break;
                    }
                
                case EStylePanel.panel_B:
                    {
                        setMargin(  ATheme.panel_B_marginLeft,
                                    ATheme.panel_B_marginRight,
                                    ATheme.panel_B_marginTop,
                                    ATheme.panel_B_marginBottom);

                        width       = ATheme.panel_B_width;
                        height      = ATheme.panel_B_height;
                        mSpriteID   = ATheme.panel_B_spriteID;
                        break;
                    }

                case EStylePanel.panel_C:
                    {
                        setMargin(  ATheme.panel_C_marginLeft,
                                    ATheme.panel_C_marginRight,
                                    ATheme.panel_C_marginTop,
                                    ATheme.panel_C_marginBottom);

                        width       = ATheme.panel_C_width;
                        height      = ATheme.panel_C_height;
                        mSpriteID   = ATheme.panel_C_spriteID;
                        break;
                    }

                case EStylePanel.panel_D:
                    {
                        setMargin(  ATheme.panel_D_marginLeft,
                                    ATheme.panel_D_marginRight,
                                    ATheme.panel_D_marginTop,
                                    ATheme.panel_D_marginBottom);

                        width       = ATheme.panel_D_width;
                        height      = ATheme.panel_D_height;
                        mSpriteID   = ATheme.panel_D_spriteID;
                        break;
                    }

                case EStylePanel.panel_E:
                    {
                        setMargin(  ATheme.panel_E_marginLeft,
                                    ATheme.panel_E_marginRight,
                                    ATheme.panel_E_marginTop,
                                    ATheme.panel_E_marginBottom);

                        width       = ATheme.panel_E_width;
                        height      = ATheme.panel_E_height;
                        mSpriteID   = ATheme.panel_E_spriteID;
                        break;
                    }

                case EStylePanel.panel_F:
                    {
                        setMargin(  ATheme.panel_F_marginLeft,
                                    ATheme.panel_F_marginRight,
                                    ATheme.panel_F_marginTop,
                                    ATheme.panel_F_marginBottom);

                        width       = ATheme.panel_F_width;
                        height      = ATheme.panel_F_height;
                        mSpriteID   = ATheme.panel_F_spriteID;
                        break;
                    }
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим тип панели
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public EStylePanel panelStyle
        {
            get
            {
                return mStyle;
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка виджета
        /// рисуем заголовок и фон окна
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onDrawBefore(ASpriteBatch spriteBatch)
        {
            spriteBatch.begin();

            float alpha = this.alpha;

            int left = parentLeft + this.left;
            int top = parentTop + this.top;

            spriteBatch.Draw(spriteBatch.getSprite(mSpriteID), new Vector2(left, top), Color.White * alpha);


#if RENDER_DEBUG
            if (AInputDevice.testRenderDebug)
            {
                spriteBatch.flush();
                spriteBatch.primitives.drawBorder(new Rectangle(screenLeft + marginLeft, screenTop + marginTop, contentWidth, contentHeight), 2, Color.BlueViolet);
            }
            //
#endif 
  
          

  


            spriteBatch.end();
            

        }
        ///--------------------------------------------------------------------------------------











    }
}
