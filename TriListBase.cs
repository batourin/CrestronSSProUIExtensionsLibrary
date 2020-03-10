using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;


namespace Daniels.UI
{
    public abstract class BasicTriListParameters
    {
        public uint Id;
        public string Name;
        public uint BooleanOffset;
        public uint AnalogOffset;
        public uint SerialOffset;
    }

    public abstract class TriListBase 
    {
        private readonly uint _booleanOffset;
        private readonly uint _analogOffset;
        private readonly uint _serialOffset;

        private BasicTriList _panel;

        private readonly uint _id;
        public uint Id { get { return _id; } }
        private readonly string _name;
        public string Name { get { return _name; } }

        public TriListBase(uint id, string name, uint booleanOffset, uint analogOffset, uint serialOffset)
        {
            _id = id;
            _name = name;
            _booleanOffset = booleanOffset;
            _analogOffset = analogOffset;
            _serialOffset = serialOffset;
        }
        public TriListBase(BasicTriListParameters basicTriListParams)
            : this(basicTriListParams.Id, basicTriListParams.Name, basicTriListParams.BooleanOffset, basicTriListParams.AnalogOffset, basicTriListParams.SerialOffset)
        {

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.GetType().Name + "\r\n");
            sb.AppendLine("\tId: " + Id);
            sb.AppendLine("\tName: " + Name);
            sb.AppendLine("\tPanel: " + Panel.Description);
            sb.AppendLine("\tBooleanOffset: " + _booleanOffset);
            sb.AppendLine("\tAnalogOffset: " + _analogOffset);
            sb.AppendLine("\tSerialOffset: " + _serialOffset);

            return sb.ToString();
        }

        internal virtual BasicTriList Panel
        {
            get { return _panel; }
            set
            {
                if (_panel != null)
                    _panel.SigChange -= panel_SigChange;
                _panel = value;
                _panel.SigChange += new SigEventHandler(panel_SigChange);
            }
        }

        protected virtual void panel_SigChange(BasicTriList currentDevice, Crestron.SimplSharpPro.SigEventArgs args)
        {
        }

        public uint BooleanRelativeJoin(uint join)
        {
            return join - _booleanOffset;
        }

        public uint BooleanAbsoluteJoin(uint join)
        {
            return _booleanOffset + join;
        }

        public uint AnalogRelativeJoin(uint join)
        {
            return join - _analogOffset;
        }

        public uint AnalogAbsoluteJoin(uint join)
        {
            return _analogOffset + join;
        }

        public uint SerialRelativeJoin(uint join)
        {
            return join - _serialOffset;
        }

        public uint SerialAbsoluteJoin(uint join)
        {
            return _serialOffset + join;
        }

        public bool BooleanJoinGet(uint join)
        {
            return _panel.UShortOutput[BooleanAbsoluteJoin(join)].BoolValue;
        }

        public void BooleanJoinSet(uint join, bool value)
        {
            _panel.BooleanInput[BooleanAbsoluteJoin(join)].BoolValue = value;
        }

        public ushort AnalogJoinGet(uint join)
        {
            return _panel.UShortOutput[AnalogAbsoluteJoin(join)].UShortValue;
        }

        public void AnalogJoinSet(uint join, ushort value)
        {
            _panel.UShortInput[AnalogAbsoluteJoin(join)].UShortValue = value;
        }

        public string SerialJoinGet(uint join)
        {
            return _panel.StringOutput[SerialAbsoluteJoin(join)].StringValue;
        }

        public void SerialJoinSet(uint join, string value)
        {
            _panel.StringInput[SerialAbsoluteJoin(join)].StringValue = value;
        }



    }
}