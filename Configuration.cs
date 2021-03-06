using Dalamud.Configuration;
using Dalamud.Plugin;
using ImGuiNET;
using System;
using System.Numerics;

namespace OopsAllLalafells {
    public class Configuration : IPluginConfiguration {
        [NonSerialized]
        private DalamudPluginInterface pluginInterface;

        public int Version { get; set; } = 1;
        public Race OtherRace { get; set; } = Race.LALAFELL;
        public Race SelfRace { get; set; } = Race.LALAFELL;
        public bool OtherChange { get; set; } = false;
        public bool SelfChange { get; set; } = false;

        public void Initialize(DalamudPluginInterface pluginInterface) {
            this.pluginInterface = pluginInterface;
        }

        public void Save() {
            this.pluginInterface.SavePluginConfig(this);
        }
    }
}
