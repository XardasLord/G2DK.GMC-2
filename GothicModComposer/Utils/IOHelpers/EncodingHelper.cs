using System.Text;

namespace GothicModComposer.Utils.IOHelpers
{
	public static class EncodingHelper
	{
		public static Encoding GothicEncoding => CodePagesEncodingProvider.Instance.GetEncoding(1250);
	}
}