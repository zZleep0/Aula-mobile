using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsMenu : MonoBehaviour
{

    public void Iniciar()
    {
        SceneManager.LoadScene("Fase 1");
    }

    public void FecharJogo()
    {
        Application.Quit();
        Debug.Log("Fechou o jogo");
    }
}
