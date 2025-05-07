using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Transform target;
    private Vector3 offset;
    private float rotationDamping = 3.0f;
    private float heightDamping = 2.0f;


    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var rotationalAngle = target.eulerAngles.y;
        var height = target.position.y + offset.y;

        var currentr = transform.eulerAngles.y;
        var currenth = transform.position.y;

        currentr = Mathf.LerpAngle(currentr, rotationalAngle, rotationDamping * Time.deltaTime);
        currenth = Mathf.Lerp(currenth, height, heightDamping * Time.deltaTime);

        var currentRotation = Quaternion.Euler(0, currentr, 0);

        transform.position = target.position + offset;
        transform.position -= currentRotation * Vector3.forward * offset.x * offset.z;

     //   transform.position.y = currenth;
        transform.LookAt(target);
    }
}
