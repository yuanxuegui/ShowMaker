﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Caliburn.Micro;
using ICSharpCode.AvalonEdit.Highlighting;
using OpenRcp;
using ShowMaker.Desktop.Modules.ExhibitionDocument.Views;
using ShowMaker.Desktop.Models.Domain;
using ShowMaker.Desktop.Models.Parser;
using ShowMaker.Desktop.Models.Util;
using System.Runtime.InteropServices;

namespace ShowMaker.Desktop.Modules.ExhibitionDocument.ViewModels
{
    /// <summary>
    /// 展会文件文档编辑器的ViewModel
    /// </summary>
    public class ExhibitionEditorViewModel : Document, IPropertySelectable, IHandle<ExhibitionContentChangedMessage>
    {
        private string _originalText;
        private string _path;
        private string _fileName;
        private bool _isDirty;

        private Exhibition contentObject;
        private ExhibitionEditorView editor;

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

        private void selectExhibiton(Exhibition content)
        {
            // 发送故事版发送更换展区消息
            IoC.Get<IEventAggregator>().Publish(new SelectedExhibitionChangedMessage()
            {
                SelectedExhibition = content
            });
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (ExhibitionDocumentModule.SHOW_FILE_EXTENSION.Equals(Path.GetExtension(_path)))
            {
                selectExhibiton(contentObject);
            }
        }

        public override void CanClose(System.Action<bool> callback)
        {
            if (IsDirty)
            {
                System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("是要保存修改吗？", "Exhibition Document", System.Windows.MessageBoxButton.YesNoCancel);
                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    // 保存修改后关闭
                    Save();
                    selectExhibiton(null);
                    callback(true);
                }
                else if (result == System.Windows.MessageBoxResult.No)
                {
                    selectExhibiton(null);
                    // 不保存修改，直接关闭
                    callback(true);
                }
                else if (result == System.Windows.MessageBoxResult.Cancel)
                {
                    // 不关闭
                    callback(false);
                }
            }
            else
            {
                selectExhibiton(null);
                callback(true);
            }
        }

        protected override void OnViewLoaded(object view)
        {
            string showDecryptFile = ShowFileEncryptDecrypt.LoadShowFile(_path);

            using (var stream = File.OpenText(showDecryptFile))
            {
                _originalText = stream.ReadToEnd();
            }
  
            editor = (ExhibitionEditorView)view;
            string ext = Path.GetExtension(_path);
            editor.textEditor.Text = _originalText;
            editor.textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(ExhibitionDocumentModule.SHOW_FILE_EXTENSION.Equals(ext) ? ".xml" : Path.GetExtension(_path));
            
            editor.textEditor.TextChanged += delegate
            {
                IsDirty = string.Compare(_originalText, editor.textEditor.Text) != 0;
            };

            // 解析.show文件
            if (ExhibitionDocumentModule.SHOW_FILE_EXTENSION.Equals(ext))
            {
                IExhibitionParser parser = new XmlSerializerExhibitionParser();
                contentObject = parser.Parse(showDecryptFile);
                initExhibition(contentObject);
                selectExhibiton(contentObject);
            }

            File.Delete(showDecryptFile);

            IoC.Get<IEventAggregator>().Subscribe(this);

            CommandBinding saveCommandBinding = new CommandBinding(ApplicationCommands.Save, SaveHandler, CanSaveHandler);
            editor.CommandBindings.Add(saveCommandBinding);
        }

        private void initExhibition(Exhibition exh)
        {
            foreach (Area a in exh.AreaItems)
            {
                a.SetParent(exh);
                foreach (Device dev in a.DeviceItems)
                {
                    dev.SetParent(a);
                    foreach (Operation op in dev.OperationItems)
                    {
                        op.SetParent(dev);
                    }
                }
            }
        }

        #endregion

        public void SaveHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
            IsDirty = false;
        }

        public void CanSaveHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsDirty;
        }

        public void Open(string path)
        {
            _path = path;
            _fileName = Path.GetFileName(_path);
        }

        public void Save()
        {
            using (FileStream fs = new FileStream(_path, FileMode.Truncate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(editor.textEditor.Text);
                }
            }

            ShowFileEncryptDecrypt.SaveShowFile(_path);
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

        public void Handle(ExhibitionContentChangedMessage message)
        {
            if (message.Content == this.contentObject)
            {
                editor.textEditor.Text = XmlSerializerUtil.SaveXmlToString(contentObject);
                this.IsDirty = true;
            }
        }
    }
}
