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
    /// ������� ������������ ������ � �������� ������
    /// 
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ABinaryWriter
    {
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� ������
        /// ���� �� ����� �� ������ �����, �� �������� ����, ����� �����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void write(Stream storage , IStream stream)
        {
            BinaryWriter bin = new BinaryWriter(storage);
            bin.Write((Int32)stream.getVersion());
            writeStream(bin, stream);
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� ����
        /// string - typeName
        /// 
        /// points
        /// floats
        /// strings
        /// integers
        /// bools
        /// Vector2
        /// long
        /// 
        /// int count childs
        /// 
        /// ...
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void writeStream(BinaryWriter bin, IStream stream)
        {
            var childs = stream.getChilds();

            ATypeContent content = new ATypeContent();
            content.setPoint    (stream.keysPoint().Count > 0);
            content.setFloat    (stream.keysFloat().Count > 0);
            content.setString   (stream.keysString().Count > 0);
            content.setInteger  (stream.keysInteger().Count > 0);
            content.setUInteger (stream.keysUInteger().Count > 0);
            content.setBoolean  (stream.keysBoolean().Count > 0);
            content.setVector2  (stream.keysVector2().Count > 0);
            content.setLong     (stream.keysLong().Count > 0);
            content.setBinary   (stream.keysBinary().Count > 0);
            content.setChilds   (childs.Length > 0);

            bin.Write(content.typeContent);
            bin.Write(stream.getTypeName());

            if (content.isPoint())      writePoint      (bin, stream);
            if (content.isFloat())      writeFloat      (bin, stream);
            if (content.isString())     writeString     (bin, stream);
            if (content.isInteger())    writeInteger    (bin, stream);
            if (content.isUInteger())   writeUInteger   (bin, stream);
            if (content.isBoolean())    writeBoolean    (bin, stream);
            if (content.isVector2())    writeVector2    (bin, stream);
            if (content.isLong())       writeLong       (bin, stream);
            if (content.isBinary())     writeBinary     (bin, stream);

            if (content.isChilds())
            {
                bin.Write((Int32)childs.Length);
                foreach (var child in childs)
                {
                    writeStream(bin, child);
                }
            }
        }
        ///--------------------------------------------------------------------------------------










         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� � ����� 
        /// ������ �����
        /// int count - ���������� ���������
        /// 
        /// string key - �����
        /// int X,
        /// int Y
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void writePoint(BinaryWriter bin, IStream stream)
        {
            var keys = stream.keysPoint();
            bin.Write((Int32)keys.Count);
            foreach (string key in keys)
            {
                Point value = stream.readPoint(key, Point.Zero);
                bin.Write(key);
                bin.Write((Int32)value.X);
                bin.Write((Int32)value.Y);
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
        protected void writeFloat(BinaryWriter bin, IStream stream)
        {
            var keys = stream.keysFloat();
            bin.Write((Int32)keys.Count);
            foreach (string key in keys)
            {
                bin.Write(key);
                bin.Write((double)stream.readFloat(key, 0));
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
        protected void writeString(BinaryWriter bin, IStream stream)
        {
            var keys = stream.keysString();
            bin.Write((Int32)keys.Count);
            foreach (string key in keys)
            {
                bin.Write(key);
                bin.Write(stream.readString(key, string.Empty));
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
        protected void writeInteger(BinaryWriter bin, IStream stream)
        {
            var keys = stream.keysInteger();
            bin.Write((Int32)keys.Count);
            foreach (string key in keys)
            {
                bin.Write(key);
                bin.Write((Int32)stream.readInteger(key, 0));
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
        protected void writeUInteger(BinaryWriter bin, IStream stream)
        {
            var keys = stream.keysUInteger();
            bin.Write((Int32)keys.Count);
            foreach (string key in keys)
            {
                bin.Write(key);
                bin.Write((UInt32)stream.readUInteger(key, 0));
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
        /// bool value
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void writeBoolean(BinaryWriter bin, IStream stream)
        {
            var keys = stream.keysBoolean();
            bin.Write((Int32)keys.Count);
            foreach (string key in keys)
            {
                bin.Write(key);
                bin.Write(stream.readBoolean(key, false));
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� � ����� 
        /// ������ Vector2
        /// int count - ���������� ���������
        /// 
        /// string key - �����
        /// float X,
        /// float Y
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void writeVector2(BinaryWriter bin, IStream stream)
        {
            var keys = stream.keysVector2();
            bin.Write((Int32)keys.Count);
            foreach (string key in keys)
            {
                Vector2 value = stream.readVector2(key, Vector2.Zero);
                bin.Write(key);
                bin.Write((double)value.X);
                bin.Write((double)value.Y);
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
        protected void writeLong(BinaryWriter bin, IStream stream)
        {
            var keys = stream.keysLong();
            bin.Write((Int32)keys.Count);
            foreach (string key in keys)
            {
                bin.Write(key);
                bin.Write((Int64)stream.readLong(key, 0));
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
        /// int32 size - ������ �������� ������
        /// byte value - ���� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void writeBinary(BinaryWriter bin, IStream stream)
        {
            var keys = stream.keysBinary();
            bin.Write((Int32)keys.Count);
            foreach (string key in keys)
            {
                bin.Write(key);

                var buf = stream.readBinary(key, null);
                int size = buf == null ? 0 : buf.Length;
                bin.Write((Int32)size);
                for (int j = 0; j < size; j++)
                {
                    bin.Write((byte)buf[j]);
                }
            }
        }
        ///--------------------------------------------------------------------------------------


    }
}
