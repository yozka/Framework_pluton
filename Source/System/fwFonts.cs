#region Using framework
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion


namespace Pluton.SystemProgram
{
    public class AFonts
    {
        ///--------------------------------------------------------------------------------------
        public static SpriteFont normal;
        public static SpriteFont small;
        public static SpriteFont big;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// «агрузка шривтов по умолчанию
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static void loadContent(ContentManager content)
        {
            normal      = content.Load<SpriteFont>("Fonts\\fontNormal");
            small       = content.Load<SpriteFont>("Fonts\\fontSmall");
            big         = content.Load<SpriteFont>("Fonts\\fontBig");
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// создадние массива спрайтов, который описывает целочисленное число
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static uint[] intToDigit(int number)
        {
            if (number == 0)
            {
                uint[] zero = new uint[1];
                zero[0] = sprite.aa_gui_digit_0;
                return zero;
            }

            int count = 0;
            int n = number;
            while (n > 0)
            {
                count++;
                n /= 10;
            }

            uint[] digit = new uint[count];
            n = number;
            while (n > 0)
            {
                count--;
                switch (n % 10)
                {
                    case 0: { digit[count] = sprite.aa_gui_digit_0; break; }
                    case 1: { digit[count] = sprite.aa_gui_digit_1; break; }
                    case 2: { digit[count] = sprite.aa_gui_digit_2; break; }
                    case 3: { digit[count] = sprite.aa_gui_digit_3; break; }
                    case 4: { digit[count] = sprite.aa_gui_digit_4; break; }
                    case 5: { digit[count] = sprite.aa_gui_digit_5; break; }
                    case 6: { digit[count] = sprite.aa_gui_digit_6; break; }
                    case 7: { digit[count] = sprite.aa_gui_digit_7; break; }
                    case 8: { digit[count] = sprite.aa_gui_digit_8; break; }
                    case 9: { digit[count] = sprite.aa_gui_digit_9; break; }
                }
                n /= 10;
            }
            return digit;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ќтрисовка числа ввиде списка спрайтов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private const float cHeightDigit = 80.0f;
        public static void renderDigit(ASpriteBatch spriteBatch, uint[] digit, Vector2 pos, Color color, float scale)
        {
            Vector2 next = new Vector2(cHeightDigit * scale, 0);
            pos += new Vector2(cHeightDigit * scale, 128.0f * scale) / 2;
            foreach (var spriteID in digit)
            {
                spriteBatch.drawSprite(spriteID, pos, color, 0, scale);
                pos += next;
            }
 
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим размер базового числа
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static Vector2 measureDigit(uint[] digit, float scale)
        {
            return new Vector2((digit.Length * cHeightDigit) * scale, 128.0f * scale);
        }
        ///--------------------------------------------------------------------------------------




    }
}