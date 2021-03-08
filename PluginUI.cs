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
        private int otherRemoving = -1;
        private int selfRemoving = -1;
        
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
            ImGui.Separator();
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

        private static bool ShowChangeItems(string targetType, List<Customization> customizations, ref Customization adding, ref int removing, ref bool change) {
	        bool changed = false;
	        ImGui.Checkbox("Change " + targetType.ToLower(), ref change);
	        if (change) {
		        if (ImGui.TreeNode(targetType)) {
			        Race race = Race.LALAFELL;
			        for (var index = 0; index < customizations.Count; index++) {
				        ImGui.PushID(targetType + "-cust-" + index);
				        Customization cust = customizations[index];
				        if (cust.GetCustomization() == CustomizeIndex.Race) {
					        race = (Race) cust.GetValue();
				        }

				        if (cust.MenuItem()) {
					        changed = true;
				        }

				        ImGui.SameLine();
				        ImGui.PopID();
				        ImGui.PushID(targetType + "-cust-remove-" + index);
				        if (ImGui.Button("-")) {
					        removing = index;
				        }
				        ImGui.PopID();
			        }

			        if (CUSTOMIZATIONS.Length > customizations.Count) {
				        ImGui.Separator();
				        string label = adding != null ? "" : "Add Customization";
				        if (ImGui.BeginCombo(label, "")) {
					        foreach (CustomizeIndex ndx in CUSTOMIZATIONS) {
						        if (!contains(customizations, ndx)) {
							        ImGui.PushID(targetType + "-adding-" + ndx);
							        if (ImGui.Selectable(ndx.ToString(), adding != null && adding.GetCustomization() == ndx)) {
								        adding = Customizers.GetFactory(ndx).New(race);
							        }

							        if (adding != null && adding.GetCustomization() == ndx) {
								        ImGui.SetItemDefaultFocus();
							        }
							        ImGui.PopID();
						        }
					        }

					        ImGui.EndCombo();
				        }

				        if (adding != null) {
					        ImGui.SameLine();
					        adding.MenuItem();

					        ImGui.SameLine();
					        if (ImGui.Button("+")) {
						        customizations.Add(adding);
						        adding = null;
						        changed = true;
					        }
				        }
			        }
		        }
	        }
	        
	        if (removing >= 0) {
		        customizations.RemoveAt(removing);
		        removing = -1;
		        changed = true;
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
