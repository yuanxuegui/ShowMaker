using System.IO;

namespace OpenRcp
{
	public interface IOutput : ITool
	{
		TextWriter Writer { get; }
		void AppendLine(string text);
		void Append(string text);
		void Clear();
	}
}