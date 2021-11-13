using System.Text;

namespace GothicModComposer.Core.Utils.IOHelpers
{
    public static class EncodingHelper
    {
        public static Encoding GothicEncoding => CodePagesEncodingProvider.Instance.GetEncoding(1250);
    }
}