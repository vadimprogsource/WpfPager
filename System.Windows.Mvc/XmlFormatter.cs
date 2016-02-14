using System;
using System.Reflection;

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using System.Text;

namespace System.Windows.Mvc.Xml.Linq
{
	public static class XmlFormatter
	{


		internal static T DeserializeCore<T>(this T @this, XElement element) 
		{

			foreach (PropertyInfo property in typeof(T).GetProperties())
			{
				XAttribute x = element.Attribute(property.Name);
				if (x == null) continue;


				try
				{
					property.SetValue(@this, Convert.ChangeType(x.Value, property.PropertyType), null);
				}
				catch { }

			}


			return @this;
		}


		public static XElement ToXml(this string @this)
		{
			return XDocument.Load(@this).Root;   
		}


		public static XElement Run(this XElement @this , Action<XElement> action)
		{
			action(@this);
			return @this ;
		}

		

		public static IEnumerable<T> Deserialize<T>(this IEnumerable<XElement> @this) where  T :  new()
		{
			foreach (XElement x in @this) yield return new T().DeserializeCore(x);
		}


		public static T Deserialize<T>(this XElement @this) where T :  new()
		{
			return new T().DeserializeCore(@this);
		}





		public static IEnumerable<T> Deserialize<T>(this IEnumerable<XElement> @this, Action<XElement, T> onYield) where T :  new()
		{
			foreach (XElement x in @this)
			{
				T value = new T().DeserializeCore(x);
				onYield(x, value);
				yield return value;
			}
		}


		public static T DeserializeOne<T>(this XElement @this) where T :  new()
		{
			return @this.Elements(typeof(T).Name).First().Deserialize<T>() ;
		}

		public static IEnumerable<T> DeserializeMany<T>(this XElement @this, Action<XElement, T> onYield) where T :  new()
		{
			return @this.Elements(typeof(T).Name).Deserialize(onYield);
		}


		public static IEnumerable<T> DeserializeMany<T>(this XElement @this) where T :  new()
		{
			return @this.Elements(typeof(T).Name).Deserialize<T>();
		}

	


	}
}
