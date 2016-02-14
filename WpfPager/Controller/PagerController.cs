using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Mvc;
using System.Windows.Mvc.Xml.Linq;



using WpfPager.Model;
using WpfPager.ViewModel; 


namespace WpfPager.Controller
{
	
	public class PagerController : FrontController , IMessageHost
	{


		public  static Address              From       ;
		private static ICollection<Group>   m_addressBook;
		private static ICollection<Message> m_history    ; 



		public static void StartUp(string configXmlFile)
		{
			m_addressBook = configXmlFile
						  .ToXml()
						  .Run(x=>From = x.Elements("From").First().Deserialize<Address>())
						  .DeserializeMany<Group>((x, y) => y.Addresses = x.DeserializeMany<Address>().ToArray())
						  .ToArray(); 

            m_history = new List<Message>(); 
	 
		}


		public void Main()
		{

			View("mainview", new Pager
			{
				To            = null,
				History       = new ObservableCollection<Message>(),
				Message       = string.Empty,
				Groups        = m_addressBook,
				Send          = Route<Pager>(Send),
				SelectCulture = Route<object>(SelectCulture),
				SelectAddress = Route<Pager, Address>(SelectAddress),
				ViewMessage   = Route<Message>(Message),
			});
		}



		public static void Message(Message model)
		{

			model.Addresses = m_addressBook.SelectMany(x => x.Addresses.Select(y=>new Address{Name = y.Name , Uri = y.Uri})  ).ToArray();

			View("messageview", new ViewModel<Message>
			{
				 Model = model , 
				 Close = Route(Back)  
			});

		}


		public static void SelectCulture(object model)
		{
			Culture = model.ToString();
			ReView("mainview");
		}


		public static void SelectAddress(Pager owner, Address model)
		{
			 using (owner.Modifier(x => x.History).Modifier(x=>x.To))
			{

				owner.To = model; 
				 
				 if (model == null)
				{
					owner.History = new ObservableCollection<Message>(m_history.OrderByDescending(x => x.CreatedTime));
					return;
				}


				owner.History = new ObservableCollection<Message>(m_history.Where(x => x.To.Name == model.Name || x.From.Name == model.Name).OrderByDescending(x=>x.CreatedTime));    
			}
		}


		public static void Send(Pager model)
		{

			if (model.To == null || string.IsNullOrWhiteSpace(model.Message)) return;


			using (model.Modifier(x => x.Message))
			{
				
				Message message = new Message
				{
					CreatedTime   = DateTime.Now,
					From          = From,
					To            = model.To,
					ContentAsText = model.Message
				};



				model.Message = string.Empty;
				model.History.Add(message)  ;

				m_history.Add(message); 


				using (var factory = new ChannelFactory<IMessageHost>(new BasicHttpBinding(), message.To.Uri))
				{
					 factory.Open();
 					 factory.CreateChannel().Send(new Request<Message>(message)); 
				}

			}
		}


		void IMessageHost.Send(Request<Message> message)
		{
			m_history.Add(message.Content);
			ExecuteForEach<Pager>(model => model.History.Add(message.Content));   
		}
	}
}
