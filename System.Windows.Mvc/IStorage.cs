using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Mvc
{
	public interface  IStorage : IDisposable
	{
		T             Get<T>          (Guid Id) where T : class;
		IQueryable <T> AsQueryable<T> (       ) where T : class;

		T    StoreAsNew<T>(T value) where T : class;
		T    Store     <T>(T value) where T : class;
		void Remove    <T>(T value) where T : class; 
	
	}
}
