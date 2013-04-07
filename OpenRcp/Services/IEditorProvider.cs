namespace OpenRcp
{
	public interface IEditorProvider
	{
		bool Handles(string path);
		IDocument Create(string path);
	}
}