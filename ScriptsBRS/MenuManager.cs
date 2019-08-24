using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject gameOverMenu;
    public static MenuManager instance;
    GameObject activeMenu;
	// Use this for initialization
	void Start () {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
		if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            SpawnMainMenu();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("TheLab");
            
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                if (pauseMenu != null)
                {
                    PauseGame();
                }
            }
        }
	}
    public void SpawnMainMenu()
    {
        if(activeMenu != null)
        {
            return;
        }
        activeMenu = Instantiate(mainMenu);
        activeMenu.transform.SetParent(transform);
        activeMenu.transform.localPosition = Vector3.zero;
    }
    public void PauseGame()
    {
        if (activeMenu != null)
        {
            return;
        }
        activeMenu = Instantiate(pauseMenu);
        activeMenu.transform.SetParent(transform);
        activeMenu.transform.localPosition = Vector3.zero;
        Time.timeScale = 0f;
    }
    public void GameOver()
    {
        if (activeMenu != null)
        {
            return;
        }
        activeMenu = Instantiate(gameOverMenu);
        activeMenu.transform.SetParent(transform);
        activeMenu.transform.localPosition = Vector3.zero;
        Time.timeScale = 0f;
    }
   
}
