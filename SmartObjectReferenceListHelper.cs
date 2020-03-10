using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace Daniels.UI
{
    public class SmartObjectReferenceListHelperParameters : SmartObjectDynamicListHelperParameters
    {
        public uint DigitalIncrement = 0;
        public uint AnalogIncrement = 0;
        public uint SerialIncrement = 0;
    }

    public class SmartObjectReferenceListHelper : SmartObjectDynamicListHelper
    {

        public const string BooleanInputPrefix = "fb";
        public const string BooleanOutputPrefix = "press";
        public const string UShortInputPrefix = "an_fb";
        public const string UShortOutputPrefix = "an_act";
        public const string StringInputPrefix = "text-o";
        public const string StringOutputPrefix = "text-i";

        private const uint booleanItemsStartIndex = 4016;
        private const uint ushortItemsStartIndex = 12;
        private const uint serialItemsStartIndex = 12;

        private readonly uint _digitalIncrement;
        private readonly uint _analogIncrement;
        private readonly uint _serialIncrement;

        public SmartObjectReferenceListHelper(SmartObject smartObject, SmartObjectReferenceListHelperParameters slrParams)
            : base(smartObject, slrParams)
        {
            _digitalIncrement = slrParams.DigitalIncrement;
            _analogIncrement = slrParams.AnalogIncrement;
            _serialIncrement = slrParams.SerialIncrement;
        }

        #region Properties

        public override ushort NumberOfItems 
        {
            get {return base.NumberOfItems;}
            set
            {
                base.NumberOfItems = value;
                if (_smartObject != null && NumberOfItems > 0)
                {
                    Dictionary<uint, SmartObjectReferenceListItem> items = new Dictionary<uint, SmartObjectReferenceListItem>(NumberOfItems);
                    for (uint i = 1; i <= NumberOfItems; i++)
                    {
                        items.Add(i, new SmartObjectReferenceListItem
                                        (
                                            this,
                                            i,
                                            getItemSigCollection<BoolInputSig>(i, _digitalIncrement),
                                            getItemSigCollection<BoolOutputSig>(i, _digitalIncrement),
                                            getItemSigCollection<UShortInputSig>(i, _analogIncrement),
                                            getItemSigCollection<UShortOutputSig>(i, _analogIncrement),
                                            getItemSigCollection<StringInputSig>(i, _serialIncrement),
                                            getItemSigCollection<StringOutputSig>(i, _serialIncrement)
                                        )
                                 );

                    }
                    Items = new ReadOnlyDictionary<uint, SmartObjectReferenceListItem>(items);
                }
            }
        }

        #endregion Properties

        #region Methods

        #endregion Methods

        public new ReadOnlyDictionary<uint, SmartObjectReferenceListItem> Items;

        private ReadOnlyDictionary<uint, T> getItemSigCollection<T>(uint index, uint increment) where T : Crestron.SimplSharpPro.Sig
        {
            string prefix = String.Empty;
            SigCollectionBase<T> smartObjectSignalCollection = null;

            if (typeof(T) == typeof(BoolInputSig))
            {
                prefix = BooleanInputPrefix;
                smartObjectSignalCollection = _smartObject.BooleanInput as SigCollectionBase<T>;
            }
            else if (typeof(T) == typeof(BoolOutputSig))
            {
                increment = _digitalIncrement;
                prefix = BooleanOutputPrefix;
                smartObjectSignalCollection = _smartObject.BooleanOutput as SigCollectionBase<T>;
            }
            else if (typeof(T) == typeof(UShortInputSig))
            {
                prefix = UShortInputPrefix;
                smartObjectSignalCollection = _smartObject.UShortInput as SigCollectionBase<T>;
            }
            else if (typeof(T) == typeof(UShortOutputSig))
            {
                prefix = UShortOutputPrefix;
                smartObjectSignalCollection = _smartObject.UShortOutput as SigCollectionBase<T>;
            }
            else if (typeof(T) == typeof(StringInputSig))
            {
                prefix = StringInputPrefix;
                smartObjectSignalCollection = _smartObject.StringInput as SigCollectionBase<T>;
            }
            else if (typeof(T) == typeof(StringOutputSig))
            {
                prefix = StringOutputPrefix;
                smartObjectSignalCollection = _smartObject.StringOutput as SigCollectionBase<T>;
            }
            else
                throw new Exception("Signal type is unknown");

            Dictionary<uint, T> itemSignals = new Dictionary<uint, T>((int)increment);
            uint startSigIndex = 1 + (index - 1) * increment;
            uint sigIndex = 0;
            for (uint i = startSigIndex; i < startSigIndex+increment; i++)
            {
                sigIndex++;
                itemSignals.Add(sigIndex, smartObjectSignalCollection[prefix + i]);
            }

            return new ReadOnlyDictionary<uint,T>(itemSignals);
        }

    }
}