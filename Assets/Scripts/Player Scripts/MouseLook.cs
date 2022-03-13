using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviourPunCallbacks
{
    public float mouseSensitivity=1f;
    private float mouseSensMultiplier = 50f;
    private bool mouseIsLocked;
    public Transform playerWholeBody;
    public Transform playerCamera;
    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime* mouseSensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime* mouseSensMultiplier;
        if (photonView.IsMine)
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f , 0f);
            playerWholeBody.Rotate(Vector3.up * mouseX);
            
            mouseIsLocked = Cursor.lockState == CursorLockMode.Locked;

            if (mouseIsLocked)
            {
                if (Input.GetButtonDown("Cancel"))
                    Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                if(Input.GetButtonDown("Cancel"))
                    Cursor.lockState = CursorLockMode.Locked;
            }

        }
    }
}
