using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace Daniels.UI
{
    public class SmartObjectDynamicButtonIconListItem : SmartObjectDynamicListItem
    {
        public const string ItemSelectedFormat = "Item {0} Selected";
        public const string ItemPressedFormat = "Item {0} Pressed";
        public const string SetItemTextFormat = "Set Item {0} Text";
        public const string SetItemIconAnalogFormat = "Set Item {0} Icon Analog";
        public const string SetItemIconSerialFormat = "Set Item {0} Icon Serial";

        public SmartObjectDynamicButtonIconListItem(SmartObjectDynamicListHelper smartObjectHelper, uint id)
            : base(smartObjectHelper, id)
        {
        }

        #region Properties

        public bool Selected
        {
            get
            {
                return _smartObjectHelper._smartObject.BooleanInput[String.Format(ItemSelectedFormat, Id)].BoolValue;
            }
            set
            {
                _smartObjectHelper._smartObject.BooleanInput[String.Format(ItemSelectedFormat, Id)].BoolValue = value;
            }
        }

        public Action<bool> PressedAction
        {
            set { _smartObjectHelper._smartObject.BooleanOutput[String.Format(ItemPressedFormat, Id)].UserObject = value; }
        }

        public string Text
        {
            get
            {
                return _smartObjectHelper._smartObject.StringInput[String.Format(SetItemTextFormat, Id)].StringValue;
            }
            set
            {
                _smartObjectHelper._smartObject.StringInput[String.Format(SetItemTextFormat, Id)].StringValue = value;
            }
        }

        public ushort IconAnalog
        {
            get
            {
                return _smartObjectHelper._smartObject.UShortInput[String.Format(SetItemIconAnalogFormat, Id)].UShortValue;
            }
            set
            {
                _smartObjectHelper._smartObject.UShortInput[String.Format(SetItemIconAnalogFormat, Id)].UShortValue = value;
            }
        }

        public string IconSerial
        {
            get
            {
                return _smartObjectHelper._smartObject.StringInput[String.Format(SetItemIconSerialFormat, Id)].StringValue;
            }
            set
            {
                _smartObjectHelper._smartObject.StringInput[String.Format(SetItemIconSerialFormat, Id)].StringValue = value;
            }
        }

        #endregion Properties

    }
}