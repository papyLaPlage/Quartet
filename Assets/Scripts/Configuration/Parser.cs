using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class Parser : MonoBehaviour {

    #region setup & PARSE

    void Awake()
    {
        this.enabled = true;
    }

    void Start()
    {
        // read config and apply it
        Load7Situations();

        this.enabled = false;
    }

    #endregion


    #region STATES

    void Load7Situations()
    {
        XmlDocument xmlAll = new XmlDocument();
        xmlAll.LoadXml(Resources.Load<TextAsset>("Situations").text);
        foreach(XmlNode node in xmlAll.GetElementsByTagName("Situations")[0])
        {
            if(node.Name == "Situation")
            {
                CreateSituation(node);
            }
        }
    }

    void CreateSituation(XmlNode situationNode)
    {
        foreach (XmlNode node in situationNode.ChildNodes)
        {
            if (node.Name == "Decision")
            {
                CreateDecision(node);
            }
        }
    }

    void CreateDecision(XmlNode decisionNode)
    {
        Debug.Log(decisionNode.Attributes["minister"].Value + " " + decisionNode.InnerText);
        foreach (XmlNode node in decisionNode.ChildNodes)
        {
            if (node.Name == "Answer")
            {
                CreateAnswer(node);
            }
        }
    }

    void CreateAnswer(XmlNode answerNode)
    {
        Debug.Log(answerNode.Attributes["minister"].Value + " " + answerNode.InnerText);
        foreach (XmlNode node in answerNode.ChildNodes)
        {
            if (node.Name == "parameter")
            {

            }
        }
    }

    /*StateDefinition CreateState(XmlNode stateNode)
    {
        foreach (XmlNode node in stateNode.ChildNodes)
        {
            switch (node.Name)
            {
                case "Duration":
                    short duration = short.Parse(node.Attributes["value"].Value);
                    output.duration = duration;
                    output.followupDefault = new CombatUtils.Followup(node.Attributes["followup"].Value);
                    break;

                case "Buffer":
                    string bufferTemp = node.Attributes["value"].Value;
                    output.canAct = bufferTemp == "None" ? false : true;
                    output.bufferTag = bufferTemp;
                    break;

                case "CanTurn":
                    output.canTurn = true;
                    break;

                case "PushBoxes":
                    output.pushBoxes = CreateHitBoxes(node, "PushBox");
                    break;

                case "MovementX":
                    output.movementX = new CombatUtils.MovementX(   float.Parse(node.Attributes["value"].Value),
                                                                    int.Parse(node.Attributes["duration"].Value),
                                                                    bool.Parse(node.Attributes["additive"].Value));
                    break;

                case "MovementY":
                    output.movementY = new CombatUtils.MovementY(   float.Parse(node.Attributes["value"].Value),
                                                                    float.Parse(node.Attributes["gravity"].Value),
                                                                    int.Parse(node.Attributes["duration"].Value),
                                                                    bool.Parse(node.Attributes["additive"].Value));
                    break;

                case "Landing":
                    output.followupLanding = new CombatUtils.Followup(node.Attributes["followup"].Value);
                    break;
            }
        }

        return output;
    }*/

    #endregion
}