using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControll : MonoBehaviour
{

    public static GameControll Instance;
    public GameObject[] FallBallPrefabs;
    public GameObject[] BombPrefabs;

    public List<GameObject> FallBalls = new List<GameObject>();
    public List<FallBallControll> FallBallControll = new List<FallBallControll>();

    public GameObject BallParent;

    private float _timer = 0;
    private float _timer0 = 0;
    private float _timer1 = 0;

    

    void Awake()
    {
       
    }
    // Use this for initialization
    void Start()
    {


        if (_timer0 == 0)
        {

            InitFallBalls(FallBallPrefabs.Length);

        }
       

    }

    // Update is called once per frame
    void Update()
    {

        _timer += Time.fixedDeltaTime;
        var times = _timer / 2;
        if (times > 8)
        {
            times = 8;
        }

        _timer0 += Time.fixedDeltaTime;
        _timer1 += Time.fixedDeltaTime;

        if (_timer0 >= 0.4f)
        {

            InitFallBalls(times);

            _timer0 = 0;
        }

        //BOMB

        if (_timer1 >= 1.5f)
        {

            InitBomb(3);
            _timer1 = 0;
        }




    }

    public void InitFallBalls(float times)
    {
        
        for (int i = 0; i < times; i++)
        {
            int index = Random.Range(0, FallBallPrefabs.Length);
            var ball = Instantiate(FallBallPrefabs[index]);
            FallBalls.Add(ball);
            ball.transform.SetParent(BallParent.transform);
            var ballcontroll = ball.GetComponent<FallBallControll>();
            if (ballcontroll)
            {
                FallBallControll.Add(ballcontroll);
                ballcontroll.InitItem();
            }
        }
    }

    public void InitBomb(int times)
    {
        for (int i = 0; i < times; i++)
        {
            int index = Random.Range(0, BombPrefabs.Length);
            var Bomb = Instantiate(BombPrefabs[index]);
            var BombControll = Bomb.GetComponent<BombControll>();
            if (BombControll)
            {
                BombControll.InitBombItems();
            }
        }
    }

 
}
