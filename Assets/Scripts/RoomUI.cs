using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    public void WriteQuestion(string question)
    {
        transform.Find("Fibonacci_Sequence_Text").GetComponent<Text>().text = question;
    }

    public void WriteAnswers(int[] answers, int hallwayVariantUsed)
    {
        Transform answersGroup = transform.Find("Answers");

        //TODO: find better alternative
        if(hallwayVariantUsed == 0) //Left
        {
            answersGroup.GetChild(0).GetComponent<Text>().text = answers[0].ToString(); //Correct Answer
            answersGroup.GetChild(1).GetComponent<Text>().text = answers[1].ToString(); //Wrong Answer
            answersGroup.GetChild(2).GetComponent<Text>().text = answers[2].ToString(); //Wrong Answer
        }
        else if(hallwayVariantUsed == 1) //Middle
        {
            answersGroup.GetChild(0).GetComponent<Text>().text = answers[1].ToString(); //Wrong Answer
            answersGroup.GetChild(1).GetComponent<Text>().text = answers[0].ToString(); //Correct Answer
            answersGroup.GetChild(2).GetComponent<Text>().text = answers[2].ToString(); //Wrong Answer
        }
        else if(hallwayVariantUsed == 2) //Right
        {
            answersGroup.GetChild(0).GetComponent<Text>().text = answers[2].ToString(); //Wrong Answer
            answersGroup.GetChild(1).GetComponent<Text>().text = answers[1].ToString(); //Wrong Answer
            answersGroup.GetChild(2).GetComponent<Text>().text = answers[0].ToString(); //Correct Answer
        }
        else
        {
            Debug.LogError("Incorrect hallway variant passed into WriteAnswers()");
        }
    }
}
