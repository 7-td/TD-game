using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Timers;

public class Enemy : MonoBehaviour
{
    public SimpleHealthBar healthBar;
    public Transform healthBarTransform;
    public Transform healthBarDefineTransform;
    public string enemyName;//兵种名称
    public float enemyTotalHealth;//总生命值
    private float enemyCurrentHealth;//当前生命值
    public float enemyMoveSpeed;//移动速度,每2s移动的距离
    public BasicInfo.elementAttr enemyAttr;//五行属性
    public float enemyAttackPower;//攻击力
    public float enemyAttackSpeed;//攻击速度,每2s攻击的次数
    public int enemyAttackScope;//攻击范围
    public List<Vector2> enemyDetectScope;//检测范围
    public float enemyPrice;//价格


    private List<GameObject> towerList = GeneralPool.get_towerList();
    private List<GameObject> detectedTowerList = new List<GameObject>();//检测到的塔
    private List<Vector3> pathList = new List<Vector3>();//路径
    private int nextWayPointIndex = 0;//路径下一个点的坐标
    private BasicInfo.enemyStates enemyState;
    private BasicInfo.enemyStates enemyLastState = BasicInfo.enemyStates.Walk;

    private Animation modelAnim;
    Tween pathMoveTween;
    private bool dieAlready = false;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.SetTweensCapacity(1000, 50);
        enemyCurrentHealth = enemyTotalHealth;
        modelAnim = transform.GetChild(0).GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {

        towerList = GeneralPool.get_towerList();
        healthBarTransform.position = Camera.main.WorldToScreenPoint(healthBarDefineTransform.position);
        Detect();
        if (enemyState == BasicInfo.enemyStates.Stop)
        {
            UpdateEnemyByState();
        }
        else
        {
            if (detectedTowerList.Count > 0)
            {
                enemyState = BasicInfo.enemyStates.Attack;
                UpdateEnemyByState();
                enemyLastState = BasicInfo.enemyStates.Attack;
            }
            else
            {
                enemyState = BasicInfo.enemyStates.Walk;
                UpdateEnemyByState();
                enemyLastState = BasicInfo.enemyStates.Walk;
            }
        }
        // UpdateEnemyByState();

    }
    public void UpdateEnemyByState()
    {
        if (!dieAlready)
        {
            switch (enemyState)
            {
                case BasicInfo.enemyStates.Walk:
                    {
                        pathMoveTween.Play();
                        //modelAnim.Play("walk");



                        if (enemyLastState == BasicInfo.enemyStates.Attack)
                            transform.DOLookAt(pathList[nextWayPointIndex], 0.3f, AxisConstraint.Y, Vector3.up);
                        break;
                    }
                case BasicInfo.enemyStates.Stop:
                    {
                        pathMoveTween.Pause();
                        //modelAnim.Play("idle");

                        break;
                    }
                case BasicInfo.enemyStates.Attack:
                    {
                        pathMoveTween.Pause();

                        transform.DOLookAt(detectedTowerList[0].transform.position, 0.3f, AxisConstraint.Y, Vector3.up);

                        modelAnim.Play("attack");




                        //Debug.Log("检测到塔");
                        //Debug.Log (2 / enemyAttackSpeed);
                        if (TimersManager.GetTimerByName(AttackTimer) == null)
                        {
                            TimersManager.SetTimer(this, 1f, AttackTimer);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
        }
    }

    void AttackTimer()
    {
        if (detectedTowerList.Count > 0)
        {
            //Debug.Log("攻击一次");
            Tower detectedTowerComp = detectedTowerList[0].GetComponent<Tower>();
            float enemyAttackPower_final = BasicInfo.calcuteAttackPower(enemyAttackPower, this.enemyAttr, detectedTowerComp.towerAttr);
            detectedTowerComp.GetAttacked(enemyAttackPower_final);
        }
        else
        {
            TimersManager.ClearTimer(AttackTimer);
        }
    }
    private void Detect()
    {
        List<Vector2> gridPosList = new List<Vector2>();
        Vector2 pos = new Vector2((float)Mathf.Floor(transform.position.x) + 0.5f, (float)Mathf.Floor(transform.position.z) + 0.5f);
        foreach (Vector2 off in enemyDetectScope)
        {
            Vector2 temppos = pos;
            temppos += off;
            gridPosList.Add(temppos);
        }

        List<Vector2> towerPosList = new List<Vector2>();
        towerList = GeneralPool.get_towerList();
        //Debug.Log(towerList.Count);
        foreach (GameObject tower in towerList)
        {
            Vector2 towerpos = new Vector2((float)Mathf.Floor(tower.transform.position.x) + 0.5f, (float)Mathf.Floor(tower.transform.position.z) + 0.5f);
            towerPosList.Add(towerpos);
        }

        detectedTowerList.Clear();
        //List<Vector2> detectedTowerPosList_toGridList = new List<Vector2>();
        for (int i = 0; i < towerPosList.Count; i++)
        {
            for (int j = 0; j < gridPosList.Count; j++)
            {
                //Debug.Log(towerPosList[i].x.ToString() + "," + towerPosList[i].y.ToString() + "/" + gridPosList[i].x.ToString() + "," + gridPosList[i].y.ToString());
                if (towerPosList[i] == gridPosList[j])
                {
                    detectedTowerList.Add(towerList[i]);
                    //detectedTowerPosList_toGridList.Add(new Vector2(towerList[i].transform.position.x, towerList[i].transform.position.y) - pos);
                }
            }
        }
        // Debug.Log("检测到的塔："+detectedTowerList.Count);
        // Debug.Log("塔总数："+towerList.Count);


        // detectedTowerList.Sort(delegate(GameObject t1,GameObject t2){
        //     Vector2 t1Vec2=new Vector2(t1.transform.position.x,t1.transform.position.z)-pos;
        //     float t1Vec2Len=t1Vec2.magnitude;
        //     Vector2 t2Vec2=new Vector2(t2.transform.position.x,t2.transform.position.z)-pos;
        //     float t2Vec2Len=t2Vec2.magnitude;
        //     return t1Vec2Len.CompareTo(t2Vec2Len);            
        // });
        // foreach (GameObject tower in detectedTowerList)
        // {
        //     Debug.Log(tower.transform.position);
        // }

    }

    public void InitWithData(GameObject path)
    {
        int pathNodeCount = path.transform.childCount;
        // if(pathNodeCount>1)
        // {
        //     transform.DOLookAt(path.transform.GetChild(1).transform.position,0,AxisConstraint.Y,Vector3.up);
        // }
        for (int i = 0; i < pathNodeCount; ++i)
        {
            pathList.Add(path.transform.GetChild(i).transform.position);
        }
        Vector3[] pathArray = pathList.ToArray();
        float duration = (pathNodeCount - 1) / (enemyMoveSpeed / 2);

        pathMoveTween = this.transform.DOPath(pathArray, duration, PathType.Linear, PathMode.Full3D, 10, new Color(20, 20, 20));
        pathMoveTween.OnWaypointChange(LookToNextPoint);

        pathMoveTween.Pause();
        enemyState = BasicInfo.enemyStates.Stop;


        pathMoveTween.OnKill(delegate
        {
            enemyState = BasicInfo.enemyStates.Stop;
        });
    }

    public void LookToNextPoint(int waitpointIndex)
    {
        if (waitpointIndex < pathList.Count - 1)
        {
            transform.DOLookAt(pathList[waitpointIndex + 1], 0.3f, AxisConstraint.Y, Vector3.up);
            nextWayPointIndex++;
        }

    }


    public void StartWalk()
    {
        enemyState = BasicInfo.enemyStates.Walk;
    }

    public void GetAttacked(float decrease)
    {
        enemyCurrentHealth -= decrease;
        if (enemyCurrentHealth <= 0)
        {

            Die();

        }
        else
        {
            healthBar.UpdateBar(enemyCurrentHealth, enemyTotalHealth);
        }
    }

    public void Die()
    {
        dieAlready = true;
        Debug.Log("当前敌人死亡前，enemyList数量为：" + GeneralPool.get_enemyList().Count);
        GeneralPool.removeFromEnemyGroup(gameObject);

        modelAnim.Play("death");
        Invoke("destroyGameObject", 3f);


    }
    void destroyGameObject()
    {
        Destroy(gameObject);
    }
    public BasicInfo.enemyStates getEnemyState()
    {
        return enemyState;
    }

    public List<Vector3> getPathList()
    {
        return pathList;
    }

    public float getEnemyCurrentHealth()
    {
        return enemyCurrentHealth;
    }

}
