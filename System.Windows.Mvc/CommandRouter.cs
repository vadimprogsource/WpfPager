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


	public class CommandRouter : ICommand
	{
		private Func  <bool> m_condition = ()=>true;
		private Action       m_action    = ()=>{};



		public CommandRouter If(Func<bool> condition)
		{
			m_condition = condition;
			return this;
		}


		public CommandRouter Route(Action command)
		{
			m_action = command;
			return this;
		}


		
		bool ICommand.CanExecute(object parameter)
		{
			return m_condition();  
		}

		event EventHandler ICommand.CanExecuteChanged
		{
			add    {}
			remove {}
		}

		void ICommand.Execute(object parameter)
		{
			FrontController.NotifyOnExecute(); 
			m_action(); 
		}



	}


	public class CommandRouter<T> : ICommand 
	{

		private Func  <T, bool> m_condition = x=>true;
		private Action<T>       m_action    = x=>{};



		public CommandRouter<T> If(Func<T, bool> condition)
		{
			m_condition = condition;
			return this;
		}


		public  CommandRouter<T> Route(Action<T> command)
		{
			m_action = command;
			return this;
		}



		bool ICommand.CanExecute(object parameter)
		{
			return m_condition((T)parameter);  
		}

		event EventHandler ICommand.CanExecuteChanged
		{
			add    {}
			remove {}
		}

		void ICommand.Execute(object parameter)
		{
			FrontController.NotifyOnExecute(); 
			m_action((T)parameter); 
		}
	}


	public class CommandRouter<T1, T2> : ICommand
	{

		private Func  <T1,T2,bool> m_condition = (x,y)=>true;
		private Action<T1,T2     > m_action    = (x,y)=>{};



		public CommandRouter<T1,T2> If(Func<T1,T2, bool> condition)
		{
			m_condition = condition;
			return this;
		}


		public CommandRouter<T1,T2> Route(Action<T1,T2> command)
		{
			m_action = command;
			return this;
		}




		bool ICommand.CanExecute(object parameter)
		{
			return m_condition(parameter.GetParam<T1>(0), parameter.GetParam<T2>(1));     
		}

		event EventHandler ICommand.CanExecuteChanged
		{
			add   {}
			remove{}
		}

		void ICommand.Execute(object parameter)
		{
			m_action(parameter.GetParam<T1>(0), parameter.GetParam<T2>(1)); 
		}
	}
}
