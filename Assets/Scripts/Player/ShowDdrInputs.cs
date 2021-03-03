using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ShowDdrInputs : MonoBehaviour
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

    #region Arrows
    public GameObject m_upArrow;
    public GameObject m_leftArrow;
    public GameObject m_rightArrow;
    public GameObject m_downArrow;

    public Material m_ArrowOn;
    public Material m_ArrowOff;
    #endregion

    void Update()
    {
        if(Input.GetButtonDown("Left") || GetDdrLeftInput())
        {
            m_leftArrow.GetComponent<MeshRenderer>().material = m_ArrowOn;
        }
        else if(!Input.GetButton("Left") && !GetDdrLeftInput())
        {
            if (m_leftArrow.GetComponent<MeshRenderer>().material != m_ArrowOff)
            {
                m_leftArrow.GetComponent<MeshRenderer>().material = m_ArrowOff;
            }
        }

        if (Input.GetButtonDown("Right") || GetDdrRightInput())
        {
            m_rightArrow.GetComponent<MeshRenderer>().material = m_ArrowOn;
        }
        else if (!Input.GetButton("Right") && !GetDdrRightInput())
        {
            if (m_rightArrow.GetComponent<MeshRenderer>().material != m_ArrowOff)
            {
                m_rightArrow.GetComponent<MeshRenderer>().material = m_ArrowOff;
            }
        }

        if (Input.GetButtonDown("Up") || GetDdrUpInput())
        {
            m_upArrow.GetComponent<MeshRenderer>().material = m_ArrowOn;
        }
        else if (!Input.GetButton("Up") && !GetDdrUpInput())
        {
            if (m_upArrow.GetComponent<MeshRenderer>().material != m_ArrowOff)
            {
                m_upArrow.GetComponent<MeshRenderer>().material = m_ArrowOff;
            }
        }

        if (Input.GetButtonDown("Down") || GetDdrDownInput())
        {
            m_downArrow.GetComponent<MeshRenderer>().material = m_ArrowOn;
        }
        else if (!Input.GetButton("Down") && !GetDdrDownInput())
        {
            if (m_downArrow.GetComponent<MeshRenderer>().material != m_ArrowOff)
            {
                m_downArrow.GetComponent<MeshRenderer>().material = m_ArrowOff;
            }
        }
    }

    private bool GetDdrUpInput()
    {
        if (m_ddrUpInput.GetState(m_rightHandType) || m_ddrUpInput.GetState(m_leftHandType))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool GetDdrDownInput()
    {
        if (m_ddrDownInput.GetState(m_rightHandType) || m_ddrDownInput.GetState(m_leftHandType))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool GetDdrLeftInput()
    {
        if (m_ddrLeftInput.GetState(m_rightHandType) || m_ddrLeftInput.GetState(m_leftHandType))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool GetDdrRightInput()
    {
        if (m_ddrRightInput.GetState(m_rightHandType) || m_ddrRightInput.GetState(m_leftHandType))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
