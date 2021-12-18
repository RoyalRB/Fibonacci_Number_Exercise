using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questions : MonoBehaviour
{
    List<int> fibonacciList = new List<int>() { 1, 1, 2, 3, 5 };
    string fibonacciString = "";
    bool preventDoubleIntAtStart = true; //Prevents a bug from happening where the first room incorrectly displays the last number twice on the sequence

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < fibonacciList.Count; i++)
        {
            fibonacciString += fibonacciList[i] + " ";
        }

        fibonacciString += "x";

        GenerateQuestion(GameObject.Find("Room1").transform.Find("Room_UI").GetComponent<RoomUI>(), GameObject.FindGameObjectWithTag("RoomPooler").GetComponent<Room_Pooling>().ReturnHallwayVariantUsed()); //Change!
    }

    public void GenerateQuestion(RoomUI roomUI, int hallwayVariantUsed)
    {
        fibonacciString = fibonacciString.Substring(0, fibonacciString.Length - 1); //Remove the x at the end
        int nextNumber = fibonacciList[fibonacciList.Count - 1] + fibonacciList[fibonacciList.Count - 2];

        //Add the number and the x to the string
        if (!preventDoubleIntAtStart)
        {
            fibonacciString += fibonacciList[fibonacciList.Count - 1] + " x";
        }
        else
        {
            fibonacciString += "x";
            preventDoubleIntAtStart = false;
        }

        fibonacciList.Add(nextNumber); //Create the next number in the sequence

        roomUI.WriteQuestion(fibonacciString);

        //Creating wrong answers for the other two doors
        int firstWrongAnswer = nextNumber + Random.Range(-5, 6);
        int secondWrongAnswer = nextNumber + Random.Range(-5, 6);

        if (firstWrongAnswer == secondWrongAnswer)
        {
            firstWrongAnswer++;

            if (firstWrongAnswer == nextNumber)
                firstWrongAnswer++;
        }

        roomUI.WriteAnswers(new int[] { nextNumber, firstWrongAnswer, secondWrongAnswer }, hallwayVariantUsed);
    }
}
