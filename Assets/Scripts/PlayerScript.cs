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
    
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>(); //Get player rigidbody
        score.text = "Score: " + pointValue.ToString(); //Display score
        outcomeText.text = ""; //Display outcome text
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Controls in game
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        //Move player in game
        playerRB.AddForce(new Vector2(horizontalMove * playerSpeed, verticalMove * playerSpeed));

        //Escape
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    //The player will earn 1 point each time they collect a coin
    private void OnCollisionEnter2D(Collision2D coin)
    {
        if (coin.collider.tag == "Coin")
        {
            pointValue += 1;
            score.text = "Score: " + pointValue.ToString(); //Update score in the text
            Destroy(coin.collider.gameObject); //Remove coin

            //Display win message if the player has collected all four coins
            if (pointValue == 4)
            {
                outcomeText.text = "Congrats! You have collected all coins! Game by Brandon Perez.";
            }
        }
    }

    //Check if the player can jump by pressing W from the ground
    private void OnCollisionStay2D (Collision2D floor)
    {
        if (floor.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerRB.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }
}
