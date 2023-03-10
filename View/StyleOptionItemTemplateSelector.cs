using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using YourNoteUWP.Models;

namespace YourNoteUWP.View
{
    internal class StyleOptionItemTemplateSelector : ItemsDataTemplateSelector
    {
        public DataTemplate NoteColorOptions { get; set; }
        public DataTemplate FontColorOptions { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            DataTemplate _returnTemplate = new DataTemplate();
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
            ObservableCollection<Note> contents = itemsControl.ItemsSource as ObservableCollection<Note>;
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



            return _returnTemplate;
        }
    }
}
