using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //UI Enter the game
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    //UI Quit the game
    public void QuitButton()
    {
        Application.Quit();
    }
}
