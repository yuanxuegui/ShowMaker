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
    [Export(typeof(NewOperationViewModel))]
    public class NewOperationViewModel : Screen
    {
        #region View Data

        private string operationName;
        public string OperationName 
        { 
            get
            {
                return operationName;
            }
            set
            {
                operationName = value;
                NotifyOfPropertyChange(() => OperationName);
            }
        }

        public Operation NewOperation { get; set; }

        #endregion

        #region Override Screen Method

        public override string DisplayName
        {
            get
            {
                return "新建操作";
            }
            set
            {
                base.DisplayName = value;
            }
        }

        #endregion


        #region Interaction

        /*
        public bool CanOnNewOperation()
        {
            return string.IsNullOrEmpty(OperationName) ? false : true;
        }
        */

        public void OnNewDevice()
        {
            NewOperation = new Operation();
            NewOperation.Name = OperationName;
            
            TryClose();
        }

        #endregion
    }
}
