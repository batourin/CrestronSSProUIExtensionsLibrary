using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace Daniels.UI
{
    /// <summary>
    /// Sig extensions for UserObject based actions
    /// </summary>
    public static class SigActionExtensions
    {
        /// <summary>
        /// Set Press action 
        /// </summary>
        /// <param name="sig">The BoolOutputSig to attach the Action to</param>
        /// <param name="pressAction">Action when button pressed</param>
        public static void Press(this BoolOutputSig sig, Action pressAction)
        {
            sig.UserObject = new Action<bool>(x => {if(x) pressAction();});
        }

        /// <summary>
        /// Set Release action 
        /// </summary>
        /// <param name="sig">The BoolOutputSig to attach the Action to</param>
        /// <param name="releaseAction">Action when button released</param>
        public static void Release(this BoolOutputSig sig, Action releaseAction)
        {
            sig.UserObject = new Action<bool>(x => { if (!x) releaseAction(); });
        }

        /// <summary>
        /// Set Release action 
        /// </summary>
        /// <param name="sig">The BoolOutputSig to attach the Action to</param>
        /// <param name="pressAction">Action when button is pressed</param>
        /// <param name="pressAction">Action when button is released</param>
        public static void PressRelease(this BoolOutputSig sig,  Action pressAction, Action releaseAction)
        {
            sig.UserObject = new Action<bool>(x => { if (x) pressAction(); else releaseAction(); });
        }

        /// <summary>
        /// Sets an action to press and release, press and hold and press and relase after hold
        /// Code taken from PaperDash Essentialc Core, little refactored for my own clarity
        /// </summary>
        /// <param name="sig">The BoolOutputSig to attach the Action to</param>
        /// <param name="pressTime">time in milliseconds to react on press and hold events</param>
        /// <param name="pressAction">Action when button was pressed and released before press and hold timer expired</param>
        /// <param name="holdAction">Action when button pressed, timer expired but not yet released.</param>
        /// <param name="holdAction">Action when button pressed and released after hold time.</param>
        /// <returns>BoolOutputSig, so it can be chained</returns>
        public static void PressHoldRelease(this BoolOutputSig sig, uint pressTime, Action pressAction, Action holdAction, Action holdReleasedAction)
        {
            CTimer holdTimer = null;
            bool holdFlag = false; // when true indicates press and hold time passed

            Action<bool> action = (press =>
            {
                if (press)
                {
                    holdFlag = false;
                    holdTimer = new CTimer(o =>
                    {
                        // if still held and there's an action
                        if (sig.BoolValue && holdAction != null)
                        {
                            holdFlag = true;
                            holdAction();
                        }
                    }, pressTime);
                }
                else if (!press && !holdFlag) // released, no hold, i.e. press and release before timer expires
                {
                    holdTimer.Stop();
                    if (pressAction != null)
                        pressAction();
                }
                else // !press && holdFlag // released after held
                {
                    holdTimer.Stop();
                    if (holdReleasedAction != null)
                        holdReleasedAction();
                }
            });

            sig.UserObject = action;
            //return sig;
        }

    }
}