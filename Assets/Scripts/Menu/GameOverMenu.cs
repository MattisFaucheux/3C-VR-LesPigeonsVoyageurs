using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [Header("Menu Settings")]
    public bool m_isDead = false;

    public void Restart()
    {
        m_isDead = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        m_isDead = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
