using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D playerRB; //Access player rigidbody
    public float playerSpeed; //Speed for player to move

    public Text score; //Access score text
    private int pointValue = 0; //Value of points
    
    public Text outcomeText; //Display outcome
    
    public Text healthText; //Display health
    public int playerHealth; //Lives

    public Timer secs; //Get timer script

    public Text stage; //Get stage text
    private int stageNum;

    public AudioSource soundMaker; //Play music here
    public AudioSource effects; //Play sound effects

    //Different audio to play during the game and when the player wins
    public AudioClip backgroundMusic;
    public AudioClip victoryMusic;
    public AudioClip death;
    public AudioClip enemySound;

    //Sound effects
    public AudioClip collectedCoin;
    public AudioClip jumping;

    Animator playerAnim; //Get Player animator

    private bool faceRight = true; //Determine if the player is facing left or right

    public bool touchingGround; //See if the player is on the ground
    public Transform floorCheck; //Use it to check the player on the ground
    public float checkRadius; //The relation between the player and the ground
    public LayerMask groundTiles;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>(); //Get player rigidbody
        score.text = "Score: " + pointValue.ToString(); //Display score
        outcomeText.text = ""; //Display outcome text

        playerHealth = 3; //Default lives value
        healthText.text = "Lives: " + playerHealth.ToString(); //Display lives

        stageNum = 1; //Default level number when starting the game
        stage.text = "Stage: " + stageNum.ToString(); //Display stage number

        //Play background music
        soundMaker.clip = backgroundMusic;
        soundMaker.Play();
        soundMaker.loop = true;

        effects.loop = false; //Sound effects should only play once

        playerAnim = GetComponent<Animator>(); //Access animator
    }
    
    void Update()
    {
        //Play animations
        //Run
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || 
        Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            // if (touchingGround == true)
            // {
            //     //playerAnim.SetInteger("State", 1);
            //     //playerAnim.SetBool("isJumping", false);
            // }
            
            playerAnim.SetInteger("State", 1);
        }

        //Idle
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || 
        Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            // if (touchingGround == true)
            // {
            //     //playerAnim.SetInteger("State", 0);
            //     //playerAnim.SetBool("isJumping", false);
            // }
            
            playerRB.velocity = Vector2.zero; //Stop player from sliding
            playerAnim.SetInteger("State", 0);
        }

        //Jump
        // if (Input.GetKey(KeyCode.W))
        // {
        //     if (touchingGround == false)
        //     {
        //         playerAnim.SetBool("isJumping", true);
        //     }
            
        //     // playerAnim.SetBool("isJumping", true);
        // }

        //Death
        if (playerHealth <= 0 || secs.seconds <= 0)
        {
            playerAnim.SetBool("isDead", true);
        }

        //Victory
        if (pointValue == 8 && playerHealth > 0 && secs.seconds > 0)
        {
            playerAnim.SetBool("didWin", true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Controls in game
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        //Move player in game
        playerRB.AddForce(new Vector2(horizontalMove * playerSpeed, verticalMove * playerSpeed));

        //Calculate if the player is on the floor
        touchingGround = Physics2D.OverlapCircle(floorCheck.position, checkRadius, groundTiles);

        //Change the orientation of the player when they move left or right
        if (faceRight == false && horizontalMove > 0)
        {
            FlipPlayer(); //Change to the right
        }

        else if (faceRight == true && horizontalMove < 0)
        {
            FlipPlayer(); //Change to the left
        }

        //Control jumping
        //The player can jump and jumpAnim will only play when they are not on the ground
        if (verticalMove > 0 && touchingGround == false)
        {
            playerAnim.SetBool("isJumping", true); 
        }

        //The player can no longer jump and jumpAnim will stop playing when they landed on the ground
        if (verticalMove == 0 && touchingGround == true)
        {
            playerAnim.SetBool("isJumping", false);
        }

        //Display lose text if there is no health remaining or time has ran out
        if (playerHealth <= 0 || secs.seconds <= 0)
        {
            secs.countdown = false; //Stop timer

            outcomeText.text = "You lose! Game by Brandon Perez.";

            // soundMaker.loop = false; //Turn off looping
            // soundMaker.clip = death; //Switch clip to death
            // soundMaker.Play(); //Play the death sound effect

            //Destroy(this.gameObject); //The player is removed
        }

        //Escape
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D coin)
    {
        //The player will earn 1 point each time they collect a coin
        if (coin.collider.tag == "Coin")
        {
            //Play sound effect
            effects.clip = collectedCoin;
            effects.Play();

            pointValue += 1; //The player earns one point
            score.text = "Score: " + pointValue.ToString(); //Update score in the text
            Destroy(coin.collider.gameObject); //Remove coin

            //Move player to the second stage when he/she collects 4 points
            if (pointValue == 4)
            {
                transform.position = new Vector2(153, 0);//Move the player to the second stage
                stageNum = 2; //Change stage number to 2
                stage.text = "Stage: " + stageNum.ToString(); //Display currentstage number
                playerHealth = 3; //Reset health to 3
                healthText.text = "Lives: " + playerHealth.ToString(); //Display lives
            }

            //Display win message if the player has collected all four coins and still has lives before the timer runs out
            if (pointValue == 8 && playerHealth > 0 && secs.seconds > 0)
            {
                secs.countdown = false; //Stop timer

                //Display win message
                outcomeText.text = "Congrats! You have collected all coins! Game by Brandon Perez.";

                //Play different music when the player wins
                soundMaker.clip = victoryMusic;
                soundMaker.Play();
            }
        }

        //Play death sound effect when the player runs out of lives after colliding with the enemy
        // if (coin.collider.tag == "Enemy" && playerHealth <= 1)
        // {
        //     effects.clip = death;
        //     effects.Play();
        // }

        //Play enemy attack sound
        if (coin.collider.tag == "Enemy")
        {
            //Play death sound effect of the player when the player has no lives
            if (playerHealth == 0)
            {
                soundMaker.loop = false; //Turn off looping
                soundMaker.clip = death; //Switch clip to death
                soundMaker.Play(); //Play the death sound effect
            }

            //Default enemy sound effect
            effects.clip = enemySound;
            effects.Play();
        }
    }

    //Check if the player can jump by pressing W from the ground and play the jump animation appropriately
    private void OnCollisionStay2D (Collision2D floor)
    {
        //Play jump animation when the player is not on the ground
        if (floor.collider.tag == "Ground" && touchingGround == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                //Play sound effect
                effects.clip = jumping;
                effects.Play();

                playerRB.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);

                touchingGround = false;
            }
        }

        //Stop jump animation when the player touches the ground again
        // else if (touchingGround == false)
        // {
        //     touchingGround = true;
        // }
    }

    //Flip the player when the player moves left or right
    void FlipPlayer()
    {
        faceRight = !faceRight; //Change direction
        Vector2 Scaler = transform.localScale; //Get the scale
        Scaler.x = Scaler.x * -1; //Flip the sprite
        transform.localScale = Scaler; //Show the flipped character
    }
}
