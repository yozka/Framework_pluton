#region Using framework
using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion



namespace Pluton.SystemProgram
{
    ///--------------------------------------------------------------------------------------







     ///=====================================================================================
    ///
    /// <summary>
    /// Система чтения из бинарного потока
    /// 
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ABinaryReader<T>
                                    where T : IStream, new()
                                    
    {
        ///--------------------------------------------------------------------------------------






 
         ///=====================================================================================
        ///
        /// <summary>
        /// Чтение данных
        /// Если на входе не пустой поток, то создадим свой, новый поток
        /// первые данные, это версия интерфейса
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public IStream read(Stream storage)
        {
            BinaryReader bin = new BinaryReader(storage);
            IStream stream = new T();
            int version = bin.ReadInt32();
            if (version == stream.getVersion())
            {
                readStream(bin, stream);
            }
            return stream;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// чтение узла
        /// string - typeName
        /// 
        /// points
        /// floats
        /// strings
        /// integers
        /// uintegers
        /// bools
        /// vectors2
        /// long
        /// byte
        /// 
        /// int count childs
        /// 
        /// ...
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readStream(BinaryReader bin, IStream stream)
        {
            //определяем тип использования данных
            ATypeContent content = new ATypeContent(bin.ReadUInt16());
            stream.setTypeName(bin.ReadString());

            if (content.isPoint())      readPoint   (bin, stream);
            if (content.isFloat())      readFloat   (bin, stream);
            if (content.isString())     readString  (bin, stream);
            if (content.isInteger())    readInteger (bin, stream);
            if (content.isUInteger())   readUInteger(bin, stream);
            if (content.isBoolean())    readBoolean (bin, stream);
            if (content.isVector2())    readVector2 (bin, stream);
            if (content.isLong())       readLong    (bin, stream);
            if (content.isBinary())     readBinary  (bin, stream);
            if (content.isChilds())
            {
                int count = bin.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    readStream(bin, stream.creationChild());
                }
            }
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// чтение в поток 
        /// массив точек
        /// int count - количество элементов
        /// 
        /// string key - ключь
        /// int X,
        /// int Y
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readPoint(BinaryReader bin, IStream stream)
        {
            int count = bin.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                stream.writePoint(bin.ReadString(), new Point(bin.ReadInt32(), bin.ReadInt32()));
            }
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// сохранение в поток 
        /// массив флоатов
        /// int count - количество элементов
        /// 
        /// string key - ключь
        /// float value
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readFloat(BinaryReader bin, IStream stream)
        {
            int count = bin.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                stream.writeFloat(bin.ReadString(), (float)bin.ReadDouble());
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// сохранение в поток 
        /// массив строчек
        /// int count - количество элементов
        /// 
        /// string key - ключь
        /// string value
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readString(BinaryReader bin, IStream stream)
        {
            int count = bin.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                stream.writeString(bin.ReadString(), bin.ReadString());
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// сохранение в поток 
        /// массив целочисленных данных
        /// int count - количество элементов
        /// 
        /// string key - ключь
        /// int value
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readInteger(BinaryReader bin, IStream stream)
        {
            int count = bin.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                stream.writeInteger(bin.ReadString(), bin.ReadInt32());
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// сохранение в поток 
        /// массив целочисленных безнаковых данных
        /// int count - количество элементов
        /// 
        /// string key - ключь
        /// int value
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readUInteger(BinaryReader bin, IStream stream)
        {
            int count = bin.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                stream.writeUInteger(bin.ReadString(), bin.ReadUInt32());
            }
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// чтение из потока
        /// массив буллевыых данных
        /// int count - количество элементов
        /// 
        /// string key - ключь
        /// bool value
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readBoolean(BinaryReader bin, IStream stream)
        {
            int count = bin.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                stream.writeBoolean(bin.ReadString(), bin.ReadBoolean());
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// чтение из потока
        /// массив Vector2 данных
        /// int count - количество элементов
        /// 
        /// string key - ключь
        /// float x
        /// float y
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readVector2(BinaryReader bin, IStream stream)
        {
            int count = bin.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                stream.writeVector2(bin.ReadString(), new Vector2((float)bin.ReadDouble(), (float)bin.ReadDouble()));
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// сохранение в поток 
        /// массив целочисленных безнаковых данных
        /// int count - количество элементов
        /// 
        /// string key - ключь
        /// long value
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readLong(BinaryReader bin, IStream stream)
        {
            int count = bin.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                stream.writeLong(bin.ReadString(), bin.ReadInt64());
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// сохранение в поток 
        /// массив бинарных данных
        /// int count - количество элементов
        /// 
        /// string key - ключь
        /// int32 size - размер буфера
        /// byte value - сами данные
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void readBinary(BinaryReader bin, IStream stream)
        {
            int count = bin.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string key = bin.ReadString();
                int size = bin.ReadInt32();
                byte[] buf = new byte[size];
                for (int j = 0; j < size; j++)
                {
                    buf[j] = bin.ReadByte();
                }
                stream.writeBinary(key, buf);
            }
        }
        ///--------------------------------------------------------------------------------------




    }
}
