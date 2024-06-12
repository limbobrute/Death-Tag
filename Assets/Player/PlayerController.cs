using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Created: 03/19/2022
 * Created by: William HP.
 * Notes: Followed  tutorial at this link: https://www.youtube.com/watch?v=5n_hmqHdijM
 *        Also added lines that normalized camera due to comments on the above link
 *        All things related to sprinting are not from the tutorial
 *        Last Updated: 05/23/2023
 */
public class PlayerController : MonoBehaviour
{
    /*#region PROPERTIES
    private InputManger inputManger;
    public CharacterController controller;
    private GameObject GroundCheck;
    private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;
    private bool isSprint = false;
    private Transform cameraTransform;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header ("Movement")]
    public float playerSpeed = 2.0f;
    public float SprintMulti = 0f;
    public float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    #endregion

    private void Start()
    {
        inputManger = InputManger.Instance;
        cameraTransform = Camera.main.transform;
        groundedPlayer = true;
        GroundCheck = GameObject.Find("GroundCheck");
    }

    void FixedUpdate()
    {
        groundedPlayer = Physics.CheckSphere(GroundCheck.transform.position, groundDistance, groundMask);
        if (groundedPlayer && playerVelocity.y < 0)
        { playerVelocity.y = 0f; }

        Vector2 movement = inputManger.GetPlayerMovement();//Check for input from the input manger, which is watching the keyboard WASD and mouse
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        Vector3 camera = cameraTransform.forward;
        camera.y = 0f;
        camera.Normalize();
        move = camera * move.z + cameraTransform.right * move.x;
        move.y = 0f;

        if (isSprint == true)//Are we trying to sprint?
        { move *= SprintMulti; }
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))//Either shift key will allow us to sprint
        { isSprint = true; }
        else
        { isSprint = false; }

        // Changes the height position of the player..
        if (inputManger.PlayerJumped() && groundedPlayer)
        { playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue); }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }*/
}
