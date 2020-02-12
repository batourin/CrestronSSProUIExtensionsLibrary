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
    public class SubPage
    {
        protected SubPageManager _manager;
        internal readonly uint VisibilityJoin;
        internal readonly uint TransitionJoin;
        protected List<uint> _closeJoins = new List<uint>();
        protected readonly uint _booleanOffset;
        protected readonly uint _analogOffset;
        protected readonly uint _serialOffset;

        //internal bool 

        public SubPage(uint visibilityJoin, uint transitionJoin, List<uint> closeJoins, uint booleanOffset, uint analogOffset, uint serialOffset)
        {
            VisibilityJoin = visibilityJoin;
            TransitionJoin = transitionJoin;
            _closeJoins = closeJoins;
            _booleanOffset = booleanOffset;
            _analogOffset = analogOffset;
            _serialOffset = serialOffset;
        }

        internal SubPageManager Manager
        {
            set
            {
                _manager = value;
                _manager._panel.SigChange += new SigEventHandler(panel_SigChange);
            }
        }

        public virtual event EventHandler<ReadOnlyEventArgs<bool>> VisibilityChanged;
        protected virtual void OnVisibilityChanged(ReadOnlyEventArgs<bool> e)
        {
            EventHandler<ReadOnlyEventArgs<bool>> handler = VisibilityChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private bool visible = false;
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                if(value)
                    _manager.MakeVisible(this);
                else
                    _manager.Hide(this);
                OnVisibilityChanged(new ReadOnlyEventArgs<bool>(visible)); 
            }
        }

        /*
        public uint BooleanJoin1(uint join)
        {
            return _booleanOffset + join;
        }
        public uint AnalogJoin1(uint join)
        {
            return _analogOffset + join;
        }
         */ 
        
        /*public uint SerialJoinGet(uint join)
        {
            return _serialOffset + join;
        }*/

        public string SerialJoinGet(uint join)
        {
            return _manager._panel.StringOutput[_serialOffset + join].StringValue;
        }

        public void SerialJoinSet(uint join, string value)
        {
            _manager._panel.StringInput[_serialOffset + join].StringValue = value;
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

        protected virtual void panel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {
            if (args.Sig.Type == eSigType.Bool && _closeJoins.Contains(args.Sig.Number - _analogOffset))
            {
                Visible = false;
                OnSubPageClosed(new SubPageClosedEventArgs(args.Sig.Number - _analogOffset));
            }
        }

    }
}