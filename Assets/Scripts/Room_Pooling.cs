using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Pooling : MonoBehaviour
{
    //BUG: sometimes doors won't open (maybe animation can't be replayed)

    //Two main rooms that the script switches between to create the illusion of an infinitely long sequence of rooms
    [SerializeField] GameObject[] mainRooms;

    //Due to the way the camera moves and rooms connect, a second set of rooms is needed in order for the possibility of the same door choice to be correct twice in a row
    //The script switches between the two 
    [SerializeField] GameObject[] hallwaysFirstSet;
    [SerializeField] GameObject[] hallwaysSecondSet;

    int hallwayVariantUsed = 0;

    GameObject previouslyUsedHallway;

    float room_z_Distance = -43f;
    float hallway_z_offset = -25f;

    private void Awake()
    {
        hallwayVariantUsed = Random.Range(0, 3);
    }

    private void Start()
    {
        hallwaysFirstSet[hallwayVariantUsed].transform.position = new Vector3(0f, 0f, -25f);
        Debug.Log(hallwayVariantUsed);
    }

    public void PoolRooms(int numberOfQuestionsAnswered)
    {
        int roomsToUse = numberOfQuestionsAnswered % 2;
        hallwayVariantUsed = Random.Range(0, 3);

        if(roomsToUse == 0) //Even
        {
            float zPosition = numberOfQuestionsAnswered * room_z_Distance + hallway_z_offset;
            hallwaysFirstSet[hallwayVariantUsed].transform.position = new Vector3(0f, 0f, zPosition);
        }
        else //Uneven
        {
            float zPosition = numberOfQuestionsAnswered * room_z_Distance + hallway_z_offset;
            hallwaysSecondSet[hallwayVariantUsed].transform.position = new Vector3(0f, 0f, zPosition);
        }

        mainRooms[roomsToUse].transform.position = new Vector3(0f, 0f, numberOfQuestionsAnswered * room_z_Distance);

        Debug.Log(hallwayVariantUsed);
    }

    public RoomUI ReturnRoomUI(int numberOfQuestionsAnswered)
    {
        return mainRooms[numberOfQuestionsAnswered % 2].GetComponentInChildren<RoomUI>();
    }

    public int ReturnHallwayVariantUsed()
    {
        Debug.Log(hallwayVariantUsed);
        return hallwayVariantUsed;
    }
}
