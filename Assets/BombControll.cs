using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombControll : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    Physics.IgnoreLayerCollision(9, 9);
    }


    void Update()
    {
        if (transform.localPosition.y < -13)
        {
            Destroy(gameObject);
        }
    }


    void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }



    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
    // Update is called once per frame
   

    public void InitBombItems()
    {       
            int x = Random.Range(-9, 9);
            float z = Random.Range(-1f, 1f);
            transform.localPosition = new Vector3(x, 0, 0);
   
    }
}
