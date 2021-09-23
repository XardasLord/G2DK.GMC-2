using System.Collections.Generic;

namespace GothicModComposer.Models.VdfFiles
{
    public static class VdfFileHelper
    {
	    /// <summary>
	    ///     This regex was created to extract VDF files Filename, Extension and if it's Disabled
	    ///     <Filenmame />.<Extension />.<Disabled />
	    /// </summary>
	    public const string VdfFileNameRegex =
            @"(?<FileName>[\w\W]+?)[.](?<VdfExtension>vdf)(?:(?<Disabled>[.][\w\W]+)?)";

        private static readonly List<string> BaseVdfFilesUpper = BaseVdfFiles.ConvertAll(item => item.ToUpper());

        public static List<string> BaseVdfFiles => new()
        {
            "Anims", "Anims_Addon", "Meshes", "Meshes_Addon", "Sounds", "Sounds_Addon", "Sounds_Bird_01",
            "Speech_Addon", "Speech1", "Speech2", "Textures", "Textures_Addon", "Worlds", "Worlds_Addon"
        };

        public static bool IsBaseVdf(string vdfName)
            => BaseVdfFilesUpper.Contains(vdfName.ToUpper());
    }
}