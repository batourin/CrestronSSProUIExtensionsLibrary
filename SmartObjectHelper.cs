using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace Daniels.UI
{
    public abstract class SmartObjectHelperParameters : BasicTriListParameters
    {
        //public uint Id;
        //public string Name;
        public uint VisibilityJoin;
    }

    public abstract class SmartObjectHelper
    {
        private readonly uint _visibilityJoin;
        internal SmartObject _smartObject;

        public SmartObjectHelper(SmartObject smartObject, SmartObjectHelperParameters smartObjectParams)
        {
            _smartObject = smartObject;
            _visibilityJoin = smartObjectParams.VisibilityJoin;
        }

        #region Properties

        public uint Id { get { return _smartObject.ID; } }
        //public SmartObject SmartObject { get { return _smartObject; } }

        #endregion Properties

    }
}