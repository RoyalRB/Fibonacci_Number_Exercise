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

        //JUST MAKING THIS WORK PLEASE DELETE!

        int[] doomApproach = { -5, -4, -3, -2, -1, 1, 2, 3, 4, 5 };

        int firstWrongAnswer = nextNumber + doomApproach[Random.Range(0, doomApproach.Length)];
        int secondWrongAnswer = nextNumber + doomApproach[Random.Range(0, doomApproach.Length)];

        if(firstWrongAnswer == secondWrongAnswer)
        {
            firstWrongAnswer++;
        }

        roomUI.WriteAnswers(new int[] { nextNumber, firstWrongAnswer, secondWrongAnswer }, hallwayVariantUsed);
    }
}
