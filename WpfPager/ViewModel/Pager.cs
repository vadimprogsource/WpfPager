using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using System.Windows.Input;
using System.ComponentModel;
using WpfPager.Model;

namespace WpfPager.ViewModel
{
	public class Pager : System.Windows.Mvc.ViewModel 
	{

		public ICollection<Group> Groups { get; set; } 

	

		public ObservableCollection<Message> History    {get;set;}
		
		public ICollection<Address> AddressBook
		{
			get
			{
				return Groups.SelectMany(x => x.Addresses).ToArray();   
			}
		}


		
		public Address To      { get; set; }
		public string  Message { get; set; }

		public ICommand Send         {get; set;}
		public ICommand SelectCulture{get; set;}
		public ICommand SelectAddress{get; set;}
		public ICommand ViewMessage  {get; set; }
	}
}
