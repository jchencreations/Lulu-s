using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void StartGame()
    {
        if(GameManager.instance != null)
        {
            Destroy(GameManager.instance);
        }
        SceneTransitionManager.instance.LoadScene(1);
    }

    public void StartMenu()
    {
        SceneTransitionManager.instance.LoadScene(2);
    }
}
