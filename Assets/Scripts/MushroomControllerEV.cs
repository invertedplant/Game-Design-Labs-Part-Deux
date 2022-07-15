using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomControllerEV : MonoBehaviour
{
    public GameConstants gameConstants;
    private bool collidedWithMario;
    private bool launched = false;
    private Rigidbody2D mushroomBody;
    private BoxCollider2D mushroomCollider;
    private bool movingRightState;
    private bool onGround;
    public AudioSource mushroomAudio;
    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        mushroomCollider = GetComponent<BoxCollider2D>();
        mushroomAudio = GetComponent<AudioSource>();
        mushroomBody = GetComponent<Rigidbody2D>();
        mushroomBody.AddForce(gameConstants.mushroomLaunchForce, ForceMode2D.Impulse);
        launched = true;
        if (rand.NextDouble() > 0.5)
        {
            movingRightState = true;
        }
        else
        {
            movingRightState = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!launched || collidedWithMario) { return; }
        var velocity = mushroomBody.velocity;
        velocity = movingRightState ? new Vector2(gameConstants.mushroomSpeed, velocity.y) : new Vector2(-gameConstants.mushroomSpeed, velocity.y);
        mushroomBody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!collidedWithMario && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Mushroom hit Mario!");
            collidedWithMario = true;
            mushroomBody.velocity = Vector2.zero;
            mushroomBody.gravityScale = 0;
            mushroomCollider.enabled = false;
            PlayAudio();
            StartCoroutine(absorb());
            return;
        }

        var direction = other.GetContact(0).normal;
        if (direction == Vector2.up) { return; }
        if (other.gameObject.CompareTag("Obstacle"))
        {
            movingRightState = !movingRightState;
        }

    }

    void OnBecameInvisible()
    {
        // Destroy(gameObject);
    }

    IEnumerator absorb()
    {
        float stepper = gameConstants.mushroomExpandSize / (float)gameConstants.mushroomAbsorbSteps;
        var initialScale = this.transform.localScale;
        for (int i = 0; i < gameConstants.mushroomAbsorbSteps; i++)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + stepper, this.transform.localScale.y + stepper, this.transform.localScale.z + stepper);
            yield return null;
        }
        while (this.transform.localScale.x > 0)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x - stepper, this.transform.localScale.y - stepper, this.transform.localScale.z - stepper);
            if (this.transform.localScale.y <= 0)
            {
                this.transform.localScale = new Vector3(0, 0, 0);
            }
            yield return null;
        }
        yield break;
    }

    void PlayAudio(){
        mushroomAudio.PlayOneShot(mushroomAudio.clip);
    }
}