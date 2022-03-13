using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerMoviment : MonoBehaviourPunCallbacks
{
    private float[] playerMovInput = new float[3];
    private CharacterController characterController;
    private float gravity = -18f;
    private Vector3 velocity;
    private float jumpHeight=1.4f;
    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            playerMovInput[0] = Input.GetAxis("Horizontal");
            playerMovInput[2] = Input.GetAxis("Vertical");
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            //Só dá para correr quando o utilizador está a premir a tecla left shift e está a andar para a frente
            float moveSpeed = isRunning && playerMovInput[2] > 0 ? 8f : 5f;
            Vector3 move = transform.right * playerMovInput[0] + transform.forward * playerMovInput[2];
            characterController.Move(move * moveSpeed * Time.deltaTime);


            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            }
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

    }
}
