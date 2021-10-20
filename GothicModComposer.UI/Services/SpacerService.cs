using System.Diagnostics;
using System.IO;
using GothicModComposer.UI.Interfaces;

namespace GothicModComposer.UI.Services
{
    public class SpacerService : ISpacerService
    {
        private const string PathToSpacer = "System/Spacer2.exe";

        public bool SpacerExists(string gothicRootPath)
            => File.Exists(Path.Combine(gothicRootPath, PathToSpacer));

        public Process RunSpacer(string gothicRootPath)
        {
            if (!SpacerExists(gothicRootPath))
                return null;

            var spacerPath = Path.Combine(gothicRootPath, PathToSpacer);

            return Process.Start(spacerPath);
        }
    }
}