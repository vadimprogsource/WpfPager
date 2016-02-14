using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using System.ComponentModel; 

namespace System.Windows.Mvc
{


	public class ViewModel : INotifyPropertyChanged, IDisposable
	{

		private List<PropertyInfo> m_events;


		internal void AddPropertyChanged(PropertyInfo property)
		{
			(m_events ?? (m_events = new List<PropertyInfo>())).Add(property);
		}


		public event PropertyChangedEventHandler PropertyChanged;




		void IDisposable.Dispose()
		{
			if (m_events != null) foreach (PropertyChangedEventArgs @event in m_events.Select(x => new PropertyChangedEventArgs(x.Name))) PropertyChanged(this, @event);
		}
	}


	public class ViewModel<T> : ViewModel
	{
		public T        Model       { get; set; }
		public ICommand Save        {get; set;}
		public ICommand SaveAndClose{get; set; }
		public ICommand Close       {get; set;}
		public ICommand Cancel      {get; set;}

	}
}
