using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float originalX;
    // private float maxOffset = 5.0f;
    // private float enemyPatroltime = 2.0f;
    private bool moveRightState;
    public Vector2 velocity;
    private Rigidbody2D enemyBody;
    private SpriteRenderer enemySprite;
    public bool collidedWithMario = false;
    private float localOffset;
    public GameConstants gameConstants;
    private System.Random rand = new System.Random();
    private AudioSource enemyAudio;
    public Animator enemyAnimator;
    private bool flipRightLate;

    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        enemyAudio = GetComponent<AudioSource>();
        enemyAnimator = GetComponent<Animator>();
        // get the starting position
        originalX = transform.position.x;
        // subscribe to player event
        GameManager.OnPlayerDeath += EnemyRejoice;
        if (rand.NextDouble() > 0.5)
        {
            moveRightState = true;
            flipRightLate = true;
        }
        else
        {
            moveRightState = false;
            flipRightLate = false;
        }
        // calculate enemy's unique speed modifier
        localOffset = (float) rand.NextDouble() * gameConstants.enemySpeedOffset;
    }


    // Update is called once per frame
    void Update()
    {
        /*
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move gomba
            MoveGomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            MoveGomba();
        }
        */
    }

    /*
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void MoveGomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }
    */

    void FixedUpdate()
    {
        if (collidedWithMario) { return; }
        var velocity = enemyBody.velocity;
        velocity = moveRightState ? new Vector2(gameConstants.enemySpeed + localOffset, velocity.y) : new Vector2(-gameConstants.enemySpeed - localOffset, velocity.y);
        enemyBody.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collidedWithMario && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Mario hit Enemy!");
            collidedWithMario = true;
            float yoffset = (other.transform.position.y - this.transform.position.y);
            if (yoffset > gameConstants.killHeight)
            {
                KillSelf();
            }
            else
            {
                // hurt the player
                CentralManager.centralManagerInstance.damagePlayer();
            }
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var direction = other.GetContact(0).normal;
        if (direction == Vector2.up) { return; }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            moveRightState = !moveRightState;
            flipRightLate = !flipRightLate;
        }
    }
    void KillSelf()
    {
        // enemy dies
        CentralManager.centralManagerInstance.increaseScore();
        StartCoroutine(flatten());
        PlaySquishSound();
        Debug.Log("Kill sequence ends");
    }

    IEnumerator flatten()
    {
        Debug.Log("Flatten starts");
        float stepper = gameConstants.enemyFlattenLimit / (float)gameConstants.enemyFlattenSteps;
        var initialScale = this.transform.localScale;
        for (int i = 0; i < gameConstants.enemyFlattenSteps; i++)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y - stepper, this.transform.localScale.z);
            if(this.transform.localScale.y <= 0){
                this.transform.localScale = new Vector3(this.transform.localScale.x, 0, this.transform.localScale.z);
            }
            // make sure enemy is still above ground
            this.transform.position = new Vector3(this.transform.position.x, gameConstants.groundSurface + GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
            yield return null;
        }
        Debug.Log("Flatten ends");
        this.gameObject.SetActive(false);
        this.transform.localScale = initialScale;
        collidedWithMario = false;
        Debug.Log("Enemy returned to pool");
        yield break;
    }

    void PlaySquishSound()
    {
        enemyAudio.PlayOneShot(enemyAudio.clip);
    }

    // animation when player is dead
    void EnemyRejoice()
    {
        Debug.Log("Enemy killed Mario");
        collidedWithMario = true;
        enemyAnimator.SetBool("partying", true);
    }

    void LateUpdate()
    {
        if(flipRightLate){
            enemySprite.flipX = !enemySprite.flipX;
        }
    }

    void OnDestroy()
    {
        GameManager.OnPlayerDeath -= EnemyRejoice;
    }

}
