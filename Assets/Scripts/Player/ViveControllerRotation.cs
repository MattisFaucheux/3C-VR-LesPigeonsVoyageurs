using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveControllerRotation : MonoBehaviour
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

    public SteamVR_Action_Single m_triggerAction;

    #endregion

    #region Lasers

    [Header("Laser Settings")]
    public GameObject m_laserPrefab;

    public GameObject m_dirArrowPrefab;
    public Material m_laserOn;
    public Material m_laserOff;

    [SerializeField]
    [Tooltip("Distance in Unit")]
    private float m_targetDistance = 50f;

    private GameObject m_laserRight;
    private Transform m_laserTransformRight;
    private GameObject m_laserLeft;
    private Transform m_laserTransformLeft;
    private GameObject m_targetDirArrow;
    #endregion

    #region Rotation

    [Header("Rotation Speed Settings")]
    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_minRotateSpeed = 0.2f;

    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_maxRotateSpeed = 0.4f, m_rotateSpeedAcceleration = 0.1f, m_rotateSpeedDeceleration = 0.1f;

    private float m_rotateSpeed;
    private Quaternion m_lastRotate;

    #endregion

    #region Phares
    [Header("Phares Settings")]
    public Light m_rightPhare;
    public Light m_leftPhare;
    public SteamVR_Action_Boolean m_phareInput;

    private bool m_isPhareOn = false;
    #endregion

    void Start()
    {
        m_laserRight = Instantiate(m_laserPrefab);
        m_laserTransformRight = m_laserRight.transform;

        m_laserLeft = Instantiate(m_laserPrefab);
        m_laserTransformLeft = m_laserLeft.transform;

        m_targetDirArrow = Instantiate(m_dirArrowPrefab);

        m_rotateSpeed = m_minRotateSpeed;

    }

    void Update()
    {
        if (GetTriggerLeft() > 0.5f && GetTriggerRight() > 0.5f)
        {
            RotateSpaceship();
        }
        else if (m_rotateSpeed > m_minRotateSpeed)
        {
            SpaceshipInertie();
        }

        if(GetPhareInput())
        {
            UpdatePhare();
        }
    }

    void LateUpdate()
    {

        if (Time.timeScale == 0)
        {
            m_laserLeft.SetActive(false);
            m_laserRight.SetActive(false);
            m_targetDirArrow.SetActive(false);
        }
        else
        {
            m_laserLeft.SetActive(true);
            m_laserRight.SetActive(true);
            m_targetDirArrow.SetActive(true);
        }

        UpdateAllLasersMaterial();
        UpdateControllersLaser();
        ShowTargetDirection();
    }

    private void SpaceshipInertie()
    {
        m_rotateSpeed -= m_rotateSpeedDeceleration * Time.deltaTime;
        if (m_rotateSpeed < m_minRotateSpeed)
        {
            m_rotateSpeed = m_minRotateSpeed;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, m_lastRotate, m_rotateSpeed * Time.deltaTime);
    }

    private void RotateSpaceship()
    {
        Vector3 lookPoint = ((m_rightController.forward + m_leftController.forward) * m_targetDistance) / 2;
        Vector3 dir = lookPoint - ((m_rightController.localPosition + m_leftController.localPosition) / 2);

        Quaternion Rotation = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Lerp(transform.rotation, Rotation, m_rotateSpeed * Time.deltaTime);

        m_lastRotate = Rotation;

        m_rotateSpeed += m_rotateSpeedAcceleration * Time.deltaTime;
        if (m_rotateSpeed > m_maxRotateSpeed)
        {
            m_rotateSpeed = m_maxRotateSpeed;
        }
    }

    private void UpdateAllLasersMaterial()
    {
        if (GetTriggerLeft() > 0.5f)
        {
            m_laserLeft.GetComponent<MeshRenderer>().material = m_laserOn;
        }
        else
        {
            m_laserLeft.GetComponent<MeshRenderer>().material = m_laserOff;
        }

        if (GetTriggerRight() > 0.5f)
        {
            m_laserRight.GetComponent<MeshRenderer>().material = m_laserOn;
        }
        else
        {
            m_laserRight.GetComponent<MeshRenderer>().material = m_laserOff;
        }

        if (GetTriggerLeft() > 0.5f && GetTriggerRight() > 0.5f)
        {
            m_targetDirArrow.GetComponent<MeshRenderer>().material = m_laserOn;
        }
        else
        {
            m_targetDirArrow.GetComponent<MeshRenderer>().material = m_laserOff;
        }

    }

    private void ShowTargetDirection()
    {
        m_targetDirArrow.transform.position = (m_rightController.position + m_leftController.position) / 2 + (m_rightController.forward + m_leftController.forward) / 2;
        m_targetDirArrow.transform.LookAt((m_leftController.forward + m_rightController.forward) + (m_leftController.position + m_rightController.position) / 2);
    }


    private void UpdateControllersLaser()
    {
        //Update Laser for the right controller
        m_laserTransformRight.localScale = new Vector3(m_laserTransformRight.localScale.x, m_laserTransformRight.localScale.y, 1);
        m_laserTransformRight.position = m_rightController.position + (m_rightController.forward / 2);
        m_laserTransformRight.LookAt(m_rightController.forward + m_rightController.position);

        //Update Laser for the left controller
        m_laserTransformLeft.localScale = new Vector3(m_laserTransformLeft.localScale.x, m_laserTransformLeft.localScale.y, 1);
        m_laserTransformLeft.position = m_leftController.position + (m_leftController.forward / 2);
        m_laserTransformLeft.LookAt(m_leftController.forward + m_leftController.position);
    }

    private void UpdatePhare()
    {
        if(m_isPhareOn)
        {
            m_isPhareOn = false;
        }
        else
        {
            m_isPhareOn = true;
        }

        m_leftPhare.enabled = m_isPhareOn;
        m_rightPhare.enabled = m_isPhareOn;
    }

    private float GetTriggerRight()
    {
        return m_triggerAction.GetAxis(m_rightHandType);
    }

    private float GetTriggerLeft()
    {
        return m_triggerAction.GetAxis(m_leftHandType);
    }
    private bool GetPhareInput()
    {
        if ((m_phareInput.GetState(m_rightHandType) && m_phareInput.GetChanged(m_rightHandType)) || 
            (m_phareInput.GetState(m_leftHandType) && m_phareInput.GetChanged(m_leftHandType)))
        {
            return true;
        }
        
        return false;
    }
}
