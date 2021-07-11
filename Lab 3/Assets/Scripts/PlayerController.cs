using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float upSpeed;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private Animator marioAnimator;
    private AudioSource marioAudioSource;
    public ParticleSystem dustCloud;

    private bool faceRightState = true;
   
    // Mario is on ground
    private bool onGroundState = true;
    
    // Scoring System
    // public Transform enemyLocation;
    // public Text scoreText;
    // private int score = 0;
    // private bool countScoreState = false;

    // Game Over
    
    // Start is called before the first frame update
    void  Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            // animation , skid when a is pressed
            if (Mathf.Abs(marioBody.velocity.x) >  0.05) 
            {
                marioAnimator.SetTrigger("onSkid");
            };
        }

        if(Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;

            // animation , skid when d is pressed
            if (Mathf.Abs(marioBody.velocity.x) >  0.05) 
            {
                marioAnimator.SetTrigger("onSkid");
            };
        }
        
        // animation , always update the xSpeed parameter 
        // to match Mario’s current speed along the x-axis
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        // animation , match the current onGroundState value 
        // whenever it’s changed in the script
        marioAnimator.SetBool("onGround", onGroundState);


        // when jumping, and Gomba is near Mario and we haven't registered our score
        // if (!onGroundState && countScoreState)
        // {
        //     if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
        //     {
        //         countScoreState = false;
        //         score++;
        //         Debug.Log(score);
        //     }
        // }
    }

    void  FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);
        marioBody.AddForce(movement * speed);

        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
          // Stop Mario from moving
          marioBody.velocity = Vector2.zero;
        }

        // Jump
        if(Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            // countScoreState = true; // check if Gomba is underneath
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }
    
    // Called when Mario hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) 
        {
            Debug.Log("Hit the ground!");
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
            // countScoreState = false;
            // scoreText.text = "Score: " + score.ToString();
            dustCloud.Play();
        };

        if(col.gameObject.CompareTag("Obstacles") && Mathf.Abs(marioBody.velocity.y) < 0.01f)
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        };

        if(col.gameObject.CompareTag("Pipe") && Mathf.Abs(marioBody.velocity.y) < 0.01f)
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        };
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");
            Time.timeScale = 0.0f; //Game Over
        }

        if (other.gameObject.CompareTag("Mushroom"))
        {
            Debug.Log("Collided with Mushroom!");
            Time.timeScale = 0.0f; //Game Over
        }
    }

    // play audio from script
    // when mario jumps, trigger event and set a call back for the event
    void PlayJumpSound() {
        // event handler
        marioAudioSource.PlayOneShot(marioAudioSource.clip);
    }
}
