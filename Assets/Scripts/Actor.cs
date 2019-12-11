using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Actor : MonoBehaviour
{
    public Canvas canvas;
    public Text scoreInfo;

    private float move_;
    private float accelerateX;
    private float last_accelerateX = 0f;

    private Rigidbody2D rb;

    public Animator animator;

    private AudioSource audioCrackle;
    private AudioSource audioGameOver;

    private bool isRightSide = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        audioCrackle = GameObject.Find("audio_crackle").GetComponent<AudioSource>();
        audioGameOver = GameObject.Find("audio_game_over").GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        move();
    }

    private void move()
    {
        if (!Controller.isOnGame)
            return;
        //For testing on PC
        if (Application.platform == RuntimePlatform.Android)
        {
            accelerateX = Input.acceleration.x;
            if (Math.Abs(accelerateX - last_accelerateX) > 0.075f)
            {
                if (accelerateX > 0f)
                    move_ = 1f;
                else
                    move_ = -1f;
            }
            else
            {
                move_ = 0f;
            }
        }
        else
        {
            move_ = Input.GetAxis("Horizontal");
        }
        
        rb.velocity = new Vector2(move_ * GameVariables.ACTOR_SPEED, rb.velocity.y);
        
        // Spin actor
        if ((move_ > 0f && !isRightSide) || (move_ < 0f && isRightSide))
        {
            if (move_ != 0f)
                Spin();
        }

        // Change animation
        if (move_ == 0f)
        {
            animator.SetBool("isRunning", false);
            if (!audioCrackle.isPlaying && Random.Range(-3f, 1f) > 0)
            {
                audioCrackle.Play();
            }
        }
        else
        {
            animator.SetBool("isRunning", true);
        }

        // Teloport to opposite side border
        if (rb.position.x < -3.0f)
        {
            rb.position = new Vector2(3.0f, -3.98f);
        }
        else if (rb.position.x > 3.0f)
        {
            rb.position = new Vector2(-2.9f, -3.98f);
        }
    }


    private void Spin()
    {
        isRightSide = !isRightSide;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!Controller.isOnGame)
            return;
        if (other.gameObject.tag == "schwarz")
        {
            audioGameOver.Play();
            animator.SetTrigger("dead");
            Controller.gameOver();
            canvas.enabled = true;
            scoreInfo.text = "Your score: " + Controller.game_score.ToString() + "\n" +
                             "Record score: " + Controller.record_game_score.ToString();
        }
    }
}