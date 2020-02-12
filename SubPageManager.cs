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
        internal BasicTriList _panel;
        List<SubPage> _subPages = new List<SubPage>();

        #region Constructors

        public SubPageManager(BasicTriList panel)
        {
            _panel = panel;

            _panel.SigChange += new SigEventHandler(_panel_SigChange);
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
                subPage.Manager = this;
            }
        }

        public void Add(SubPage subPage)
        {
            subPage.Manager = this;
            _subPages.Add(subPage);
        }

        internal void MakeVisible(SubPage subPage)
        {
            _panel.BooleanInput[subPage.VisibilityJoin].BoolValue = true;
        }

        internal void Hide(SubPage subPage)
        {
            _panel.BooleanInput[subPage.VisibilityJoin].BoolValue = false;
        }

        #endregion Constructors

        #region Event Handlers

        void _panel_SigChange(BasicTriList currentDevice, SigEventArgs args)
        {

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