using Dalamud.Plugin;
using ImGuiNET;
using Dalamud.Game.ClientState.Actors;
using System;
using System.Collections.Generic;
using System.Numerics;
using OopsAllLalafells.customizations;

namespace OopsAllLalafells {
    public class PluginUI {
        private static readonly string[] RACE_NAME = {
            "Hyur",
            "Elezen",
            "Lalafell",
            "Miqo'te",
            "Roegadyn",
            "Au Ra",
            "Hrothgar",
            "Viera"
        };
        private static readonly Array CUSTOMIZATIONS = Enum.GetValues(typeof(CustomizeIndex));

        private readonly DalamudPluginInterface pluginInterface;
        private readonly Plugin plugin;

        public readonly List<Customization> otherCustomizations = new List<Customization>();
        public readonly List<Customization> selfCustomizations = new List<Customization>();
        
        private Customization otherAdding = null;
        private Customization selfAdding = null;
        private Customization otherRemoving = null;
        private Customization selfRemoving = null;
        
        public PluginUI(Plugin plugin, DalamudPluginInterface pluginInterface) {
            this.plugin = plugin;
            this.pluginInterface = pluginInterface;
        }

        public void Draw() {
            if (!this.plugin.SettingsVisible) {
                return;
            }

            bool settingsVisible = this.plugin.SettingsVisible;
            bool uiOtherChange = this.plugin.config.OtherChange;
            bool uiSelfChange = this.plugin.config.SelfChange;
            ImGui.SetNextWindowSize(new Vector2(350, 400), ImGuiCond.FirstUseEver);
            ImGui.Begin("Oops, All Lalafells!", ref settingsVisible, ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse);

            bool changed = false;
            changed |= ShowChangeItems("Others", otherCustomizations, ref otherAdding, ref otherRemoving, ref uiOtherChange);
            ImGui.Spacing();
            changed |= ShowChangeItems("Self", selfCustomizations, ref selfAdding, ref selfRemoving, ref uiSelfChange);

            ImGui.End();

            this.plugin.SettingsVisible = settingsVisible;
            this.plugin.ToggleOtherRace(uiOtherChange);
            this.plugin.ToggleSelfRace(uiSelfChange);
            
            bool configSaved = this.plugin.SaveConfig();
            if (!configSaved && changed) {
                this.plugin.RefreshAllPlayers();
            }
        }

        private static bool ShowChangeItems(string targetType, List<Customization> customizations, ref Customization adding, ref Customization removing, ref bool change) {
	        bool changed = false;
	        ImGui.Checkbox("Change " + targetType.ToLower(), ref change);
	        if (change) {
		        if (ImGui.TreeNode(targetType)) {
			        if (removing != null) {
				        customizations.Remove(removing);
				        removing = null;
				        changed = true;
			        }
			        Race race = Race.LALAFELL;
			        foreach (Customization cust in customizations) {
				        if (cust.GetCustomization() == CustomizeIndex.Race) {
					        race = (Race) cust.GetValue();
				        }
				        ImGui.Separator();
				        if (cust.MenuItem()) {
					        changed = true;
				        }

				        ImGui.SameLine();
				        if (ImGui.Button("-")) {
					        removing = cust;
				        }
			        }
			        if (adding != null) {
				        ImGui.Separator();
				        adding.MenuItem();

				        ImGui.SameLine();
				        if (ImGui.Button("+")) {
					        customizations.Add(adding);
					        adding = null;
					        changed = true;
				        }
			        }

			        if (CUSTOMIZATIONS.Length > customizations.Count) {
				        ImGui.Separator();
				        if (ImGui.BeginCombo("Add Customization", "")) {
					        foreach (CustomizeIndex ndx in CUSTOMIZATIONS) {
						        if (!contains(customizations, ndx)) {
							        if (ImGui.Selectable(ndx.ToString(), adding != null && adding.GetCustomization() == ndx)) {
								        adding = Customizers.GetFactory(ndx).New(race);
							        }

							        if (adding != null && adding.GetCustomization() == ndx) {
								        ImGui.SetItemDefaultFocus();
							        }
						        }
					        }

					        ImGui.EndCombo();
				        }
			        }
		        }
	        }

	        return changed;
        }

        private static bool contains(List<Customization> customizations, CustomizeIndex ndx) {
	        foreach (Customization cust in customizations) {
		        if (cust.GetCustomization() == ndx) {
			        return true;
		        }
	        }

	        return false;
        }
    }
}
