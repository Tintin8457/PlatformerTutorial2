using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer; //Get timer
    public float seconds;

    public bool countdown; //Tell timer to continue/stop countdowning

    // Start is called before the first frame update
    void Start()
    {
        seconds = 60; //Default amount of seconds
        countdown = true; //Countdown starts
    }

    // Update is called once per frame
    void Update()
    {
        //Countdown
        if (countdown == true)
        {
            seconds -= Time.deltaTime;
        }

        //Display timer regularly
        if (seconds >= 10)
        {
            timer.text = Mathf.RoundToInt(seconds).ToString(); //Display timer
        }

        //Display a 0 from 9 to 1
        if (seconds <= 9)
        {
             timer.text = 0 + Mathf.RoundToInt(seconds).ToString(); //Display timer
        }

        //Stop timer at 0
        if (seconds <= 0.9)
        {
            seconds = 0;
            timer.text = Mathf.RoundToInt(seconds).ToString(); //Display timer
        }
    }
}
