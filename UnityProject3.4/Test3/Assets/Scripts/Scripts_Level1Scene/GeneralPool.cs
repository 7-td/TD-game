using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeneralPool
{
    static public int player;//玩家阵营，0表示未确定、1表示进攻方、2表示防守方
    static public int winner;//胜利者，0表示未确定、1表示进攻方、2表示防守方

    static public float coins;//金币总数

    static public float coinsPerRound;//金币数量，默认为600.0f 
    // Start is called before the first frame update
    static private Dictionary<GameObject, GameObject> towerAndBPDict = new Dictionary<GameObject, GameObject>();//塔和可建造面的映射词典
    static private List<GameObject> towerList = new List<GameObject>();//塔列表
    static private List<List<GameObject>> enemyGroupList = new List<List<GameObject>>();//敌人列表
    static public bool addToTowerList(GameObject tower)
    {
        towerList.Add(tower);
        return true;
    }
    static public bool removeFromTowerList(GameObject tower)
    {
        if (towerList.Contains(tower))
        {
            towerList.Remove(tower);
            return true;
        }
        return false;
    }

    static public int addToEnemyGroupList(List<GameObject> enemyList)
    {
        enemyGroupList.Add(enemyList);
        return enemyGroupList.Count - 1;
    }
    static public bool removeFromEnemyGroupList(List<GameObject> enemyList)
    {
        if (enemyGroupList.Contains(enemyList))
        {
            enemyGroupList.Remove(enemyList);
            return true;
        }
        return false;
    }
    static public bool addToEnemyGroup(GameObject enemy, int i)//i表示军团编号
    {
        if (i < enemyGroupList.Count)
        {
            enemyGroupList[i].Add(enemy);
            return true;
        }
        return false;
    }
    static public bool removeFromEnemyGroup(GameObject enemy)
    {
        foreach (List<GameObject> enemyGroup in enemyGroupList)
        {
            if (enemyGroup.Contains(enemy))
            {
                enemyGroup.Remove(enemy);
                return true;
            }
        }
        return false;
    }
    static public bool addToTowerAndBPDict(GameObject tower, GameObject BP)
    {
        towerAndBPDict.Add(tower, BP);
        return true;
    }
    static public bool removeFromTowerAndBPDict(GameObject tower)
    {
        if (towerAndBPDict.ContainsKey(tower))
        {
            towerAndBPDict.Remove(tower);
            return true;
        }
        return false;
    }

    static public void resetPool()
    {
        winner=0;
        coins=0;
        coinsPerRound=600.0f;
        towerAndBPDict.Clear();
        towerList.Clear();
        foreach (List<GameObject> enemyList in enemyGroupList)
        {
            enemyList.Clear();
        }
        enemyGroupList.Clear();
    }



    static public Dictionary<GameObject, GameObject> get_towerAndBPDict()
    {
        return towerAndBPDict;
    }

    static public GameObject getBP(GameObject tower)
    {
        if (towerAndBPDict.ContainsKey(tower))
            return towerAndBPDict[tower];
        else return null;
    }
    static public List<GameObject> get_towerList()
    {
        return towerList;
    }
    static public List<List<GameObject>> get_enemyGroupList()
    {
        return enemyGroupList;
    }
    static public int get_enemyGroupIndex(GameObject enemy)
    {
        foreach(List<GameObject> enemyGroup in enemyGroupList)
        {
            if(enemyGroup.Contains(enemy))
            {
                return enemyGroup.FindIndex(a=>a==enemy);
            }
        }
        return -1;
    }

    
    static public List<GameObject> get_enemyList(int i)
    {
        return enemyGroupList[i];
    }
    static public List<GameObject> get_enemyList()
    {
        List<GameObject> enemyList = new List<GameObject>();
        foreach (List<GameObject> list in enemyGroupList)
        {
            foreach (GameObject enemy in list)
            {
                enemyList.Add(enemy);
            }
        }
        return enemyList;
    }


}
