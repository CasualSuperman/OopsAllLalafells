using System;
using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.Actors;
using ImGuiNET;
using OopsAllLalafells;
using OopsAllLalafells.customizations;

namespace OopsAllLalafells.customizations {
	public class GenericCustomization : Customization {
		private CustomizeIndex ndx;
		private byte selected;

		public GenericCustomization(CustomizeIndex ndx, byte starting) {
			this.ndx = ndx;
			this.selected = starting;
		}

		public CustomizeIndex GetCustomization() {
			return ndx;
		}

		public bool MenuItem() {
			bool changed = false;
			string uiCustom = selected.ToString();
			ImGui.InputText(ndx.ToString(), ref uiCustom, 3);
			if (!uiCustom.Equals(selected.ToString()) && uiCustom.Length > 0) {
				if (Byte.TryParse(uiCustom, out this.selected)) {
					changed = true;
				}
			}

			return changed;
		}
		
		public byte GetValue() {
			return selected;
		}

		private static byte parseByte(string val, byte current) {
			if (val.Length == 0) {
				return current;
			}
			return Byte.Parse(val);
		}
	}

	public class GenericCustomizationFactory : CustomizationFactory {
		private CustomizeIndex ndx;

		public GenericCustomizationFactory(CustomizeIndex ndx) {
			this.ndx = ndx;
		}
		
		public Customization New(Race race) {
			return new GenericCustomization(ndx, 1);
		}
	}
}