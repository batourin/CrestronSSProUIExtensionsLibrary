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
        public SmartObjectDynamicButtonIconListHelper(SmartObject smartObject, SmartObjectDynamicListHelperParameters helperParams)
            : base(smartObject, helperParams)
        {

        }

        #region Properties

        public override ushort NumberOfItems
        {
            get { return base.NumberOfItems; }
            set
            {
                base.NumberOfItems = value;
                if (_smartObject != null && NumberOfItems > 0)
                {
                    Dictionary<uint, SmartObjectDynamicButtonIconListItem> items = new Dictionary<uint, SmartObjectDynamicButtonIconListItem>(NumberOfItems);
                    for (uint i = 1; i <= NumberOfItems; i++)
                        items.Add(i, new SmartObjectDynamicButtonIconListItem(this, i));
                    Items = new ReadOnlyDictionary<uint, SmartObjectDynamicButtonIconListItem>(items);
                }
            }
        }

        #endregion Properties

        public new ReadOnlyDictionary<uint, SmartObjectDynamicButtonIconListItem> Items;

    }
}