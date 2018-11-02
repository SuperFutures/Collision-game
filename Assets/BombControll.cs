using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombControll : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitBombItems()
    {       
            int x = Random.Range(-9, 9);
            float z = Random.Range(-1f, 1f);
            transform.localPosition = new Vector3(x, 0, z);
   
    }
}
