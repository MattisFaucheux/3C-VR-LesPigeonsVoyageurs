using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconGlow : MonoBehaviour
{
    private Light _spot;

    [SerializeField]
    private float _glowFactor;

    // Start is called before the first frame update
    void Start()
    {
        _spot = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        _spot.intensity = _spot.intensity+Mathf.Sin(Time.time)*_glowFactor;
    }
}
