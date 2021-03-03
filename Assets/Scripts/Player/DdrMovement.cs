using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(CharacterController))]

public class DdrMovement : MonoBehaviour
{
    #region Vive Controllers
    [Header("Vive Controllers Settings")]

    public SteamVR_Input_Sources m_rightHandType;
    public SteamVR_Input_Sources m_leftHandType;

    public SteamVR_Action_Boolean m_ddrUpInput;
    public SteamVR_Action_Boolean m_ddrDownInput;
    public SteamVR_Action_Boolean m_ddrLeftInput;
    public SteamVR_Action_Boolean m_ddrRightInput;
    #endregion

    #region Speed
    [Header("Speed Settings")]
    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_startSpeed = 0;

    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_classicSpeed = 20, m_maxSpeed = 30, m_minSpeed = 10, m_acceleration = 3, m_deceleration = 3, m_ddrInputAcceleration = 3, m_ddrInputDeceleration = 3;

    [Tooltip("Text to print player's speed")]
    public TextMesh m_speedTxt;
    [Tooltip("Particles for giving speed sensation")]
    public ParticleSystem m_particles;

    private float m_actualSpeed;
    #endregion


    #region Haptic
    [Header("Haptic Settings")]
    public SteamVR_Action_Vibration m_hapticSignal;
    [SerializeField]
    [Tooltip("Time in Sec")]
    private float m_durationHapticDash = 0.1f;
    [SerializeField]
    private float m_frequencyHapticDash = 160f;
    [SerializeField]
    private float m_amplitudeHapticDash = 0.5f;
    #endregion


    #region Dash
    [Header("Dash Settings")]
    [SerializeField] 
    [Tooltip("Time in Sec")]
    private float m_dashTime = 0.3f;
    [SerializeField] 
    [Tooltip("Speed in Unit/Sec")]
    private float m_minDashSpeed = 25f, m_maxDashSpeed = 35f, m_dashSpeedDeceleration = 20f;

    private float m_actualDashSpeed;
    private bool m_dashRight = false, m_dashLeft = false;
    private Vector3 m_lastDashMove;
    #endregion

    #region Movement
    private Vector3 m_move;
    private CharacterController m_controller;
    #endregion

    void Start()
    {
        m_controller = GetComponent<CharacterController>();
        m_actualSpeed = m_startSpeed;
    }

    void Update()
    {
        m_move = Vector3.zero;
        ChangeSpeed();
        MoveForward();
        DashActivation();
        Dash();

        m_speedTxt.text = ((int)m_actualSpeed).ToString();
    }

    void ChangeSpeed()
    {
        if (m_actualSpeed > m_classicSpeed)
        {
            if (m_particles.isStopped)
            {
                m_particles.Play();
            }
        }
        else
        {
            m_particles.Stop();
        }


        if ((Input.GetButton("Up") || GetDdrUpInput()))
        {
           
            if (m_actualSpeed >= m_maxSpeed)
            {
                m_actualSpeed = m_maxSpeed;
            }
            else
            {
                m_actualSpeed += m_ddrInputAcceleration * Time.deltaTime;
            }
        }
        else if ((Input.GetButton("Down") || GetDdrDownInput()) && m_actualSpeed > m_minSpeed)
        {
            m_actualSpeed -= m_ddrInputDeceleration * Time.deltaTime;
        }
        else
        {
            if (m_actualSpeed < m_classicSpeed)
            {
                m_actualSpeed += m_acceleration * Time.deltaTime;
            }

            if (m_actualSpeed > m_classicSpeed)
            {
                m_actualSpeed -= m_deceleration * Time.deltaTime;
            }

            if (m_actualSpeed > m_classicSpeed - 0.5f && m_actualSpeed < m_classicSpeed + 0.5f)
            {
                m_actualSpeed = m_classicSpeed;
            }
        }


    }

    void MoveForward()
    {
        m_move = Vector3.zero;
        m_move = transform.forward;
        m_controller.Move(m_move * m_actualSpeed * Time.deltaTime);
    }

    void Dash()
    {
        m_move = Vector3.zero;

        if (m_dashRight)
        {
            m_move = transform.right;
            m_controller.Move(m_move * m_actualDashSpeed * Time.deltaTime);

            m_lastDashMove = m_move;
        }
        else if (m_dashLeft)
        {
            m_move = -transform.right;
            m_controller.Move(m_move * m_actualDashSpeed * Time.deltaTime);

            m_lastDashMove = m_move;
        }
        else if(m_actualDashSpeed > m_minDashSpeed)
        {
            m_actualDashSpeed -= m_dashSpeedDeceleration * Time.deltaTime;
            if (m_actualDashSpeed < m_minDashSpeed)
            {
                m_actualDashSpeed = m_minDashSpeed;
            }

            m_controller.Move(m_lastDashMove * m_actualDashSpeed * Time.deltaTime);
        }
    }

    void DashActivation()
    {
        if ((Input.GetButtonDown("Right") || GetDdrRightInput()) && !m_dashRight && !m_dashLeft)
        {
            StartCoroutine(DashRight());
        }
        
        if ((Input.GetButtonDown("Left") || GetDdrLeftInput()) && !m_dashRight && !m_dashLeft)
        {
            StartCoroutine(DashLeft());
        }
    }


    IEnumerator DashLeft()
    {
        m_hapticSignal.Execute(0.0f, m_durationHapticDash, m_frequencyHapticDash, m_amplitudeHapticDash, m_leftHandType);
        m_actualDashSpeed = m_maxDashSpeed;
        m_dashLeft = true;
        yield return new WaitForSeconds(m_dashTime);
        m_dashLeft = false;
    }

    IEnumerator DashRight()
    {
        m_hapticSignal.Execute(0.0f, m_durationHapticDash, m_frequencyHapticDash, m_amplitudeHapticDash, m_rightHandType);
        m_actualDashSpeed = m_maxDashSpeed;
        m_dashRight = true;
        yield return new WaitForSeconds(m_dashTime);
        m_dashRight = false;
    }

    private bool GetDdrUpInput()
    {
        if (m_ddrUpInput.GetState(m_rightHandType) || m_ddrUpInput.GetState(m_leftHandType))
        {
            return true;
        }

        return false;
    }

    private bool GetDdrDownInput()
    {
        if (m_ddrDownInput.GetState(m_rightHandType) || m_ddrDownInput.GetState(m_leftHandType))
        {
            return true;
        }

        return false;
    }

    private bool GetDdrLeftInput()
    {
        if ((m_ddrLeftInput.GetChanged(m_rightHandType) && m_ddrLeftInput.GetState(m_rightHandType)) ||
            (m_ddrLeftInput.GetChanged(m_leftHandType)) && m_ddrLeftInput.GetState(m_leftHandType))
        {
            return true;
        }
        
        return false;
    }

    private bool GetDdrRightInput()
    {
        if ((m_ddrRightInput.GetChanged(m_rightHandType) && m_ddrRightInput.GetState(m_rightHandType)) || 
            (m_ddrRightInput.GetChanged(m_leftHandType)) && m_ddrRightInput.GetState(m_leftHandType))
        {
            return true;
        }

        return false;
    }

}


