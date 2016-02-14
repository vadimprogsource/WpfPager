using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

using System.Windows.Mvc;


using WpfPager.Model; 

namespace WpfPager.Controller
{
	[ServiceContract(SessionMode = SessionMode.NotAllowed)]
	public interface IMessageHost
	{
		[OperationContract(IsOneWay = true)]
		void Send(Request<Message>  message);
	}
}
