using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    private UIHelper uiHelper;
    public int totalRound;
    private int currentRound;
    private int gameStep;
    private float timerTime=30.0f;
    // Start is called before the first frame update
    void Start()
    {
        uiHelper=GetComponent<UIHelper>();
        currentRound=0;
        toStep0();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStep==0)
        {

        }
        else if(gameStep==1)
        {
            timerTime-=Time.deltaTime;
            uiHelper.updateTimerText(timerTime);
            if(timerTime<0)
            {
                toStep2();
            }
        }
        else if(gameStep==2)
        {

        }
    }


    /*
    跳转到30s出兵阶段（step 1）
     */
    public void toStep1()
    {
        Debug.Log("进入30s出兵阶段");
        gameStep=1;
        timerTime=30.0f;
        uiHelper.toStep1();
    }

    /*
    跳转到30s之后的交战阶段（step 2）
     */
    public void toStep2()
    {
        Debug.Log("进入30s后的交战阶段");
        gameStep=2;
        uiHelper.toStep2();
    }

    /*
    跳转到准备阶段（step 0），即跳入下一回合
     */
    public void toStep0()
    {
        currentRound++;
        if(currentRound>totalRound)
        {
            gameOver();
            Debug.Log("所有回合结束，游戏结束");
        }
        else
        {

            Debug.Log("进入准备阶段");
            gameStep=0;
            uiHelper.toStep0();//进入下一回合的准备阶段时，修改ui
            uiHelper.updateRoundCountText(currentRound,totalRound);
        }
    }

    public void gameOver()
    {

    }
}
