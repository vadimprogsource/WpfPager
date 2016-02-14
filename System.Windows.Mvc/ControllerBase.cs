using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;



namespace System.Windows.Mvc
{
	public class FrontController
	{


		private static readonly Stack<ContentControl> m_history = new Stack<ContentControl>();


		protected static void ClearHistory()
		{
			m_history.Clear();  
		}


		public static void Back()
		{
			ContentControl control =   m_history.Pop();
			Desktop.DataContext    = control.DataContext;
			Desktop.Content        = control;
		}

   

		private static Dispatcher m_dispatcher;


		public static string Location;
		public static string Culture;


		public static ContentControl Desktop = null;



		public static void NotifyOnExecute()
		{
			m_dispatcher = Dispatcher.CurrentDispatcher;
		}




		protected static CommandRouter If(Func<bool> condition)
		{
			return new CommandRouter().If(condition);
		}

		protected static ICommand Route(Action command)
		{
			return new CommandRouter().Route(command);
		}


		protected static CommandRouter<T> If<T>(Func<T, bool> condition)
		{
			return new CommandRouter<T>().If(condition); 
		}

		protected static ICommand Route<T>(Action<T> command)
		{
			return new CommandRouter<T>().Route(command);  
		}



		protected static CommandRouter<T1, T2> If<T1, T2>(Func<T1, T2, bool> condition)
		{
			return new CommandRouter<T1, T2>().If(condition);
		}

		protected static ICommand Route<T1, T2>(Action<T1, T2> command)
		{
			return new CommandRouter<T1, T2>().Route(command);
		}



		protected static void Close()
		{
			foreach (var win in Application.Current.Windows.OfType<Window>()) win.Close(); 
		}


		protected static void ExecuteForEach<T>(Action<T> action) where T : class 
		{

			

			Action wrapper = () =>
			{
				foreach (T t in Application.Current.Windows.OfType<Window>().Where(x => x.DataContext is T).Select(x => x.DataContext as T)) action(t);
			};

			System.Threading.Thread.Sleep(3000);
		
			m_dispatcher.Invoke(wrapper); 

		}




		private static UserControl LoadUserControl(string name)
		{
			Uri u = new Uri(new StringBuilder(Location).Append('/').Append(Culture).Append('/').Append(name).Append(".xaml").ToString(), UriKind.Relative);
			return (UserControl)System.Windows.Application.LoadComponent(u);
		}



		protected static void ReView(string name)
		{

			UserControl x = LoadUserControl(name);
			x.DataContext = Desktop.DataContext;

			Desktop.Content     = x    ;
		}



		protected static void View<T>(string name,T model) 
		{
			UserControl x = LoadUserControl(name);
			x.DataContext = model;


			if (Desktop.Content != null)
			{
				m_history.Push(Desktop.Content as ContentControl); 
			}


			Desktop.Content     = x    ;
			Desktop.DataContext = model;
		}


		protected static void ViewOnWindow<T>(string name, T model)
		{

			UserControl x = LoadUserControl(name);
			x.DataContext = model;

			new Window { Content = x, DataContext = model }.Show();
		}

		protected static bool ViewOnDialog<T>(string name, T model)
		{

			UserControl x = LoadUserControl(name);
			x.DataContext = model;

			bool? y =  new Window { Content = x, DataContext = model }.ShowDialog();
			return y.HasValue ? y.Value : false;
		}


	}
}
