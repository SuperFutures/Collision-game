using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Button PauseBtn;
    public Button StartBtn;
    public Button ExitBtn;

    void Awake()
    {
        StartBtn.gameObject.SetActive(false);
        ExitBtn.gameObject.SetActive(false);



    }
	// Use this for initialization
	void Start () {

	    PauseBtn.onClick.AddListener(Pause);
	    StartBtn.onClick.AddListener(StartMth);
	    ExitBtn.onClick.AddListener(Exit);
    }
	
	// Update is called once per frame
	void Update () {
		
	}




    void Pause()
    {
        Time.timeScale =0;
        PauseBtn.gameObject.SetActive(false);
        StartBtn.gameObject.SetActive(true);
        ExitBtn.gameObject.SetActive(true);

    }

    void StartMth()
    {
        Time.timeScale = 1;
        StartBtn.gameObject.SetActive(false);
        ExitBtn.gameObject.SetActive(false);
    }

    void Exit()
    {
        Application.Quit();

    }
}
