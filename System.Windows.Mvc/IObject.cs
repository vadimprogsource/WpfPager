using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Mvc
{
	public interface  IObject
	{
		Guid GetIdentity (       );
		void SetIndentity(Guid id);
	}
}
