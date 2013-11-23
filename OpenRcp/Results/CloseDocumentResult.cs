using System;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;

namespace OpenRcp
{
	public class CloseDocumentResult : CloseResultBase<IDocument>
	{
		private readonly IDocument _editor;
		private readonly Type _editorType;
		private readonly string _path;

		[Import]
		private IShell _shell = null;

		public CloseDocumentResult(IDocument editor)
		{
			_editor = editor;
		}

		public CloseDocumentResult(string path)
		{
			_path = path;
		}

        public CloseDocumentResult(Type editorType)
		{
			_editorType = editorType;
		}

		public override void Execute(ActionExecutionContext context)
		{
			var editor = _editor ??
				(string.IsNullOrEmpty(_path)
					? (IDocument)IoC.GetInstance(_editorType, null)
					: GetEditor(_path));

			if (editor == null)
			{
				OnCompleted(null);
				return;
			}

			if (_setData != null)
				_setData(editor);

			if (_onConfigure != null)
				_onConfigure(editor);

			editor.Deactivated += (s, e) =>
			{

				if (!e.WasClosed)
					return;

				if (_onShutDown != null)
					_onShutDown(editor);

				OnCompleted(null);

			};

			_shell.CloseDocument(editor);
		}

		private static IDocument GetEditor(string path)
		{
			return IoC.GetAllInstances(typeof(IEditorProvider))
				.Cast<IEditorProvider>()
				.Where(provider => provider.Handles(path))
				.Select(provider => provider.Create(path))
				.FirstOrDefault();
		}
	}
}