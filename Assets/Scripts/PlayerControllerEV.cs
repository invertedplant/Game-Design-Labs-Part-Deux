using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControllerEV : MonoBehaviour
{
    // Start is called before the first frame update
    private float force;
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public GameConstants gameConstants;
    // private bool countScoreState = false;
    private Rigidbody2D marioBody;
    private bool onGroundState = true;
    private Animator marioAnimator;
    private BoxCollider2D marioCollider;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    private bool marioDead = false;
    private AudioSource[] marioAudio;
    private AudioSource jumpClip;
    private AudioSource deathClip;
    // public UnityEvent onPlayerDeath;
    public CustomCastEvent castPowerup;

    void Start()
    {
        Application.targetFrameRate = gameConstants.targetFrameRate;
        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        Debug.Log(marioUpSpeed.Value);
        marioMaxSpeed.SetValue(gameConstants.playerMaxSpeed);
        force = gameConstants.playerDefaultForce;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator = GetComponent<Animator>();
        marioCollider = GetComponent<BoxCollider2D>();
        marioAudio = GetComponents<AudioSource>();
        jumpClip = marioAudio[0];
        deathClip = marioAudio[1];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (marioDead) return;
        if (Input.GetButtonUp("Horizontal"))
        {
            marioBody.velocity = Vector2.zero;
        }

        if (Input.GetButton("Jump") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
            onGroundState = false;
            /* countScoreState = true; //check if Gomba is underneath, removed for week 2 */
        }

        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.x < marioMaxSpeed.Value && onGroundState == true)
                marioBody.AddForce(movement * force);
            else if (Mathf.Abs(marioBody.velocity.x) < (marioMaxSpeed.Value - gameConstants.AirSpeedOffset) && onGroundState == false)
                marioBody.AddForce(movement * force);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (marioDead) return;
        // toggle state
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (Mathf.Abs(marioBody.velocity.x) > gameConstants.skidLimit)
                marioAnimator.SetTrigger("onSkid");
        }

        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (Mathf.Abs(marioBody.velocity.x) > gameConstants.skidLimit)
                marioAnimator.SetTrigger("onSkid");
        }
        // when jumping, and Gomba is near Mario and we haven't registered our score
        /*         if (!onGroundState && countScoreState) removed for week 2
                {
                    if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
                    {
                        countScoreState = false;
                        score++;
                        Debug.Log(score);
                    }
                } */
        
        if (Input.GetKeyDown("z"))
        {
            castPowerup.Invoke(KeyCode.Z);
        }

        if (Input.GetKeyDown("x"))
        {
            castPowerup.Invoke(KeyCode.X);
        }
        
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetBool("onGround", onGroundState);

    }
        void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            onGroundState = true; // back on ground
            /*             countScoreState = false; // reset score state
                        scoreText.text = "Score: " + score.ToString(); removed for week 2 */
        };

        if (col.gameObject.CompareTag("Obstacle") && Mathf.Abs(marioBody.velocity.y) < 0.01f)
        {
            onGroundState = true; // back on ground
            /*             countScoreState = false; // reset score state
                        scoreText.text = "Score: " + score.ToString(); removed for week 2 */
        }
    }

    void PlayJumpSound()
    {
        jumpClip.PlayOneShot(jumpClip.clip);
    }

    void PlayDeathSound()
    {
        deathClip.PlayOneShot(deathClip.clip);
    }

    public void PlayerDiesSequence()
    {
        // Mario dies
        marioDead = true;
        PlayDeathSound();
        marioCollider.enabled = false;
        marioAnimator.SetTrigger("unAlive");
        marioBody.velocity = Vector2.zero;
        marioBody.AddForce(gameConstants.deathForce, ForceMode2D.Impulse);
    }

    void OnDestroy()
    {
        
    }
}
