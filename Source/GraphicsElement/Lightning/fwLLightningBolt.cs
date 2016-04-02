using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


using Pluton.SystemProgram;
using Pluton.Helper;
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
    /// Один луч
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ALLightingBolt
    {
        ///--------------------------------------------------------------------------------------
        private readonly static int     cSpeed = 150; //скорость базовая
        private readonly static int     cSpeedRange = 100; //диапазон
        ///--------------------------------------------------------------------------------------

        

        private ACollectionConstLength<ALLine> m_segments =     new ACollectionConstLength<ALLine>(30);
        private ACollectionConstLength<AFloat> m_positions =    new ACollectionConstLength<AFloat>(30);


        private Vector2     m_source    = Vector2.Zero;     //начало молнии
        private Vector2     m_dest      = Vector2.Zero;     //конец молнии
        private Color       m_color     = Color.White;      //цвет всех частиц
        private float       m_thickness = 2;                //толщина молний
        private float       m_alpha     = 1.0f;             //анимация
        private float       m_speed     = 1.0f / 100.0f;    //скорость анимации
        private int         m_nodeCount = 15; //количество узлов
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ALLightingBolt()
        {
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// удаление всего
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void clear()
        {
            m_source = Vector2.Zero;
            m_dest = Vector2.Zero;
            m_color = Color.White;
            m_thickness = 2;
            m_alpha = 1.0f;
            m_speed = 1.0f / 100.0f;
            m_nodeCount = 15;
            m_segments.clear();
            m_positions.clear();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// создание новой молнии
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void create(Vector2 source, Vector2 dest, Color color, float thickness, int nodeCount)
        {
            m_source = source;
            m_dest = dest;
            m_color = color;
            m_thickness = thickness;
            m_nodeCount = nodeCount;
            createBolt();
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// создание новой молнии
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void create(Vector2 source, Vector2 dest, Color color)
        {
            m_source    = source;
            m_dest      = dest;
            m_color     = color;
            createBolt();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// создание новой молнии
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void create(Vector2 source, Vector2 dest)
        {
            m_source = source;
            m_dest = dest;
            createBolt();
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Данные с числом 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private class AFloat : AElementConstLength
        {
            public float data;
            public override int compareTo(AElementConstLength obj)
            {
                float objData = (obj as AFloat).data;
                return (data < objData) ? -1 :
                        (data > objData) ? 1 : 0;
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Создание молнии
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void createBolt()
        {
            AHelper helper = AHelper.instance;

            
            //скорость
            m_speed = 1.0f / (cSpeed + (helper.random.Next(0, cSpeedRange) - cSpeedRange / 2));
            m_alpha = 1.0f;


            //молния
            m_segments.clear();
            Vector2 tangent = m_dest - m_source;
            Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
            float length = tangent.Length();

            m_positions.clear();
            m_positions.getNew().data = 0;
            for (int i = 0; i < length / m_nodeCount; i++)
            {
                m_positions.getNew().data = (float)helper.random.NextDouble();
            }
            m_positions.sort();

            const float sway = 80;//80
            const float Jaggedness = 1 / sway;

            Vector2 prevPoint = m_source;
            float fPrevDisplacement = 0;
            for (int i = 1; i < m_positions.count; i++)
            {
                float pos = m_positions[i].data;

                // used to prevent sharp angles by ensuring very close positions also have small perpendicular variation.
                float scale = (length * Jaggedness) * (pos - m_positions[i - 1].data);

                // defines an envelope. Points near the middle of the bolt can be further from the central line.
                float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

                float displacement = helper.random.Next((int)-sway, (int)sway);
                displacement -= (displacement - fPrevDisplacement) * (1 - scale);
                displacement *= envelope;

                Vector2 point = m_source + pos * tangent + displacement * normal;

                var line = m_segments.getNew();
                line.init(prevPoint, point, m_thickness);

                prevPoint = point;
                fPrevDisplacement = displacement;
            }


            var lineBack = m_segments.getNew();
            lineBack.init(prevPoint, m_dest, m_thickness);
        }
        ///--------------------------------------------------------------------------------------




      




         ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка сегмента
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void render(ASpriteBatch spriteBatch, Vector2 ptRegion)
        {
            m_alpha -= (float)spriteBatch.gameTime.TotalMilliseconds * m_speed;
            
            Color color = m_color * (m_alpha * 0.4f + 0.3f);
            foreach (ALLine segment in m_segments)
            {
                segment.render(spriteBatch, color, ptRegion);
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// проверка на анимацию
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isAnimation
        {
            get
            {
                return m_alpha >= 0.0f ? true : false;
            }

        }





    }//ALLightingBolt
}