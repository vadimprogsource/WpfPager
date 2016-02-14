using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Windows.Mvc.Binary;


namespace System.Windows.Mvc
{
	[Serializable]
	public class Request<T> : ISerializable where T : class, new() 
	{

		public readonly T Content;

		public Request(T content)
		{
			Content = content;
		}

		public Request(SerializationInfo info, StreamingContext context)
		{
			Content = (info.GetValue("_", typeof(byte[])) as byte[]).Deserialize<T>();  
		}


		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_", Content.Serialize()); 
		}
	}
}
