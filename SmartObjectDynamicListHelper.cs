using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace Daniels.UI
{
    public class SmartObjectDynamicListHelperParameters : SmartObjectHelperParameters
    {
        public ushort NumberOfItems = 0;
    }

    public class SmartObjectDynamicListHelper : SmartObjectHelper
    {
        public const string ScrollToItem = "Scroll To Item";
        public const string SetNumberOfItems = "Set Number of Items";
        public const string ItemClicked = "Item Clicked";

        public SmartObjectDynamicListHelper(SmartObject smartObject, SmartObjectDynamicListHelperParameters helperParams)
            : base(smartObject, helperParams)
        {
            NumberOfItems = helperParams.NumberOfItems;
        }

        #region Properties

        private ushort _numberOfItems = 0;
        public virtual ushort NumberOfItems
        {
            get { return _numberOfItems; }
            set
            {
                _numberOfItems = value;
                if (_smartObject != null && NumberOfItems > 0)
                {
                    _smartObject.UShortInput[SetNumberOfItems].UShortValue = NumberOfItems;

                    Dictionary<uint, SmartObjectDynamicListItem> items = new Dictionary<uint, SmartObjectDynamicListItem>(NumberOfItems);
                    for (uint i = 1; i <= NumberOfItems; i++)
                        items.Add(i, new SmartObjectDynamicListItem( this, i));
                    Items = new ReadOnlyDictionary<uint, SmartObjectDynamicListItem>(items);
                }
            }
        }

        public Action<ushort> ItemClickedAction
        {
            get { return _smartObject.UShortOutput[ItemClicked].UserObject as Action<ushort>; }
            set { _smartObject.UShortOutput[ItemClicked].UserObject = value; }
        }

        #endregion Properties

        public ReadOnlyDictionary<uint, SmartObjectDynamicListItem> Items;

    }
}