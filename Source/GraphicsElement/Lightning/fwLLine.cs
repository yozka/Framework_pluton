using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


using Pluton.SystemProgram;
using Pluton.Collections;

namespace Pluton.GraphicsElement.Lighting
{
    ///--------------------------------------------------------------------------------------
    ///
    /// ������� ��������� ������
    /// 
    ///--------------------------------------------------------------------------------------







     ///=====================================================================================
    ///
    /// <summary>
    /// ���� ������� ������
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ALLine 
                    : 
                        AElementConstLength
    {
        ///--------------------------------------------------------------------------------------
        private Vector2  m_a;
        private Vector2  m_b;
        private float    m_thickness;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void init(Vector2 a, Vector2 b, float thickness)
        {
            this.m_a = a;
            this.m_b = b;
            this.m_thickness = thickness;
        }
        ///--------------------------------------------------------------------------------------






 



         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void render(ASpriteBatch spriteBatch, Color color, Vector2 ptRegion)
        {
            spriteBatch.primitives.drawLineBlurred(m_a + ptRegion, m_b + ptRegion, m_thickness, color);           
        }
        ///--------------------------------------------------------------------------------------









    }//ALLine
}