using System.Collections.Generic;

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
		public Queue<Models.Decision> decisions;
		public Answer[] answerByMinister;

		public Situation (string desc, Queue<Models.Decision> decis) {
			description = desc;
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
}
