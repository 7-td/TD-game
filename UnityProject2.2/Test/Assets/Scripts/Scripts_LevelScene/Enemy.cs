using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private List<GameObject> towerList=GeneralPool.get_towerList();
    private List<Vector3> pathList = new List<Vector3>();
    int pathIndex = 0;
    private Vector3 targetPos = Vector3.zero;
    private bool distanceFlag = true;
    private float detectRadius=0.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Detect())
        {

        }
        else
        {
            float distance = Vector3.Distance(transform.position, targetPos);
            if (distance <= 0.1f && distanceFlag)
            {
                //Debug.Log("到达一个路径点");
                pathIndex++;
                targetPos = Vector3.zero;
                distanceFlag = false;
            }
            else
            {
                if (distance > 0.1f)
                    distanceFlag = true;
            }

            if (pathIndex > pathList.Count)
            { }
            else if (pathIndex == pathList.Count)
            {
                //Debug.Log("到达终点了");
                GeneralPool.removeFromEnemyList(this.gameObject);
                Destroy(this.gameObject);

            }
            else
            {
                if (pathList.Count != 0 && targetPos.Equals(Vector3.zero))
                {
                    targetPos = pathList[pathIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.05f);
            }
        }
    }

    private bool Detect()
    {

        return false;
    }

    public void InitWithData(GameObject path, EnemyController enemyController)
    {
        int pathNodeCount = path.transform.childCount;
        for (int i = 0; i < pathNodeCount; ++i)
        {
            pathList.Add(path.transform.GetChild(i).transform.position);
        }
    }
}
