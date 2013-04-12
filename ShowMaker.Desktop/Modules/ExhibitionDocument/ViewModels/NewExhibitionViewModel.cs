using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using OpenRcp;
using ShowMaker.Desktop.Domain;

namespace ShowMaker.Desktop.Modules.ExhibitionDocument.ViewModels
{
    [Export(typeof(NewExhibitionViewModel))]
    public class NewExhibitionViewModel : Screen
    {
        #region View Data

        private string exhibitionDescription;
        public string ExhibitionDescription 
        { 
            get
            {
                return exhibitionDescription;
            }
            set
            {
                exhibitionDescription = value;
                NotifyOfPropertyChange(() => ExhibitionDescription);
            }
        }

        private string serverIp = "255.255.255.0";
        public string SeverIp 
        {
            get
            {
                return serverIp;
            }
            set
            {
                serverIp = value;
                NotifyOfPropertyChange(() => SeverIp);
            }
        }

        private string serverPort = "9999";
        public string SeverPort
        {
            get
            {
                return serverPort;
            }
            set
            {
                serverPort = value;
                NotifyOfPropertyChange(() => SeverPort);
            }
        }

        public Exhibition NewExhibition { get; set; }

        #endregion

        #region Override Screen Method

        public override string DisplayName
        {
            get
            {
                return "新建展示会";
            }
            set
            {
                base.DisplayName = value;
            }
        }

        #endregion


        #region Interaction

        /*
        public bool CanOnNewExhibition()
        {
            return string.IsNullOrEmpty(ExhibitionDescription) ? false : true;
        }
        */

        public void OnNewExhibition()
        {
            NewExhibition = new Exhibition();
            NewExhibition.Description = ExhibitionDescription;
            Property serverIp = new Property();
            serverIp.Name = "server.ip";
            serverIp.Value = SeverIp;
            Property serverPort = new Property();
            serverPort.Name = "server.port";
            serverPort.Value = SeverPort;
            NewExhibition.PropertyItems.Add(serverIp);
            NewExhibition.PropertyItems.Add(serverPort);
            
            TryClose();
        }

        #endregion
    }
}
