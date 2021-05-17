using System.Collections.Generic;
using System.Linq;
using GothicModComposer.Commands.ExecutedCommandActions.Interfaces;
using GothicModComposer.Utils;

namespace GothicModComposer.Commands.ExecutedCommandActions
{
	public static class CommandActionExtensions
	{
		public static void Undo(this Stack<ICommandActionIO> executedActions)
		{
			if (!executedActions.Any())
			{
				Logger.Info("There is nothing to undo, because no actions were executed.", true); // TODO: Introduce something like NoAction()
				return;
			}

			while (executedActions.Count > 0)
			{
				var executedAction = executedActions.Pop();
				executedAction?.Undo();
			}
		}
		
		public static void Undo(this Stack<ICommandActionVDF> executedActions)
		{
			if (!executedActions.Any())
			{
				Logger.Info("There is nothing to undo, because no actions were executed.", true);
				return;
			}

			while (executedActions.Count > 0)
			{
				var executedAction = executedActions.Pop();
				executedAction?.Undo();
			}
		}
		
		public static void Undo(this Stack<ICommandActionVideoBik> executedActions)
		{
			if (!executedActions.Any())
			{
				Logger.Info("There is nothing to undo, because no actions were executed.", true);
				return;
			}

			while (executedActions.Count > 0)
			{
				var executedAction = executedActions.Pop();
				executedAction?.Undo();
			}
		}
	}
}