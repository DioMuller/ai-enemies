using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace Enemies.GUI
{
    public partial class EntitySelection
    {
        public class Item
        {
            public string Name { get; set; }
        }

        IEnumerable<Item> _items;
        int _firstIndex = 0;
        int _itemsPerPage = 1;

        public IEnumerable<Item> Items
        {
            set
            {
                _items = value;
                RefreshItems();
            }
        }

        public EntitySelection()
        {
            InitializeComponent();
        }

        void RefreshItems()
        {
            Stack.Children.Clear();
            if (_items == null)
                return;
            int i = 0;
            foreach (var item in _items.Skip(_firstIndex))
            {
                if (i >= _itemsPerPage)
                    break;
                Stack.Children.Add(new SelectionItem { BindingContext = item });
                i++;
            }
        }

        public void PageLeft(object sender, EventArgs e)
        {
            _firstIndex -= _itemsPerPage;
            _firstIndex = Math.Max(0, _firstIndex);
            RefreshItems();
        }

        public void PageRight(object sender, EventArgs e)
        {
            _firstIndex += _itemsPerPage;
            _firstIndex = Math.Min(_items.Count() - _itemsPerPage, _firstIndex);
            RefreshItems();
        }
    }
}
