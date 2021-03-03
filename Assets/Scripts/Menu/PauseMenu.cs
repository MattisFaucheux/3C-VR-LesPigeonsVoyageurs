using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Settings")]
    public bool m_isPause = false;

    public void Restart()
    {
        m_isPause = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume(GameObject ui)
    {
        ui.SetActive(false);
        m_isPause = false;
        Time.timeScale = 1;
    }

    public void GoToMenu()
    {
        m_isPause = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
