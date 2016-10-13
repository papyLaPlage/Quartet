using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class EventUtility : MonoBehaviour {

    #region setup & PARSE

    void Awake()
    {
        //this.enabled = true;
    }

    void Start()
    {
        // read config and apply it
        situationsDebug = Load7Situations();
        endingsDebug = LoadEnds();

        //this.enabled = false;
    }

    [SerializeField]
    private Models.Situation[] situationsDebug;
    [SerializeField]
    private Models.EndDefinition[] endingsDebug;

    #endregion


    #region SITUATIONS

    public Models.Situation[] Load7Situations()
    {
        List<Models.Situation> output = new List<Models.Situation>();
        XmlDocument xmlAll = new XmlDocument();
        xmlAll.LoadXml(Resources.Load<TextAsset>("Situations").text);
        foreach(XmlNode node in xmlAll.GetElementsByTagName("Situations")[0])
        {
            if(node.Name == "Situation")
            {
                output.Add(CreateSituation(node));
            }
        }

        return output.ToArray();
        //return new Models.Situation[0] { };
    }

    Models.Situation CreateSituation(XmlNode situationNode)
    {
        Models.Situation output = new Models.Situation();
        List<Models.Decision> tempDecisions = new List<Models.Decision>();

        //Debug.Log(situationNode.InnerText.Trim());
        foreach (XmlNode node in situationNode.ChildNodes)
        {
            if (node.Name == "Decision")
            {
                tempDecisions.Add(CreateDecision(node));
            }
            else if (node.Name == "Image")
            {
                output.imagePath = node.Attributes["path"].Value.Trim();
            }
        }
		Queue<Models.Decision> decisions = new Queue<Models.Decision>(tempDecisions.ToArray());
		output.decisions = decisions;
		output.answerByMinister = new Models.Answer[System.Enum.GetNames(typeof(Models.Ministers)).Length];
        output.description = FormatText(situationNode.InnerText);

        return output;
    }

    Models.Decision CreateDecision(XmlNode decisionNode)
    {
        Models.Decision output = new Models.Decision();
        List<Models.Answer> tempAnswers = new List<Models.Answer>();

        //Debug.Log(ToEnum<Models.Ministers>(decisionNode.Attributes["minister"].Value) + " " + decisionNode.InnerText.Trim());
        foreach (XmlNode node in decisionNode.ChildNodes)
        {
            if (node.Name == "Answer")
            {
                tempAnswers.Add(CreateAnswer(node));
            }
        }
        output.answers = tempAnswers.ToArray();
        output.minister = ToEnum<Models.Ministers>(decisionNode.Attributes["minister"].Value);
        output.description = FormatText(decisionNode.InnerText);

        return output;
    }

    Models.Answer CreateAnswer(XmlNode answerNode)
    {
        Models.Answer output = new Models.Answer();
        List<Models.Operation> tempOperation = new List<Models.Operation>();

        //Debug.Log(ToEnum<Models.Ministers>(answerNode.Attributes["minister"].Value) + " " + answerNode.InnerText.Trim());
        foreach (XmlNode node in answerNode.ChildNodes)
        {
            if (node.Name == "parameter")
            {
                //Debug.Log(ToEnum<Models.Ministers>(node.Attributes["minister"].Value) + " " + int.Parse(node.Attributes["operation"].Value + node.Attributes["value"].Value));
                tempOperation.Add(  new Models.Operation(ToEnum<Models.Ministers>(node.Attributes["minister"].Value), 
                                    int.Parse(node.Attributes["operation"].Value + node.Attributes["value"].Value)));
            }
        }
        output.operations = tempOperation.ToArray();
        output.minister = ToEnum<Models.Ministers>(answerNode.Attributes["minister"].Value);
        output.text = FormatText(answerNode.InnerText);

        return output;
    }

    #endregion


    #region ENDINGS

    public Models.EndDefinition[] LoadEnds()
    {
        List<Models.EndDefinition> output = new List<Models.EndDefinition>();
        XmlDocument xmlAll = new XmlDocument();
        xmlAll.LoadXml(Resources.Load<TextAsset>("Ends").text);

        foreach (XmlNode node in xmlAll.GetElementsByTagName("Ends")[0])
        {
            if (node.Name == "End")
            {
                output.Add(CreateEndDefinition(node));
            }
        }

        return output.ToArray();
        //return new Models.EndDefinition[0] { };
    }

    Models.EndDefinition CreateEndDefinition(XmlNode endDefNode)
    {
        Models.EndDefinition output = new Models.EndDefinition();
        //List<Models.Ministers> tempWinners = new List<Models.Ministers>();
        output.text = FormatText(endDefNode.InnerText);

        foreach (XmlNode node in endDefNode.ChildNodes)
        {
            switch (node.Name)
            {
                /*case "Winner":
                    tempWinners.Add(ToEnum<Models.Ministers>(node.Attributes["minister"].Value));
                    break;*/

                case "Image":
                    output.imagePath = node.Attributes["path"].Value.Trim();
                    break;    
                        
                case "ParamScore":
                    output.paramScore = CreateParameterVerification(node);
                    break;

                case "ParamMinister1":
                    output.paramMinister1 = CreateParameterVerification(node);
                    break;
                case "ParamMinister2":
                    output.paramMinister2 = CreateParameterVerification(node);
                    break;
                case "ParamMinister3":
                    output.paramMinister3 = CreateParameterVerification(node);
                    break;
                case "ParamMinister4":
                    output.paramMinister4 = CreateParameterVerification(node);
                    break;

                case "ParamGovernment":
                    output.paramGovernment = CreateParameterVerification(node);
                    break;
                case "ParamConfidence":
                    output.paramConfidence = CreateParameterVerification(node);
                    break;
            }
        }
        //output.winners = tempWinners.ToArray();

        return output;
    }

    Models.ParameterVerification CreateParameterVerification(XmlNode paramVerifNode)
    {
        Models.ParameterVerification output = new Models.ParameterVerification();
        output.isRelevant = true;
        output.isOverTargetValue = (paramVerifNode.Attributes["operation"].Value == "gt" ? true : false);
        output.value = float.Parse(paramVerifNode.Attributes["value"].Value);

        return output;
    }

    #endregion

    public void CheckEnds()
    {
        Debug.Log("CheckEnds");

        foreach (Models.EndDefinition end in endingsDebug)
        {
            if (VerifyEndValidity(end))
            {
                //stop and use this end
                FindObjectOfType<GameUIManager>().testoText.text += end.text;

				// Prepare Global State with end params (text & win)
				GlobalState gs = GameObject.FindObjectOfType<GlobalState> ();
				GameController gc = FindObjectOfType<GameController> ();
				gs.endText = end.text;

				// Calculate Score. Win if >= 200
				int totalScore = gc.paramMinister1 + gc.paramMinister2 + gc.paramMinister3 + gc.paramMinister4;
				gs.gameWin = totalScore >= 200 ? true : false;

                if (gs.gameWin)
                {
                    foreach (MinisterController player in FindObjectsOfType<MinisterController>())
                    {
                        if (!player.isLocalPlayer)
                            continue;
                        foreach (Models.Ministers minister in player.roles)
                        {
                            gs.personalWin = gc.GetWinByMinister(minister);
                            if (gs.personalWin)
                                break;
                        }
                    }
                }

                gs.showEnd = true;
                SceneManager.LoadScene ("End", LoadSceneMode.Single);
                //MyNetworkLobbyManager.singleton.GetComponent<MyNetworkLobbyManager>().OnDisconnectClicked();
                break;  
            }
        }
    }

    bool VerifyEndValidity(Models.EndDefinition end)
    {
        GameController gameController = GetComponent<GameController>();

        if (!VerifyParamaterValidity(end.paramScore, end.paramScore.value))
            return false;

        if (!VerifyParamaterValidity(end.paramMinister1, gameController.paramMinister1))
            return false;
        if (!VerifyParamaterValidity(end.paramMinister2, gameController.paramMinister2))
            return false;
        if (!VerifyParamaterValidity(end.paramMinister3, gameController.paramMinister3))
            return false;
        if (!VerifyParamaterValidity(end.paramMinister4, gameController.paramMinister4))
            return false;

        if (!VerifyParamaterValidity(end.paramGovernment, gameController.paramMinister1 + gameController.paramMinister2 + gameController.paramMinister3 + gameController.paramMinister4))
            return false;
        if (!VerifyParamaterValidity(end.paramConfidence, gameController.paramConfidence))
            return false;

        return true;
    }

    /*private Models.Ministers winningMinister;
    bool VerifyRankingValidity(Models.Ministers[] winners)
    {
        if (winners.Length <= 0)
            return true;

        for(short i = 0; i<winners.Length; i++)
        {
            if( != //minister//)
                return false;
        }
        return true;
    }*/

    bool VerifyParamaterValidity(Models.ParameterVerification param, float value)
    {
        if (param.isRelevant) {
            if (param.isOverTargetValue)
            {
                if (param.value > value)
                {
                    //Debug.Log(param.value + " > " + value +" "+ param.isOverTargetValue);
                    return false;
                }
            }
            else if (param.value <= value)
            {
                //Debug.Log(param.value + " <= " + value + " " + param.isOverTargetValue);
                return false;
            }
        }
        //Debug.Log(param.value + " ? " + value + " " + param.isOverTargetValue);
        return true;
    }


    #region UTILITY

    public Func<string, SpriteRenderer, bool> TryInstantiate = (s, sr) => {
        try
        {
            sr.sprite = Instantiate(Resources.Load<Sprite>(s));
            return true;
        }
        catch
        {
            Debug.LogWarning("Failed to instantiate sprite " + s);
            return false;
        }
    };

    public T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public string FormatText(string text)
    {
        string output = text.Trim();
        while (output.IndexOf("  ") > 0)
        {
            output = output.Replace("  ", " ");
        }
        return output;
    }

    #endregion
}