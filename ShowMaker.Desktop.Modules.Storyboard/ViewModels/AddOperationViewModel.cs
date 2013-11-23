using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using OpenRcp;
using ShowMaker.Desktop.Models.Domain;

namespace ShowMaker.Desktop.Modules.Storyboard.ViewModels
{
    [Export(typeof(AddOperationViewModel))]
    public class AddOperationViewModel : DisplayBase
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
                NotifyOfPropertyChange(() => CanOnAddOperation);
            }
        }

        public Operation NewOperation { get; set; }

        #endregion

        #region Override DisplayBase Method

        public override string Name
        {
            get
            {
                return StoryboardModule.STORYBOARD_ADD_OPERATION;
            }
        }

        #endregion


        #region Interaction

        public bool CanOnAddOperation
        {
            get { return string.IsNullOrEmpty(OperationName) ? false : true; }
        }

        public void OnAddOperation()
        {
            NewOperation = new Operation();
            NewOperation.Name = OperationName;
            
            TryClose();
        }

        #endregion

        public void Clear()
        {
            NewOperation = null;
        }
    }
}
