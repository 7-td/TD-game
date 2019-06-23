using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    private bool ready = false;

    private List<GameObject> enemyList;

    private List<GameObject> path = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsReady()
    {
        return path != null;
    }

    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }

    public void SetPath(List<GameObject> path)
    {
        this.path = new List<GameObject>(path);
    }

    public List<GameObject> GetPath()
    {
        return path;
    }
}
