using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace Daniels.UI
{
    public class PinLockSubPageParameters: SubPageParameters
    {
        public uint KeyPadSmartObjectId;
        public uint AuthErrorJoin;
        public List<uint> PinJoins;
        public List<uint> Level1Pins;
        public List<uint> Level2Pins;
    }

    public class PinLockSubPage: ModalSubPage
    {
        private const ushort PinOff = 0;
        private const ushort PinOn = 1;
        private PinLockSubPageParameters _params;

        private int _currentPinIndex = 0;
        private List<ushort> _currentPinValue = new List<ushort>(4) { 0,0,0,0};

        private CTimer _authShowErrorTimer;

        public PinLockSubPage(PinLockSubPageParameters subPageParams)
            : base(subPageParams.Name, subPageParams.VisibilityJoin, subPageParams.TransitionJoin, new List<uint>(), subPageParams.BooleanOffset, subPageParams.AnalogOffset, subPageParams.SerialOffset)
        {
            _params = subPageParams;
            _authShowErrorTimer = new CTimer(hideAuthError, Timeout.Infinite);
        }

        internal Func<ushort, bool> Authenticate;

        internal new BasicTriListWithSmartObject Panel
        {
            set
            {
                base.Panel = value;

                BasicTriListWithSmartObject panel = value;

                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["1"].UserObject = new Action<bool>(x => { if (x) addPinNumber(1); else checkPin();});
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["2"].UserObject = new Action<bool>(x => { if (x) addPinNumber(2); else checkPin(); });
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["3"].UserObject = new Action<bool>(x => { if (x) addPinNumber(3); else checkPin(); });
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["4"].UserObject = new Action<bool>(x => { if (x) addPinNumber(4); else checkPin(); });
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["5"].UserObject = new Action<bool>(x => { if (x) addPinNumber(5); else checkPin(); });
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["6"].UserObject = new Action<bool>(x => { if (x) addPinNumber(6); else checkPin(); });
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["7"].UserObject = new Action<bool>(x => { if (x) addPinNumber(7); else checkPin(); });
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["8"].UserObject = new Action<bool>(x => { if (x) addPinNumber(8); else checkPin(); });
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["9"].UserObject = new Action<bool>(x => { if (x) addPinNumber(9); else checkPin(); });
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["0"].UserObject = new Action<bool>(x => { if (x) addPinNumber(0); else checkPin(); });
                panel.SmartObjects[_params.KeyPadSmartObjectId].BooleanOutput["Misc_2"].UserObject = new Action<bool>(x =>
                {
                    if (x)
                    {
                        if (_currentPinIndex > 0)
                        {
                            _currentPinValue[_currentPinIndex] = 0;
                            _currentPinIndex--;
                            AnalogJoinSet(_params.PinJoins[_currentPinIndex], PinOff);
                        }
                    }
                });
                panel.SmartObjects[_params.KeyPadSmartObjectId].SigChange += new SmartObjectSigChangeEventHandler(EventHandlers.ActionEventHandler);
            }
        }

        private void addPinNumber(ushort number)
        {
            _currentPinValue[_currentPinIndex] = number;
            AnalogJoinSet(_params.PinJoins[_currentPinIndex], PinOn);
            _currentPinIndex++;
            authError = false;
        }

        private void checkPin()
        {
            if (_currentPinIndex >= _currentPinValue.Count)
            {
                _currentPinIndex = 0;
                foreach (uint pinJoin in _params.PinJoins)
                    AnalogJoinSet(pinJoin, PinOff);
                ushort pin = (ushort)(_currentPinValue[0] * 1000 + _currentPinValue[1] * 100 + _currentPinValue[2] * 10 + _currentPinValue[3]);
                if (Authenticate != null)
                    authError = !Authenticate(pin);
            }
        }

        private bool authError
        {
            set
            {
                BooleanJoinSet(_params.AuthErrorJoin, value);
                if (value)
                    _authShowErrorTimer.Reset(3000);
                else
                    _authShowErrorTimer.Stop();
            }
        }

        private void hideAuthError(Object sender)
        {
            BooleanJoinSet(_params.AuthErrorJoin, false);
        }
    }
}