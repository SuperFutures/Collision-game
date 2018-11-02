using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBallControll : MonoBehaviour
{
    public AudioClip GetCoin;




    // Use this for initialization
    void Awake()
    {
        Physics.IgnoreLayerCollision(9, 9);
        GameControll game = new GameControll();
    }

    void Start()
    {

    }

    // Update is called once per frame
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



    public void InitItem()
    {
        int x = Random.Range(-9, 9);
        float z = Random.Range(-1f, 1f);
        transform.localPosition = new Vector3(x, 0, z);

    }
}
