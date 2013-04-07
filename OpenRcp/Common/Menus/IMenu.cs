using System.Collections.Generic;
using Caliburn.Micro;

namespace OpenRcp
{
	public interface IMenu : IObservableCollection<MenuItemBase>
	{
		IEnumerable<MenuItemBase> All { get; }
        void Add(params MenuItemBase[] items);
        MenuItemBase this[string name] { get; }
	}
}