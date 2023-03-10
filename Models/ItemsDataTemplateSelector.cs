using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using YourNoteUWP.Models;
using System.Collections.ObjectModel;
using System.Reflection;

namespace YourNoteUWP
{
    public class ItemsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AllItems { get; set; }
        public DataTemplate LastItems { get; set; }
     
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            DataTemplate _returnTemplate = new DataTemplate();
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
 
       
            Type tp = item.GetType();

            if (tp.Equals(typeof(Models.User)))
            {
                var contents = itemsControl.ItemsSource as ObservableCollection<Models.User>;
                int count = contents.Count;
                var i = contents.IndexOf((Models.User)item);

                if (contents.Count == 1)
                    _returnTemplate = LastItems;
                else
                {
                    if (i == count - 1)
                        _returnTemplate = LastItems;
                    else
                        _returnTemplate = AllItems;
                }

            }
            else
            {
                var contents = itemsControl.ItemsSource as ObservableCollection<Note>;
                int count = contents.Count;
                var i = contents.IndexOf((Note)item);

                if (contents.Count == 1)
                    _returnTemplate = LastItems;
                else
                {
                    if (i == count - 1)
                        _returnTemplate = LastItems;
                    else
                        _returnTemplate = AllItems;
                }
            }

           
         


            return _returnTemplate;
        }
     }
}
