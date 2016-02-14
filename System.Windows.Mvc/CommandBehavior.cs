using System;
using System.Windows.Input;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Mvc
{
	public static class CommandBehavior
	{
		public static void TryExecute(this ICommand @this, params object[] @params)
		{
			if (@this != null && @this.CanExecute(@params)) @this.Execute(@params);
		}


		internal static T GetParam<T>(this object @this, int index)
		{

			try
			{
				if (@this is Array)
				{
					@this = (@this as Array).GetValue(index);
				}

				return (T)Convert.ChangeType(@this, typeof(T));
			}
			catch
			{
				return default(T);
			}
		}
	}

}
