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

        private Dictionary<uint, SmartObjectDynamicListItem> _items;

        public SmartObjectDynamicListHelper(SmartObject smartObject, SmartObjectDynamicListHelperParameters helperParams)
            : base(smartObject, helperParams)
        {
            MaxNumberOfItems = (ushort)_smartObject.BooleanOutput.Count(s => s.Name.EndsWith("Pressed"));

            _items = new Dictionary<uint, SmartObjectDynamicListItem>(MaxNumberOfItems);
            for (uint i = 1; i <= MaxNumberOfItems; i++)
                _items.Add(i, new SmartObjectDynamicListItem(this, i));
            Items = new ReadOnlyDictionary<uint, SmartObjectDynamicListItem>(_items);

            NumberOfItems = helperParams.NumberOfItems;
            if (helperParams.NumberOfItems > MaxNumberOfItems)
                throw new ArgumentOutOfRangeException("helperParams.NumberOfItems", "Only " + MaxNumberOfItems + " items defined in the SGD file");
            else
                NumberOfItems = helperParams.NumberOfItems;
        }

        #region Properties

        public ushort MaxNumberOfItems { get; private set; } 

        public virtual ushort NumberOfItems
        {
            get { return _smartObject.UShortInput[SetNumberOfItems].UShortValue; }
            set
            {
                if (value > 0 && value <= MaxNumberOfItems)
                {
                    _smartObject.UShortInput[SetNumberOfItems].UShortValue = value;
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