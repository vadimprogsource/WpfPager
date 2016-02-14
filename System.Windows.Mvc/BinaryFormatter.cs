using System;
using System.Reflection;

using System.IO;

using System.Collections;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Mvc.Binary
{
	internal static class BinaryFormatter
	{


		private static void WritePropertyCore(this BinaryWriter writer , Type propertyType, object value)
		{

			switch (Type.GetTypeCode(propertyType))
			{
				case TypeCode.Boolean : writer.Write((bool)value); break;
				case TypeCode.Byte    : writer.Write((byte)value);break;
 				case TypeCode.Char    : writer.Write((char)value);break;
				case TypeCode.DateTime: writer.Write(((DateTime)value).Ticks); break;
				case TypeCode.Decimal : writer.Write((decimal)value); break;
				case TypeCode.Double  : writer.Write((double)value); break;
				case TypeCode.Int16   : writer.Write((short)value); break;
				case TypeCode.Int32   : writer.Write((int)value); break;
				case TypeCode.Int64   : writer.Write((long )value); break;
				case TypeCode.UInt16  : writer.Write((ushort)value); break;
				case TypeCode.UInt32  : writer.Write((uint)value); break;
				case TypeCode.UInt64  : writer.Write((ulong)value); break; 
				case TypeCode.SByte   : writer.Write((sbyte)value);break;
				case TypeCode.Single  : writer.Write((Single)value);break;
				case TypeCode.String  : writer.Write((value ?? string.Empty).ToString()); break;
				case TypeCode.DBNull  :
				case TypeCode.Empty   : break;
				case TypeCode.Object  :

					                     if (propertyType==typeof(Guid))
										 {
											  writer.Write(((Guid)value).ToByteArray());   
  											 break;
										 }

										 if (propertyType == typeof(Uri))
										 {
											 if (value == null)
											 {
												 writer.Write(string.Empty);
												 break;
											 }
											
											 writer.Write(((Uri)value).AbsoluteUri);
											 break;
										 }


										 if (propertyType == typeof(byte[]))
										 {
											 byte[] x = (byte[])value;

											 if (x == null)
											 {
												 writer.Write(-1);
												 break;
  
											 }

											 writer.Write(x.Length);
											 writer.Write(x);
											 break;
										 }


										 if (value == null)
										 {
											 writer.Write(byte.MinValue);
											 break;
										 }

										 writer.Write(byte.MaxValue);

										 if (propertyType.IsArray)
										 {
											 writer.WriteArrayCore(propertyType.GetElementType(), (Array)value);
											 break;
										 }


										 if (typeof(IObject).IsAssignableFrom(propertyType))
										 {
											 writer.Write((value as IObject).GetIdentity().ToByteArray());
											 break;
										 }


										 writer.WriteObjectCore(propertyType, value);  
										 break;

				default: throw new NotSupportedException();

		
			}
		}


		private static object ReadPropertyCore(this BinaryReader reader, Type propertyType)
		{
			switch (Type.GetTypeCode(propertyType))
			{
				case TypeCode.Boolean : return reader.ReadBoolean(); 
				case TypeCode.Byte    : return reader.ReadByte(); 
 				case TypeCode.Char    : return reader.ReadChar();
				case TypeCode.DateTime: return new DateTime(reader.ReadInt64()); 
				case TypeCode.Decimal : return reader.ReadDecimal();  
				case TypeCode.Double  : return reader.ReadDouble ();
				case TypeCode.Int16   : return reader.ReadInt16  ();
				case TypeCode.Int32   : return reader.ReadInt32  ();
				case TypeCode.Int64   : return reader.ReadInt64  (); 
				case TypeCode.UInt16  : return reader.ReadUInt16 ();
				case TypeCode.UInt32  : return reader.ReadUInt32 ();
				case TypeCode.UInt64  : return reader.ReadUInt64 (); 
				case TypeCode.SByte   : return reader.ReadSByte  (); 
				case TypeCode.Single  : return reader.ReadSingle (); 
				case TypeCode.String  : return reader.ReadString ();  
				case TypeCode.DBNull  :
				case TypeCode.Empty   : break;
				case TypeCode.Object  :
					                    if (propertyType == typeof(Guid)) return new Guid(reader.ReadBytes(16));

										if (propertyType == typeof(Uri))
										{
											string uriString = reader.ReadString();
											return uriString == null || 1 > uriString.Length ? null : new Uri(uriString);
										}



										if (propertyType == typeof(byte[]))
										{
											int i = reader.ReadInt32();
											if (i < 0) return null;
											return i == 0 ? new byte[0] : reader.ReadBytes(i); 
										}


										if (reader.ReadByte() == byte.MinValue)
										{
											return null;
										}

										if (propertyType.IsArray)
										{
											return reader.ReadArrayCore(propertyType.GetElementType());  
										}

										if (typeof(IObject).IsAssignableFrom(propertyType)) using(IStorage storage = Locator.Get<IStorage>()) 
										{
											return storage.Get<object>(new Guid(reader.ReadBytes(16)));
 										}
					 


										return reader.ReadObjectCore(propertyType);

									
			}

			throw new NotSupportedException();

		}


		private static void WriteArrayCore(this BinaryWriter writer,Type elementType ,  Array array)
		{
			writer.Write(array.Length);
			for(int i = 0 ; i < array.Length ;i++) writer.WritePropertyCore( elementType , array.GetValue(i)); 
   		}


		private static void WriteObjectCore(this BinaryWriter writer, Type objType, object obj)
		{
			foreach (PropertyInfo property in objType.GetProperties().Where(x=>x.CanRead && x.CanWrite).OrderBy(x => x.Name))
			{
				writer.WritePropertyCore(property.PropertyType, property.GetValue(obj, null));  
			}
		}


		private static Array ReadArrayCore(this BinaryReader reader , Type elementType)
		{
			Array array = Array.CreateInstance( elementType , reader.ReadInt32());

			for (int i = 0; i < array.Length; i++) array.SetValue(reader.ReadPropertyCore(elementType), i);
			return array;
		}


		private static object ReadObjectCore(this BinaryReader reader,Type objType)
		{

			object obj = Activator.CreateInstance(objType);

			foreach (PropertyInfo property in objType.GetProperties().Where(x => x.CanRead && x.CanWrite).OrderBy(x => x.Name))
			{
				property.SetValue(obj, reader.ReadPropertyCore(property.PropertyType), null);
			}

			return obj;
		}


		public static byte[] Serialize<T>(this T @this) where T : class
		{

			using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
			{


				if (typeof(T).IsArray)
				{
					writer.WriteArrayCore(typeof(T).GetElementType(), @this as Array);
				}
				else
				{
					writer.WriteObjectCore(typeof(T), @this);
				}


				writer.Flush();
				return (writer.BaseStream as MemoryStream).ToArray();
			}

		}


		public static T Deserialize<T>(this byte[] @this) where T : class
		{
			using (BinaryReader reader = new BinaryReader(new MemoryStream(@this)))
			{

				if (typeof(T).IsArray)
				{
					return reader.ReadArrayCore(typeof(T).GetElementType()) as T;
				}


				return reader.ReadObjectCore(typeof(T)) as T;  
			}
		}

	}
}
