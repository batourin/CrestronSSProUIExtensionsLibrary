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
    public class SubPageParameters: BasicTriListParameters
    {
        public uint VisibilityJoin;
        public uint TransitionJoin;
        public List<uint> CloseJoins;
    }

    public class SubPage: TriListBase
    {
        internal readonly uint VisibilityJoin;
        internal readonly uint TransitionJoin;
        protected List<uint> _closeJoins = new List<uint>();
        //internal bool 

        public SubPage(string name, uint visibilityJoin, uint transitionJoin, List<uint> closeJoins, uint booleanOffset, uint analogOffset, uint serialOffset)
            : base(visibilityJoin, name, booleanOffset, analogOffset, serialOffset)
        {
            VisibilityJoin = visibilityJoin;
            TransitionJoin = transitionJoin;
            _closeJoins = closeJoins;
        }

        public SubPage(SubPageParameters subPageParameters)
            : this(subPageParameters.Name, subPageParameters.VisibilityJoin, subPageParameters.TransitionJoin, subPageParameters.CloseJoins, subPageParameters.BooleanOffset, subPageParameters.AnalogOffset, subPageParameters.SerialOffset) 
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            sb.AppendLine("\tVisibilityJoin: " + VisibilityJoin);
            sb.AppendLine("\tTransitionJoin: " + TransitionJoin);
            sb.AppendLine("\tVisible: " + Visible);
            if (_closeJoins.Count > 0)
                sb.AppendLine("\tCloseJoins: " + String.Join(",", _closeJoins.Select(n => n.ToString()).ToArray()));
            else
                sb.AppendLine("\tCloseJoins: NONE");
            return sb.ToString();
        }

        internal Func<SubPage, bool> VisibilityChange;

        public virtual event EventHandler<ReadOnlyEventArgs<bool>> VisibilityChanged;
        protected virtual void OnVisibilityChanged(ReadOnlyEventArgs<bool> e)
        {
            EventHandler<ReadOnlyEventArgs<bool>> handler = VisibilityChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public bool Visible
        {
            get { return Panel.BooleanInput[VisibilityJoin].BoolValue; }
            set
            {
                Func<SubPage, bool> visibilityChange = VisibilityChange;
                if (visibilityChange == null || (visibilityChange != null && visibilityChange(this)))
                {
                    Panel.BooleanInput[VisibilityJoin].BoolValue = value;
                    OnVisibilityChanged(new ReadOnlyEventArgs<bool>(value));
                }
            }
        }

        public class SubPageClosedEventArgs : EventArgs
        {
            public readonly uint CloseReason;
            public SubPageClosedEventArgs(uint closeReason)
            {
                CloseReason = closeReason;
            }
        }
        protected EventHandler<SubPageClosedEventArgs> _subPageClosed;
        public virtual event EventHandler<SubPageClosedEventArgs> SubPageClosed
        {
            add { _subPageClosed += value; }
            remove { _subPageClosed -= value;}
        }
        protected virtual void OnSubPageClosed(SubPageClosedEventArgs e)
        {
            EventHandler<SubPageClosedEventArgs> handler = _subPageClosed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void panel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            //CrestronConsole.PrintLine("panel-SigChange: args.Sig.Ty[e");
            if (args.Sig.Type == eSigType.Bool && args.Sig.BoolValue && _closeJoins.Contains(AnalogRelativeJoin(args.Sig.Number)))
            {
                Visible = false;
                OnSubPageClosed(new SubPageClosedEventArgs(AnalogRelativeJoin(args.Sig.Number)));
            }
        }

    }
}