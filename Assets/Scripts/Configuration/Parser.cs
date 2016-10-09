using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;

public class Parser : MonoBehaviour {

    #region setup & PARSE

    void Awake()
    {
        this.enabled = true;
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
        }
		Queue<Models.Decision> decisions = new Queue<Models.Decision>(tempDecisions.ToArray());
		output.decisions = decisions;
		output.answerByMinister = new Models.Answer[System.Enum.GetNames(typeof(Models.Ministers)).Length];
        output.description = situationNode.InnerText.Trim();

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
        output.description = decisionNode.InnerText.Trim();

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
        output.text = answerNode.InnerText.Trim();

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
    }

    Models.EndDefinition CreateEndDefinition(XmlNode endDefNode)
    {
        Models.EndDefinition output = new Models.EndDefinition();
        //List<Models.Ministers> tempWinners = new List<Models.Ministers>();
        output.text = endDefNode.InnerText.Trim();

        foreach (XmlNode node in endDefNode.ChildNodes)
        {
            switch (node.Name)
            {
                /*case "Winner":
                    tempWinners.Add(ToEnum<Models.Ministers>(node.Attributes["minister"].Value));
                    break;*/        
                case "paramScore":
                    output.paramScore = CreateParameterVerification(node);
                    break;

                case "paramMinister1":
                    output.paramMinister1 = CreateParameterVerification(node);
                    break;
                case "paramMinister2":
                    output.paramMinister2 = CreateParameterVerification(node);
                    break;
                case "paramMinister3":
                    output.paramMinister3 = CreateParameterVerification(node);
                    break;
                case "paramMinister4":
                    output.paramMinister4 = CreateParameterVerification(node);
                    break;

                case "paramGovernment":
                    output.paramGovernment = CreateParameterVerification(node);
                    break;
                case "paramConfidence":
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


    public T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }


    public void CheckEnds()
    {
        Debug.Log("CheckEnds");

        foreach (Models.EndDefinition end in endingsDebug)
        {
            if (VerifyEndValidity(end))
            {
                //stop and use this end
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
            if (param.isOverTargetValue) {
                if (param.value >= value)
                    return false;
                else if (param.value < value)
                    return false;
            }
        }
        return true;
    }
}