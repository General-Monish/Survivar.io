using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
   
   [SerializeField] private float rotateSpeed;

    PlayerStats playerStats;

    Rigidbody2D rb;
    Animator anim;

    [HideInInspector]
    public bool isWalking;
    [HideInInspector]
    public Vector2 moveInput;

    public JoyStickMovement JoyStickMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
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
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
         moveInput = new Vector2(JoyStickMovement.JoyStickVec.x , JoyStickMovement.JoyStickVec.y);
         moveInput.Normalize();
        if (JoyStickMovement.JoyStickVec.y != 0)
        {
            rb.velocity = moveInput * playerStats.CurrentMoveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
         

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
