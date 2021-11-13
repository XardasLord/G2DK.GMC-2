using GothicModComposer.Core.Models.Interfaces;

namespace GothicModComposer.Core.Models.Profiles
{
    public class CommandsConditions : ICommandsConditions
    {
        public bool UpdateDialoguesStepRequired { get; set; }
        public bool ExecuteGothicStepRequired { get; set; }
    }
}