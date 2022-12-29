using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoors : MonoBehaviour
{
    private GameObject roomManager;
    private RoomManager roomManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = GameObject.Find("Room Manager");
        roomManagerScript = roomManager.GetComponent<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "North Door":
                roomManagerScript.DoorEntered(RoomManager.Direction.North);
                break;
            case "East Door":
                roomManagerScript.DoorEntered(RoomManager.Direction.East);
                break;
            case "South Door":
                roomManagerScript.DoorEntered(RoomManager.Direction.South);
                break;
            case "West Door":
                roomManagerScript.DoorEntered(RoomManager.Direction.West);
                break;
            default:
                break;
        }
    }
}
