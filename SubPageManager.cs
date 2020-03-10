using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;

namespace Daniels.UI
{
    public class SubPageManager: IEnumerable<SubPage>
    {
        List<SubPage> _subPages = new List<SubPage>();

        #region Constructors

        public SubPageManager(BasicTriList panel)
        {
            Panel = panel;

            Panel.SigChange += new SigEventHandler(_panel_SigChange);
        }

        public SubPageManager(BasicTriList panel, SubPage subPage)
            : this(panel)
        {
            this.Add(subPage);
        }

        public SubPageManager(BasicTriList panel, List<SubPage> subPages)
            : this(panel)
        {
            _subPages = subPages;
            foreach (var subPage in _subPages)
            {
                subPage.Panel = Panel;
                subPage.VisibilityChange = new Func<SubPage, bool>(subPage_VisibilityChange);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.GetType().Name + "\r\n");
            sb.AppendLine("\tPanel: " + Panel.Description);
            foreach (var subPage in _subPages)
            {
                sb.AppendFormat("\t\t{0}: {1}\t {2}\r\n", subPage.Id, subPage.Name, subPage.Visible);
            }

            return sb.ToString();
        }

        public BasicTriList Panel
        {
            get;
            protected set;
        }

        public void Add(SubPage subPage)
        {
            subPage.Panel = Panel;
            _subPages.Add(subPage);
        }

        /*
        internal void MakeVisible1(SubPage subPage)
        {
            Panel.BooleanInput[subPage.VisibilityJoin].BoolValue = true;
        }

        internal void Hide1(SubPage subPage)
        {
            Panel.BooleanInput[subPage.VisibilityJoin].BoolValue = false;
        }
        */
        #endregion Constructors

        #region Event Handlers

        void _panel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {

        }

        /// <summary>
        /// subPage_VisibilityChange: Event Handler that permit to allow or block Visibility change 
        /// </summary>
        /// <param name="args">SubPage that is asking permission to change visibility</param>
        protected virtual bool subPage_VisibilityChange(SubPage arg)
        {
            return true;
        }

        #endregion Event Handlers

        #region Indexes

        public SubPage this[int index]
        {
            get
            {
                return _subPages[index];
            }
        }
        public SubPage this[uint visibilityJoin]
        {
            get
            {
                foreach (var subPage in _subPages)
                {
                    if (subPage.VisibilityJoin == visibilityJoin)
                        return subPage;
                }
                throw new IndexOutOfRangeException("visibilityJoin");
            }
        }

        #endregion Indexes

        #region IEnumerable<SubPage>

        public IEnumerator<SubPage> GetEnumerator()
        {
            return _subPages.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion IEnumerable<SubPage>

    }
}