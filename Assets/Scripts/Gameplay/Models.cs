public class Models {

	public enum Ministers {
		Communication,
		Security,
		Foreign,
		Financial
	}


    #region SITUATION

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
		public Operation[] operations;
	}

	[System.Serializable]
	public struct Operation {
        public Operation(Ministers newMinister, int newValue)
        {
            minister = newMinister;
            value = newValue;
        }
		public Ministers minister;
		public int value;
	}

    #endregion


    #region ENDINGS

    [System.Serializable]
    public struct EndDefinition
    {
        public string text;

        public Ministers[] winners; // ministers in winning order (only the necessary one(s))

        public ParameterVerification paramMinister1;
        public ParameterVerification paramMinister2;
        public ParameterVerification paramMinister3;
        public ParameterVerification paramMinister4;

        public ParameterVerification paramGovernment;
        public ParameterVerification paramConfidence;
    }

    [System.Serializable]
    public struct ParameterVerification
    {
        public bool isRelevant;
        public float value;
        public bool isOverTargetValue;
    }

    #endregion
}
