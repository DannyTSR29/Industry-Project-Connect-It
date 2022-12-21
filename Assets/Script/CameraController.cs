using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private Vector3 cameraPrsetVec = new Vector3(0,0.6f,0.6f);
    [SerializeField] private float lookSensitivity, smoothRate;
    Vector2 smoothedVeclocity;
    private Vector3 currentLookPos;

    void Start()
    {
        //playerObj = transform.parent.gameObject;
        transform.position = cameraPrsetVec;
    }

    
    void Update()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        Vector2 inputVec = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        //Same as Multiply the vector with lookSensitivity 
        inputVec = Vector2.Scale(inputVec, new Vector2(lookSensitivity * smoothRate, lookSensitivity * smoothRate));

        //Smoothing Camera using lerp
        smoothedVeclocity.x = Mathf.Lerp(smoothedVeclocity.x, inputVec.x, 1f/smoothRate);
        smoothedVeclocity.y = Mathf.Lerp(smoothedVeclocity.y, inputVec.y, 1f / smoothRate);
    }
}
