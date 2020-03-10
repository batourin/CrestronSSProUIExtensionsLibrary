using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;


namespace Daniels.UI
{
    public enum AuthenticatedLevel
    {
        None,
        Level1,
        Level2,
    }

    public class AuthenticatedSubPageManager: SubPageManager
    {
        private PinLockSubPage _lockPage;
        private CTimer _inactivityTimer;
        private long _inactivityTime;

        public delegate AuthenticatedLevel AuthenticateCallback(ushort pin, out string user);
        private AuthenticateCallback _authenticateCallback;

        /// <summary>
        /// AuthenticatedSubPageManager constructor 
        /// </summary>
        /// <param name="authenticateCallback">Callback function to authenticateCallback</param>
        /// <param name="panel">The panel Auth manager has to link</param>
        /// <param name="lockPage">Instance of the PinLockSubPage class to collect auth info</param>
        /// <param name="inactivityTime">Amount of time of inactivity to automaticaly lock the panel, in milliseconds</param>
        public AuthenticatedSubPageManager(AuthenticateCallback authenticateCallback, BasicTriListWithSmartObject panel, PinLockSubPage lockPage, long inactivityTime)
            : this(authenticateCallback, panel, lockPage, inactivityTime, new List<SubPage>())
        {
        }

        /// <summary>
        /// AuthenticatedSubPageManager constructor 
        /// </summary>
        /// <param name="authenticateCallback">Callback function to authenticateCallback</param>
        /// <param name="panel">The panel Auth manager has to link</param>
        /// <param name="lockPage">Instance of the PinLockSubPage class to collect auth info</param>
        /// <param name="inactivityTime">Amount of time of inactivity to automaticaly lock the panel, in milliseconds</param>
        /// <param name="subPages">List of other subpages this SubPage manager shall manage</param>
        public AuthenticatedSubPageManager(AuthenticateCallback authenticateCallback, BasicTriListWithSmartObject panel, PinLockSubPage lockPage, long inactivityTime, List<SubPage> subPages)
            : base(panel, subPages)
        {
            _authenticateCallback = authenticateCallback;
            _lockPage = lockPage;
            _lockPage.Panel = panel;
            _lockPage.Authenticate = new Func<ushort, bool>(pageAuthenticateCallback);

            _inactivityTime = inactivityTime;
            _inactivityTimer = new CTimer(inactivityTimerCallback, Timeout.Infinite);

            AuthenticatedLevel = AuthenticatedLevel.None;

            panel.SigChange += new SigEventHandler(activityDetectionHandler);
            foreach (var kv in panel.SmartObjects)
                kv.Value.SigChange += new SmartObjectSigChangeEventHandler(activityDetectionHandler);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            sb.AppendLine("\tAuthenticatedLevel: " + AuthenticatedLevel);
            if (AuthenticatedLevel != AuthenticatedLevel.None)
                sb.AppendLine("\tUser: " + AuthenitcatedUser);
            sb.AppendLine("\tLock page:");
            sb.Append(_lockPage.ToString());

            return sb.ToString();
        }

        private AuthenticatedLevel _authenticatedLevel;
        public AuthenticatedLevel AuthenticatedLevel
        {
            get { return _authenticatedLevel; }
            private set 
            {
                _authenticatedLevel = value;
                switch (_authenticatedLevel)
                {
                    case AuthenticatedLevel.None:
                        AuthenitcatedUser = String.Empty;
                        _lockPage.Visible = true;
                        _inactivityTimer.Stop();
                        break;
                    case AuthenticatedLevel.Level1:
                    case AuthenticatedLevel.Level2:
                        _lockPage.Visible = false;
                        _inactivityTimer.Reset(_inactivityTime);
                        break;
                }
                OnAuthenticated(new AuthenticatedEventArgs(AuthenticatedLevel, AuthenitcatedUser));
            }
        }

        public string AuthenitcatedUser
        {
            get;
            private set;
        }

        public class AuthenticatedEventArgs : EventArgs
        {
            public readonly AuthenticatedLevel Level;
            public readonly string User;
            public AuthenticatedEventArgs(AuthenticatedLevel level, string user)
            {
                Level = level;
                User = user;
            }
        }
        public virtual event EventHandler<AuthenticatedEventArgs> Authenticated;
        protected virtual void OnAuthenticated(AuthenticatedEventArgs e)
        {
            EventHandler<AuthenticatedEventArgs> handler = Authenticated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private bool pageAuthenticateCallback(ushort pin)
        {
            string user;
            AuthenticatedLevel = _authenticateCallback(pin, out user);
            if(AuthenticatedLevel != AuthenticatedLevel.None)
            {
                AuthenitcatedUser = user;
                return true;
            }
            return false;
        }

        public void Lock()
        {
            AuthenticatedLevel = AuthenticatedLevel.None;
        }

        private void inactivityTimerCallback(Object sender)
        {
            AuthenticatedLevel = AuthenticatedLevel.None;
        }

        private void activityDetectionHandler(BasicTriList currentDevice, SigEventArgs args)
        {
            if (AuthenticatedLevel != AuthenticatedLevel.None && args.Sig.Type == eSigType.Bool && args.Sig.BoolValue && args.Sig.Number < 17000)
                _inactivityTimer.Reset(_inactivityTime);
        }

        private void activityDetectionHandler(GenericBase currentDevice, SmartObjectEventArgs args)
        {
            if (AuthenticatedLevel != AuthenticatedLevel.None && args.Sig.Type == eSigType.Bool && args.Sig.BoolValue)
                _inactivityTimer.Reset(_inactivityTime);
        }
    }
}