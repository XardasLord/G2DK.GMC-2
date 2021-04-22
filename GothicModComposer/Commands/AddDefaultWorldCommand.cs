using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands
{
	public class AddDefaultWorldCommand : ICommand
	{
		public string CommandName => "Add default world";

		private readonly IProfile _profile;

		public AddDefaultWorldCommand(IProfile profile )
			=> _profile = profile;

		public void Execute()
		{
			if (string.IsNullOrWhiteSpace(_profile.DefaultWorld))
			{
				Logger.Warn("'DefaultWorld' value in configuration is missing.");
				return;
			}

			AddDefaultWorld();
		}

		public void Undo() 
			=> Logger.Info("No need to undo this action since it didn't execute anything related to the Gothic system.", true);

		private void AddDefaultWorld()
		{
			_profile.GothicArguments.AddArgument_3D(_profile.DefaultWorld);
			Logger.Info($"Added --3D:{_profile.DefaultWorld} to Gothic arguments.", true);
		}
	}
}
