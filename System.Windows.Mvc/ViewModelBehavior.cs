using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using System.Text;
using System.ComponentModel;

using System.Reflection;

namespace System.Windows.Mvc
{
	public static class ViewModelBehavior
	{

		public static T Modifier<T, V>(this T @this, Expression<Func<T, V>> modifier) where T : ViewModel
		{
			@this.AddPropertyChanged((modifier.Body as MemberExpression).Member as PropertyInfo);  
			return @this;
		}



		
	}
}
