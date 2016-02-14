using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;


namespace System.Windows.Mvc
{
	[Serializable]
	public class Response<T> : Request<T> where T : class , new()
	{
		public Response(T content): base(content)
		{
		}


		public Response(SerializationInfo info, StreamingContext context): base(info, context)
		{
		}

	}
}
