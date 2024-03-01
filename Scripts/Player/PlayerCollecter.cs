using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollecter : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollecter;
   public float pullSpeed;
    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollecter = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        playerCollecter.radius = player.currentmagnet;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out ICollectables collectables))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDiretion = (transform.position - collision.transform.position).normalized;
            rb.AddForce(forceDiretion * pullSpeed);
            collectables.Collect();
        }
    }
}
