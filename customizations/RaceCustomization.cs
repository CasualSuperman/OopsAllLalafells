using System;
using Dalamud.Game.ClientState.Actors;
using ImGuiNET;

namespace OopsAllLalafells.customizations {
	public class RaceCustomization : Customization {
		private Race selected;

		public RaceCustomization(Race race) {
			this.selected = race;
		}

		public CustomizeIndex GetCustomization() {
			return CustomizeIndex.Race;
		}

		public bool MenuItem() {
			bool changed = false;
			if (ImGui.BeginCombo("Race", selected.ToString())) {
				foreach (Race race in Enum.GetValues(typeof(Race))) {
					ImGui.PushID((byte) race);
					if (ImGui.Selectable(race.ToString(), race == selected)) {
						selected = race;
						changed = true;
					}
					if (race == selected) {
						ImGui.SetItemDefaultFocus();
					}
					ImGui.PopID();
				}

				ImGui.EndCombo();
			}

			return changed;
		}
		
		public byte GetValue() {
			return (byte) selected;
		}
	}

	public class RaceCustomizationFactory : CustomizationFactory {
		public Customization New(Race race) {
			return new RaceCustomization(race);
		}
	}
}