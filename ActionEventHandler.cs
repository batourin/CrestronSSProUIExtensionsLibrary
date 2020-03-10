using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;

namespace Daniels.UI
{
    public static class EventHandlers
    {
        /// <summary>
        /// AuthenticatedSubPageManager constructor 
        /// </summary>
        /// <param name="currentDevice">Device originating the event</param>
        /// <param name="args">Event arguments</param>
        public static void ActionEventHandler(GenericBase currentDevice, SigEventArgs args)
        {
            var sig = args.Sig;
            var uo = sig.UserObject;

            if (uo is Action<bool>)                             // If the userobject for this signal with boolean
            {
                (uo as Action<bool>)(sig.BoolValue);            // cast this signal's userobject as delegate Action<bool>
                // passing one parm - the value of the bool
            }
            else if (uo is Action<ushort>)
            {
                (uo as Action<ushort>)(sig.UShortValue);
            }
            else if (uo is Action<string>)
            {
                (uo as Action<string>)(sig.StringValue);
            }
        }
    }

}