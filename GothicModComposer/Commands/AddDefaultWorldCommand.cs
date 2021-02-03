using System.Collections.Generic;
using System.Linq;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Models.Profiles;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands
{
	public class AddDefaultWorldCommand : ICommand
	{
		public string CommandName => "Add default world";

		private readonly IProfile _profile;
		private static readonly Stack<ICommandActionIO> ExecutedActions = new();

		public AddDefaultWorldCommand(IProfile profile )
			=> _profile = profile;

		public void Execute()
		{
			if (string.IsNullOrWhiteSpace(_profile.DefaultWorld))
			{
				Logger.Warn("'DefaultWorld' value in configuration is missing.");
				return;
			}

			_profile.GothicArguments._3D(_profile.DefaultWorld);
			Logger.Info($"Added --3D:{_profile.DefaultWorld} to Gothic arguments.");
		}

		public void Undo()
		{
			if (!ExecutedActions.Any())
			{
				Logger.Info("There is nothing to undo, because no actions were executed.");
				return;
			}

			while (ExecutedActions.Count > 0)
			{
				var executedAction = ExecutedActions.Pop();
				executedAction?.Undo();
			}
		}
	}
}
