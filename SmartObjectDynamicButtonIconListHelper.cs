using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace Daniels.UI
{
    public class SmartObjectDynamicButtonIconListHelper : SmartObjectDynamicListHelper
    {
        private Dictionary<uint, SmartObjectDynamicButtonIconListItem> _items;

        public SmartObjectDynamicButtonIconListHelper(SmartObject smartObject, SmartObjectDynamicListHelperParameters helperParams)
            : base(smartObject, helperParams)
        {
            _items = new Dictionary<uint, SmartObjectDynamicButtonIconListItem>(MaxNumberOfItems);
            for (uint i = 1; i <= MaxNumberOfItems; i++)
                _items.Add(i, new SmartObjectDynamicButtonIconListItem(this, i));
            Items = new ReadOnlyDictionary<uint, SmartObjectDynamicButtonIconListItem>(_items);
        }

        #region Properties

        public override ushort NumberOfItems
        {
            get { return base.NumberOfItems; }
            set
            {
                base.NumberOfItems = value;
                if (NumberOfItems > 0)
                {
                }
            }
        }

        #endregion Properties

        public new ReadOnlyDictionary<uint, SmartObjectDynamicButtonIconListItem> Items;

    }
}