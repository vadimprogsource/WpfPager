using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Mvc
{
	public static  class Locator
	{
		public static T Get<T>() where T : class
		{
			return default(T);
		}


		public static T Get<T>(Uri uri) where T : class
		{
			return default(T);
		}
	}
}
