using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGameClicked()
    {
        Debug.Log("New Game!");
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGameClicked()
    {
        Debug.Log("New Game!");
        Application.Quit();
    }
}
