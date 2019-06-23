using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AssetBundleFramework;


public class TechtreeItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private GameObject descriptionText;
    private float descriptionTimer = 0;
    private bool isPointerToItem = false;

    private Sprite darkIcon;
    private Sprite brightIcon;


    public TechnologyTree technologyTree;
    public enum DepartmentType
    {
        ElementFort,
        Economy,
        FiveElementCreator
    }

    public DepartmentType departmentType;

    public int StrategicPoint;

    public string itemName;
    private string description;

    private bool isActive = false;
    private bool isSelected = false;



    // Start is called before the first frame update
    void Start()
    {
        itemName = gameObject.name;
        switch (itemName)
        {
            case "ZnysTechItem":
                description = "重农抑商(3):每回合初始获得的金币+60。";
                StrategicPoint = 3;
                break;
            case "NgbfTechItem":
                description = "农耕变法(2):每回合初始获得的金币+60。";
                StrategicPoint = 2;
                break;
            case "YtgyTechItem":
                description = "盐铁国营(3):每回合初始获得的金币+90。";
                StrategicPoint = 3;
                break;
            case "SwxzTechItem":
                description = "税务新制(2):每回合初始获得的金币+90。";
                StrategicPoint = 2;
                break;
            case "GyglTechItem":
                description = "工艺改良(3):防御塔维修以及紧急维修所需的金币下降20%。";
                StrategicPoint = 3;
                break;
            case "JrjsTechItem":
                description = "匠人精神(2):所有防御塔的造价降低16%。";
                StrategicPoint = 2;
                break;
            case "NzjfTechItem":
                description = "纳征军费(3):每回合初始获得的金币+60。";
                StrategicPoint = 3;
                break;
            case "ZljcTechItem":
                description = "占路劫财(2):每当击杀一个敌方士兵时，获得其造价10%的金币（可与技能巧取豪夺同时发动）。";
                StrategicPoint = 2;
                break;
            case "QqhdTechItem":
                description = "巧取豪夺(3):每当击杀一个敌方士兵时，获得其造价15%的金币（可与技能占路劫财同时发动）。";
                StrategicPoint = 3;
                break;
            case "ZyydTechItem":
                description = "战亦有盗(2):在回合内击杀的士兵总造价达到200，则可获得90金币。";
                StrategicPoint = 2;
                break;
            default:
                description = "";
                StrategicPoint = 3;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPointerToItem)
        {
            descriptionTimer += Time.deltaTime;
            if (descriptionTimer > 0.35f)
            {
                showDescriptionText(true);
            }
        }

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (departmentType == DepartmentType.Economy)
            isPointerToItem = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerToItem = false;
        descriptionTimer = 0;
        showDescriptionText(false);
    }

    public void setDescriptionText(GameObject descriptionText)
    {
        this.descriptionText = descriptionText;
    }

    private void showDescriptionText(bool activeState)
    {
        descriptionText.SetActive(activeState);
        if (activeState)
        {
            descriptionText.transform.GetChild(0).GetComponent<Text>().text = description.Clone().ToString();
            Vector3 pos = new Vector3(Input.mousePosition.x + 4, Input.mousePosition.y - 4, 0);
            descriptionText.transform.position = pos;
        }
    }

    //0 for dark    1 for bright 
    public void SetIcon(Sprite sprite, int type)
    {
        if (type == 0)
            darkIcon = sprite;
        else if (type == 1)
            brightIcon = sprite;
    }

    public void OnClicked()
    {
        if (isSelected == false && technologyTree.IsMeetDependency(this))
        {
            isSelected = true;
            GetComponent<Button>().image.sprite = brightIcon;
        }
    }

    public void Save()
    {
        //if (isActive == false)
        //{
        //    isActive = isSelected;
        //}
        //if (isActive == false)
        //{
        //    GetComponent<Button>().image.sprite = darkIcon;
        //}
        isActive = isSelected;
    }

    public void Cancel()
    {
        isSelected = isActive;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void OnEnable()
    {
        isSelected = isActive;
        if (isActive == false)
            GetComponent<Button>().image.sprite = darkIcon;
    }
}
