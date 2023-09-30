using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    public Transform loc;

    // Update is called once per frame
    void Update()
    {
        transform.position = loc.position;
    }
}
