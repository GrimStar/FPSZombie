using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenu;
    GameObject activeMenu;
    
    public void ClickExit()
    {
        Application.Quit();
    }
    public void ClickResume()
    {
        
        activeMenu = null;
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }
    public void ClickRestart()
    {
        
        SceneManager.LoadScene("BackUp");
    }
    public void ClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
