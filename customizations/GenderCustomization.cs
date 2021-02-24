using System;
using Dalamud.Game.ClientState.Actors;
using ImGuiNET;

namespace OopsAllLalafells.customizations {
	public class GenderCustomization : Customization {
		private readonly bool genderForced;
		private Gender selected;

		public GenderCustomization(Race race, Gender selected) {
			this.genderForced = race switch {
				Race.HROTHGAR => true,
				Race.VIERA => true,
				_ => false
			};
			this.selected = this.genderForced ? race == Race.HROTHGAR ? Gender.Male : Gender.Female : selected;
		}

		public CustomizeIndex GetCustomization() {
			return CustomizeIndex.Gender;
		}

		public bool MenuItem() {
			bool changed = false;

			if (genderForced) {
				ImGui.Text("Gender: " + selected.ToString());
			} else if (ImGui.BeginCombo("Gender", selected.ToString())) {
				foreach (Gender gender in Enum.GetValues(typeof(Gender))) {
					ImGui.PushID((byte) gender);
					if (ImGui.Selectable(gender.ToString(), gender == selected)) {
						selected = gender;
						changed = true;
					}
					if (gender == selected) {
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

	public class GenderCustomizationFactory : CustomizationFactory {
		public Customization New(Race race) {
			return new GenderCustomization(race, Gender.Male);
		}
	}
}