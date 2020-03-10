using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Daniels.UI
{
    public class SmartObjectDynamicListItem
    {
        public const string ItemVisibleFormat = "Item {0} Visible";
        public const string ItemEnabledFormat = "Item {0} Enabled";

        public readonly uint Id;
        protected SmartObjectDynamicListHelper _smartObjectHelper;

        public SmartObjectDynamicListItem(
                SmartObjectDynamicListHelper smartObjectHelper,
                uint id)
        {
            _smartObjectHelper = smartObjectHelper;
            Id = id;
        }

        #region Properties

        public bool Enable
        {
            get
            {
                return _smartObjectHelper._smartObject.BooleanInput[String.Format(ItemEnabledFormat, Id)].BoolValue;
            }
            set
            {
                _smartObjectHelper._smartObject.BooleanInput[String.Format(ItemEnabledFormat, Id)].BoolValue = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _smartObjectHelper._smartObject.BooleanInput[String.Format(ItemVisibleFormat, Id)].BoolValue;
            }
            set
            {
                _smartObjectHelper._smartObject.BooleanInput[String.Format(ItemVisibleFormat, Id)].BoolValue = value;
            }
        }

        #endregion Properties
    }
}