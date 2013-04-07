using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using OpenRcp;
using ShowMaker.Desktop.Modules.ExhibitionDocument.ViewModels;

namespace ShowMaker.Desktop.Modules.ExhibitionDocument
{
    [Export(typeof(IEditorProvider))]
    public class ExhibitionEditorProvider : IEditorProvider
    {
        private readonly List<string> _extensions = new List<string>
        {
            ".xml",
            ".show" //ShowMaker文件格式
        };

        public bool Handles(string path)
        {
            var extension = Path.GetExtension(path);
            bool result = _extensions.Contains(extension);
            return result;
        }

        public IDocument Create(string path)
        {
            var editor = new ExhibitionEditorViewModel();
            editor.Open(path);
            return editor;
        }
    }
}
