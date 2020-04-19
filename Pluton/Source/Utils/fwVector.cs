﻿#region Using framework
using System;
using System.Collections;
#endregion

namespace Pluton.Collections
{
    ///--------------------------------------------------------------------------------------








     ///=====================================================================================
    ///
    /// <summary>
    /// базовый класс управления самой коллекцией
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AVector<T>
                           
    {
        ///----------------------------------------------------------------------------------
        private T[] m_elements; //массив элементов
        ///----------------------------------------------------------------------------------







        ///=================================================================================
        ///
        /// <summary>
        /// Инициализация колекции
        /// </summary>
        /// <param name="length">Длинна коллекции</param>
        ///----------------------------------------------------------------------------------
        public AVector(int length)
        {
            m_elements = new T[length];
        }
        ///----------------------------------------------------------------------------------







         ///=================================================================================
        ///
        /// <summary>
        /// Количество задействованых элементов в коллекции
        /// </summary>
        ///----------------------------------------------------------------------------------
        public int count
        {
            get
            {
                return m_elements.Length;
            }
        }
        ///----------------------------------------------------------------------------------






        ///=================================================================================
        ///
        /// <summary>
        /// Количество размера массива элементов всего
        /// </summary>
        ///----------------------------------------------------------------------------------
        public int reserved
        {
            get
            {
                return m_elements.Length;
            }
        }
        ///----------------------------------------------------------------------------------






        ///=================================================================================
        ///
        /// <summary>
        /// Доступ к элементу по индексу
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Элемент</returns>
        ///----------------------------------------------------------------------------------
        
        /*
        public T this[int index]
        {
            get
            {
                return m_elements[index];
            }
        }
        ///----------------------------------------------------------------------------------

        */





         ///=================================================================================
        ///
        /// <summary>
        /// Добавим элемент в массив
        /// </summary>
        ///----------------------------------------------------------------------------------
        public void add(T e)
        {
            int index = searchFree();


            // если список полон, то увеличим базовый размер
            if (index < 0)
            {
                Array.Resize(ref m_elements, m_elements.Length + 1);
                index = m_elements.Length - 1;
            }
            //

            m_elements[index] = e;
        }
        ///----------------------------------------------------------------------------------




         
         ///=================================================================================
        ///
        /// <summary>
        /// Поиск свободного элемента
        /// </summary>
        ///----------------------------------------------------------------------------------
        private int searchFree()
        {
            int iCount = m_elements.Length;
            for (int i = 0; i < iCount; i++)
            {
                if (m_elements[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }
        ///----------------------------------------------------------------------------------








         ///=================================================================================
        ///
        /// <summary>
        /// Поиск элемента
        /// </summary>
        ///----------------------------------------------------------------------------------
        public int indexOf(T e)
        {
            int index = Array.IndexOf(m_elements, e);
            return index;
        }
        ///----------------------------------------------------------------------------------







        ///=================================================================================
        ///
        /// <summary>
        /// Очистка списка
        /// </summary>
        ///----------------------------------------------------------------------------------
        public void clear()
        {
            int iCount = m_elements.Length;
            for (int i = 0; i < iCount; i++)
            {
                m_elements[i] = default(T);
            }
        }
        ///----------------------------------------------------------------------------------








        ///=================================================================================
        ///
        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="e">Элемент</param>
        ///----------------------------------------------------------------------------------
        public void remove(T e)
        {
            int index = indexOf(e);
            if (index >= 0)
            {
                m_elements[index] = default(T);
            }
        }
        ///----------------------------------------------------------------------------------





        ///=================================================================================
        ///
        /// <summary>
        /// Возвращаем итератор для forech
        /// </summary>
        ///----------------------------------------------------------------------------------
        public IEnumerator GetEnumerator()
        {
            return new AIterator(this);
        }
        ///----------------------------------------------------------------------------------






        ///=================================================================================
        ///
        /// <summary>
        /// Реализация итератора для обхода колеекции
        /// </summary>
        /// 
        ///----------------------------------------------------------------------------------
        public class AIterator : IEnumerator
        {
            ///------------------------------------------------------------------------------
            private AVector<T> m_parent = null;
            private int m_currentIndex = -1;
            ///------------------------------------------------------------------------------
            //
            public AIterator(AVector<T> parent)
            {
                m_parent = parent;
            }
            //
            public Object Current
            {
                get
                {
                    return m_parent.m_elements[m_currentIndex];
                }
            }
            //
            public bool MoveNext()
            {
                int iLength = m_parent.m_elements.Length;
                m_currentIndex++;
                while (m_currentIndex < iLength && m_parent.m_elements[m_currentIndex] == null)
                {
                    m_currentIndex++;
                }
                return m_currentIndex < iLength ? true : false;
            }
            //
            public void Reset()
            {
                m_currentIndex = 0;
            }
        }
        ///----------------------------------------------------------------------------------





    }
    ///AVector
    ///--------------------------------------------------------------------------------------


}