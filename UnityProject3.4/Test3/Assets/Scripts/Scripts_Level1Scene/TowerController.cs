using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AssetBundleFramework;

public class TowerController : MonoBehaviour, ISetList
{
    public Transform TowerParent;
    public GameObject towerPrefab;
    public Transform towerScrollViewContent;
    public GameObject towerIconFramePrefab;

    public GameObject buildablePlane;
    public GameObject cancelBuildTowerButton;
    public DisplayScopePlane displayScopePlane;
    public GameObject towerOperatePanel_Prepare;
    public GameObject towerOperatePanel_Battle;


    private List<GameObject> TowerPrefabsList; private bool loadedflag1 = false;
    private List<Sprite> TowerIconSpriteList = new List<Sprite>(); bool loadedflag2 = false;

    private UIHelper uiHelper;

    private GameObject tempTower; private Tower tempTowerComp;


    private GameObject focusTower;

    private int currentRound = 0;
    private int gameStep = 0;

    private bool loadedflag_inner = false; public void loadedOK() { loadedflag_inner = true; }

    private bool towerMode = false;



    // Start is called before the first frame update
    void Start()
    {
        loadedflag_inner = false;
        uiHelper = GetComponent<UIHelper>();
        tempTower = Instantiate(towerPrefab, new Vector3(1000.5f, -100f, 1000.5f), new Quaternion());
        tempTower.transform.parent = TowerParent;
        tempTower.SetActive(false);
        tempTowerComp = tempTower.GetComponent<Tower>();

        GameObject tempTowerScopePlane = tempTowerComp.getScopePlane();
        List<Vector3> tempTowerScopePlanePos = tempTowerComp.getScopePlanePosList();
        displayScopePlane.DisplayScopeOfTower(tempTowerScopePlanePos, tempTowerScopePlane.transform);

    }

