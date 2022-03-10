using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform viewPoint;
    public float mouseSensitivity=1f;
    private float verticalRotStore;
    private Vector2 mouseInput;
    private bool mouseIsLocked;
    public bool invertedLook;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseIsLocked = Cursor.lockState == CursorLockMode.Locked;

        if(mouseIsLocked && Input.GetButtonDown("Cancel"))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"))*mouseSensitivity;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,mouseInput.x+ transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        verticalRotStore += mouseInput.y;
        verticalRotStore = Mathf.Clamp(verticalRotStore, -60f, 60f);
        viewPoint.rotation = Quaternion.Euler(invertedLook ? verticalRotStore : -verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
    }
}
