using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerController : MonoBehaviour
{
    public GameObject towerPrefab;

    public GameObject buildablePlane;

    public GameObject cancelBuildTowerButton;

    private GameObject tempTower;

    private bool towerMode = false;

    // Start is called before the first frame update
    void Start()
    {
        tempTower = Instantiate(towerPrefab);
        tempTower.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (towerMode)
        {
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
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (tempTower.activeSelf)
                        {
                            GameObject realTower = Instantiate(towerPrefab);
                            realTower.transform.position = tempTower.transform.position;
                            realTower.GetComponent<Collider>().enabled=true;
                            GeneralPool.addToTowerList(realTower);
                            GeneralPool.addToTowerAndBPDict(realTower, rayHit.collider.gameObject);
                            rayHit.collider.gameObject.SetActive(false);
                            //Debug.Log(rayHit.collider.gameObject.name);
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
        return list.Count > 0;
    }

    public void changeTowerMode()
    {
        towerMode = !towerMode;
        buildablePlane.SetActive(!buildablePlane.activeSelf);
        if (tempTower.activeSelf)
        {
            tempTower.SetActive(false);
        }
        cancelBuildTowerButton.SetActive(!cancelBuildTowerButton.activeSelf);
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

    

}