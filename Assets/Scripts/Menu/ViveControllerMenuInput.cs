using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class ViveControllerMenuInput : MonoBehaviour
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

    public SteamVR_Action_Boolean m_triggerAction;

    #endregion

    #region Lasers

    [Header("Laser Settings")]
    public GameObject m_laserPrefab;

    public Material m_laserOn;
    public Material m_laserOff;

    [SerializeField]
    [Tooltip("Distance in Unit")]
    private float m_targetDistance = 50f;

    private GameObject m_laserRight;
    private Transform m_laserTransformRight;
    private GameObject m_laserLeft;
    private Transform m_laserTransformLeft;
    #endregion

    #region Menu
    [Header("Menu Settings")]
    [Tooltip("Indicate UI LayerMask")]
    public LayerMask m_ui;

    private Button m_lastButton;
    private bool m_isRightControllerActive = true;
    #endregion

    void Start()
    {
        m_laserRight = Instantiate(m_laserPrefab);
        m_laserTransformRight = m_laserRight.transform;

        m_laserLeft = Instantiate(m_laserPrefab);
        m_laserTransformLeft = m_laserLeft.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if(m_isRightControllerActive)
        {
            ClickRight();
        }
        else
        {
            ClickLeft();
        }

        if (GetTriggerRight())
        {
            m_isRightControllerActive = true;
        }
        else if (GetTriggerLeft())
        {
            m_isRightControllerActive = false;
        }
    }

    private void ClickRight()
    {
        RaycastHit hit;

        if (Physics.Raycast((m_laserTransformRight.position + m_laserTransformRight.forward/2), m_laserTransformRight.forward, out hit, m_ui))
        {
            Button button = hit.transform.gameObject.GetComponent<Button>();
            
            if (button == null)
            {
                return;
            }

            if (button != m_lastButton)
            {
                m_lastButton = button;
            }

            button.Select();

            if (GetTriggerRight() && m_triggerAction.GetChanged(m_rightHandType))
            {
                m_laserRight.SetActive(false);
                button.onClick.Invoke();
            }

        }
    }

    private void ClickLeft()
    {
        RaycastHit hit;

        if (Physics.Raycast((m_laserTransformLeft.position + m_laserTransformLeft.forward/2), m_laserTransformLeft.forward, out hit, m_ui))
        {
            Button button = hit.transform.gameObject.GetComponent<Button>();

            if (button == null)
            {
                return;
            }

            if (button != m_lastButton)
            {
                m_lastButton = button;
            }

            button.Select();

            if (GetTriggerLeft() && m_triggerAction.GetChanged(m_leftHandType))
            {
                m_laserLeft.SetActive(false);
                button.onClick.Invoke();
            }
        }
    }

    void LateUpdate()
    {
        UpdateAllLasersMaterial();
        UpdateControllersLaser();
    }

    private void UpdateAllLasersMaterial()
    {
        if (GetTriggerLeft())
        {
            m_laserLeft.GetComponent<MeshRenderer>().material = m_laserOn;
        }
        else
        {
            m_laserLeft.GetComponent<MeshRenderer>().material = m_laserOff;
        }

        if (GetTriggerRight())
        {
            m_laserRight.GetComponent<MeshRenderer>().material = m_laserOn;
        }
        else
        {
            m_laserRight.GetComponent<MeshRenderer>().material = m_laserOff;
        }
    }

    private void UpdateControllersLaser()
    {
        if (m_isRightControllerActive)
        {
            m_laserRight.SetActive(true);
            m_laserLeft.SetActive(false);
            //Update Laser for the right controller
            m_laserTransformRight.localScale = new Vector3(m_laserTransformRight.localScale.x, m_laserTransformRight.localScale.y, m_laserTransformRight.localScale.z);
            m_laserTransformRight.position = m_rightController.position + ((m_rightController.forward * m_laserTransformRight.localScale.z) / 2);
            m_laserTransformRight.LookAt(m_rightController.forward + m_rightController.position);
        }
        else
        {
            m_laserRight.SetActive(false);
            m_laserLeft.SetActive(true);
            //Update Laser for the left controller
            m_laserTransformLeft.localScale = new Vector3(m_laserTransformLeft.localScale.x, m_laserTransformLeft.localScale.y, m_laserTransformLeft.localScale.z);
            m_laserTransformLeft.position = m_leftController.position + ((m_leftController.forward * m_laserTransformLeft.localScale.z) / 2);
            m_laserTransformLeft.LookAt(m_leftController.forward + m_leftController.position);
        }

    }

    public bool GetTriggerRight()
    {
        return m_triggerAction.GetState(m_rightHandType);
    }

    public bool GetTriggerLeft()
    {
        return m_triggerAction.GetState(m_leftHandType);
    }
}
