using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Altar : MonoBehaviour
{
    private GameObject player;
    private GameManagerScript gmScript;

    private bool playerNear;

    // Start is called before the first frame update
    void Start()
    {
        playerNear = false;

        player = GameObject.Find("Player");
        gmScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        gmScript.pressE.SetActive(false);
        gmScript.altarMenu.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            playerNear = true;
            gmScript.pressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            playerNear = false;
            gmScript.pressE.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (gmScript.inMenu == false)
            {
                gmScript.EnterMenu();
                gmScript.altarMenu.SetActive(true);
            }
            else if (gmScript.inMenu)
            {
                gmScript.LeaveMenu();
                gmScript.altarMenu.SetActive(false);
            }
        }
    }
}
