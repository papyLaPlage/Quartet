public class Models {

	public enum ministers {
		communication,
		security,
		foreign,
		financial
	}

	[System.Serializable]
	public struct Situation {
		public string description;
		public Decision[] decisions;
	}

	[System.Serializable]
	public struct Decision {
		public string description;
		public ministers minister;
		public Answer[] answers;
	}

	[System.Serializable]
	public struct Answer {
		public string text;
		public ministers minister;
		public Operation[] operation;
	}

	[System.Serializable]
	public struct Operation {
		public ministers minister;
		public int value;
		public string operation;
	}

}
