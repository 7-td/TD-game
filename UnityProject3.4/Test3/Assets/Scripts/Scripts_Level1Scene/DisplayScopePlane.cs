using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScopePlane : MonoBehaviour
{
    //private LineGenerator lineGenerator;
    //private Transform path;//一条路径的父节点
    public GameObject scopePlanePrefab;
    //List<GameObject> createdScopePlaneList=new List<GameObject>();
    List<Vector3> scopePosList;//路径上按序所有点的坐标
    // Start is called before the first frame update
    void Start()
    {
        //lineGenerator=GetComponent<LineGenerator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayScopeOfTower(Transform scope,Transform parent)
    {
        scopePosList=new List<Vector3>();
        for(int i=0;i<scope.childCount;++i)
        {
            scopePosList.Add(scope.GetChild(i).position+new Vector3(0,0.2f,0));
        }
        // lineGenerator.Interval=0.5f;//修改lineGenerator生成物体的距离
        // lineGenerator.nodePosition=scopePosList;//修改lineGenerator的line的节点集合
        // lineGenerator.Generator();//在line上生成物体
        foreach(Vector3 scopePos in scopePosList)
        {
            GameObject scopePlane=Instantiate(scopePlanePrefab,scopePos,new Quaternion());
            scopePlane.transform.parent=parent;
            //createdScopePlaneList.Add(scopePlane);
        }

    }
    public void DisplayScopeOfTower(List<Vector3> scopePosList,Transform parent)
    {
        this.scopePosList=scopePosList;
        // lineGenerator.Interval=0.5f;//修改lineGenerator生成物体的距离
        // lineGenerator.nodePosition=scopePosList;//修改lineGenerator的line的节点集合
        // lineGenerator.Generator();//在line上生成物体
        foreach(Vector3 scopePos in scopePosList)
        {
            GameObject scopePlane=Instantiate(scopePlanePrefab,scopePos,new Quaternion());
            scopePlane.transform.parent=parent;
            //createdScopePlaneList.Add(scopePlane);
        }

    }

    // public void HideScopeOfTower()
    // {
    //     //lineGenerator.AllDelete();//把生成的物体全部删除
    //     foreach(GameObject scopePlane in createdScopePlaneList)
    //     {
    //         Destroy(scopePlane);
    //     }
    //     createdScopePlaneList.Clear();
    // }
}
