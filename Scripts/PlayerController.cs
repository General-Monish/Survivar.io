using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
   [SerializeField] private float speed;
   [SerializeField] private float rotateSpeed;


    Rigidbody2D rb;
    Animator anim;

    [HideInInspector]
    public bool isWalking;
    [HideInInspector]
    public Vector2 moveInput;
    
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
        Rotate();
    }

    public void Movement()
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

    private void Rotate()
    {
        transform.up = Vector3.Slerp(transform.forward, moveInput,  rotateSpeed); 
    }
}
