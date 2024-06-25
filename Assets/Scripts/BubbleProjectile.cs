using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    public float speed = 250.0f;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    private Vector2 bubbleVelocity;

    private ShooterController shooterController;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        myRigidbody.AddForce(transform.up * speed);
    }

    private void Update()
    {
        bubbleVelocity = myRigidbody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float speed = bubbleVelocity.magnitude;

        //Debug.Log("Hit");

        if (collision.transform.CompareTag("Wall"))
        {
            
            Vector2 normalOfWall = collision.contacts[0].normal;

            Vector2 newVelocity = Vector2.Reflect(bubbleVelocity.normalized, normalOfWall);

            myRigidbody.velocity = newVelocity * speed;
        }

        else if(collision.gameObject.CompareTag("TopWall") ||
            collision.gameObject.CompareTag("Bubble"))
        {
            //get location of grid nearest to collision point


            shooterController.ChangeState(ShooterController.State.Shoot);
            Destroy(gameObject);
        }
    }

    public void SetShootController(ShooterController controller)
    {
        shooterController = controller;
    }
}
