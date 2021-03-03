using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviour
{
    #region Life
    [Header("Life Settings")]
    [SerializeField]
    [Tooltip("Number of player's life")]
    private int m_nbrOfLifes = 4;
    [Tooltip("Where to print images to indicate player's life")]
    public Image m_life;
    [Tooltip("images to print to indicate player's life")]
    public Sprite[] m_lifeImages;
    #endregion

    #region Invincibility
    [Header("Invincibility Settings")]
    [SerializeField]
    [Tooltip("Time of invincibility after a collision in sec")]
    private float m_timeOfInvincibility = 5.0f;
    [Tooltip("Lights in the spaceship to indicate player's invincibility")]
    public Light[] m_invicibilityLights;

    private bool m_isInvicible = false;
    #endregion

    #region collision
    [Header("Collision Settings")]
    [Tooltip("Indicate who don't collide with the player")]
    public LayerMask m_dontCollideWith;
    #endregion

    #region Menu
    [Header("Menu Settings")]
    public GameObject m_gameOverMenu;
    private GameOverMenu m_gameOverMenuScript;
    private PlayerPause m_playerPauseScript;
    #endregion


    private void Start()
    {
        m_gameOverMenuScript = m_gameOverMenu.GetComponent<GameOverMenu>();
        m_playerPauseScript = GetComponent<PlayerPause>();

        CheckLife();
    }

    //private void Update()
    //{
    //    if (GetComponent<CharacterController>().detectCollisions)
    //    {
    //        if (!m_isInvicible)
    //        {
    //            StartCoroutine(InvincibilityTime());
    //        }
    //    }
    //}

    //public void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log("conar");

    //    if (!m_isInvicible && collision.gameObject.tag != "Player")
    //    {
    //        StartCoroutine(InvincibilityTime());
    //    }
    //}

    public void OnTriggerStay(Collider other)
    {
        if (!m_isInvicible && other.gameObject.tag != "Player" && other.gameObject.tag != "checkpoint")
        {
            StartCoroutine(InvincibilityTime());
        }
    }

    private IEnumerator InvincibilityTime()
    {
        m_nbrOfLifes -= 1;
        CheckLife();
        m_isInvicible = true;

        for(int i=0; i < 10; i++)
        {
            for(int j=0; j < m_invicibilityLights.Length; j++)
            {
                m_invicibilityLights[j].enabled = !m_invicibilityLights[j].enabled;
            }
            yield return new WaitForSeconds(m_timeOfInvincibility/10);
        }

        m_isInvicible = false;
    }

    private void CheckLife()
    {
        int index = m_nbrOfLifes - 1;

        if (index < 0)
        {
            index = 0;
        }
        else if(index > m_lifeImages.Length -1)
        {
            index = m_lifeImages.Length - 1;
        }


        m_life.sprite = m_lifeImages[index];


        if(m_gameOverMenuScript.m_isDead && Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else if(m_nbrOfLifes == 0 && !m_gameOverMenuScript.m_isDead)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        m_playerPauseScript.enabled = false;

        m_gameOverMenuScript.m_isDead = true;
        m_gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
