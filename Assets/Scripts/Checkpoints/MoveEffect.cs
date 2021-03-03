using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour
{
    private Vector3 _checkpointPos;

    [SerializeField]
    private float _moveFactor;

    // Start is called before the first frame update
    void Start()
    {
        _checkpointPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_checkpointPos.x, _checkpointPos.y + Mathf.Sin(Time.time) * _moveFactor, _checkpointPos.z);
    }
}
