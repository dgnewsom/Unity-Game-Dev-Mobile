using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private GameObject mainCamera;

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    private void Update()
    {
        Vector3 lookPos = transform.position - mainCamera.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }


}
