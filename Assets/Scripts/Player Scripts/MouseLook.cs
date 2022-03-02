using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 100f;
    private Transform playerBody;
    private float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.Find("Player").transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float[] mouseCoords = { Input.GetAxis("Mouse X")*sensitivity*Time.deltaTime, Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime };
        xRotation -= mouseCoords[1];
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseCoords[0]);
    }
}
