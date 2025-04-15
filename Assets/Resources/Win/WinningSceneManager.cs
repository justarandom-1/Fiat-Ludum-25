using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningSceneManager : MonoBehaviour
{
    // Play Again Button
    public void PlayAgain()
    {
        SceneManager.LoadScene("Level");
    }
    // Menu Button
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
