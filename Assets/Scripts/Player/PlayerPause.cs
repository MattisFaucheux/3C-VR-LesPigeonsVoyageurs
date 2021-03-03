using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerPause : MonoBehaviour
{
    #region Vive Controllers

    [Header("Vive Controllers Settings")]

    #region Right Hand
    public Transform m_rightController;
    public SteamVR_Input_Sources m_rightHandType;
    #endregion

    #region Left Hand
    public Transform m_leftController;
    public SteamVR_Input_Sources m_leftHandType;
    #endregion

    public SteamVR_Action_Boolean m_menuInput;

    #endregion

    #region Pause Menu
    [Header("Menu Settings")]
    public GameObject m_pauseMenu;
    private PauseMenu m_pauseMenuScript;
    #endregion

    void Start()
    {
        m_pauseMenuScript = m_pauseMenu.GetComponent<PauseMenu>();
        m_pauseMenu.SetActive(false);
    }

    void Update()
    {
        CheckGoToPauseMenu();
    }

    private void CheckGoToPauseMenu()
    {

        if (Time.timeScale == 0 && !m_pauseMenuScript.m_isPause)
        {
            Pause();
        }
        else if (m_pauseMenuScript.m_isPause && Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else if ((m_menuInput.GetState(m_rightHandType) && m_menuInput.GetChanged(m_rightHandType)) ||
                (m_menuInput.GetState(m_leftHandType) && m_menuInput.GetChanged(m_leftHandType)))
        {
            Pause();
        }

    }

    public void Pause()
    {
        m_pauseMenuScript.m_isPause = true;
        m_pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
