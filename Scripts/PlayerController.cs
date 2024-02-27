using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    Animator anim;
    private bool isWalking=false;
    Vector2 moveInput;
    public float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
        rotate();
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
         moveInput = new Vector2(horizontal, vertical);
        moveInput.Normalize();
        rb.velocity = moveInput * speed;

        //  if there is any input to determine if the player is walking
        isWalking = moveInput.magnitude > 0.1f;

        //  the animator parameter
        anim.SetBool("run", isWalking);
    }

    private void rotate()
    {
        transform.up = Vector3.Slerp(transform.forward, moveInput,  rotateSpeed);
        
    }
}
