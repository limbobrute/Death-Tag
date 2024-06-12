using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerV3Movement : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 6.0f;
    public float JumpForce = 10f;
    public float MoveMultiplier = 10f;
    public float GroundDrag = 6.0f;
    public float AirDrag = 2.0f;
    public KeyCode JumpKey = KeyCode.Space;

    float horizontalMove;
    float verticalMove;

    bool isGround;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        MoveInput();
        Drag();

        if(Input.GetKeyDown(JumpKey) && isGround)
        { Jump(); }
    }

    private void Drag()
    {
        if(isGround)
        { rb.drag = GroundDrag; }
        else
        { rb.drag = AirDrag; }
    }

    void Jump()
    {
        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    void MoveInput()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMove + transform.right * horizontalMove;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        rb.AddForce((moveDirection.normalized * MoveSpeed) * MoveMultiplier, ForceMode.Acceleration);
    }
}
