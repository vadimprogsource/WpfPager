using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfPager.Model
{
	public class Group
	{
		public string Name { get; set; }
	
		public ICollection<Address>  Addresses  { get; set; }

		internal void Add(Address address)
		{
			(Addresses ?? (Addresses = new List<Address>())).Add(address);  
		}

	}
}
