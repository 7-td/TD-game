using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetBundleFramework;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TechnologyTree : MonoBehaviour, ISetList
{

    public GameObject economyItems;
    public GameObject elementFortItems;
    public GameObject fiveElementCreatorItems;
    public GameObject descriptionText;

    private List<TechtreeItem> allItems = new List<TechtreeItem>();

    private List<Sprite> itemIcons = new List<Sprite>();
    bool loadedFlag = false;


    private Dictionary<string, string> techtreeItemDependencies = new Dictionary<string, string>()
    {
        {"SwxzTechItem", "YtgyTechItem"},
        {"YtgyTechItem", "NgbfTechItem"},
        {"NgbfTechItem", "ZnysTechItem"},
        {"JrjsTechItem", "GyglTechItem"},
        {"GyglTechItem", "NgbfTechItem"},
        {"ZyydTechItem", "QqhdTechItem"},
        {"QqhdTechItem", "ZljcTechItem"},
        {"ZljcTechItem", "NzjfTechItem"},
        {"MetalItem","GroundItem"},
        {"GroundItem","FireItem"},
        {"FireItem","GrassItem"},
        {"GrassItem","WaterItem"},
        {"WaterItem","MetalItem"}
    };

    private List<TechtreeItem> activeItems = new List<TechtreeItem>();


    // Start is called before the first frame update
    void Start()
    {
        DoInit();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator waitForAssetsLoad()
    {
        while (!loadedFlag)
        {
            yield return null;
        }
        //加载完成！执行以下语句
        for (int i = 0; i < economyItems.transform.childCount; i++)
        {
            GameObject economyItem = economyItems.transform.GetChild(i).gameObject;
            economyItem.AddComponent<TechtreeItem>();
            economyItem.GetComponent<TechtreeItem>().departmentType = TechtreeItem.DepartmentType.Economy;
            economyItem.GetComponent<TechtreeItem>().setDescriptionText(descriptionText);
            economyItem.GetComponent<TechtreeItem>().SetIcon(itemIcons.Find(a => (a.name == economyItem.name + "D")), 0);
            economyItem.GetComponent<TechtreeItem>().SetIcon(itemIcons.Find(a => (a.name == economyItem.name + "B")), 1);
            economyItem.GetComponent<TechtreeItem>().technologyTree = this;
            economyItem.GetComponent<Button>().onClick.AddListener(() => economyItem.GetComponent<TechtreeItem>().OnClicked());
            allItems.Add(economyItem.GetComponent<TechtreeItem>());
        }
        descriptionText.SetActive(false);
        for (int i = 0; i < elementFortItems.transform.childCount; i++)
        {
            GameObject elementFortItem = elementFortItems.transform.GetChild(i).gameObject;
            elementFortItem.AddComponent<TechtreeItem>();
            elementFortItem.GetComponent<TechtreeItem>().departmentType = TechtreeItem.DepartmentType.ElementFort;
            elementFortItem.GetComponent<TechtreeItem>().setDescriptionText(descriptionText);
            elementFortItem.GetComponent<TechtreeItem>().SetIcon(itemIcons.Find(a => (a.name == elementFortItem.name + "D")), 0);
            elementFortItem.GetComponent<TechtreeItem>().SetIcon(itemIcons.Find(a => (a.name == elementFortItem.name + "B")), 1);
            elementFortItem.GetComponent<TechtreeItem>().technologyTree = this;
            elementFortItem.GetComponent<Button>().onClick.AddListener(() => elementFortItem.GetComponent<TechtreeItem>().OnClicked());
            allItems.Add(elementFortItem.GetComponent<TechtreeItem>());
        }
        for (int i = 0; i < fiveElementCreatorItems.transform.childCount; i++)
        {
            GameObject fiveElementCreatorItem = fiveElementCreatorItems.transform.GetChild(i).gameObject;
            fiveElementCreatorItem.AddComponent<TechtreeItem>();
            fiveElementCreatorItem.GetComponent<TechtreeItem>().departmentType = TechtreeItem.DepartmentType.FiveElementCreator;
            fiveElementCreatorItem.GetComponent<TechtreeItem>().setDescriptionText(descriptionText);
            fiveElementCreatorItem.GetComponent<TechtreeItem>().SetIcon(itemIcons.Find(a => (a.name == fiveElementCreatorItem.name + "D")), 0);
            fiveElementCreatorItem.GetComponent<TechtreeItem>().SetIcon(itemIcons.Find(a => (a.name == fiveElementCreatorItem.name + "B")), 1);
            fiveElementCreatorItem.GetComponent<TechtreeItem>().technologyTree = this;
            fiveElementCreatorItem.GetComponent<Button>().onClick.AddListener(() => fiveElementCreatorItem.GetComponent<TechtreeItem>().OnClicked());
            allItems.Add(fiveElementCreatorItem.GetComponent<TechtreeItem>());
        }

    }

    private void DoInit()
    {
        StartCoroutine(AssetbundleLoader.LoadResOfType<Sprite>("icons/techtreeicons", this));
        StartCoroutine(waitForAssetsLoad());
    }

    public void setList<T>(List<T> TList)
    {
        List<Sprite> SpriteList = new List<Sprite>();
        foreach (T t in TList)
        {
            SpriteList.Add(t as Sprite);
        }
        itemIcons = SpriteList;
        loadedFlag = true;
    }

    public void Save()
    {
        GetComponent<UIHelper>().hideTechnologyTree();
        foreach (TechtreeItem item in allItems)
        {
            item.Save();
        }
    }

    public void Cancel()
    {
        GetComponent<UIHelper>().hideTechnologyTree();
        foreach (TechtreeItem item in allItems)
        {
            item.Cancel();
        }
    }

    public bool IsMeetDependency(TechtreeItem item)
    {
        if (IsNoSelectedFiveElementCreatorItems() && item.departmentType == TechtreeItem.DepartmentType.FiveElementCreator)
        {
            return true;
        }
        if (IsNoSelectedElementFortItems() && item.departmentType == TechtreeItem.DepartmentType.ElementFort)
        {
            return true;
        }
        if (item.itemName == "ZnysTechItem" || item.itemName == "NzjfTechItem")
        {
            return true;
        }
        int index = allItems.FindIndex(a => (a.IsSelected() && a.itemName == techtreeItemDependencies[item.itemName] && a.departmentType == item.departmentType));
        return index != -1;
    }

    private bool IsNoSelectedFiveElementCreatorItems()
    {
        foreach (TechtreeItem item in allItems)
        {
            if (item.departmentType == TechtreeItem.DepartmentType.FiveElementCreator && item.IsSelected())
            {
                return false;
            }
        }
        return true;
    }

    private bool IsNoSelectedElementFortItems()
    {
        foreach (TechtreeItem item in allItems)
        {
            if (item.departmentType == TechtreeItem.DepartmentType.ElementFort && item.IsSelected())
            {
                return false;
            }
        }
        return true;
    }
}

