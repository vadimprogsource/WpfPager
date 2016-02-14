using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using System.Windows.Controls; 


namespace System.Windows.Mvc
{
	public static class TreeViewBehavior
	{
		public static readonly DependencyProperty SelectItemProperty = DependencyProperty.RegisterAttached("SelectItem", typeof(ICommand), typeof(TreeViewBehavior), new PropertyMetadata(null, (x, y) =>
		{
			FrameworkElement element = x as FrameworkElement;

			if (element != null)
			{

	
				RoutedPropertyChangedEventHandler<object> SelectedItemChangedEvent = (sender, e) =>
				{
					GetSelectItem(sender as DependencyObject).TryExecute((sender as FrameworkElement).DataContext, e.NewValue);
                };


				element.AddHandler(TreeView.SelectedItemChangedEvent, SelectedItemChangedEvent); 
			}

		}));


		public static void SetSelectItem(this DependencyObject @this, ICommand command)
		{
			@this.SetValue(SelectItemProperty, command);
		}

		public static ICommand GetSelectItem(DependencyObject @this)
		{
			return (ICommand)@this.GetValue(SelectItemProperty); 
		}
	}
}
