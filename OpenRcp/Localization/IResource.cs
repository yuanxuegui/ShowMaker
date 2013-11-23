using System.Globalization;

namespace OpenRcp
{
    /// <summary>
    /// Denotes the interface to provide string resource.
    /// </summary>
    public interface IResource
    {
        string GetString(string name);
        CultureInfo CurrentCulture { set; }
    }
}
