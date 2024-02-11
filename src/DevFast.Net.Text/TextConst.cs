using System.Text;

namespace DevFast.Net.Text
{
    /// <summary>
    /// Static class holding constant or fixed values for text processing.
    /// </summary>
    public static class TextConst
    {
        /// <summary>
        /// Instance of <see cref="Encoding.UTF8"/> encoding which will not emit BOM.
        /// </summary>
        public static readonly Encoding Utf8NoBom = new UTF8Encoding(false);
    }
}