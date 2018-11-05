using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{

    public Text ScoreText;

    private int score = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    ScoreText.text = score.ToString();

	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FallBall")
        {
            score++;
        }

        if (collision.gameObject.tag == "Bomb")
        {
            score=0;
        }
            
    }

    
}
