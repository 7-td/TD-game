using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AssetBundleFramework;


public class EnemyController : MonoBehaviour, ISetList
{
    public Transform enemyParent;
    public Transform enemyScrollViewContent_Prepare;
    public Transform enemyScrollViewContent_Battle;
    public GameObject enemyIconFramePrefab_Prepare;
    public GameObject enemyIconFramePrefab_Battle;
    public DisplayPath displayPath;

    private UIHelper uiHelper;

    private int enemyKindsCount = BasicInfo.enemyKinds.Length;
    private Dictionary<string, int> enemyCountsDict = new Dictionary<string, int>();

    private int currentRound = 0;
    private int gameStep = 0;


    // Start is called before the first frame update
    private bool loadedflag_inner = false; public void loadedOK() { loadedflag_inner = true; }

    private float hitpoint_x_center;
    private float hitpoint_z_center;
    void Start()
    {
        loadedflag_inner = false;
        uiHelper = GetComponent<UIHelper>();
        enemyCountsDict.Clear();
        for (int i = 0; i < enemyKindsCount; ++i)
        {
            enemyCountsDict.Add(BasicInfo.enemyKinds[i], 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (loadedflag_inner)
        {
            if (CheckGuiRaycastObjects()) return;
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 定义射线
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, 200)) // 参数1 射线，参数2 碰撞信息， 参数3 碰撞层
                {
                    GameObject rayHitObject = rayHit.collider.gameObject;
                    //Debug.Log(rayHitObject.name);
                    if (rayHitObject.name == "Road1" || rayHitObject.name == "Road2")
                    {
                        Vector3 hitpoint = rayHit.point;
                        float hitpoint_x = hitpoint.x;
                        float hitpoint_y = hitpoint.y;
                        float hitpoint_z = hitpoint.z;
                        hitpoint_x_center = Mathf.Floor(hitpoint_x) + 0.5f;
                        hitpoint_z_center = Mathf.Floor(hitpoint_z) + 0.5f;
                        displayPath.HidePathOfEnemyGroup();
                        foreach (GameObject enemy in GeneralPool.get_enemyList())
                        {
                            enemy.GetComponent<QuickOutline>().enabled = false;
                        }
                    }
                    else if (rayHitObject.tag == "Tower" || rayHitObject.tag == "BuildablePlane")
                    {

                    }
                    else
                    {
                        hitpoint_x_center = 1000f;
                        hitpoint_z_center = 1000f;
                        displayPath.HidePathOfEnemyGroup();
                        foreach (GameObject enemy in GeneralPool.get_enemyList())
                        {
                            enemy.GetComponent<QuickOutline>().enabled = false;
                        }
                    }
                }

                List<GameObject> insideEnemyList = new List<GameObject>();
                foreach (GameObject enemy in GeneralPool.get_enemyList())
                {
                    //displayPath.HidePathOfEnemyGroup();
                    //enemy.GetComponent<QuickOutline>().enabled = false;
                    if (Mathf.Floor(enemy.transform.position.x) + 0.5f == hitpoint_x_center && Mathf.Floor(enemy.transform.position.z) + 0.5f == hitpoint_z_center)
                    {
                        insideEnemyList.Add(enemy);
                    }
                }
                //List<int> insideEnemyGroupIndexs = new List<int>();
                foreach (GameObject insideEnemy in insideEnemyList)
                {
                    insideEnemy.GetComponent<QuickOutline>().enabled = true;
                    //int enemyGroupIndex = GeneralPool.get_enemyGroupIndex(insideEnemy);
                    //if (enemyGroupIndex == -1)
                    //{
                    //    Debug.Log("存在Bug：某一enemy没有加入军团");
                    //}
                    //else
                    //{
                    //    if (!insideEnemyGroupIndexs.Contains(enemyGroupIndex))
                    //        insideEnemyGroupIndexs.Add(enemyGroupIndex);
                    //}
                }

                changeEnemyScrollView_and_DisplayPath(insideEnemyList);
            }
            updateEnemyScrollView();
        }
    }



    public void changeEnemyScrollView_and_DisplayPath(List<GameObject> insideEnemyList)
    {

        if (gameStep == 0)//准备阶段对EnemyScrollView和DisplayPath的操作
        {
            if (insideEnemyList.Count > 0)
            {
                displayPath.HidePathOfEnemyGroup();
                List<Vector3> pathList = insideEnemyList[0].GetComponent<Enemy>().getPathList();
                List<Vector3> newPathList = new List<Vector3>(pathList);
                for (int i = 0; i < pathList.Count; ++i)
                {
                    newPathList[i] += new Vector3(0, 0.02f, 0);
                }
                displayPath.DisplayPathOfEnemyGroup(newPathList);
            }

            for (int i = 0; i < enemyScrollViewContent_Prepare.childCount; ++i)
            {
                Destroy(enemyScrollViewContent_Prepare.GetChild(i).gameObject);
            }

            for (int i = 0; i < enemyKindsCount; ++i)
            {
                enemyCountsDict[BasicInfo.enemyKinds[i]] = 0;
            }

            foreach (GameObject insideEnemy in insideEnemyList)
            {
                Enemy insideEnemyComp = insideEnemy.GetComponent<Enemy>();
                string EnemyFullName = insideEnemyComp.enemyName + insideEnemyComp.enemyAttr.ToString();
                //Debug.Log(EnemyFullName);
                if (enemyCountsDict.ContainsKey(EnemyFullName))
                    enemyCountsDict[EnemyFullName]++;
            }
            for (int i = 0; i < enemyKindsCount; ++i)
            {
                string curEnemyKindName = BasicInfo.enemyKinds[i];
                int curEnemyCount = enemyCountsDict[curEnemyKindName];


                if (curEnemyCount > 0)
                {

                    if ((EnemyIconSpriteList.FindIndex(a => (a.name == curEnemyKindName + "Icon")) != -1))
                    {
                        GameObject enemyIcon = Instantiate(enemyIconFramePrefab_Prepare, new Vector3(1000, 1000, 0), new Quaternion());

                        enemyIcon.transform.Find("Text").GetComponent<Text>().text = "x" + curEnemyCount;//修改显示的数量
                        //还要修改图片，待完善
                        enemyIcon.transform.Find("Image").GetComponent<Image>().sprite = EnemyIconSpriteList.Find(a => (a.name == curEnemyKindName + "Icon"));
                        enemyIcon.transform.SetParent(enemyScrollViewContent_Prepare);//设置父对象
                        enemyIcon.transform.localScale = new Vector3(1, 1, 1);//修改缩放值
                    }

                }
            }


        }
        else
        {
            //Debug.Log("why1");
            displayPath.HidePathOfEnemyGroup();
            for (int i = 0; i < enemyScrollViewContent_Battle.childCount; ++i)
            {
                Destroy(enemyScrollViewContent_Battle.GetChild(i).gameObject);
            }
            for (int i = 0; i < insideEnemyList.Count; ++i)
            {
                Enemy insideEnemyComp = insideEnemyList[i].GetComponent<Enemy>();
                string curInsideEnemyFullName = insideEnemyComp.enemyName + insideEnemyComp.enemyAttr.ToString();
                if ((EnemyIconSpriteList.FindIndex(a => (a.name == curInsideEnemyFullName + "Icon")) != -1))
                {
                    GameObject enemyIcon = Instantiate(enemyIconFramePrefab_Battle, new Vector3(1000, 1000, 0), new Quaternion());
                    enemyIcon.GetComponent<EnemyBind>().setEnemy(insideEnemyList[i]);

                    //修改图片
                    enemyIcon.transform.Find("Image").GetComponent<Image>().sprite = EnemyIconSpriteList.Find(a => (a.name == curInsideEnemyFullName + "Icon"));
                    enemyIcon.transform.SetParent(enemyScrollViewContent_Battle);//设置父对象
                    enemyIcon.transform.localScale = new Vector3(1, 1, 1);//修改缩放值
                }

            }
        }
    }

    private void updateEnemyScrollView()
    {
        if (gameStep == 0)
        {

        }
        else
        {
            for (int i = 0; i < enemyScrollViewContent_Battle.childCount; ++i)
            {
                Transform insideEnemyIcon = enemyScrollViewContent_Battle.GetChild(i);
                if (insideEnemyIcon != null)
                {
                    GameObject insideEnemy = insideEnemyIcon.GetComponent<EnemyBind>().getEnemy();
                    if (insideEnemy != null)
                    {
                        Enemy insideEnemyComp = insideEnemy.GetComponent<Enemy>();
                        Transform healthBar = insideEnemy.transform.Find("Canvas/Simple Bar/Status Fill 01");
                        if (healthBar != null)
                        {
                            insideEnemyIcon.Find("Simple Bar/Status Fill 01").GetComponent<SimpleHealthBar>()
                            .UpdateBar(insideEnemyComp.getEnemyCurrentHealth(), insideEnemyComp.enemyTotalHealth);
                            if (insideEnemyComp.getEnemyCurrentHealth() <= 0)
                            {
                                Destroy(insideEnemyIcon.gameObject);
                            }

                        }
                    }

                }
            }
        }

    }

    private bool CheckGuiRaycastObjects()
    {
        // PointerEventData eventData = new PointerEventData(Main.Instance.eventSystem);

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        // Main.Instance.graphicRaycaster.Raycast(eventData, list);
        EventSystem.current.RaycastAll(eventData, list);
        //Debug.Log(list.Count);
        int HealthBarIndex1 = list.FindIndex(a => a.gameObject.name == "Status Fill 01");
        if (HealthBarIndex1 != -1)
        {
            list.RemoveAt(HealthBarIndex1);
        }
        int HealthBarIndex2 = list.FindIndex(a => a.gameObject.name == "Simple Bar");
        if (HealthBarIndex2 != -1)
        {
            list.RemoveAt(HealthBarIndex2);
        }
        // if (list.Count > 0)
        // {
        //     Debug.Log(list[0].gameObject.name);
        // }
        return list.Count > 0;
    }




    // public void createEnemy(GameObject path)
    // {
    //     Debug.Log("开始创建enemy");
    //     GameObject enemy = Instantiate(enemyPrefab, new Vector3(0f, -100f, 0f), new Quaternion());
    //     //enemy.transform.parent = transform;
    //     enemy.transform.position = path.transform.GetChild(0).transform.position;
    //     enemy.transform.parent = enemyParent;
    //     enemy.transform.GetComponent<Enemy>().InitWithData(path);
    //     if (!GeneralPool.addToEnemyGroup(enemy, 0))
    //     {
    //         Destroy(enemy);
    //     }

    // }

    // public void creatEnemyGroup()
    // {
    //     List<GameObject> enemyGroup0 = new List<GameObject>();
    //     GeneralPool.addToEnemyGroupList(enemyGroup0);
    // }

    // public void startWalkOfEnemyGroup0()
    // {
    //     List<GameObject> enemyGroup0 = GeneralPool.get_enemyList(0);
    //     foreach (GameObject enemy in enemyGroup0)
    //     {
    //         enemy.GetComponent<Enemy>().StartWalk();
    //     }
    // }


    public void setRound(int currentRound)
    {
        this.currentRound = currentRound;
    }
    public void setGameStep(int gameStep)
    {
        this.gameStep = gameStep;
        if (gameStep == 1)
        {
            displayPath.HidePathOfEnemyGroup();
            foreach (GameObject enemy in GeneralPool.get_enemyList())
            {
                enemy.GetComponent<QuickOutline>().enabled = false;
            }
            for (int i = 0; i < enemyScrollViewContent_Prepare.childCount; i++)
            {
                Destroy(enemyScrollViewContent_Prepare.GetChild(i).gameObject);
            }
        }
    }


    private List<Sprite> EnemyIconSpriteList = new List<Sprite>(); bool loadedflag = false;
    public void LoadEnemyIconPrefabs()
    {
        StartCoroutine(AssetbundleLoader.LoadResOfType<Sprite>("icons/enemyicons", this));
    }

    public void setList<T>(List<T> TList)
    {
        List<Sprite> SpriteList = new List<Sprite>();
        foreach (T t in TList)
        {
            SpriteList.Add(t as Sprite);
        }
        EnemyIconSpriteList = SpriteList;
        Debug.Log("Enemy图标加载数量：" + EnemyIconSpriteList.Count);
        loadedflag = true;
    }

    public bool getLoadedFlag()
    {
        return loadedflag;
    }




}