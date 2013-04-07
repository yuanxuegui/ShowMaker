using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenRcp;
using ShowMaker.Desktop.Modules.ExhibitionDocument.Views;
using ICSharpCode.AvalonEdit.Highlighting;
using Caliburn.Micro;
using ShowMaker.Desktop.Modules.Storyboard.ViewModels;
using ShowMaker.Desktop.Parser;
using ShowMaker.Desktop.Domain;

namespace ShowMaker.Desktop.Modules.ExhibitionDocument.ViewModels
{
    public class ExhibitionEditorViewModel : Document, IPropertySelectable
    {
        private string _originalText;
        private string _path;
        private string _fileName;
        private bool _isDirty;

        private Exhibition contentObject;

        public string FirstName
        {
            get
            {
                return "Hello";
            }
            set { }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (value == _isDirty)
                    return;

                _isDirty = value;
                NotifyOfPropertyChange(() => IsDirty);
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        #region Override Document Methods

        public override string DisplayName
        {
            get
            {
                if (IsDirty)
                    return _fileName + "*";
                return _fileName;
            }
        }
        
        protected override void OnActivate()
        {
            base.OnActivate();
            if (ShowMakerDesktopModule.SHOW_FILE_EXTENSION.Equals(Path.GetExtension(_path)))
            {
                IoC.Get<StoryboardViewModel>().SelectedExhibition = contentObject;
            }
        }

        public override void CanClose(System.Action<bool> callback)
        {
            if (IsDirty)
            {
                System.Windows.MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("是要保存修改吗？", "Exhibition Document", System.Windows.MessageBoxButton.YesNoCancel);
                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    // 保存修改后关闭
                    // TODO. 保存文件
                    callback(true);
                }
                else if (result == System.Windows.MessageBoxResult.No)
                {
                    // 不保存修改，直接关闭
                    callback(true);
                }
                else if(result == System.Windows.MessageBoxResult.Cancel)
                {
                    // 不关闭
                    callback(false);
                }
            }
            else
                callback(true);
        }

        protected override void OnViewLoaded(object view)
        {
           
            using (var stream = File.OpenText(_path))
            {
                _originalText = stream.ReadToEnd();
            }
  
            var editor = (ExhibitionEditorView)view;
            string ext = Path.GetExtension(_path);
            editor.textEditor.Text = _originalText;
            editor.textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(_path));
            
            editor.textEditor.TextChanged += delegate
            {
                IsDirty = string.Compare(_originalText, editor.textEditor.Text) != 0;
            };

            // 解析.show文件
            if (ShowMakerDesktopModule.SHOW_FILE_EXTENSION.Equals(ext))
            {
                IExhibitionParser parser = new XmlSerializerExhibitionParser();
                contentObject = parser.Parse(_path);
                IoC.Get<StoryboardViewModel>().SelectedExhibition = contentObject;
            }
        }

        #endregion

        public void Open(string path)
        {
            _path = path;
            _fileName = Path.GetFileName(_path);
        }
        
        public override bool Equals(object obj)
        {
            var other = obj as ExhibitionEditorViewModel;
            return other != null && string.Compare(_path, other._path) == 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object SelectProperty()
        {
            return this;
        }
    }
}
