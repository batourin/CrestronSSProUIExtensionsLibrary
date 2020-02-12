using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace Daniels.UI
{
    public class ModalSubPage: SubPage
    {
        private ShowModalCallback _callBack;
        public ModalSubPage(uint visibilityJoin, uint transitionJoin, List<uint> closeJoins, uint booleanOffset, uint analogOffset, uint serialOffset)
            : base(visibilityJoin, transitionJoin, closeJoins, booleanOffset, analogOffset, serialOffset)
        {

        }

        public delegate void ShowModalCallback(ModalSubPage sender, SubPageClosedEventArgs args);
        public void ShowModal(ShowModalCallback callBack)
        {
            Visible = true;
            _callBack = callBack;
        }

        protected override void panel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            base.panel_SigChange(currentDevice, args);
            if (args.Sig.Type == eSigType.Bool && _closeJoins.Contains(args.Sig.Number - _analogOffset))
            {
                if (_callBack != null)
                {
                    _callBack.Invoke(this, new SubPageClosedEventArgs(args.Sig.Number - _analogOffset));
                    _callBack = null;
                }
            }
        }
    }
}