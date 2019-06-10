using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private List<GameObject> enemyList=GeneralPool.get_enemyList();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        detect();
    }

    private bool detect()
    {
        for(int i=0;i<enemyList.Count;++i)
        {
            float distance=Vector3.Distance(enemyList[i].transform.position,this.transform.position);
            if(distance<1.5)
            {
                Debug.Log("检测到敌人");
                return true;
            }
        }
        return false;
    }

    public void updateEnemyList(List<GameObject> enemyList)
    {
        this.enemyList=enemyList;
    }

    
    
}
