using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smouthSpeed = 0.125f;

    public Vector3 offset = new Vector3(0f, 10f, 0f);

    public void FixedUpdate()
    {
        CameraBehavier();
    }

    public void CameraBehavier()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smooothedPosiotion = Vector3.Lerp(transform.position, desiredPosition, smouthSpeed);
        transform.position = smooothedPosiotion;
    }
}

