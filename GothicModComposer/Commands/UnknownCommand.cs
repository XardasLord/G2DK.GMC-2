﻿namespace GothicModComposer.Commands
{
    public class UnknownCommand : ICommand
    {
        public string CommandName => "Unknown";

        public void Execute()
        {
        }

        public void Undo()
        {
        }
    }
}