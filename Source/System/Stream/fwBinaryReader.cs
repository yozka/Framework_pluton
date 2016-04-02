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
    /// ������� ������ �� ��������� ������
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
        /// ������ ������
        /// ���� �� ����� �� ������ �����, �� �������� ����, ����� �����
        /// ������ ������, ��� ������ ����������
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
        /// ������ ����
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
            //���������� ��� ������������� ������
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
        /// ������ � ����� 
        /// ������ �����
        /// int count - ���������� ���������
        /// 
        /// string key - �����
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
        /// ���������� � ����� 
        /// ������ �������
        /// int count - ���������� ���������
        /// 
        /// string key - �����
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
        /// ���������� � ����� 
        /// ������ �������
        /// int count - ���������� ���������
        /// 
        /// string key - �����
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
        /// ���������� � ����� 
        /// ������ ������������� ������
        /// int count - ���������� ���������
        /// 
        /// string key - �����
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
        /// ���������� � ����� 
        /// ������ ������������� ���������� ������
        /// int count - ���������� ���������
        /// 
        /// string key - �����
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
        /// ������ �� ������
        /// ������ ��������� ������
        /// int count - ���������� ���������
        /// 
        /// string key - �����
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
        /// ������ �� ������
        /// ������ Vector2 ������
        /// int count - ���������� ���������
        /// 
        /// string key - �����
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
        /// ���������� � ����� 
        /// ������ ������������� ���������� ������
        /// int count - ���������� ���������
        /// 
        /// string key - �����
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
        /// ���������� � ����� 
        /// ������ �������� ������
        /// int count - ���������� ���������
        /// 
        /// string key - �����
        /// int32 size - ������ ������
        /// byte value - ���� ������
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
