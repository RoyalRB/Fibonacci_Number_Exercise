using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //TODO: The dependancies are all over the place. Try to trace it all back to the player?
    //TODO: Add error messages

    [SerializeField] float moveSpeed = 8f;
    
    Camera playerCamera;
    bool canChooseDoor = true;
    bool moveCamera = false;
    int questionsAnswered = 0;

    bool answeredCorrectly = false;

    List<Vector3> moveCoordinates = new List<Vector3>();
    int numberOfCoordinatesVisited = 0;

    Room_Pooling roomPooler;
    Questions questions;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponent<Camera>();
        roomPooler = GameObject.FindGameObjectWithTag("RoomPooler").GetComponent<Room_Pooling>();
        questions = GameObject.FindGameObjectWithTag("QuestionManager").GetComponent<Questions>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canChooseDoor && Input.GetMouseButtonDown(0))
        {
            Ray ray;
            RaycastHit hit;

            ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                Transform target = hit.transform;

                if (target.tag.StartsWith("Door")) //Prevents the bottom block of code from executing when clicking anything else than a door. I prefer this approach over copying the bottom code block into the 3 if statements below.
                {
                    if (target.CompareTag("Door/Left"))
                    {
                        target.GetComponent<Animator>().Play("Door_Open_Left");
                        answeredCorrectly = roomPooler.ReturnHallwayVariantUsed() == 0;

                        //Filling  with coordinates for the camera to follow
                        moveCoordinates.Add(new Vector3(8f, transform.position.y, transform.position.z - 6.4f)); //In front of the left door
                        moveCoordinates.Add(new Vector3(8f, transform.position.y, transform.position.z - 39f)); //At the end of the left hallway
                        moveCoordinates.Add(new Vector3(0f, transform.position.y, transform.position.z - 43f)); //At the end of the left hallway
                    }
                    else if (target.CompareTag("Door/Middle"))
                    {
                        target.GetComponent<Animator>().Play("Door_Open_Middle");
                        answeredCorrectly = roomPooler.ReturnHallwayVariantUsed() == 1;

                        //Creating a list with coordinates for the camera to follow
                        moveCoordinates.Add(new Vector3(0f, transform.position.y, transform.position.z - 6.4f)); //In front of the middle door
                        moveCoordinates.Add(new Vector3(0f, transform.position.y, transform.position.z - 43f)); //At the end of the middle hallway
                    }
                    else if (target.CompareTag("Door/Right"))
                    {
                        target.GetComponent<Animator>().Play("Door_Open_Right");
                        answeredCorrectly = roomPooler.ReturnHallwayVariantUsed() == 2;

                        //Creating a list with coordinates for the camera to follow
                        moveCoordinates.Add(new Vector3(-8f, transform.position.y, transform.position.z - 6.4f)); //In front of the right door
                        moveCoordinates.Add(new Vector3(-8f, transform.position.y, transform.position.z - 39f)); //At the end of the right hallway
                        moveCoordinates.Add(new Vector3(0f, transform.position.y, transform.position.z - 43f)); //At the end of the right hallway
                    }

                    //This is causing problems
                    questionsAnswered++;
                    RoomUI parameterRoomUI = roomPooler.ReturnRoomUI(questionsAnswered);
                    roomPooler.PoolRooms(questionsAnswered);
                    int hallwayVariantUsed = roomPooler.ReturnHallwayVariantUsed();
                    questions.GenerateQuestion(parameterRoomUI, hallwayVariantUsed);
                    moveCamera = true;
                    canChooseDoor = false;
                }
            }
        }

        if (moveCamera)
        {
            MoveCameraThroughDoor();
        }
    }

    void MoveCameraThroughDoor()
    {
        if(numberOfCoordinatesVisited < moveCoordinates.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveCoordinates[numberOfCoordinatesVisited], moveSpeed * Time.deltaTime);

            if (transform.position == moveCoordinates[numberOfCoordinatesVisited])
            {
                numberOfCoordinatesVisited++;

                if (!answeredCorrectly)
                {
                    Debug.Log("Game Over!");
                    moveCamera = false;
                    GameObject.Find("GameOver_UI").GetComponent<GameOver>().ShowGameOverScreen(questionsAnswered - 1);
                }
            }
        }
        else
        {
            moveCamera = false;
            canChooseDoor = true;
            moveCoordinates.Clear();
            numberOfCoordinatesVisited = 0;
        }
    }
}
