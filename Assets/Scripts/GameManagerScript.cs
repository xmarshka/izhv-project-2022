using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public bool inMenu;
    private GameObject player;

    public GameObject pressE;
    public GameObject altarMenu;

    public void EnterMenu()
    {
        inMenu = true;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void LeaveMenu()
    {
        inMenu = false;
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        pressE = GameObject.Find("PressE UI");
        altarMenu = GameObject.Find("AltarMenu UI");

        inMenu = false;
        player = GameObject.Find("Player");
    }
}
