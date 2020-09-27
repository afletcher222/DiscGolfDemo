using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform disc;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - disc.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = disc.position + offset;
    }
}
