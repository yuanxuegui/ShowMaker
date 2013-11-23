using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using OpenRcp;
using ShowMaker.Desktop.Models.Domain;
using ShowMaker.Desktop.Models.Util;
using System.Reflection;
using System.IO;

namespace ShowMaker.Desktop.Modules.ExhibitionDocument.ViewModels
{
    [Export(typeof(NewExhibitionViewModel))]
    public class NewExhibitionViewModel : DisplayBase
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
                NotifyOfPropertyChange(() => CanOnNewExhibition);
            }
        }

        private string serverId = "1";
        public string SeverId
        {
            get
            {
                return serverId;
            }
            set
            {
                serverId = value;
                NotifyOfPropertyChange(() => serverId);
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

        #region Override DisplayBase Method

        public override string Name
        {
            get
            {
                return ExhibitionDocumentModule.MENU_FILE_NEW_SHOW;
            }
        }

        #endregion


        #region Interaction

        public bool CanOnNewExhibition
        {
            get { return string.IsNullOrEmpty(ExhibitionDescription) ? false : true; }
        }

        public void OnNewExhibition()
        {
            string exhibitionTemplate = Assembly.GetExecutingAssembly().Location + @"\..\" + Constants.EXHIBITION_TEMPLATE_FILE;
            if (File.Exists(exhibitionTemplate))
            {
                NewExhibition = (Exhibition)XmlSerializerUtil.LoadXml(typeof(Exhibition), exhibitionTemplate);
            }
            else
            {
                NewExhibition = new Exhibition();
            }
            
            NewExhibition.Description = ExhibitionDescription;
            NewExhibition.SetPropertyValue(Constants.SERVER_ID_KEY, SeverId);
            NewExhibition.SetPropertyValue(Constants.SERVER_IP_KEY, SeverIp);
            NewExhibition.SetPropertyValue(Constants.SERVER_PORT_KEY, SeverPort);
            
            TryClose();
        }

        #endregion
    }
}
