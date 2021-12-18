using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //TODO: The dependancies are all over the place. Try to trace it all back to the player?

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
        //If the player presses the left mouse button and the player is allowed to choose a door
        if (canChooseDoor && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            Ray ray;
            RaycastHit hit;

            if (Input.touchCount > 0) //Tapping on an android phone
            {
                Touch touch = Input.GetTouch(0);
                ray = playerCamera.ScreenPointToRay(touch.position);
            }
            else //Clicking with a mouse
            {
                ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            }
                

            //If the player clicked on something
            if(Physics.Raycast(ray, out hit))
            {
                Transform target = hit.transform;

                //If the player clicked on a door
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
        //Move to the coordinates until the last coordinate has been reached
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
        else //When the camera is at its final coordinate in the next room
        {
            moveCamera = false;
            canChooseDoor = true;
            moveCoordinates.Clear();
            numberOfCoordinatesVisited = 0;
        }
    }
}
