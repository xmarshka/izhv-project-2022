using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public enum R
    {
        Null, Start, Room, Boss
    }

    public struct Room
    {
        public R type;
        public int layout;
        public bool cleared;
    }

    public int mapDimension = 8; // should be an odd number, doesn't have to
    private int roomsToGenerate = 34;

    public Room[,] roomArray = new Room[8, 8];

    public int StartRoomX, StartRoomY;
    [SerializeField] int layoutAmount;

    void StartGeneration()
    {
        StartRoomX = mapDimension / 2;
        StartRoomY = mapDimension - 2;
        Debug.Log(StartRoomX);
        Debug.Log(StartRoomY);

        roomArray[StartRoomX, StartRoomY].type = R.Start;
    }

    void Traverse(int X, int Y, int roomsToGenerate)
    {
        roomArray[X, Y].type = R.Room;
        roomArray[X, Y].layout = Random.Range(2, layoutAmount + 2);
        roomsToGenerate--;

        while (roomsToGenerate > 0)
        {
            int next = Random.Range(0, 4);
            switch (next)
            {
                case 0:
                    if (X < mapDimension - 1)
                        X++;
                    break;
                case 1:
                    if (X > 0)
                        X--;
                    break;
                case 2:
                    if (Y < mapDimension - 1)
                        Y++;
                    break;
                case 3:
                    if (Y > 0)
                        Y--;
                    break;
                default:
                    break;
            }

            if (roomArray[X, Y].type == R.Null)
            {
                roomArray[X, Y].type = R.Room;
                roomArray[X, Y].layout = Random.Range(2, layoutAmount + 2);
                roomsToGenerate--;
            }
        }
    }

    public void GenerateMap()
    {
        StartGeneration();
        Traverse(StartRoomX, StartRoomY - 1, roomsToGenerate);
    }
}
