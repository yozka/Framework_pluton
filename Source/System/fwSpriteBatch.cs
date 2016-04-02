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
    /// ���������� � ���������� ������
    /// </summary>
    /// 
    /// -----------------------------------------------------------------------------------------
    public class ADisplayInfo
    {
        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// ���������� ������ ������ � ��������
        /// </summary>
        public Point displayHardScreen = Point.Zero;




        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// ������ ������������ ������� ��� ������� ���������� ���������������
        /// </summary>
        public Point displayVirtual = Point.Zero;



        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// ��� ���������� �������
        /// ��� ������� true - ��� � ������� �������� �������� ����� ������� ��� ���������
        /// ��� �������� ������� ������ ����������
        /// </summary>
        public bool displayHD = false; 



        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// ����������� ���������� ������, ��� ������� ������� ���� ��������������
        /// </summary>
        public Point viewPortMin = Point.Zero;

      

      
        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// ������������ ���������� ������, ��� ������� ������� ���� ��������������
        /// </summary>
        public Point viewPortMax = Point.Zero;



        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// ���������� ������������� ��������� ������� �����
        /// </summary>
        public Vector2 scaleTouch = new Vector2(1.0f);

    }
    /// -----------------------------------------------------------------------------------------









     ///=====================================================================================
    ///
    /// <summary>
    /// �������� ��������� 2D �����������
    /// �����, �������� ��������
    /// � ������� ��������������
    /// </summary>
    /// 
    /// -----------------------------------------------------------------------------------------
    public class ASpriteBatch : SpriteBatch
    {
        ///--------------------------------------------------------------------------------------

        
        

        
        
        
        /// <summary>
        /// ����������� ������
        /// </summary>
        static public Point viewPort = Point.Zero;



        /// <summary>
        /// ����������� ��������
        /// </summary>
        static public int size = AFrameworkSettings.spriteSize;




        /// <summary>
        /// ����������� ���������� ����������� ��������
        /// </summary>
        static public Vector2 scaleXY = new Vector2(1);




        /// <summary>
        /// ����������� ���������� ����������� ��������
        /// </summary>
        static public float scale = 1.0f;



        /// <summary>
        /// ��� �������, ��������� ��� �������
        /// </summary>
        static public bool displayHD = false; //������� ������ ������� (��������� ipad � ����������)




        /// <summary>
        /// ������� ���������
        /// </summary>
        public readonly ASpritePrimitives primitives = null;


        /// <summary>
        /// ����� � ���������� ������ ����� ���������
        /// </summary>
        public TimeSpan gameTime = TimeSpan.Zero;

        ///--------------------------------------------------------------------------------------



        /// <summary>
        /// ��������� ����������� ������� ��������� ���������
        /// </summary>
        public struct TState
        {
            public BlendState blend;
            public SamplerState sampler;
        }



        /// <summary>
        /// ������� ���� �������� ����� � ���������
        /// </summary>
        private TState m_state = new TState();
        




        /// <summary>
        /// �������� ���������������� �������
        /// </summary>
        private readonly ASpriteContent mContent = null;

        
        private int         m_tickFrame = 0;        //����� ������ ��� ���������


        //������� �������
        //��������� ���������
        private readonly RasterizerState    mClippingState = new RasterizerState() { ScissorTestEnable = true };
        private bool                        mClipping = false;
        
        
        
        //������� �������������� ������ �������
        //
        private Point               m_hardDisplaySize = Point.Zero; //���������� ������ ������
        static private Vector2      m_displayScale = Vector2.Zero; //���������� ��������������
        private bool                m_isScalled = false; //���� �������������� �����������
        private RenderTarget2D      m_backBuffer = null; //����� ��� ��������� �� ����� ��������������
        static private Vector2      m_spriteScale = new Vector2(1); //���������� �������������� ��������
        
        
        //������� ��� ������
        private Rectangle           mRectAtlas = Rectangle.Empty;
        private int                 mAtlasWidth = 0;
        private int                 mAtlasHeight = 0;
        private int                 mAtlasMargin = 0;


        //������� �������������� ����������� �����
        //��� ������������� �������� �����������
        private static Vector2      mScaleTocuh = new Vector2(1.0f);
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// �������� �������� ������������ ���������
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
        /// ����������� ���������� ��������
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


            // XNA 4.0 �������� ���������� �� ����������� �  ��������� ������ � ����� ������� 
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
        /// ��������� ������� ������ � ��������������
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        protected void setDisplaySize(Point displaySize, Point viewPortMin, Point viewPortMax) 
        {
            m_hardDisplaySize = displaySize;


            //���������� ����������� ������ ����������� �������
            //� �������� ������������ �������� ������������ �������
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
        /// ��������� �������� ������� ��� ����������� ��������
        /// ���������� ���������� �������������� ��������
        /// </summary>
        /// 
        /// <param name="HD">
        /// ��� ������� true - ��� � ������� �������� �������� ����� ������� ��� ���������
        /// ��� �������� ������� ������ ����������
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
        /// ��������� ���������� ������������ ������
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
        /// ��������� ������ ��� ���������
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
        /// �������� �������
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
        /// ������ ��������� � ������ ����� �����
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
        /// ����� ��������� �������� ������, � ����� ������ �� ����� � ������ ������������
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
        /// ����������� �������� �������
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
        /// ��������� �������
        /// ������� �������� ��������� � ������
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos)
        {
            if (spriteID < 65535)
            {
                //������� ���������
                Draw(mContent.getSprite(spriteID), pos, Color.White);
            }
            else
            {
                //��������� �� ������
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //����� �� ��������, �������� �� ��������
                    Draw(getSprite(index), pos, Color.White);
                }
                else
                {
                    //����� ��������, ������ ��������� ������� � �������� ������
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
        /// ��������� �������
        /// ������� �������� ��������� � ������
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos, Color color, float rotation, float scale)
        {
            if (spriteID < 65535)
            {
                //������� ���������
                Draw(getSprite(spriteID), pos, color);
            }
            else
            {
                //��������� �� ������
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //����� �� ��������, �������� �� ��������
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //����� ��������, ������ ��������� ������� � �������� ������
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
        /// ��������� �������
        /// ������� �������� ��������� � ������
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos, Color color, float rotation, float scale, float layerDepth)
        {
            if (spriteID < 65535)
            {
                //������� ���������
                Draw(getSprite(spriteID), pos, color);
            }
            else
            {
                //��������� �� ������
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //����� �� ��������, �������� �� ��������
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //����� ��������, ������ ��������� ������� � �������� ������
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
        /// ��������� �������
        /// ������� �������� ��������� � ������
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos, Color color, float rotation, Vector2 origin, float scale, float layerDepth)
        {
            if (spriteID < 65535)
            {
                //������� ���������
                Draw(getSprite(spriteID), pos, null, color, rotation, origin, scale, SpriteEffects.None, layerDepth);
            }
            else
            {
                //��������� �� ������
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //����� �� ��������, �������� �� ��������
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //����� ��������, ������ ��������� ������� � �������� ������
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
        /// ��������� �������
        /// ������� �������� ��������� � ������
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Vector2 pos, Color color, float rotation, Vector2 origin, Vector2 scale, float layerDepth)
        {
            if (spriteID < 65535)
            {
                //������� ���������
                Draw(getSprite(spriteID), pos, color);
            }
            else
            {
                //��������� �� ������
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //����� �� ��������, �������� �� ��������
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //����� ��������, ������ ��������� ������� � �������� ������
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
        /// ��������� �������
        /// ������� �������� ��������� � ������
        /// </summary>
        /// 
        ///-------------------------------------------------------------------------------------
        public void drawSprite(uint spriteID, Rectangle pos, Color color, float rotation, Vector2 origin, float layerDepth)
        {
            if (spriteID < 65535)
            {
                //������� ���������
                Draw(getSprite(spriteID), pos, color);
            }
            else
            {
                //��������� �� ������
                uint index = spriteID >> 16;
                Texture2D atlas = mContent[index];
                if (atlas == null)
                {
                    //����� �� ��������, �������� �� ��������
                    Draw(getSprite(index), pos, color);
                }
                else
                {
                    //����� ��������, ������ ��������� ������� � �������� ������
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
        /// ������� ����� ������
        /// ����� ��� ����� �������� ������������ ������
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
        /// �������� �� ������������ ������
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
        /// ��������� ���������
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
        /// ������ ��������� �� ����������
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        protected void beginState()
        {
            //������������
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
        /// ������ ��������� ����� ��������
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
        /// ������ ��������� ����� ��������
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
        /// ������ ��������� ����� �������� ��� �����������
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
        /// ������ ��������� c ������������
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
        /// ������ ��������� c �������� ���������
        /// </summary>
        /// 
        /// -------------------------------------------------------------------------------------
        public void beginClipping(Rectangle region)
        {
            end();
            mClipping = true;
            GraphicsDevice.ScissorRectangle = region;
            beginState();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ����� ��������� ����� ���������
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
        /// ����� ��������� ����� ��������
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
        /// ����� ��������� ����� ��������
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
        /// �������� �� ��������� ��� ������� ����� ��������
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