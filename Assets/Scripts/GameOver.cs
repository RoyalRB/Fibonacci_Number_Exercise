using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //Dependancies: None
    //Function calls: None

    public void ShowGameOverScreen(int score)
    {
        GetComponent<Canvas>().enabled = true; //Sneaky little trick where the canvas won't display anything, but the gameobject is still there, able to be accessed by other scripts (in this case, the Player.cs script). This avoids a [SerializedField] reference.
        transform.GetChild(0).GetComponent<Text>().text = "GAME OVER!\nScore: " + score;
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }
}
