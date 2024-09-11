using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GridManager gridManager;
    public GameObject buffScoreBtn;
    public GameObject plusMovesBtn;

    private void Start()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("Pause Menu");
        }
        pauseMenu.SetActive(false);

        buffScoreBtn.SetActive(false);
        plusMovesBtn.SetActive(false);
    }
    public void Update()
    {
        Pause();
        if (gridManager.Score >= 10 && gridManager.Score <= 19)
        {
            buffScoreBtn.SetActive(true);
            //plusMovesBtn.SetActive(true); // Nao deu certo oque eu queria :(
            
        }
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
        gridManager.Score *= 2;
    }

    public void PlusMoves()
    {
        gridManager.NumMove += 3;
    }
}
