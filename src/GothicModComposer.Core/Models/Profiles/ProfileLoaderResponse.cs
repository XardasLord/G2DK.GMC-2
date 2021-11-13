using System.Collections.Generic;
using GothicModComposer.Core.Commands;

namespace GothicModComposer.Core.Models.Profiles
{
    public class ProfileLoaderResponse
    {
        public IProfile Profile { get; set; }
        public List<ICommand> Commands { get; set; }
    }
}