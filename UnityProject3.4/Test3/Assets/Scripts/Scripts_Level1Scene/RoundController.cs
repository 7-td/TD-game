using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class RoundController : MonoBehaviour
{
    public List<GameObject> gateList;
    private TowerController towerController;
    private EnemyController enemyController;
    private EnemySpawner enemySpawner;
    private UIHelper uiHelper;
    public int totalRound;
    private int currentRound;
    private int gameStep;//回合中的阶段，有0、1、2三个数值，0表示准备阶段，1表示30s攻方出兵阶段，2表示30s之后的阶段
    private float timerTime = 30.0f;
    private int gateCount;
    private bool waitflag = false;

    // Start is called before the first frame update
    void Start()
    {
        GeneralPool.resetPool();
        gateCount = gateList.Count;
        foreach (GameObject gate in gateList)
            GeneralPool.addToTowerList(gate);
        uiHelper = GetComponent<UIHelper>();
        towerController = GetComponent<TowerController>();
        enemyController = GetComponent<EnemyController>();
        enemySpawner = GetComponent<EnemySpawner>();
        currentRound = 0;
        toStep0();
    }

    public void ClearAll()
    {
        foreach (GameObject enemy in GeneralPool.get_enemyList())
        {
            Destroy(enemy);
        }
        foreach (GameObject tower in GeneralPool.get_towerList())
        {
            Destroy(tower);
        }
        GeneralPool.resetPool();
    }


    // Update is called once per frame
    void Update()
    {
        if (!waitflag)
        {
            if (gameStep == 0)
            {

            }
            else
            {
                List<GameObject> enemyList = GeneralPool.get_enemyList();
                //Debug.Log(enemyList.Count);
                if (enemyList.Count == 0)
                {
                    toStep0();
                }
                else
                {
                    bool stopFlag = true;
                    foreach (GameObject enemy in enemyList)
                    {
                        if (enemy.GetComponent<Enemy>().getEnemyState() != BasicInfo.enemyStates.Stop)
                        {
                            stopFlag = false;
                            break;
                        }
                    }
                    if (stopFlag)
                    {
                        toStep0();
                    }
                    else
                    {
                        if (gameStep == 1)
                        {
                            timerTime -= Time.deltaTime;
                            uiHelper.updateTimerText(timerTime);
                            if (timerTime < 0)
                            {
                                toStep2();
                            }
                        }
                        else if (gameStep == 2)
                        {

                        }
                    }
                }


                //Debug.Log(gateList.Count+","+gateCount);
                if (gateList.Count < gateCount)
                {
                    gameOver(0);
                }
            }
        }
    }


    /*
    跳转到30s出兵阶段（step 1）
     */
    public void toStep1()
    {
        //Debug.Log("进入30s出兵阶段");

        uiHelper.toStep1();
        uiHelper.ShowAlarm("进入出兵阶段");//waitflag=true;
        gameStep = 1;
        timerTime = 30.0f;

        enemySpawner.ClearAllEnemies();
        enemySpawner.switchtestwalkflag();
        enemySpawner.createEnemyForRound(currentRound, true);

        enemyController.setGameStep(1);
        towerController.setGameStep(1);
    }

    /*
    跳转到30s之后的交战阶段（step 2）
     */
    public void toStep2()
    {
        //Debug.Log("进入30s后的交战阶段");
        gameStep = 2;
        uiHelper.toStep2();
        //uiHelper.ShowAlarm("进入交战阶段");
        enemyController.setGameStep(2);
        towerController.setGameStep(2);
    }

    /*
    跳转到准备阶段（step 0），即跳入下一回合
     */
    public void toStep0()
    {
        currentRound++;
        GeneralPool.coins += GeneralPool.coinsPerRound;
        uiHelper.updateCoinCountText(GeneralPool.coins);
        if (currentRound > totalRound)
        {
            gameStep = 0;

            uiHelper.ShowAlarm("所有回合结束，游戏结束");
            waitflag = true;
            StartCoroutine(DelayToInvokeDo(
            () => { },
            150 * Time.deltaTime,
            () =>
            {
                gameOver(1);
            }));
        }
        else
        {

            //Debug.Log("进入准备阶段");
            gameStep = 0;
            uiHelper.toStep0();//进入下一回合的准备阶段时，修改ui
            uiHelper.updateRoundCountText(currentRound, totalRound);
            uiHelper.ShowAlarm("第" + currentRound.ToString() + "回合 进入准备阶段");

            if (currentRound == 1)//回合1的准备阶段进行asset bundle加载
            {
                enemySpawner.LoadEnemyPrefabs();
                enemyController.LoadEnemyIconPrefabs();
                towerController.LoadTowerPrefabsAndTowerIconSprite();

                StartCoroutine("waitForAssetsLoad");//等待加载

            }
            else
            {
                enemySpawner.switchtestwalkflag();
                enemySpawner.createEnemyForRound(currentRound, false);
                enemyController.setGameStep(0);
                towerController.setGameStep(0);
                enemyController.setRound(currentRound);
                towerController.setRound(currentRound);
            }


        }
    }

    IEnumerator waitForAssetsLoad()
    {
        while (!(enemySpawner.getLoadedFlag() & enemyController.getLoadedFlag() & towerController.getLoadedFlag()))
        {
            yield return null;
        }
        //加载完成！执行以下语句
        enemySpawner.switchtestwalkflag();
        enemySpawner.createEnemyForRound(currentRound, false);
        enemyController.setGameStep(0);
        towerController.setGameStep(0);
        enemyController.setRound(currentRound);
        towerController.setRound(currentRound);
        enemyController.loadedOK();
        towerController.loadedOK();

    }


    public void gameOver(int winner)//winner:0=enemy,1=tower
    {
        ClearAll();
        switch (winner)
        {
            case 0:
                {
                    GeneralPool.winner = 1;
                    Debug.Log("进攻方胜利");
                    SceneManager.LoadScene("ResultScene");
                    break;
                }
            case 1:
                {
                    GeneralPool.winner = 2;
                    Debug.Log("防御方胜利");
                    SceneManager.LoadScene("ResultScene");
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void removeFromGateList(GameObject gate)
    {
        if (gateList.Contains(gate))
        {
            gateList.Remove(gate);
        }
    }

    public static IEnumerator DelayToInvokeDo(UnityAction actionBefore, float delaySeconds, UnityAction actionAfter)
    {
        actionBefore();
        yield return new WaitForSeconds(delaySeconds);
        actionAfter();
    }


}
