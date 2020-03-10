using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace Daniels.UI
{
    public class SmartObjectReferenceListItem : SmartObjectDynamicListItem
    {
        public readonly ReadOnlyDictionary<uint, BoolInputSig> BooleanInput;
        public readonly ReadOnlyDictionary<uint, BoolOutputSig> BooleanOutput;
        public readonly ReadOnlyDictionary<uint, UShortInputSig> UShortInput;
        public readonly ReadOnlyDictionary<uint, UShortOutputSig> UShortOutput;
        public readonly ReadOnlyDictionary<uint, StringInputSig> StringInput;
        public readonly ReadOnlyDictionary<uint, StringOutputSig> StringOutput;

        public SmartObjectReferenceListItem(
            SmartObjectReferenceListHelper smartObjectReferenceListHelper,
            uint id,
            ReadOnlyDictionary<uint, BoolInputSig> booleanInput,
            ReadOnlyDictionary<uint, BoolOutputSig> booleanOutput,
            ReadOnlyDictionary<uint, UShortInputSig> ushortInput,
            ReadOnlyDictionary<uint, UShortOutputSig> ushortOutput,
            ReadOnlyDictionary<uint, StringInputSig> stringInput,
            ReadOnlyDictionary<uint, StringOutputSig> stringOutput
            )
            : base(smartObjectReferenceListHelper, id)
        {
            BooleanInput = booleanInput;
            BooleanOutput = booleanOutput;
            UShortInput = ushortInput;
            UShortOutput = ushortOutput;
            StringInput = stringInput;
            StringOutput = stringOutput;
        }
    }
}