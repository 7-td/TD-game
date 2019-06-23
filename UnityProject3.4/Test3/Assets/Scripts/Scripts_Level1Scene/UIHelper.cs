using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//using Timers;

public class UIHelper : MonoBehaviour
{
    public Text alarmText; public Animation showAlarmAnimation;
    public GameObject menuUI;
    public GameObject technologyTreeUI;
    public GameObject timerTextObject;
    private Text timerText;
    public Text roundCountText;
    public Text coinCountText;
    public Text techCountText;
    public GameObject prepareCanvas;
    public GameObject battleCanvas;
    public Camera mainCamera;
    public Animation mainCameraAnimation;
    private Rect prepareCameraRect = new Rect(0.0f, 0.2f, 1.0f, 1.0f);
    private Rect battleCameraRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);



    // Start is called before the first frame update
    void Start()
    {
        timerText = timerTextObject.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    


    public void updateRoundCountText(int currentRound, int totalRound)
    {
        roundCountText.text = currentRound.ToString() + "/" + totalRound.ToString();
    }

    public void updateCoinCountText(float coins)
    {
        coinCountText.text = coins.ToString();
    }

    public void updateTimerText(float timer)
    {
        int timerInt = Mathf.CeilToInt(timer);
        timerText.text = timerInt.ToString() + "s";
    }



    public void toStep1()
    {
        timerTextObject.SetActive(true);
        prepareCanvas.SetActive(false);//Debug.Log("prepareCanvas.SetActive(false);");
        battleCanvas.SetActive(true);//Debug.Log("battleCanvas.SetActive(true);");
        //mainCamera.rect=battleCameraRect;
        //TimersManager.SetLoopableTimer(this, 0.01f, loopFadeOut);
        mainCameraAnimation.Play("ViewportFadeOutAnimation");
    }

    public void toStep2()
    {
        timerTextObject.SetActive(false);
    }

    public void toStep0()
    {
        timerTextObject.SetActive(false);
        battleCanvas.SetActive(false);//Debug.Log("battleCanvas.SetActive(false);");
        prepareCanvas.SetActive(true);//Debug.Log("prepareCanvas.SetActive(true);");
        mainCamera.rect = prepareCameraRect;
        //TimersManager.SetLoopableTimer(this, 0.01f, loopFadeIn);
    }


    public void showMenu()
    {
        menuUI.SetActive(true);
    }

    public void hideMnu()
    {
        menuUI.SetActive(false);
    }

    public void showTechnologyTree()
    {
        technologyTreeUI.SetActive(true);
    }

    public void hideTechnologyTree()
    {
        technologyTreeUI.SetActive(false);
    }

    public void ShowAlarm(string alarmTextStr)//展示1秒40帧（即100帧的提醒）
    {
        alarmText.text = alarmTextStr;
        showAlarmAnimation.Play();
    }

    


}
