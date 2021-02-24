using System;
using Dalamud.Game.ClientState.Actors;
using ImGuiNET;

namespace OopsAllLalafells.customizations {
	public class HairCustomization : Customization {
		private readonly byte maxValue;
		private byte selected;

		public HairCustomization(Race race, byte hair) {
			this.maxValue = RaceMappings.RaceHairs[race];
			this.selected = hair;
		}

		public CustomizeIndex GetCustomization() {
			return CustomizeIndex.HairStyle;
		}

		public bool MenuItem() {
			bool changed = false;
			if (ImGui.BeginCombo("Hair Style", selected.ToString())) {
				for (byte i = 1; i <= maxValue; i++) {
					ImGui.PushID(i);
					if (ImGui.Selectable(i.ToString(), i == selected)) {
						selected = i;
						changed = true;
					}
					if (i == selected) {
						ImGui.SetItemDefaultFocus();
					}
					ImGui.PopID();
				}

				ImGui.EndCombo();
			}

			return changed;
		}
		
		public byte GetValue() {
			return selected;
		}
	}

	public class HairCustomizationFactory : CustomizationFactory {
		public Customization New(Race race) {
			return new HairCustomization(race, 1);
		}
	}
}