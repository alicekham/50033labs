using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Rigidbody2D mushroomBody;
    private int moveLeft = 1;
    void Awake() {
        mushroomBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ComputeVelocity();
    }

    void ComputeVelocity() 
    {
        mushroomBody.velocity = new Vector2(moveLeft*6f,0f);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Pipe"))
        {
            Debug.Log("Collided with Pipe!");
            moveLeft = moveLeft * -1;
            ComputeVelocity();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with Player!");
            Time.timeScale = 0.0f; //Stop Game
        }
    }
    void  OnBecameInvisible(){
	Destroy(gameObject);	
}
}
