using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
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
}
