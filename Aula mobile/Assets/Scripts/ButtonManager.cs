using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GridManager gridManager;

    private void Start()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("Pause Menu");
        }
        pauseMenu.SetActive(false);
    }
    public void Update()
    {
        Pause();
    }

    public void Iniciar()
    {
        SceneManager.LoadScene("Fase 1");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VoltarParaMenu()
    {
        SceneManager.LoadScene("Tela Inicial");
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
        
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
    }

    public void BuffScore()
    {
        
    }
}
