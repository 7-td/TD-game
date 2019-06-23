using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;

public class Tower : MonoBehaviour
{
    public SimpleHealthBar healthBar;
    public Transform healthBarTransform;
    public Transform healthBarDefineTransform;
    public string towerName;
    public float towerTotalHealth;
    private float towerCurrentHealth;
    public BasicInfo.elementAttr towerAttr;//五行属性
    public float towerAttackPower;//攻击力
    public float towerAttackSpeed;//攻击速度
    public int towerAttackScope;//攻击范围
    public List<Vector2> towerDetectScope;//检测范围
    public float towerPrice;//价格




    private List<GameObject> detectedEnemyList = new List<GameObject>();

    private int RoundWhenCreated = 0;

    // Start is called before the first frame update
    void Start()
    {
        towerCurrentHealth = towerTotalHealth;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(healthBarDefineTransform.position);
        healthBarTransform.position = Camera.main.WorldToScreenPoint(healthBarDefineTransform.position);
        Detect();
        if (detectedEnemyList.Count > 0)
        {
            //Debug.Log("侦测到敌人");
            if (TimersManager.GetTimerByName(AttackTimer) == null)
            {
                TimersManager.SetTimer(this, 1f, AttackTimer);
            }
        }
    }

    void AttackTimer()
    {
        if (detectedEnemyList.Count > 0)
        {
            //Debug.Log("攻击一次");

            if (towerAttackScope > 0)//攻击范围大于0
            {
                List<Vector2> offList = new List<Vector2>();
                for (int i = 1; i <= towerAttackScope; ++i)
                {
                    for (int j = 0; j <= i; ++j)
                    {
                        int k = i - j;
                        if (j > 0 && k == 0)
                        {
                            offList.Add(new Vector2(j, k));
                            offList.Add(new Vector2(-j, k));
                        }
                        else if (j == 0 && k > 0)
                        {
                            offList.Add(new Vector2(j, k));
                            offList.Add(new Vector2(j, -k));
                        }
                        else if (j > 0 && k > 0)
                        {
                            offList.Add(new Vector2(j, k));
                            offList.Add(new Vector2(-j, k));
                            offList.Add(new Vector2(j, -k));
                            offList.Add(new Vector2(-j, -k));
                        }
                    }
                }
                List<Vector2> gridPosList = new List<Vector2>();
                foreach (Vector2 off in offList)
                {
                    Vector2 pos = new Vector2((float)Mathf.Floor(detectedEnemyList[0].transform.position.x) + 0.5f, (float)Mathf.Floor(transform.position.z) + 0.5f);
                    pos += off;
                    gridPosList.Add(pos);
                }

                List<Vector2> enemyPosList = new List<Vector2>();
                List<GameObject> enemyList = GeneralPool.get_enemyList();
                foreach (GameObject enemy in enemyList)
                {
                    Vector2 pos = new Vector2((float)Mathf.Floor(enemy.transform.position.x) + 0.5f, (float)Mathf.Floor(enemy.transform.position.z) + 0.5f);
                    enemyPosList.Add(pos);
                }

                List<GameObject> detectedEnemyList_inAttackScope = new List<GameObject>();
                detectedEnemyList_inAttackScope.Add(detectedEnemyList[0]);
                for (int i = 0; i < enemyPosList.Count; i++)
                {
                    for (int j = 0; j < gridPosList.Count; j++)
                    {
                        if (enemyPosList[i] == gridPosList[j])
                        {
                            detectedEnemyList_inAttackScope.Add(enemyList[i]);
                        }
                    }
                }
                foreach (GameObject detectedEnemy_inAttackScope in detectedEnemyList_inAttackScope)
                {
                    Enemy detectedEnemyComp_inAttackScope = detectedEnemy_inAttackScope.GetComponent<Enemy>();
                    float towerAttackPower_final = BasicInfo.calcuteAttackPower(towerAttackPower, this.towerAttr, detectedEnemyComp_inAttackScope.enemyAttr);
                    detectedEnemy_inAttackScope.GetComponent<Enemy>().GetAttacked(towerAttackPower_final);
                }


            }
            else
            {
                Enemy detectedEnemyComp = detectedEnemyList[0].GetComponent<Enemy>();
                float towerAttackPower_final = BasicInfo.calcuteAttackPower(towerAttackPower, this.towerAttr, detectedEnemyComp.enemyAttr);
                detectedEnemyComp.GetAttacked(towerAttackPower_final);
            }

        }
        else
        {
            TimersManager.ClearTimer(AttackTimer);
        }
    }

