using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundleFramework;


public class EnemySpawner : MonoBehaviour, ISetList
{
    /*   编号        对应EnemyPrefab名
          0         MageEnemyFirePrefab;
          1         MageEnemyGrassPrefab;
          2         MageEnemyGroundPrefab;
          3         MageEnemyMetalPrefab;
          4         MageEnemyWaterPrefab;

          5         SoldierEnemyFirePrefab;
          6         SoldierEnemyGrassPrefab;
          7         SoldierEnemyGroundPrefab;
          8         SoldierEnemyMetalPrefab;
          9         SoldierEnemyWaterPrefab;

          10        SuperiorMageEnemyFirePrefab;
          11        SuperiorMageEnemyGrassPrefab;
          12        SuperiorMageEnemyGroundPrefab;
          13        SuperiorMageEnemyMetalPrefab;
          14        SuperiorMageEnemyWaterPrefab;

          15        SuperiorSoldierEnemyFirePrefab;
          16        SuperiorSoldierEnemyGrassPrefab;
          17        SuperiorSoldierEnemyGroundPrefab;
          18        SuperiorSoldierEnemyMetalPrefab;
          19        SuperiorSoldierEnemyWaterPrefab;
    */

    public Transform EnemyParent;
    private List<GameObject> EnemyPrefabsList; private bool loadedflag = false;

    public List<GameObject> AllPath;
    private bool tempWalkFlag = true;



    // Start is called before the first frame update
    void Start()
    {

        EnemyPrefabsList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadEnemyPrefabs()
    {
        StartCoroutine(AssetbundleLoader.LoadResOfType<GameObject>("prefabs/enemyprefabs", this));
    }

    public void setList<T>(List<T> TList)
    {
        List<GameObject> GameObjectList = new List<GameObject>();
        foreach (T t in TList)
        {
            GameObjectList.Add(t as GameObject);
        }
        EnemyPrefabsList = GameObjectList;
        Debug.Log("加载EnemyPrefabs种类数量为：" + EnemyPrefabsList.Count);
        loadedflag = true;
    }

    public bool getLoadedFlag()
    {
        return loadedflag;
    }

    public void createEnemyForRound(int round, bool waitFlag)
    {
        List<GameObject> enemyPrefabList = new List<GameObject>();
        List<int> countList = new List<int>();
        switch (round)
        {
            case 1:
                {

                    StartCoroutine(SpawnEnemy(1, new int[] { 15 }, new int[] { 16 }, waitFlag));
                    break;
                }
            case 2:
                {
                    StartCoroutine(SpawnEnemy(2, new int[] { 0 }, new int[] { 16 }, waitFlag));
                    break;
                }
            case 3:
                {
                    StartCoroutine(SpawnEnemy(1, new int[] { 5, 0 }, new int[] { 5, 5 }, waitFlag));
                    StartCoroutine(SpawnEnemy(2, new int[] { 5, 0 }, new int[] { 5, 5 }, waitFlag));
                    break;
                }
            case 4:
                {
                    StartCoroutine(SpawnEnemy(1, new int[] { 9 }, new int[] { 10 }, waitFlag));
                    StartCoroutine(SpawnEnemy(2, new int[] { 9 }, new int[] { 10 }, waitFlag));
                    break;
                }
            case 5:
                {
                    StartCoroutine(SpawnEnemy(1, new int[] { 3 }, new int[] { 10 }, waitFlag));
                    StartCoroutine(SpawnEnemy(2, new int[] { 3 }, new int[] { 10 }, waitFlag));
                    break;
                }
            case 6:
                {
                    StartCoroutine(SpawnEnemy(0, new int[] { 2, 7 }, new int[] { 7, 7 }, waitFlag));
                    StartCoroutine(SpawnEnemy(3, new int[] { 2, 7 }, new int[] { 7, 7 }, waitFlag));
                    break;
                }
            case 7:
                {
                    StartCoroutine(SpawnEnemy(0, new int[] { 2, 5 }, new int[] { 7, 7 }, waitFlag));
                    StartCoroutine(SpawnEnemy(3, new int[] { 2, 5 }, new int[] { 7, 7 }, waitFlag));
                    break;
                }
            case 8:
                {
                    StartCoroutine(SpawnEnemy(1, new int[] { 2, 5 }, new int[] { 8, 8 }, waitFlag));
                    StartCoroutine(SpawnEnemy(2, new int[] { 2, 5 }, new int[] { 8, 8 }, waitFlag));
                    break;
                }
            case 9:
                {
                    StartCoroutine(SpawnEnemy(0, new int[] { 2, 5 }, new int[] { 4, 4 }, waitFlag));
                    StartCoroutine(SpawnEnemy(1, new int[] { 2, 5 }, new int[] { 4, 4 }, waitFlag));
                    StartCoroutine(SpawnEnemy(2, new int[] { 2, 5 }, new int[] { 4, 4 }, waitFlag));
                    StartCoroutine(SpawnEnemy(3, new int[] { 2, 5 }, new int[] { 4, 4 }, waitFlag));
                    break;
                }
            case 10:
                {
                    StartCoroutine(SpawnEnemy(1, new int[] { 15, 2, 5 }, new int[] { 1, 4, 4 }, waitFlag));
                    StartCoroutine(SpawnEnemy(0, new int[] { 2, 5 }, new int[] { 4, 4 }, waitFlag));
                    StartCoroutine(SpawnEnemy(2, new int[] { 2, 5 }, new int[] { 4, 4 }, waitFlag));
                    StartCoroutine(SpawnEnemy(3, new int[] { 2, 5 }, new int[] { 4, 4 }, waitFlag));
                    break;
                }




        }

    }

    IEnumerator SpawnEnemy(int pathIndex, int[] prefabIndex, int[] count, bool waitFlag)
    {
        List<GameObject> curEnemyGroup = new List<GameObject>();
        int curEnemyGroupIndex = GeneralPool.addToEnemyGroupList(curEnemyGroup);
        for (int i = 0; i < prefabIndex.Length; i++)
        {
            for (int j = 0; j < count[i]; j++)
            {
                GameObject enemy = GameObject.Instantiate(EnemyPrefabsList[prefabIndex[i]], AllPath[pathIndex].transform.GetChild(0).transform.position, Quaternion.identity);
                enemy.transform.parent = EnemyParent;
                if (AllPath[pathIndex].transform.childCount > 1)
                {
                    enemy.transform.LookAt(AllPath[pathIndex].transform.GetChild(1).transform.position, Vector3.up);
                }
                Enemy enemyComp = enemy.GetComponent<Enemy>();
                enemyComp.InitWithData(AllPath[pathIndex]);

                GeneralPool.addToEnemyGroup(enemy, curEnemyGroupIndex);

                if (tempWalkFlag)
                    enemyComp.StartWalk();
                if (waitFlag)
                    yield return new WaitForSeconds(0.5f);
                else
                    yield return null;
            }
        }

    }

    public void switchtestwalkflag()
    {
        tempWalkFlag = !tempWalkFlag;
    }

    public void ClearAllEnemies()
    {
        foreach (GameObject enemy in GeneralPool.get_enemyList())
        {
            Destroy(enemy);
        }
        foreach (List<GameObject> enemyGroup in GeneralPool.get_enemyGroupList())
        {
            enemyGroup.Clear();
        }
        GeneralPool.get_enemyGroupList().Clear();
    }
}
