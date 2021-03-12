﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GothicModComposer.Utils.IOHelpers
{
	public static class CslWriter
    {
        public static string GenerateContent(List<Tuple<string, string>> dialogues)
        {
            var builder = new StringBuilder();
            AppendHeader(builder, dialogues.Count);
            AppendDialoguePopups(builder, dialogues);
            AppendEnd(builder);
            return builder.ToString();
        }

        private static void AppendHeader(StringBuilder builder, int objectsCount)
        {
            builder
                .AppendLine($"ZenGin Archive")
                .AppendLine($"ver 1")
                .AppendLine($"zCArchiverGeneric")
                .AppendLine($"ASCII")
                .AppendLine($"saveGame 0")
                .AppendLine($"date {DateTime.Now}")
                .AppendLine($"user GMC")
                .AppendLine($"END")
                .AppendLine($"objects {objectsCount * 3 + 1}")
                .AppendLine($"END")
                .AppendLine($"")
                .AppendLine($"[% zCCSLib 0 0]")
                .AppendLine($"\tNumOfItems=int:{objectsCount}");
        }

        private static void AppendDialoguePopups(StringBuilder builder, List<Tuple<string, string>> dialogues)
        {
            var i = 0;
            dialogues.ForEach(item => AppendOneDialogueBlock(builder, item, i++ * 3 + 1));
        }

        private static void AppendOneDialogueBlock(StringBuilder builder, Tuple<string, string> tuple, int number)
        {
            builder
                .AppendLine($"\t[% zCCSBlock 0 {number}]")
                .AppendLine($"\t\tblockName=string:{tuple.Item1}")
                .AppendLine($"\t\tnumOfBlocks=int:1")
                .AppendLine($"\t\tsubBlock0=float:0")
                .AppendLine($"\t\t[% zCCSAtomicBlock 0 {number + 1}]")
                .AppendLine($"\t\t\t[% oCMsgConversation:oCNpcMessage:zCEventMessage 0 {number + 2}]")
                .AppendLine($"\t\t\t\tsubType=enum:0")
                .AppendLine($"\t\t\t\ttext=string:{tuple.Item2}")
                .AppendLine($"\t\t\t\tname=string:{tuple.Item1.ToUpper()}.WAV")
                .AppendLine($"\t\t\t[]")
                .AppendLine($"\t\t[]")
                .AppendLine($"\t[]");
        }

        private static void AppendEnd(StringBuilder builder)
        {
            builder.AppendLine("[]");
        }
    }
}