using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


using System.Windows.Mvc.Binary;




namespace WpfPager.Model
{

	public enum MessageType
	{
		Text = 0,
		Audio = 1, 
		Video = 2,
		Doc   = 3,
	}
	
	
	public class Message 
	{

		public ICollection<Address> Addresses { get; set; }



		public Guid     Id          {get; set;}
		public Address  From        {get; set;}
		public Address  To          {get; set;}
		public DateTime CreatedTime {get; set;}
		public byte[]   Content     {get; set;}


		public MessageType Type { get; set; }


		public Message()
		{
			Id   = Guid.NewGuid(); 
			Type = MessageType.Audio;

		}


		public string ContentAsText
		{
			get
			{
			
				return Type== MessageType.Text ?(Content == null ? string.Empty : System.Text.Encoding.Default.GetString(Content,0 , Content.Length)) : Type.ToString();  
			}

			set
			{
				Type    = MessageType.Text;
				Content = System.Text.Encoding.Default.GetBytes(value); 
			}
		}


		public bool HasText  { get { return Type == MessageType.Text; } }
		public bool HasMedia { get { return Type == MessageType.Audio || Type == MessageType.Video; } }

	
	}



}
