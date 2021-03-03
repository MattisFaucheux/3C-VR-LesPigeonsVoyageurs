using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool m_isActivated = false;
    public LayerMask m_collideWith;


    private void OnTriggerEnter(Collider other)
    {
        if(!m_isActivated && other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
