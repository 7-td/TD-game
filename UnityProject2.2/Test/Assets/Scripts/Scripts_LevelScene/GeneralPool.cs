using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralPool
{
    // Start is called before the first frame update
    static private Dictionary<GameObject, GameObject> towerAndBPDict = new Dictionary<GameObject, GameObject>();//塔和可建造面的映射词典
    static private List<GameObject> towerList = new List<GameObject>();//塔列表
    static private List<GameObject> enemyList = new List<GameObject>();//敌人列表
    static public void addToTowerList(GameObject tower)
    {
        towerList.Add(tower);
        for (int i = 0; i < enemyList.Count; i++)
        {
            //enemyList[i].GetComponent<Enemy>().updatexxx();
        }
    }
    static public void removeFromTowerList(GameObject tower)
    {
        if (towerList.Contains(tower))
        {
            towerList.Remove(tower);
        }
    }
    static public void addToEnemyList(GameObject enemy)
    {
        enemyList.Add(enemy);
    }
    static public void removeFromEnemyList(GameObject enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
        }
    }
    static public void addToTowerAndBPDict(GameObject tower, GameObject BP)
    {
        towerAndBPDict.Add(tower, BP);
    }
    static public void removeFromTowerAndBPDict(GameObject tower)
    {
        if (towerAndBPDict.ContainsKey(tower))
        {
            towerAndBPDict.Remove(tower);
        }
    }

    static public void resetPool()
    {
        towerAndBPDict.Clear();
        towerList.Clear();
        enemyList.Clear();
    }

     static public Dictionary<GameObject, GameObject> get_towerAndBPDict()
     {
         return towerAndBPDict;
     }
    static public List<GameObject> get_towerList()
     {
         return towerList;
     }
    static public List<GameObject> get_enemyList()
     {
         return enemyList;
     }
}
