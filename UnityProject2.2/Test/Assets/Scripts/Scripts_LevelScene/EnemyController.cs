using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject enemyPrefab;
    
    private TowerController towerController;
    // Start is called before the first frame update
    
    void Start()
    {
        towerController=this.transform.GetComponent<TowerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void createEnemy(GameObject path)
    {
        Debug.Log("开始创建enemy");
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.parent = transform;
        enemy.transform.position = path.transform.GetChild(0).transform.position;
        enemy.transform.GetComponent<Enemy>().InitWithData(path,this);
        GeneralPool.addToEnemyList(enemy);
        
    }

}