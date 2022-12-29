using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private GameObject player;
    private EnemySpawner spawner;
    private RoomGenerator roomGenerator;

    [SerializeField] float distanceFromDoor = 1.0f;

    [SerializeField] GameObject currentRoom;
    [SerializeField] GameObject[] testRooms;

    [SerializeField] AudioClip doorClip;

    private int currentX, currentY;
    [SerializeField] private int roomColumns, roomRows;

    public int enemiesKilled;

    public enum Direction
    {
        North, East, South, West
    }

    public void DoorEntered(Direction direction)
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(doorClip);
        switch (direction)
        {
            case Direction.North:
                currentY++;
                LoadLevel(currentX, currentY);
                player.GetComponent<PlayerMovement>().SetPosition(currentRoom.transform.Find("South Door").position + distanceFromDoor * Vector3.forward);
                break;
            case Direction.East:
                currentX++;
                LoadLevel(currentX, currentY);
                player.GetComponent<PlayerMovement>().SetPosition(currentRoom.transform.Find("West Door").position + distanceFromDoor * Vector3.right);
                break;
            case Direction.South:
                currentY--;
                LoadLevel(currentX, currentY);
                player.GetComponent<PlayerMovement>().SetPosition(currentRoom.transform.Find("North Door").position - distanceFromDoor * Vector3.forward);
                break;
            case Direction.West:
                currentX--;
                LoadLevel(currentX, currentY);
                player.GetComponent<PlayerMovement>().SetPosition(currentRoom.transform.Find("East Door").position - distanceFromDoor * Vector3.right);
                break;
            default:
                Debug.LogError("DoorEntered switch default");
                break;
        }
    }

    void LoadLevel(int X, int Y)
    {
        Destroy(currentRoom);
        enemiesKilled = 0;

        if (X == roomGenerator.StartRoomX && Y == roomGenerator.StartRoomY)
        {
            currentRoom = Instantiate(testRooms[0], Vector3.zero, Quaternion.identity);
        }
        else
        {
            currentRoom = Instantiate(testRooms[roomGenerator.roomArray[X, Y].layout], Vector3.zero, Quaternion.identity);
            if (roomGenerator.roomArray[X, Y].type == RoomGenerator.R.Room && roomGenerator.roomArray[X, Y].cleared)
            {
                currentRoom.transform.Find("Enemies").gameObject.SetActive(false);
            }
        }
        if (currentRoom.transform.Find("EnemySpawner"))
        {
            spawner = currentRoom.transform.Find("EnemySpawner").gameObject.GetComponent<EnemySpawner>();
        }

        if ((Y + 1 >= roomGenerator.mapDimension) || roomGenerator.roomArray[X, Y + 1].type == RoomGenerator.R.Null) // Y++ => up
        {
            currentRoom.transform.Find("North Door").gameObject.SetActive(false);
        }
        if ((X + 1 >= roomGenerator.mapDimension) || roomGenerator.roomArray[X + 1, Y].type == RoomGenerator.R.Null)
        {
            currentRoom.transform.Find("East Door").gameObject.SetActive(false);
        }
        if ((Y - 1 < 0) || roomGenerator.roomArray[X, Y - 1].type == RoomGenerator.R.Null)
        {
            currentRoom.transform.Find("South Door").gameObject.SetActive(false);
        }
        if ((X - 1 < 0) || roomGenerator.roomArray[X - 1, Y].type == RoomGenerator.R.Null)
        {
            currentRoom.transform.Find("West Door").gameObject.SetActive(false);
        }
    }

    public void EnemyDied()
    {
        enemiesKilled++;
        if (enemiesKilled >= spawner.enemiesToSpawn)
        {
            roomGenerator.roomArray[currentX, currentY].cleared = true;
            Debug.Log("Room cleared");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        roomGenerator = gameObject.GetComponent<RoomGenerator>();
        roomGenerator.GenerateMap();

        currentX = roomGenerator.StartRoomX;
        currentY = roomGenerator.StartRoomY;
        //currentRoom = Instantiate(testRooms[0], Vector3.zero, Quaternion.identity); // north-west
        LoadLevel(roomGenerator.StartRoomX, roomGenerator.StartRoomY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
