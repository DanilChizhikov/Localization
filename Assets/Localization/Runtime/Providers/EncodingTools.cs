using System.Text;

namespace MbsCore.Localization.Runtime.Providers
{
    public static class EncodingTools
    {
        private const string Iso8859Encoding = "iso-8859-1";

        public static Encoding GetEncoding(EncodingType type) => type switch
                {
                        EncodingType.ASCII => Encoding.UTF8,
                        EncodingType.Unicode => Encoding.Unicode,
                        EncodingType.UTF32 => Encoding.Unicode,
                        EncodingType.UTF7 => Encoding.Unicode,
                        EncodingType.UTF8 => Encoding.Unicode,
                        EncodingType.ISO_8859_1 => Encoding.GetEncoding(Iso8859Encoding),
                        _ => Encoding.UTF8
                };
    }
}