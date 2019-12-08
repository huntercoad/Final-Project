using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text winText;
    public Text lives;
    public Text loseText;
    public Text timeleft;

    public AudioSource musicSource;
    public AudioSource soundSource;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioClip coinSound;

    public AudioClip hurtSound;

    Animator anim;

    private int scoreValue = 0;
    private int livesValue = 3;
    private int nextSceneToLoad;
    private bool facingRight = true;

    private float timeLeft = 45.0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winText.text = "";
        loseText.text = "";
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }


    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (vertMovement * speed != 0)
        {
            anim.SetInteger("State", 2);
        }

        else if (hozMovement * speed != 0)
        {
            anim.SetInteger("State", 1);
        }

        else
        {
            anim.SetInteger("State", 0);
        }

        if (scoreValue >= 4)
        {
            SceneManager.LoadScene(nextSceneToLoad);
        }

        if (livesValue <= 0)
        {
            Destroy(gameObject);
            loseText.text = "You lose!";
        }

        timeLeft -= Time.deltaTime;
        timeleft.text = timeLeft.ToString() + " Seconds Remain";

        if (timeLeft <= 0)
        {
            Destroy(gameObject);
            loseText.text = "You lose!";
            timeleft.text = " ";
        }

        {
            if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
            else if (facingRight == true && hozMovement < 0)
            {
                Flip();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            soundSource.clip = coinSound;
            soundSource.Play();
        }

        if (collision.collider.tag == "WinningCoin")
        {
            Destroy(collision.collider.gameObject);
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            winText.text = "You win! Game Created by Hunter Coad!";
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = true;
            soundSource.clip = coinSound;
            soundSource.Play();
        }

        if (collision.collider.tag == "Powerup")
        {
            Destroy(collision.collider.gameObject);
            speed *= 1.5f;
            soundSource.clip = coinSound;
            soundSource.Play();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);

            livesValue = livesValue - 1;
            lives.text = "Lives: " + livesValue.ToString();
            soundSource.clip = hurtSound;
            soundSource.Play();
        }


    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}