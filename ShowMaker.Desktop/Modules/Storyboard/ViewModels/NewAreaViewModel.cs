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
    public class NewAreaViewModel : DisplayBase
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
                NotifyOfPropertyChange(() => CanOnNewArea);
            }
        }

        public Area NewArea { get; set; }

        #endregion

        #region Override DisplayBase Method

        public override string Name
        {
            get
            {
                return StoryboardModule.STORYBOARD_NEW_AREA;
            }
        }

        #endregion


        #region Interaction

        
        public bool CanOnNewArea
        {
            get { return string.IsNullOrEmpty(AreaName) ? false : true; }
        }

        public void OnNewArea()
        {
            NewArea = new Area();
            NewArea.Name = AreaName;
            
            TryClose();
        }

        #endregion
    }
}
