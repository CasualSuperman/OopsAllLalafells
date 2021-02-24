using Dalamud.Game.ClientState.Actors;

namespace OopsAllLalafells.customizations {
	public interface Customization {
		
		public CustomizeIndex GetCustomization();
		public bool MenuItem();
		public byte GetValue();
	}
	
	public interface CustomizationFactory {
		// TODO: Valid values will depend on other selections, need to provide
		public Customization New(Race race);
	}

	public static class Customizers {
		public static CustomizationFactory GetFactory(CustomizeIndex ndx) {
			return ndx switch {
				CustomizeIndex.Race => new RaceCustomizationFactory(),
				CustomizeIndex.HairStyle => new HairCustomizationFactory(),
				CustomizeIndex.Gender => new GenderCustomizationFactory(),
				_ => new GenericCustomizationFactory(ndx),
			};
		}
	}
}