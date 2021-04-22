using GothicModComposer.Models.Interfaces;

namespace GothicModComposer.Models.Profiles
{
    public class CommandsConditions : ICommandsConditions
    {
        public bool UpdateDialoguesStepRequired { get; set; }
        public bool ExecuteGothicStepRequired { get; set; }
    }
}