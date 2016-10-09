using System.Collections.Generic;

public class Models {

    public enum Ministers {
        Communication,
        Security,
        Foreign,
        Financial
    }

    public static Ministers IntToMinister(int value)
    {
        return (new Ministers[4] { Ministers.Communication, Ministers.Security, Ministers.Foreign, Ministers.Financial })[value];
    }
    public static string IntToRoleText(int value)
    {
        return (new string[4] { " la communication", " la sécurité", " les affaires étrangères", " la finance" })[value];
    }


    #region SITUATION

    [System.Serializable]
	public struct Situation {
		public string description;
        public string imagePath;
        public Queue<Models.Decision> decisions;
		public Answer[] answerByMinister;

		public Situation (string desc, Queue<Models.Decision> decis) {
			description = desc;
            imagePath = desc;
            decisions = decis;
			answerByMinister = new Answer[System.Enum.GetNames(typeof(Models.Ministers)).Length];
		}
	}

	[System.Serializable]
	public struct Decision {
		public string description;
		public Ministers minister;
		public Answer[] answers;
		public int selectedAnswerIndex;

		public Decision(string desc, Ministers min, Answer[] ans) {
			description = desc;
			minister = min;
			answers = ans;
			selectedAnswerIndex = -1;
		}
	}

	[System.Serializable]
	public struct Answer {
		public string text;
		public Ministers minister;
		public Operation[] operations;

		public Answer(string txt, Ministers min, Operation[] op) {
			text = txt;
			minister = min;
			operations = op;
		}
	}

	[System.Serializable]
	public struct Operation {
		public Ministers minister;
		public int value;


		public Operation(Ministers newMinister, int newValue) {
			minister = newMinister;
			value = newValue;
		}
	}

    #endregion


    #region ENDINGS

    [System.Serializable]
    public struct EndDefinition
    {
        public string text;
        public string imagePath;

        //public Ministers[] winners; // ministers in winning order (only the necessary one(s))

        public ParameterVerification paramScore;

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
