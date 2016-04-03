﻿#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion


using Pluton;
using Pluton.SystemProgram;
using Pluton.SystemProgram.Devices;

namespace Pluton.GUI
{







    ///=========================================================================================
    ///
    /// <summary>
    /// GUI контрол фрейма где находятся базовые контролы
    /// кнопка обрабатывающая нажатия
    /// </summary>
    /// 
    ///------------------------------------------------------------------------------------------
    public class AFrame
                    : AWidget
    {

        ///--------------------------------------------------------------------------------------
        private bool m_inputEnableds = true;
        private List<AWidget> m_childs = new List<AWidget>(); //все подцепленные виджеты
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AFrame(int left, int top, int width, int height)
            :
                base(null, left, top, width, height)
        {

        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 2
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AFrame(Rectangle rect)
            :
            base(null, rect.Left, rect.Top, rect.Width, rect.Height)
        {
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 3
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AFrame(AWidget parent)
            :
            base(parent, 0, 0, parent.contentWidth, parent.contentHeight)
        {
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор 4
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AFrame(AWidget parent, int left, int top, int width, int height)
            :
                base(parent, left, top, width, height)
        {

        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор размеры от текущего окна
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AFrame()
            :
            base(null, 0, 0, ASpriteBatch.viewPort.X, ASpriteBatch.viewPort.Y)
        {
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Получить список привязанных виджетов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public List<AWidget> childs
        {
            get
            {
                return m_childs;
            }
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// взять привязанный виджет по его индексу
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AWidget child(int index)
        {
            if (index < 0 || index >= m_childs.Count)
            {
                return null;
            }
            return m_childs[index];
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// Получить количество элементов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int countChilds
        {
            get
            {
                return m_childs.Count;
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Добавление виджета в список
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AWidget addWidget(AWidget widget)
        {
            if (widget.parent != this)
            {
                throw new ArgumentException("!!!!Нельзя добавлять виджет другого родителя", "original");
            }
            m_childs.Add(widget);
            onAddWidget(widget);
            widget.addToFrame(this);
            return widget;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Добавление виджета в список
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onAddWidget(AWidget widget)
        {

        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// удаление виджета из списка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void removeWidget(AWidget widget)
        {
            if (widget.parent != this)
            {
                throw new ArgumentException("Нельзя удалить виджет из списка, родитель принадджеит к дургому списку", "original");
            }
            onRemoveWidget(widget);
            m_childs.Remove(widget);
            widget.removeToFrame(this);
            widget.setParent(null);
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// удаление видждета из списка
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onRemoveWidget(AWidget widget)
        {

        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// удаление всех виджетов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void removeWidget()
        {
            while (m_childs.Count > 0)
            {
                AWidget widget = m_childs[0];
                removeWidget(widget);
                widget.setParent(null);
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// возвращение позиции на карте в знакоместах
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override bool onHandleInput(AInputDevice input)
        {
            if (m_inputEnableds)
            {
                //обробатываем потомков
                for (int index = m_childs.Count - 1; index >= 0; index--)
                {
                    var widget = m_childs[index];
                    if (widget != null && widget.visible && widget.onHandleInput(input))
                    {
                        return true;
                    }
                }

                return base.onHandleInput(input);
            }

            if (input.touchIndex() < 0)
            {
                m_inputEnableds = true;
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------









        ///=====================================================================================
        ///
        /// <summary>
        /// Обновление логики у контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public override void onUpdate(TimeSpan gameTime)
        {
            foreach (AWidget widget in m_childs)
            {
                if (widget.visible)
                {
                    widget.onUpdate(gameTime);
                }
            }
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void draw(ASpriteBatch spriteBatch)
        {
            onDrawBefore(spriteBatch);
            spriteBatch.begin();
            foreach (AWidget widget in m_childs)
            {
                if (widget.visible && !widget.customDraw)
                {
                    widget.render(spriteBatch);
                }
            }



            //отрисуем тестовую обводку
#if RENDER_DEBUG
            if (AInputDevice.testRenderDebug)
            {
                var rect = new Rectangle(
                screenLeft,
                screenTop,
                width,
                height);
                spriteBatch.flush();
                spriteBatch.primitives.drawBorder(rect, 2, Color.RoyalBlue);
            }
            //
#endif  



            spriteBatch.end();
            onDraw(spriteBatch);
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка контрола с учетом располжения родителя
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override void onRender(ASpriteBatch spriteBatch, Rectangle rect)
        {
            ASpriteBatch.TState save = spriteBatch.state;
            spriteBatch.end();
            draw(spriteBatch);
            spriteBatch.begin(save);
        }
        ///--------------------------------------------------------------------------------------








        ///=====================================================================================
        ///
        /// <summary>
        /// Первоночальная отрисовка до контролов
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onDrawBefore(ASpriteBatch spriteBatch)
        {
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Отрисовка контрола
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected virtual void onDraw(ASpriteBatch spriteBatch)
        {
        }
        ///--------------------------------------------------------------------------------------


















        ///=====================================================================================
        ///
        /// <summary>
        /// активация фрейма
        /// после активации, сообщения о нажатии игнорируются, посылаются только новые
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AFrame activeFrame()
        {
            m_inputEnableds = false;
            return this;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// поиск виджетов потомков
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected override AWidget onFindChilds(string name)
        {
            foreach (AWidget child in m_childs)
            {
                AWidget find = child.findWidgetName(name);
                if (find != null)
                {
                    return find;
                }
            }
            return null;
        }
        ///--------------------------------------------------------------------------------------


    }
}