    // Update is called once per frame
    void Update()
    {
        if (loadedflag_inner)
        {
            //Debug.Log("gamestep情况:"+gameStep);
            if (gameStep == 0)
            {
                if (towerMode)
                {
                    foreach (GameObject tower in GeneralPool.get_towerList())
                    {
                        if (tower.tag == "Tower")
                        {
                            Collider towerColli = tower.GetComponent<Collider>();
                            if (towerColli != null)
                                tower.GetComponent<Collider>().enabled = false;
                        }

                    }
                    //创建一条从摄像机到触摸位置的射线
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 定义射线
                    RaycastHit rayHit;
                    if (Physics.Raycast(ray, out rayHit, 200)) // 参数1 射线，参数2 碰撞信息， 参数3 碰撞层
                    {
                        //Debug.Log(rayHit.collider.gameObject.name);
                        if (rayHit.collider.gameObject.tag == "BuildablePlane")
                        {
                            Vector3 hitpoint = rayHit.point;
                            float hitpoint_x = hitpoint.x;
                            float hitpoint_y = hitpoint.y;
                            float hitpoint_z = hitpoint.z;
                            float hitpoint_x_center = Mathf.Floor(hitpoint_x) + 0.5f;
                            float hitpoint_z_center = Mathf.Floor(hitpoint_z) + 0.5f;
                            tempTower.transform.position = new Vector3(hitpoint_x_center, hitpoint_y, hitpoint_z_center);
                            if (!tempTower.activeSelf)
                            {
                                tempTower.SetActive(true);
                            }

                            if (CheckGuiRaycastObjects()) return;
                            if (Input.GetMouseButtonDown(0))//建塔
                            {
                                if (tempTower.activeSelf)
                                {
                                    if (GeneralPool.coins >= tempTowerComp.towerPrice)
                                    {
                                        uiHelper.updateCoinCountText(GeneralPool.coins);
                                        GameObject realTower = Instantiate(towerPrefab, tempTower.transform.position, new Quaternion());

                                        realTower.transform.parent = TowerParent;
                                        Tower realTowerComp = realTower.GetComponent<Tower>();
                                        realTowerComp.setRoundWhenCreated(currentRound);
                                        GameObject realTowerScopePlane = realTowerComp.getScopePlane();
                                        realTowerScopePlane.SetActive(false);
                                        List<Vector3> realTowerScopePlanePos = realTowerComp.getScopePlanePosList();
                                        displayScopePlane.DisplayScopeOfTower(realTowerScopePlanePos, realTowerScopePlane.transform);
                                        GeneralPool.addToTowerList(realTower);
                                        GeneralPool.addToTowerAndBPDict(realTower, rayHit.collider.gameObject);
                                        rayHit.collider.gameObject.SetActive(false);
                                        GeneralPool.coins -= realTowerComp.towerPrice;
                                        uiHelper.updateCoinCountText(GeneralPool.coins);
                                        //Debug.Log(rayHit.collider.gameObject.name);
                                    }
                                    else
                                    {
                                        Debug.Log("金币不足");
                                        uiHelper.ShowAlarm("金币不足");
                                    }
                                }
                            }
                        }
                        else
                        {

                            if (tempTower.activeSelf)
                            {
                                //Debug.Log("nothing");
                                tempTower.SetActive(false);
                            }

                        }



                    }
                }
                else//非TowerMode
                {
                    //非TowerMode中所有塔都可以被点击，要先全设置为碰撞体
                    foreach (GameObject tower in GeneralPool.get_towerList())
                    {
                        if (tower.tag == "Tower")
                        {
                            Collider towerColli = tower.GetComponent<Collider>();
                            if (towerColli != null)
                                tower.GetComponent<Collider>().enabled = true;
                        }

                    }

                    if (CheckGuiRaycastObjects()) return;
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 定义射线
                        RaycastHit rayHit;
                        if (Physics.Raycast(ray, out rayHit, 200)) // 参数1 射线，参数2 碰撞信息， 参数3 碰撞层
                        {
                            GameObject rayHitObject = rayHit.collider.gameObject;
                            if (rayHitObject.tag == "Tower")//点击塔会显示塔的外轮廓，塔的检测范围，以及塔的选项
                            {
                                if (focusTower == rayHitObject)
                                {
                                    QuickOutline towerOutline = rayHitObject.GetComponent<QuickOutline>();
                                    towerOutline.enabled = false;
                                    focusTower = null;
                                    //towerOperatePanel_Prepare.transform.localScale = Vector3.zero;
                                }
                                else
                                {
                                    // foreach (GameObject tower in GeneralPool.get_towerList())
                                    // {
                                    //     if (tower.tag == "Tower")
                                    //     {
                                    //         tower.GetComponent<QuickOutline>().enabled = false;
                                    //         Transform OneScopePlaneTransform = tower.transform.Find("scopePlane");
                                    //         OneScopePlaneTransform.gameObject.SetActive(false);
                                    //     }
                                    // }

                                    if (focusTower == null)
                                    {

                                    }
                                    else
                                    {
                                        focusTower.GetComponent<QuickOutline>().enabled = false;
                                        Transform OneScopePlaneTransform = focusTower.transform.Find("scopePlane");
                                        OneScopePlaneTransform.gameObject.SetActive(false);
                                        towerOperatePanel_Prepare.transform.localScale = Vector3.zero;
                                    }
                                    focusTower = rayHitObject;
                                    focusTower.GetComponent<QuickOutline>().enabled = true;

                                }
                                GameObject TowerScopePlane = rayHitObject.transform.Find("scopePlane").gameObject;
                                TowerScopePlane.SetActive(!TowerScopePlane.activeSelf);
                                switchTowerOperatePanel_Prepare(rayHitObject);


                            }
                            else if (rayHitObject.name == "Road1" || rayHitObject.name == "Road2")
                            {
                                Debug.Log("its road");

                            }
                            else
                            {
                                Debug.Log(rayHitObject.name);
                                // foreach (GameObject tower in GeneralPool.get_towerList())
                                // {
                                //     if (tower.tag == "Tower")
                                //     {
                                //         tower.GetComponent<QuickOutline>().enabled = false;
                                //         Transform OneScopePlaneTransform = tower.transform.Find("scopePlane");
                                //         OneScopePlaneTransform.gameObject.SetActive(false);
                                //     }
                                // }
                                if (focusTower != null)
                                {
                                    focusTower.GetComponent<QuickOutline>().enabled = false;
                                    Transform OneScopePlaneTransform = focusTower.transform.Find("scopePlane");
                                    OneScopePlaneTransform.gameObject.SetActive(false);
                                }
                                focusTower = null;

                                towerOperatePanel_Prepare.transform.localScale = Vector3.zero;


                            }
                        }
                    }

                    if (focusTower != null)
                    {
                        towerOperatePanel_Prepare.transform.position = Camera.main.WorldToScreenPoint(focusTower.transform.position);
                    }
                }

            }
            else//在出兵阶段和交战阶段，点击塔可以显示塔的攻击范围，塔名和属性，紧急修复按钮，及技能按钮
            {
                //此时所有塔都可以被点击，要先全设置为碰撞体
                foreach (GameObject tower in GeneralPool.get_towerList())
                {
                    if (tower.tag == "Tower")
                    {
                        Collider towerColli = tower.GetComponent<Collider>();
                        if (towerColli != null)
                            tower.GetComponent<Collider>().enabled = true;
                    }

                }
                if (CheckGuiRaycastObjects()) return;
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 定义射线
                    RaycastHit rayHit;
                    if (Physics.Raycast(ray, out rayHit, 200)) // 参数1 射线，参数2 碰撞信息， 参数3 碰撞层
                    {
                        GameObject rayHitObject = rayHit.collider.gameObject;
                        if (rayHitObject.tag == "Tower")//点击塔会显示塔的外轮廓，塔的检测范围，以及塔的选项
                        {
                            if (focusTower == rayHitObject)
                            {
                                QuickOutline towerOutline = rayHitObject.GetComponent<QuickOutline>();
                                towerOutline.enabled = false;
                                focusTower = null;
                                //towerOperatePanel_Prepare.transform.localScale = Vector3.zero;
                            }
                            else
                            {
                                // foreach (GameObject tower in GeneralPool.get_towerList())
                                // {
                                //     if (tower.tag == "Tower")
                                //     {
                                //         tower.GetComponent<QuickOutline>().enabled = false;
                                //         Transform OneScopePlaneTransform = tower.transform.Find("scopePlane");
                                //         OneScopePlaneTransform.gameObject.SetActive(false);
                                //     }
                                // }

                                if (focusTower == null)
                                {

                                }
                                else
                                {
                                    focusTower.GetComponent<QuickOutline>().enabled = false;
                                    Transform OneScopePlaneTransform = focusTower.transform.Find("scopePlane");
                                    OneScopePlaneTransform.gameObject.SetActive(false);
                                    towerOperatePanel_Battle.transform.localScale = Vector3.zero;
                                }
                                focusTower = rayHitObject;
                                focusTower.GetComponent<QuickOutline>().enabled = true;

                            }
                            GameObject TowerScopePlane = rayHitObject.transform.Find("scopePlane").gameObject;
                            TowerScopePlane.SetActive(!TowerScopePlane.activeSelf);
                            switchTowerOperatePanel_Battle(rayHitObject);


                        }
                        else if (rayHitObject.name == "Road1" || rayHitObject.name == "Road2")
                        {
                            Debug.Log("its road");

                        }
                        else
                        {
                            Debug.Log(rayHitObject.name);
                            // foreach (GameObject tower in GeneralPool.get_towerList())
                            // {
                            //     if (tower.tag == "Tower")
                            //     {
                            //         tower.GetComponent<QuickOutline>().enabled = false;
                            //         Transform OneScopePlaneTransform = tower.transform.Find("scopePlane");
                            //         OneScopePlaneTransform.gameObject.SetActive(false);
                            //     }
                            // }
                            if (focusTower != null)
                            {
                                focusTower.GetComponent<QuickOutline>().enabled = false;
                                Transform OneScopePlaneTransform = focusTower.transform.Find("scopePlane");
                                OneScopePlaneTransform.gameObject.SetActive(false);
                            }
                            focusTower = null;

                            towerOperatePanel_Battle.transform.localScale = Vector3.zero;


                        }
                    }
                }

                if (focusTower != null)
                {
                    towerOperatePanel_Battle.transform.position = Camera.main.WorldToScreenPoint(focusTower.transform.position);
                }
                else
                {
                    towerOperatePanel_Battle.transform.localScale=Vector3.zero;
                }

            }

        }
        else//loadedflag_inner为false，说明资源还未全部加载完成
        {

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
        int HealthBarIndex1 = list.FindIndex(a => a.gameObject.name == "Status Fill 01");//不检查status fill 01
        if (HealthBarIndex1 != -1)
        {
            list.RemoveAt(HealthBarIndex1);
        }
        int HealthBarIndex2 = list.FindIndex(a => a.gameObject.name == "Simple Bar");//不检查simple bar
        if (HealthBarIndex2 != -1)
        {
            list.RemoveAt(HealthBarIndex2);
        }
        int TowerIndex = list.FindIndex(a => a.gameObject.tag == "Tower");//不检查tag为tower的物体
        if (TowerIndex != -1)
        {
            list.RemoveAt(TowerIndex);
        }
        int AlarmTextIndex = list.FindIndex(a => a.gameObject.name == "AlarmText");//不检查alarmText
        if (AlarmTextIndex != -1)
        {
            list.RemoveAt(AlarmTextIndex);
        }

        return list.Count > 0;
    }

    public void switchTowerOperatePanel_Prepare(GameObject tower)
    {
        Tower towerComp = tower.GetComponent<Tower>();
        towerOperatePanel_Prepare.transform.position = Camera.main.WorldToScreenPoint(tower.transform.position);
        towerOperatePanel_Prepare.transform.Find("TowerInfoPanel_in/TowerNameText").GetComponent<Text>().text = BasicInfo.EngToCHS_Dict[towerComp.towerName];
        towerOperatePanel_Prepare.transform.Find("TowerInfoPanel_in/TowerElementAttrText").GetComponent<Text>().text = BasicInfo.EngToCHS_Dict[towerComp.towerAttr.ToString()];
        //还需设置升级按钮和修复按钮，待完善（并非此处实现，最好写新的函数，然后在inspector里添加onclick事件）


        if (towerOperatePanel_Prepare.transform.localScale == Vector3.one)
            towerOperatePanel_Prepare.transform.localScale = Vector3.zero;
        else if (towerOperatePanel_Prepare.transform.localScale == Vector3.zero)
            towerOperatePanel_Prepare.transform.localScale = Vector3.one;
    }

    public void switchTowerOperatePanel_Battle(GameObject tower)
    {
        Tower towerComp = tower.GetComponent<Tower>();
        towerOperatePanel_Battle.transform.position = Camera.main.WorldToScreenPoint(tower.transform.position);
        towerOperatePanel_Battle.transform.Find("TowerInfoPanel_in/TowerNameText").GetComponent<Text>().text = BasicInfo.EngToCHS_Dict[towerComp.towerName];
        towerOperatePanel_Battle.transform.Find("TowerInfoPanel_in/TowerElementAttrText").GetComponent<Text>().text = BasicInfo.EngToCHS_Dict[towerComp.towerAttr.ToString()];
        //设置修复按钮和技能按钮，待完善（并非此处实现，最好写新的函数，然后在inspector里添加onclick事件）

        if (towerOperatePanel_Battle.transform.localScale == Vector3.one)
            towerOperatePanel_Battle.transform.localScale = Vector3.zero;
        else if (towerOperatePanel_Battle.transform.localScale == Vector3.zero)
            towerOperatePanel_Battle.transform.localScale = Vector3.one;
    }

    public void RepairFocusTower()//准备阶段进行修复，10金币修复20生命值
    {
        if (focusTower != null)
        {
            float repairCoins = 10;
            float repairHealth = 20;
            Tower focusTowerComp = focusTower.GetComponent<Tower>();



            if (focusTowerComp.getCurrentHealth() == focusTowerComp.towerTotalHealth)
            { }
            else
            {

                float newhealth = focusTowerComp.getCurrentHealth() + repairHealth;
                if (newhealth > focusTowerComp.towerTotalHealth)
                {
                    repairCoins = 10 * ((20 - (newhealth - focusTowerComp.towerTotalHealth)) / 20);
                    newhealth = focusTowerComp.towerTotalHealth;
                }
                if (GeneralPool.coins < repairCoins)
                {
                    uiHelper.ShowAlarm("金币不足");
                }
                else
                {

                    GeneralPool.coins -= repairCoins;
                    uiHelper.updateCoinCountText(GeneralPool.coins);
                    focusTowerComp.setCurrentHealth(newhealth);
                }

            }
        }
    }

    public void UrgencyRepairFocusTower()//交战阶段进行紧急修复，100金币修复100生命值，不计算比例
    {
        if (focusTower != null)
        {
            float repairCoins = 100;
            float repairHealth = 100;
            Tower focusTowerComp = focusTower.GetComponent<Tower>();

            if (focusTowerComp.getCurrentHealth() == focusTowerComp.towerTotalHealth)
            { }
            else
            {

                float newhealth = focusTowerComp.getCurrentHealth() + repairHealth;
                if (newhealth > focusTowerComp.towerTotalHealth)
                {
                    newhealth = focusTowerComp.towerTotalHealth;
                }
                if (GeneralPool.coins < repairCoins)
                {
                    uiHelper.ShowAlarm("金币不足");
                }
                else
                {
                    GeneralPool.coins -= repairCoins;
                    uiHelper.updateCoinCountText(GeneralPool.coins);
                    focusTowerComp.setCurrentHealth(newhealth);
                }

            }
        }
    }

    public void SellFocusTower()
    {
        if (focusTower != null)
        {
            float sellCoins = 0;
            Tower focusTowerComp = focusTower.GetComponent<Tower>();
            if (focusTowerComp.getRoundWhenCreated() != currentRound)
            {
                sellCoins = focusTowerComp.towerPrice / 2;
            }
            else
            {
                sellCoins = focusTowerComp.towerPrice;
            }
            sellCoins *= (focusTowerComp.getCurrentHealth() / focusTowerComp.towerTotalHealth);
            GeneralPool.coins += sellCoins;
            uiHelper.updateCoinCountText(GeneralPool.coins);


            GeneralPool.getBP(focusTower).SetActive(true);
            GeneralPool.removeFromTowerAndBPDict(focusTower);
            GeneralPool.removeFromTowerList(focusTower);
            Destroy(focusTower);
            focusTower = null;
            towerOperatePanel_Prepare.transform.localScale = Vector3.zero;
        }
    }



    public void changeTowerPrefab(string towerPrefabName)
    {
        towerPrefab = TowerPrefabsList.Find(tower => (tower.name == towerPrefabName));
        tempTower = Instantiate(towerPrefab, new Vector3(1000.5f, -100f, 1000.5f), new Quaternion());
        tempTower.transform.parent = TowerParent;
        tempTower.SetActive(false);
        tempTowerComp = tempTower.GetComponent<Tower>();

        GameObject tempTowerScopePlane = tempTowerComp.getScopePlane();
        List<Vector3> tempTowerScopePlanePos = tempTowerComp.getScopePlanePosList();
        displayScopePlane.DisplayScopeOfTower(tempTowerScopePlanePos, tempTowerScopePlane.transform);
        startTowerMode();
    }

    public void startTowerMode()
    {
        towerMode = true;
        buildablePlane.SetActive(true);
        if (tempTower.activeSelf)
        {
            tempTower.SetActive(false);
        }
        cancelBuildTowerButton.SetActive(true);

        if (focusTower != null)
        {
            towerOperatePanel_Prepare.transform.localScale = Vector3.zero;

            QuickOutline towerOutline = focusTower.GetComponent<QuickOutline>();
            towerOutline.enabled = false;
            Transform OneScopePlaneTransform = focusTower.transform.Find("scopePlane");
            OneScopePlaneTransform.gameObject.SetActive(false);
            focusTower = null;
        }

    }

    public void stopTowerMode()
    {
        towerMode = false;
        buildablePlane.SetActive(false);
        if (tempTower.activeSelf)
        {
            tempTower.SetActive(false);
        }
        cancelBuildTowerButton.SetActive(false);
    }


    public void setRound(int currentRound)
    {
        this.currentRound = currentRound;
    }
    public void setGameStep(int gameStep)
    {
        this.gameStep = gameStep;
        if (gameStep == 1 || gameStep == 0)
        {
            // foreach (GameObject tower in GeneralPool.get_towerList())
            // {
            //     if (tower.tag == "Tower")
            //     {
            //         tower.GetComponent<QuickOutline>().enabled = false;
            //         Transform OneScopePlaneTransform = tower.transform.Find("scopePlane");
            //         OneScopePlaneTransform.gameObject.SetActive(false);
            //     }
            // }
            if (focusTower != null)
            {
                focusTower.GetComponent<QuickOutline>().enabled = false;
                Transform OneScopePlaneTransform = focusTower.transform.Find("scopePlane");
                OneScopePlaneTransform.gameObject.SetActive(false);
            }
            focusTower = null;

            towerOperatePanel_Prepare.transform.localScale = Vector3.zero;
            towerOperatePanel_Battle.transform.localScale = Vector3.zero;

        }
    }
    public void LoadTowerPrefabsAndTowerIconSprite()
    {
        StartCoroutine(AssetbundleLoader.LoadResOfType<GameObject>("prefabs/towerprefabs", this));
        StartCoroutine(AssetbundleLoader.LoadResOfType<Sprite>("icons/towericons", this));
    }

    public void setList<T>(List<T> TList)//加载完成后调用
    {
        if (typeof(T) == typeof(GameObject))
        {
            List<GameObject> GameObjectList = new List<GameObject>();
            foreach (T t in TList)
            {
                GameObjectList.Add(t as GameObject);
            }
            TowerPrefabsList = GameObjectList;
            Debug.Log("加载TowerPrefabs种类数量为：" + TowerPrefabsList.Count);
            loadedflag1 = true;
        }
        else if (typeof(T) == typeof(Sprite))
        {
            List<Sprite> SpriteList = new List<Sprite>();
            foreach (T t in TList)
            {
                SpriteList.Add(t as Sprite);
            }
            TowerIconSpriteList = SpriteList;
            Debug.Log("Tower图标加载数量：" + TowerIconSpriteList.Count);
            loadedflag2 = true;

            generateTowerIconFrames();//生成下方塔scrollview的选框
        }




    }

    public bool getLoadedFlag()
    {
        return (loadedflag1 && loadedflag2);
    }

    public void generateTowerIconFrames()
    {
        for (int i = 0; i < BasicInfo.towerKindsBasic.Length; ++i)
        {
            string basicTowerKindName = BasicInfo.towerKindsBasic[i];
            GameObject towerIcon = Instantiate(towerIconFramePrefab, new Vector3(1000, 1000, 0), new Quaternion());
            towerIcon.name = basicTowerKindName + "IconFrame";

            //对按钮进行绑定
            Transform elementChooseButtonTrans = towerIcon.transform.Find("ElementChooseButtons");
            for (int j = 0; j < elementChooseButtonTrans.childCount; j++)
            {
                Button childButton = elementChooseButtonTrans.GetChild(j).GetComponent<Button>();
                childButton.onClick.AddListener(
                    delegate
                    {
                        changeTowerPrefab(basicTowerKindName + childButton.name + "Prefab");
                    }
                );
            }


            //修改图片
            towerIcon.GetComponent<Image>().sprite = TowerIconSpriteList.Find(a => (a.name == basicTowerKindName + "Icon"));
            towerIcon.transform.SetParent(towerScrollViewContent);//设置父对象
            towerIcon.transform.localScale = new Vector3(1, 1, 1);//修改缩放值（有时候修改父对象会影响缩放值）

        }
    }
}