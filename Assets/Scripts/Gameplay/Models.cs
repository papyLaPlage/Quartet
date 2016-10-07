public class Models {

	public enum Ministers {
		Communication,
		Security,
		Foreign,
		Financial
	}

	[System.Serializable]
	public struct Situation {
		public string description;
		public Decision[] decisions;
	}

	[System.Serializable]
	public struct Decision {
		public string description;
		public Ministers minister;
		public Answer[] answers;
	}

	[System.Serializable]
	public struct Answer {
		public string text;
		public Ministers minister;
		public Operation[] operation;
	}

	[System.Serializable]
	public struct Operation {
		public Ministers minister;
		public int value;
		public string operation;
	}

}
