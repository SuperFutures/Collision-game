using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBallControll : MonoBehaviour
{
    public AudioSource GetCoin;
    



    // Use this for initialization
    void Awake()
    {
        Physics.IgnoreLayerCollision(9, 9);

    }

    void Start()
    {
        GetCoin = GetComponent<AudioSource>();
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
            GetCoin.Play();
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }



    public void InitItem()
    {
        int x = Random.Range(-9, 9);
        float z = Random.Range(-1f, 1f);
        transform.localPosition = new Vector3(x, 0, 0);

    }

    public void GetCoinSound()
    {
        GetCoin.Play();
    }
}
