using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBind : MonoBehaviour
{
    private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setEnemy(GameObject enemy)
    {
        this.enemy=enemy;
    }

    public GameObject getEnemy()
    {
        return enemy;
    }

}
