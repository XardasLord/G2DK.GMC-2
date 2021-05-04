﻿using GothicModComposer.Models.Interfaces;

namespace GothicModComposer.Models.Configurations
{
    public class GothicArgumentsConfiguration : IGothicArgumentsConfiguration
    {
        public bool IsWindowMode { get; set; }
        public bool IsDevMode { get; set; }
        public bool IsMusicDisabled { get; set; }
        public bool IsSoundDisabled { get; set; }
        public bool IsReparseScript { get; set; }
    }
}