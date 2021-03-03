using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    public bool m_isActivated = false;
    public LayerMask m_collideWith;

    public Material m_checkpointOn;
    public Material m_checkpointOff;

    public Checkpoint[] m_checkpointsList;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = m_checkpointOff;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_isActivated && other.gameObject.tag == "Player" && CheckCheckpoints())
        {
            m_isActivated = true;
            GetComponent<MeshRenderer>().material = m_checkpointOn;
        }
    }

    private bool CheckCheckpoints()
    {
        for(int i=0; i < m_checkpointsList.Length; i++)
        {
            if(!m_checkpointsList[i].m_isActivated)
            {
                return false;
            }
        }

        return true;
    }


}
