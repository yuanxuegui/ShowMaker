using Microsoft.Win32;

namespace OpenRcp
{
	public static class ResultsHelper
	{
		public static ShowCommonDialogResult ShowDialog(CommonDialog commonDialog)
		{
			return new ShowCommonDialogResult(commonDialog);
		}

		public static ShowToolResult<TTool> ShowTool<TTool>()
			where TTool : ITool
		{
			return new ShowToolResult<TTool>();
		}

		public static ShowToolResult<TTool> ShowTool<TTool>(TTool tool)
			where TTool : ITool
		{
			return new ShowToolResult<TTool>(tool);
		}

        public static HideToolResult<TTool> HideTool<TTool>()
            where TTool : ITool
        {
            return new HideToolResult<TTool>();
        }

        public static HideToolResult<TTool> HideTool<TTool>(TTool tool)
            where TTool : ITool
        {
            return new HideToolResult<TTool>(tool);
        }

		public static OpenDocumentResult OpenDocument(IDocument document)
		{
			return new OpenDocumentResult(document);
		}

		public static OpenDocumentResult OpenDocument(string path)
		{
			return new OpenDocumentResult(path);
		}

		public static OpenDocumentResult OpenDocument<T>()
				where T : IDocument
		{
			return new OpenDocumentResult(typeof(T));
		}

        public static CloseDocumentResult CloseDocument(IDocument document)
        {
            return new CloseDocumentResult(document);
        }

        public static CloseDocumentResult CloseDocument(string path)
        {
            return new CloseDocumentResult(path);
        }

        public static CloseDocumentResult CloseDocument<T>()
                where T : IDocument
        {
            return new CloseDocumentResult(typeof(T));
        }

        public static ChangeThemeResult ChangeTheme(string themeName)
        {
            return new ChangeThemeResult(themeName);
        }
	}
}