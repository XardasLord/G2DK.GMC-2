namespace GothicModComposer.Models.Interfaces
{
    public interface ICommandsConditions
    {
        bool UpdateDialoguesStepRequired { get; set; }
        bool ExecuteGothicStepRequired { get; set; }
    }
}