using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //Markers where the enemy can move between
    public Transform startHere;
    public Transform endHere;

    public float enemSpeed = 1.0f; //Default speed of enemy

    private float trackMove; //Track time since movement started
    private float enemDistance; //Enemy's distance between the movement markers

    // Start is called before the first frame update
    void Start()
    {
        trackMove = Time.time; //Keep time since enemy's movement has started

        //Calculate the length of the enemy's movement
        enemDistance = Vector2.Distance(startHere.position, endHere.position);
    }

    // Update is called once per frame
    void Update()
    {
        float coveredDist = (Time.time - trackMove) * enemSpeed; //Calculate the distance that the enemy moved
        float fracMovement = coveredDist / enemDistance; //Calculate the fraction of the journey that the enemy moved by its total distance

        //Get the player's position as a fraction of the distance between the movement markers
        //This movement will get pingponged
        transform.position = Vector2.Lerp(startHere.position, endHere.position, Mathf.PingPong (fracMovement, 1));
    }

    //Remove one life from the player
    private void OnCollisionEnter2D(Collision2D player)
    {
        if (player.collider.tag == "Player")
        {
            //Remove a life and update it to the text component
            if (player.gameObject.GetComponent<PlayerScript>().playerHealth > 0)
            {
                player.gameObject.GetComponent<PlayerScript>().playerHealth -= 1;
                player.gameObject.GetComponent<PlayerScript>().healthText.text = "Lives: " + player.gameObject.GetComponent<PlayerScript>().playerHealth.ToString();
            }

            Destroy(this.gameObject); //Destroy enemy
        }
    }
}
