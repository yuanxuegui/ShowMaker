using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using OpenRcp;
using ShowMaker.Desktop.Domain;

namespace ShowMaker.Desktop.Modules.Storyboard.ViewModels
{
    [Export(typeof(NewAreaViewModel))]
    public class NewAreaViewModel : Screen
    {
        #region View Data

        private string areaName;
        public string AreaName 
        { 
            get
            {
                return areaName;
            }
            set
            {
                areaName = value;
                NotifyOfPropertyChange(() => AreaName);
            }
        }

        public Area NewArea { get; set; }

        #endregion

        #region Override Screen Method

        public override string DisplayName
        {
            get
            {
                return "新建展区";
            }
            set
            {
                base.DisplayName = value;
            }
        }

        #endregion


        #region Interaction

        /*
        public bool CanOnNewArea()
        {
            return string.IsNullOrEmpty(AreaName) ? false : true;
        }
        */

        public void OnNewArea()
        {
            NewArea = new Area();
            NewArea.Name = AreaName;
            
            TryClose();
        }

        #endregion
    }
}
