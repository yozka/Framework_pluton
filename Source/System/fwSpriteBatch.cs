#region Using framework
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace Pluton.SystemProgram
{
    ///------------------------------------------------------------------------------------
    using Pluton.Collections;
    ///------------------------------------------------------------------------------------









   
     ///=====================================================================================
    ///
    /// <summary>
    /// Информация о настройках экрана
    /// </summary>
    /// 
    /// -----------------------------------------------------------------------------------------
    public class ADisplayInfo
    {
        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Физический размер экрана в пикселях
        /// </summary>
        public Point displayHardScreen = Point.Zero;




        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Размер виртулаьного дисплея для задания коэфицента масштабирования
        /// </summary>
        public Point displayVirtual = Point.Zero;



        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Тип разрешение дисплея
        /// Тип дисплея true - это с бальшим дислпеем например айпад планшет или лапатафон
        /// где рисуется намного больше информации
        /// </summary>
        public bool displayHD = false; 



        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Минимальное разрешение экрана, при котором небудет идти маштабирование
        /// </summary>
        public Point viewPortMin = Point.Zero;

      

      
        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Максимальное разрешение экрана, при котором небудет идти маштабирование
        /// </summary>
        public Point viewPortMax = Point.Zero;



        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// кооэфицент маштабировани коорекции системы ввода
        /// </summary>
        public Vector2 scaleTouch = new Vector2(1.0f);

    }
    /// -----------------------------------------------------------------------------------------









     ///=====================================================================================
    ///
    /// <summary>
    /// Контекст отрисовки 2D изображения
    /// также, загрузка ресурсов
    /// и система маштабирования
    /// </summary>
    /// 
    /// -----------------------------------------------------------------------------------------
    public class ASpriteBatch : SpriteBatch
    {
        ///--------------------------------------------------------------------------------------

        
        

        
        
        
        /// <summary>
        /// Размерность экрана
        /// </summary>
        static public Point viewPort = Point.Zero;



        /// <summary>
        /// Размерность спрайтов
        /// </summary>
        static public int size = AFrameworkSettings.spriteSize;




        /// <summary>
        /// коэффициент увелечения размерности спрайтов
        /// </summary>
        static public Vector2 scaleXY = new Vector2(1);




        /// <summary>
        /// коэффициент увелечения размерности спрайтов
        /// </summary>
        static public float scale = 1.0f;



        /// <summary>
        /// тип дисплея, маленький или большой
        /// </summary>
        static public bool displayHD = false; //большое размер дисплея (восновном ipad и лапатафоны)




        /// <summary>
        /// базовые примитивы
        /// </summary>
        public readonly ASpritePrimitives primitives = null;


        /// <summary>
        /// Время с последнего вызова кадра отрисовки
        /// </summary>
        public TimeSpan gameTime = TimeSpan.Zero;

        ///--------------------------------------------------------------------------------------



        /// <summary>
        /// Структура описывающее текущее состояние отрисовки
        /// </summary>
        public struct TState
        {
            public BlendState blend;
            public SamplerState sampler;
        }



        /// <summary>
        /// текущий режм смещения альфы в текстурах
        /// </summary>
        private TState m_state = new TState();
        




        /// <summary>
        /// менеджер автоподгружаемых текстур
        /// </summary>
        private readonly ASpriteContent mContent = null;

        
        private int         m_tickFrame = 0;        //номер фрейма для отрисовки


        //система обрезки
        //действует глобально
        private readonly RasterizerState    mClippingState = new RasterizerState() { ScissorTestEnable = true };
        private bool                        mClipping = false;
        
        
        
        //система маштабирования экрана дисплея
        //
        private Point               m_hardDisplaySize = Point.Zero; //актуальный размер экрана
        static private Vector2      m_displayScale = Vector2.Zero; //кооэфицент маштабирования
        private bool                m_isScalled = false; //флаг маштобирования эзображения
        private RenderTarget2D      m_backBuffer = null; //буфер для отрисовки во время маштабирования
        static private Vector2      m_spriteScale = new Vector2(1); //коофициент маштобирования спрайтов
        
        
        //квадрат для атласа
        private Rectangle           mRectAtlas = Rectangle.Empty;
        private int                 mAtlasWidth = 0;
        private int                 mAtlasHeight = 0;
        private int                 mAtlasMargin = 0;


        //система маштибирования коэффицента ввода
        //при приоброзвании экранный координатов
        private static Vector2      mScaleTocuh = new Vector2(1.0f);
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Контроль текущего графического контекста
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        private int mGDHandleSave = 0;
        static private int mGDHandle = 0;
        static public int GDHandle()
        {
            return mGDHandle;
        }
        static public bool isHandle(int handle)
        {
            return mGDHandle == handle ? true : false;
        }
        /// -------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конструктор загрузчика спрайтов
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public ASpriteBatch(GraphicsDevice graphicsDevice, ADisplayInfo info, ASpriteContent content)
            :
            base(graphicsDevice)
        {
            //------------------------------------------
            if (info.displayHardScreen.X < 1)
            {
                info.displayHardScreen.X = 1;
            }

            if (info.displayHardScreen.Y < 1)
            {
                info.displayHardScreen.Y = 1;
            }
            //------------------------------------------

            mScaleTocuh = info.scaleTouch;

            mContent = content;


            // XNA 4.0 включаем смешивание по альфаканалу и  запрещаем запись в буфер глубины 
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;



            primitives      = new ASpritePrimitives(this);


            mAtlasMargin = 0;
            mAtlasWidth = sprite.sizeAtlas + mAtlasMargin;
            mAtlasHeight = sprite.sizeAtlas + mAtlasMargin;
            mRectAtlas = new Rectangle(0, 0, sprite.sizeAtlas, sprite.sizeAtlas);

            mGDHandle++;
            mGDHandleSave = mGDHandle;

            viewPort = info.displayHardScreen;

            setDisplaySize(info.displayHardScreen, info.viewPortMin, info.viewPortMax);
            setScaleFactor(info.displayVirtual, info.displayHD);
        }
        ///--------------------------------------------------------------------------------------



        







         ///=====================================================================================
        ///
        /// <summary>
        /// установка размера экрана и маштабирование
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        protected void setDisplaySize(Point displaySize, Point viewPortMin, Point viewPortMax) 
        {
            m_hardDisplaySize = displaySize;


            //подсчитаем соотношение сторон физического дисплея
            //и выставим корреционные диапазон виртаульного дисплея
            double fAspectX = (double)displaySize.X / (double)displaySize.Y;
            double fAspectY = (double)displaySize.Y / (double)displaySize.X;

            Point vMin = new Point(viewPortMin.X, (int)((double)viewPortMin.X * fAspectY));
            if (vMin.Y < viewPortMin.Y)
            {
                vMin = new Point((int)((double)viewPortMin.Y * fAspectX), viewPortMin.Y);
            }

            Point vMax = new Point(viewPortMax.X, (int)((double)viewPortMax.X * fAspectY));
            if (vMax.Y > viewPortMax.Y)
            {
                vMax = new Point((int)((double)viewPortMax.Y * fAspectX), viewPortMax.Y);
            }

            if (vMax.X < vMin.X || vMax.Y < vMin.Y)
            {
                vMax = vMin;
            }



            int vx = m_hardDisplaySize.X;
            int vy = m_hardDisplaySize.Y;

            //X
            if (vx < vMin.X)
            {
                vx = vMin.X;
            }
            if (vx > vMax.X)
            {
                vx = vMax.X;
            }


            //Y
            if (vy < vMin.Y)
            {
                vy = vMin.Y;
            }
            if (vy > vMax.Y)
            {
                vy = vMax.Y;
            }






   

            viewPort = new Point(vx, vy);
            m_displayScale = new Vector2((float)m_hardDisplaySize.X / (float)vx, (float)m_hardDisplaySize.Y / (float)vy);


            m_isScalled = (m_displayScale == new Vector2(1)) ? false : true;

            //m_isScalled = true;

            m_spriteScale = new Vector2(1);
        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// установка базового размера для загруженных спрайтов
        /// высчитывем коофициент маштабирования спрайтов
        /// </summary>
        /// 
        /// <param name="HD">
        /// Тип дисплея true - это с бальшим дислпеем например айпад планшет или лапатафон
        /// где рисуется намного больше информации
        /// </param>
        /// 
        /// -------------------------------------------------------------------------------------
        protected void setScaleFactor(Point displayVirtual, bool HD)
        {
            m_spriteScale = new Vector2(1) / displayVirtual.toVector2();
            m_spriteScale = m_spriteScale * viewPort.toVector2();
            if (m_spriteScale.X > 1.0f)
            {
                m_spriteScale.X = 1.0f;
            }
            if (m_spriteScale.Y > 1.0f)
            {
                m_spriteScale.Y = 1.0f;
            }

            float sf = Math.Min(m_spriteScale.X, m_spriteScale.Y);
            m_spriteScale = new Vector2(sf);

            scaleXY = m_spriteScale;
            double factor = ((double)m_spriteScale.X + (double)m_spriteScale.Y) / 2.0f;
            scale = (float)factor;
            size = (int)((double)AFrameworkSettings.spriteSize * factor);
            displayHD = HD;
        }
        /// -------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим координаты относительно экрана
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        static public Vector2 fromViewPort(Vector2 pos)
        {
            if (pos == Vector2.Zero)
            {
                return pos;
            }
            Vector2 pt = pos / (m_displayScale * mScaleTocuh);
            return pt;
        }
        ///--------------------------------------------------------------------------------------







        
         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим таргет для отрисовки
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public RenderTarget2D renderTarget
        {
            get
            {
                return m_backBuffer;
            }
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// создание таргета
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public RenderTarget2D createRenderTarget()
        {
            PresentationParameters _pp = GraphicsDevice.PresentationParameters;

            return new RenderTarget2D(GraphicsDevice, viewPort.X, viewPort.Y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// начало отрисовки с учетом зумма карты
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void renderBegin(TimeSpan gt)
        {
            m_tickFrame++;
            gameTime = gt;


            if (m_isScalled)
            {
                if (m_backBuffer == null)
                {
                    m_backBuffer = createRenderTarget();
                }
                GraphicsDevice.SetRenderTarget(m_backBuffer);
                //GraphicsDevice.Clear(Color.Red);
            }
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// конец отрисовки двойного буфера, и показ буфера на экран с учетом зуммирования
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void renderEnd()
        {
            if (m_isScalled)
            {
                GraphicsDevice.SetRenderTarget(null);

                Begin(SpriteSortMode.BackToFront, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
                Draw(m_backBuffer, new Rectangle(0, 0, m_hardDisplaySize.X, m_hardDisplaySize.Y), Color.White);
                End();
  
            }
        }
        ///--------------------------------------------------------------------------------------





      



        ///=====================================================================================
        ///
        /// <summary>
        /// Возвращение текстуры спрайта
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public Texture2D getSprite(uint spriteID)
        {
            return mContent.getSprite(spriteID);
        }
        ///--------------------------------------------------------------------------------------

















         ///=====================================================================================
        ///
        /// <summary>
        /// отрисовка спрайта
        /// который возможно находтися в атласе
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos)
        {
            if (spriteID < 65535)
            {
                //обычная отрисовка
                Draw(mContent.getSprite(spriteID), pos, Color.White);
            }
            else
            {
                //отрисовка из атласа
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //атлас не загружен, поставим на загрузку
                    Draw(getSprite(index), pos, Color.White);
                }
                else
                {
                    //атлас загружен, найдем кординаты спрайта в текстуре атласа
                    uint x = (spriteID >> 8) & 255;
                    uint y = spriteID & 255;

                    mRectAtlas.Location = new Point((int)x * mAtlasWidth + mAtlasMargin, (int)y * mAtlasHeight + mAtlasMargin);
                    Draw(atlas, pos, mRectAtlas, Color.White);
                }
            }
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// отрисовка спрайта
        /// который возможно находтися в атласе
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos, Color color, float rotation, float scale)
        {
            if (spriteID < 65535)
            {
                //обычная отрисовка
                Draw(getSprite(spriteID), pos, color);
            }
            else
            {
                //отрисовка из атласа
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //атлас не загружен, поставим на загрузку
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //атлас загружен, найдем кординаты спрайта в текстуре атласа
                    uint x = (spriteID >> 8) & 255;
                    uint y = spriteID & 255;
                    mRectAtlas.Location = new Point((int)x * mAtlasWidth + mAtlasMargin, (int)y * mAtlasHeight + mAtlasMargin);
                    Draw(atlas, pos, mRectAtlas, color, rotation, new Vector2(sprite.sizeAtlas / 2), scale, SpriteEffects.None, 0.0f);
                }
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// отрисовка спрайта
        /// который возможно находтися в атласе
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos, Color color, float rotation, float scale, float layerDepth)
        {
            if (spriteID < 65535)
            {
                //обычная отрисовка
                Draw(getSprite(spriteID), pos, color);
            }
            else
            {
                //отрисовка из атласа
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //атлас не загружен, поставим на загрузку
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //атлас загружен, найдем кординаты спрайта в текстуре атласа
                    uint x = (spriteID >> 8) & 255;
                    uint y = spriteID & 255;
                    mRectAtlas.Location = new Point((int)x * mAtlasWidth + mAtlasMargin, (int)y * mAtlasHeight + mAtlasMargin);
                    Draw(atlas, pos, mRectAtlas, color, rotation, new Vector2(sprite.sizeAtlas / 2), scale, SpriteEffects.None, layerDepth);
                }
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// отрисовка спрайта
        /// который возможно находтися в атласе
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos, Color color, float rotation, Vector2 origin, float scale, float layerDepth)
        {
            if (spriteID < 65535)
            {
                //обычная отрисовка
                Draw(getSprite(spriteID), pos, null, color, rotation, origin, scale, SpriteEffects.None, layerDepth);
            }
            else
            {
                //отрисовка из атласа
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //атлас не загружен, поставим на загрузку
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //атлас загружен, найдем кординаты спрайта в текстуре атласа
                    uint x = (spriteID >> 8) & 255;
                    uint y = spriteID & 255;
                    mRectAtlas.Location = new Point((int)x * mAtlasWidth + mAtlasMargin, (int)y * mAtlasHeight + mAtlasMargin);
                    Draw(atlas, pos, mRectAtlas, color, rotation, origin, scale, SpriteEffects.None, layerDepth);
                }
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// отрисовка спрайта
        /// который возможно находтися в атласе
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos, Color color, float rotation, Vector2 origin, Vector2 scale, float layerDepth)
        {
            if (spriteID < 65535)
            {
                //обычная отрисовка
                Draw(getSprite(spriteID), pos, color);
            }
            else
            {
                //отрисовка из атласа
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //атлас не загружен, поставим на загрузку
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //атлас загружен, найдем кординаты спрайта в текстуре атласа
                    uint x = (spriteID >> 8) & 255;
                    uint y = spriteID & 255;
                    mRectAtlas.Location = new Point((int)x * mAtlasWidth + mAtlasMargin, (int)y * mAtlasHeight + mAtlasMargin);
                    Draw(atlas, pos, mRectAtlas, color, rotation, origin, scale, SpriteEffects.None, layerDepth);
                }
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// отрисовка спрайта
        /// который возможно находтися в атласе
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Rectangle pos, Color color, float rotation, Vector2 origin, float layerDepth)
        {
            if (spriteID < 65535)
            {
                //обычная отрисовка
                Draw(getSprite(spriteID), pos, color);
            }
            else
            {
                //отрисовка из атласа
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //атлас не загружен, поставим на загрузку
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //атлас загружен, найдем кординаты спрайта в текстуре атласа
                    uint x = (spriteID >> 8) & 255;
                    uint y = spriteID & 255;
                    mRectAtlas.Location = new Point((int)x * mAtlasWidth + mAtlasMargin, (int)y * mAtlasHeight + mAtlasMargin);
                    Draw(atlas, pos, mRectAtlas, color, rotation, origin, SpriteEffects.None, layerDepth);
                }
            }
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// возврат номер фрейма
        /// нужно для учета пропуска отрисованных кадров
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public int tickFrame
        {
            get
            {
                return m_tickFrame;
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// проверка на актуальность фрейма
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public bool isActualTickFrame(int tickFrame)
        {
            return (m_tickFrame - 1) == tickFrame ? true : false;
        }
        ///--------------------------------------------------------------------------------------




     
        
         ///=====================================================================================
        ///
        /// <summary>
        /// Параметры отрисовки
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public TState state
        {
            get
            {
                return m_state;
            }
        }
        ///--------------------------------------------------------------------------------------
        





         ///=====================================================================================
        ///
        /// <summary>
        /// Начала отрисовки по параметром
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        protected void beginState()
        {
            //оригинальная
            //Begin(SpriteSortMode.Texture, m_state.blend, m_state.sampler, DepthStencilState.None, mClipping ? mClippingState : RasterizerState.CullNone);




            //Begin(SpriteSortMode.Deferred, m_state.blend, m_state.sampler, DepthStencilState.None, mClipping ? mClippingState : RasterizerState.CullNone);
            //11.01.2015//Begin(SpriteSortMode.Deferred, m_state.blend, SamplerState.LinearClamp, DepthStencilState.None, mClipping ? mClippingState : RasterizerState.CullNone);

            Begin(SpriteSortMode.BackToFront, m_state.blend, SamplerState.LinearClamp, DepthStencilState.None, mClipping ? mClippingState : RasterizerState.CullNone);
            
            
            
            //Begin(SpriteSortMode.Texture, m_state.blend, m_state.sampler, DepthStencilState.None, RasterizerState.CullNone);
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Начало отрисовки серии спрайтов
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void begin(TState state)
        {
            m_state = state;
            beginState();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Начало отрисовки серии спрайтов
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void begin()
        {
            //m_state.blend = BlendState.NonPremultiplied;
            //m_state.sampler = SamplerState.AnisotropicClamp;
            m_state.blend = BlendState.AlphaBlend;
            m_state.sampler = SamplerState.AnisotropicClamp;
            beginState();
        }
        ///--------------------------------------------------------------------------------------






 





         ///=====================================================================================
        ///
        /// <summary>
        /// Начало отрисовки серии спрайтов без альфаканала
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void beginOpaque()
        {
            m_state.blend = BlendState.Opaque;
            m_state.sampler = SamplerState.AnisotropicClamp;
            beginState();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Начало отрисовки c альфоканалом
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void beginAdditive()
        {
            m_state.blend = BlendState.Additive;
            m_state.sampler = SamplerState.AnisotropicClamp;
            beginState();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Начало отрисовки c регионом отчечения
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void beginClipping(Rectangle region)
        {
            
            int iX = region.X;
            int iY = region.Y;
            int iWidth = region.Width;
            int iHeight = region.Height;

            if (iX < 0)
            {
                iWidth += iX;
                iX = 0;
            }

            if (iY < 0)
            {
                iHeight += iY;
                iY = 0;
            }
            
  
            end();
            mClipping = true;
            GraphicsDevice.ScissorRectangle = new Rectangle(iX, iY, iWidth, iHeight);
            beginState();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конец отрисовки линии отсечения
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void endClipping()
        {
            end();
            mClipping = false;
            beginState();
        }
        ///--------------------------------------------------------------------------------------





    


        ///=====================================================================================
        ///
        /// <summary>
        /// Конец отрисовки серии спрайтов
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void end()
        {
            End();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Конец отрисовки серии спрайтов
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public TState endToState()
        {
            End();
            return m_state;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Сбросить на отрисовку уже готовую серию спрайтов
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void flush()
        {
            End();
            beginState();
        }



    }
    ///------------------------------------------------------------------------------------------
    ///
}