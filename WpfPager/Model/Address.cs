using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfPager.Model
{
	public class Address 
	{
		public string Name {get;set;}
		public string Uri  {get;set;}


		public override string ToString()
		{
			return Name;
		}


		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ Uri.GetHashCode();
		}


		public override bool Equals(object obj)
		{
			Address address = obj as Address;
			return address != null && GetHashCode() == address.GetHashCode() && Name == address.Name && Uri == address.Uri;    
		}
	}
}