    private void Detect()
    {
        List<Vector2> gridPosList = new List<Vector2>();
        foreach (Vector2 off in towerDetectScope)
        {
            Vector2 pos = new Vector2((float)Mathf.Floor(transform.position.x) + 0.5f, (float)Mathf.Floor(transform.position.z) + 0.5f);
            pos += off;
            gridPosList.Add(pos);
        }

        List<Vector2> enemyPosList = new List<Vector2>();
        List<GameObject> enemyList = GeneralPool.get_enemyList();
        foreach (GameObject enemy in enemyList)
        {
            Vector2 pos = new Vector2((float)Mathf.Floor(enemy.transform.position.x) + 0.5f, (float)Mathf.Floor(enemy.transform.position.z) + 0.5f);
            enemyPosList.Add(pos);
        }

        //Debug.Log(gridPosList.Count.ToString() + "  " + enemyPosList.Count.ToString());

        detectedEnemyList.Clear();
        for (int i = 0; i < enemyPosList.Count; i++)
        {
            for (int j = 0; j < gridPosList.Count; j++)
            {
                if (enemyPosList[i] == gridPosList[j])
                {
                    detectedEnemyList.Add(enemyList[i]);
                }
            }
        }
    }
    public void GetAttacked(float decrease)
    {
        towerCurrentHealth -= decrease;
        if (towerCurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            healthBar.UpdateBar(towerCurrentHealth, towerTotalHealth);
        }
    }

    public void Die()
    {

        if (towerName == "Gate")
        {
            GameObject.Find("GameController").GetComponent<RoundController>().removeFromGateList(gameObject);
        }
        else
        {
            GameObject BP = GeneralPool.getBP(gameObject);
            if (BP == null)
            {

            }
            else
            {
                BP.SetActive(true);
            }
        }
        GeneralPool.removeFromTowerAndBPDict(gameObject);
        GeneralPool.removeFromTowerList(gameObject);
        Destroy(gameObject);

    }




    public GameObject getScopePlane()
    {
        GameObject scopePlane = new GameObject("scopePlane");
        scopePlane.transform.parent = this.transform;
        scopePlane.transform.position = new Vector3(0, 0, 0);
        return scopePlane;
    }

    public List<Vector3> getScopePlanePosList()
    {
        List<Vector3> scopePlanePosList = new List<Vector3>();
        foreach (Vector2 towerDetectScopeOffset in towerDetectScope)
        {
            Vector3 scopePlanePos = new Vector3(
                Mathf.Floor(transform.position.x) + 0.5f + towerDetectScopeOffset.x,
                transform.position.y - 1f,
                Mathf.Floor(transform.position.z) + 0.5f + towerDetectScopeOffset.y);
            scopePlanePosList.Add(scopePlanePos);
        }
        return scopePlanePosList;
    }

    public void setRoundWhenCreated(int currentRound)
    {
        RoundWhenCreated = currentRound;
    }

    public int getRoundWhenCreated()
    {
        return RoundWhenCreated;
    }

    public float getCurrentHealth()
    {
        return towerCurrentHealth;
    }

    public void setCurrentHealth(float newhealth)
    {
        towerCurrentHealth = newhealth;
        healthBar.UpdateBar(towerCurrentHealth, towerTotalHealth);
    }

}
