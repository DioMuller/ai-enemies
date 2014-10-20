using Enemies.Scripting;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Enemies.GUI
{
    public partial class ScreenMapSelection
    {
        ImmutableList<MapDescription> _items = ImmutableList<MapDescription>.Empty;
        int _firstIndex = 0;
        int _itemsPerPage = 1;
        TaskCompletionSource<string> _selectItem;

        public ImmutableList<string> Items
        {
            set
            {
                if (_items != null)
                {
                    foreach (var item in _items)
                        item.OnClick -= item_OnClick;
                }

                _items = value.Select(i => new MapDescription { BindingContext = i }).ToImmutableList();
                foreach (var item in _items)
                    item.OnClick += item_OnClick;
                Refresh();
            }
        }

        public ScreenMapSelection()
        {
            InitializeComponent();
            Stack.SizeChanged += EntitySelection_SizeChanged;
        }

        void EntitySelection_SizeChanged(object sender, EventArgs e)
        {
            _itemsPerPage = (int)(Stack.Height / 32);
            Refresh();
        }

        void Refresh()
        {
            RefreshItems();
            RefreshUI();
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
                Stack.Children.Add(item);
                i++;
            }
        }

        void item_OnClick(object sender, EventArgs e)
        {
            if (_selectItem == null)
                return;
            var view = (MapDescription)sender;
            string selected = (MapDescription)view.BindingContext;
            _selectItem.TrySetResult(selected);
        }

        void RefreshUI()
        {
            BtnPageLeft.Opacity = _firstIndex <= 0 ? 0 : 1;
            BtnPageRight.Opacity = _firstIndex + _itemsPerPage >= _items.Count ? 0 : 1;
        }

        public void PageLeft(object sender, EventArgs e)
        {
            _firstIndex -= 1;//_itemsPerPage;
            _firstIndex = Math.Max(0, _firstIndex);
            Refresh();
        }

        public void PageRight(object sender, EventArgs e)
        {
            _firstIndex += 1;//_itemsPerPage;
            _firstIndex = Math.Min(_items.Count - _itemsPerPage, _firstIndex);
            Refresh();
        }

        public async void BackClick(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public async Task<string> SelectItemAsync()
        {
            _selectItem = new TaskCompletionSource<string>();
            var selected = await _selectItem.Task;
            return selected;
        }
    }
}
