#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

using Pluton.Collections;

namespace Pluton.SystemProgram
{
    ///------------------------------------------------------------------------------------
    using Pluton.SystemProgram.Devices;





     ///=====================================================================================
    ///
    /// <summary>
    /// Загрузка ресурсов графических
    /// менеджер автоподгружаемых текстур
    /// </summary>
    /// 
    /// -----------------------------------------------------------------------------------------
    public class ASpriteContent
    {
        ///--------------------------------------------------------------------------------------

        private readonly IServiceProvider   mProvider;      //Контент провайдер 
        private readonly Texture2D[]        mListSprites;   //Колллекция текстур
        private readonly uint[]             mAddLoad;       //Спрайты которые нужно загрузить в систему
        private readonly string             mRootDirectory; //адрес где находятся спрайты


        private ContentManager      mContent = null;        //провайдер для создания контента
        private int                 mCountLoad = 0;         //Количество спрайтов которые ждут загрузки
        private Texture2D           mBlankSprites = null;   //Незагруженный спрайт

       ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор загрузчика спрайтов
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public ASpriteContent(IServiceProvider provider, string rootDirectory)
        {
            mRootDirectory = rootDirectory;
            mProvider = provider;
            mListSprites = new Texture2D[sprite.maxNumber + 1];
            mAddLoad = new uint[sprite.count];
            mCountLoad = 0;
        }
        ///--------------------------------------------------------------------------------------






        ///=================================================================================
        ///
        /// <summary>
        /// Доступ к элементу по индексу
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Элемент</returns>
        ///----------------------------------------------------------------------------------
        public Texture2D this[uint index]
        {
            get
            {
                return mListSprites[index];
            }
        }
        ///----------------------------------------------------------------------------------








 
         ///=====================================================================================
        ///
        /// <summary>
        /// Возвращение текстуры спрайта
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public Texture2D getSprite(uint spriteID)
        {
            if (spriteID >= 65535)
            {
                //текстура находится в атласе
                spriteID = spriteID >> 16;
            }

            Texture2D sprite = mListSprites[spriteID];
            if (sprite == null)
            {
                if (spriteID == 0)
                {
                    return mBlankSprites;
                }


                //текстура спрайтовая еще не загружена
                //проверяем, есть ли она в списке на кондидата в загрузку
                for (int i = 0; i < mCountLoad; i++)
                {
                    if (mAddLoad[i] == spriteID)
                    {
                        //она такая спрайта уже требуется и внесена для загрузки.
                        return mBlankSprites;
                    }
                }
                //добавим ее в список на загрузку
                mAddLoad[mCountLoad] = spriteID;
                mCountLoad++;
                return mBlankSprites;
            }
            return sprite;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// Система автоматической подгрузки одного спрайта
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            if (mCountLoad > 0)
            {
                mCountLoad--;
                uint index = mAddLoad[mCountLoad];
                mAddLoad[mCountLoad] = 0;

                if (mListSprites[index] == null)
                {
                    loadSprite(index);
                }
            }
        }
        ///-------------------------------------------------------------------------------------
     





         ///=====================================================================================
        ///
        /// <summary>
        /// загрузка спрайта
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        private void loadSprite(uint index)
        {
            string sName = "Common\\Autoload\\" + ((index / 100) * 100) + "\\" + index;

            try
            {
                if (mContent == null)
                {
                    mContent = new ContentManager(mProvider, mRootDirectory);
                }
                mListSprites[index] = mContent.Load<Texture2D>(sName);
            }
            catch (Exception e)
            {
                gAnalytics.trackException(e);
                mListSprites[index] = null;
                GC.Collect();
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// загрузка контента
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            mBlankSprites = content.Load<Texture2D>("Common\\Autoload\\notLoad");
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Выгрузить контент весь
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void unloadContent()
        {
            unload();
            mBlankSprites = null;
        }
        /// -------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Выгрузить контент весь
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void unload()
        {
            //удалим весь список спрайтов
            int count = mListSprites.Length;
            for (int i = 0; i < count; i++)
            {
                if (mListSprites[i] != null)
                {
                    mListSprites[i].Dispose();
                    mListSprites[i] = null;
                }
            }

            //удалеим список на отложенную загрузку
            mAddLoad.Initialize();
            mCountLoad = 0;

            if (mContent != null)
            {
                mContent.Unload();
                mContent.Dispose();
                mContent = null;
            }
            GC.Collect();
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// проверка, есть спрайты для загрузки или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isLoading()
        {
            return mCountLoad > 0 ? true : false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Принудительная загрузка спрайта
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadForced(uint spriteID)
        {
            uint index = spriteID;
            if (index >= 65535)
            {
                //текстура находится в атласе
                index = index >> 16;
            }

            if (mListSprites[index] == null)
            {
                loadSprite(index);
            }
            
        }
        ///--------------------------------------------------------------------------------------





    }
    ///------------------------------------------------------------------------------------------
    ///
}