using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{

    KinectManager manager;
    public Transform[] Players;
   

   
    // Use this for initialization
    void Start () {
	    manager = KinectManager.Instance;
       
    }
	
	// Update is called once per frame
	void Update () {

	    for (int i = 0; i < 3; i++)
	    {
	        if (manager.IsUserDetected(i)&& !Players[i].gameObject.activeSelf)
	        {
	            Players[i].gameObject.SetActive(true);

            }
	        else if (!manager.IsUserDetected(i)&&Players[i].gameObject.activeSelf)
	      
	        {
	            Players[i].gameObject.SetActive(false);
            }
        }

        
        
    }
	   
	}

