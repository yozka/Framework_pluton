using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


using Pluton.Helper;
using Pluton.SystemProgram;
using Pluton.Collections;


namespace Pluton.GraphicsElement.Lighting
{
    ///--------------------------------------------------------------------------------------
    ///
    /// Система отрисовки молнии
    /// 
    ///--------------------------------------------------------------------------------------







     ///=====================================================================================
    ///
    /// <summary>
    /// Отрисовка молнии ввиде круга
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ALCircle
                   
    {
        ///--------------------------------------------------------------------------------------
        private List<ALLightingBolt>    m_bolts     = new List<ALLightingBolt>();
        private List<float>             m_angles    = new List<float>();
        private Vector2                 m_source    = Vector2.Zero;
        private float                   m_radius    = 0.0f;
        private int                     m_count     = 0;
        private Color                   m_color     = Color.White;
        private float                   m_thickness = 2;                //толщина молний
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Создание круга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void create(Vector2 source, float radius, int count, Color color, float thickness)
        {
            m_source    = source;
            m_radius    = radius;
            m_count     = count;
            m_color     = color;
            m_thickness = thickness;
            createCircle();
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Создание круга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void create(Vector2 source, float radius, int count, Color color)
        {
            m_source = source;
            m_radius = radius;
            m_count = count;
            m_color = color;
            createCircle();
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// Создание круга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void create(Vector2 source)
        {
            m_source    = source;   
            createCircle();
        }
        ///--------------------------------------------------------------------------------------

       











         ///=====================================================================================
        ///
        /// <summary>
        /// Создание круга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void createCircle()
        {
            m_bolts.Clear();
            m_angles.Clear();
            for (int i = 0; i < m_count; i++)
            {
                float angle = (360.0f / m_count) * i;

                ALLightingBolt bolt = new ALLightingBolt();
                bolt.create(m_source, destCircle(m_source, m_radius, angle), m_color, m_thickness, 5);
                m_bolts.Add(bolt);
                m_angles.Add(angle);
            }
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка сруга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void render(ASpriteBatch spriteBatch, Vector2 ptRegion)
        {
            for (int i = 0; i < m_bolts.Count; i++ )
            {
                ALLightingBolt bolt = m_bolts[i];
                bolt.render(spriteBatch, ptRegion);
                if (!bolt.isAnimation)
                {
                    float angle = m_angles[i];// +((180.0f / 500.0f) * (float)gameTime.TotalMilliseconds);
                    bolt.create(m_source, destCircle(m_source, m_radius, angle));
                    //m_angles[i] = angle;
                }
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// подсчет новых координат для круга
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private Vector2 destCircle(Vector2 center, float radius, float angle)
        {
            AHelper helper = AHelper.instance;
            float fAngle = angle * (float)Math.PI / 180.0f;
            Vector2 dest = new Vector2((float)Math.Sin(fAngle) * radius, (float)Math.Cos(fAngle) * radius);
            return center + dest;
        }
        ///--------------------------------------------------------------------------------------







    }//ALLine
}