using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using Daniels.Common;

namespace Daniels.UI
{
    public class TextEntrySubPage: ModalSubPage
    {
        private readonly uint _textEntry;
        public TextEntrySubPage(uint visibilityJoin, uint transitionJoin, uint textEntry, List<uint> closeJoins, uint booleanOffset, uint analogOffset, uint serialOffset)
            : base(visibilityJoin, transitionJoin, closeJoins, booleanOffset, analogOffset, serialOffset)
        {
            _textEntry = textEntry;
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
                //return SerialJoinGet(_textEntry);
            }
            set
            {
                //_manager._panel.StringOutput[_serialOffset + _textEntry].StringValue = String.Empty;
                _text = value;
                SerialJoinSet(_textEntry, _text);
            }
        }

        protected override void panel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            base.panel_SigChange(currentDevice, args);
            if (args.Sig.Type == eSigType.String && _textEntry == (args.Sig.Number - _serialOffset))
            {
                _text = SerialJoinGet(_textEntry);
            }
        }

    }
}