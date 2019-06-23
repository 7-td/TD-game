using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Level3 {

    public class UIController : MonoBehaviour
    {

        public GameObject EnemyGroupButtonScrollBar;
        public GameObject EnemyGroupButtonPrefab;

        private GameObject confirmButton;

        private RoundController roundController;
        private PathPlanner pathPlanner;

        private GameObject currentSelectedEnemyGroupButton = null;


        private Vector3 addButtonRightShiftOffset = new Vector3(120, 0, 0);

        private List<GameObject> showedStartGridList;
        private Coroutine showStartGridsCoroutine;

        private GameObject map;

        private GameObject unreachableText;

        private GameObject selectStartPointFirstText;

        private List<GameObject> showedPath;
        private Coroutine showPathCoroutine = null;

        private List<GameObject> enemyGroupButtonList = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            DoInit();
        }

        // Update is called once per frame
        void Update()
        {
            if (pathPlanner.IsPlanning() && Input.GetMouseButtonUp(0))
            {
                handleLeftClickWhenPlanPath();
            }
            if (Input.GetMouseButton(1))
            {
                handleRightClick();
            }
            if (pathPlanner.IsStartPointSelected() && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit, 100);
                if (isCollider)
                {
                    pathPlanner.PreviewPathTo(hit.collider.gameObject);
                }
            }
        }

        private void DoInit()
        {
            roundController = GetComponent<RoundController>();
            pathPlanner = GetComponent<PathPlanner>();
            confirmButton = GameObject.Find(ObjectNames.ConfirmButton);
            confirmButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                pathPlanner.FinishPlanning(currentSelectedEnemyGroupButton.GetComponent<EnemyGroup>());
                ResetUI();
            });
            unreachableText = GameObject.Find(ObjectNames.UnreachableRegionText);
            selectStartPointFirstText = GameObject.Find(ObjectNames.SelectStartPointFirstText);
            map = GameObject.Find(ObjectNames.Map);
            confirmButton.SetActive(false);
            unreachableText.SetActive(false);
            selectStartPointFirstText.SetActive(false);
        }

        public void AddEnemyGroupButton()
        {
            GameObject addButton = EnemyGroupButtonScrollBar.transform.GetChild(0).gameObject;

            GameObject enemyGroup = Instantiate(EnemyGroupButtonPrefab);           
            enemyGroup.transform.SetParent(EnemyGroupButtonScrollBar.transform);
            enemyGroup.transform.position = addButton.transform.position;
            enemyGroup.transform.localScale = addButton.transform.localScale;
            enemyGroup.AddComponent<EnemyGroup>();
            enemyGroup.AddComponent<EnemyGroupButton>();
            enemyGroup.GetComponent<EnemyGroupButton>().DoInit(this, roundController, pathPlanner);
            enemyGroup.GetComponent<Button>().onClick.AddListener(() => OnEnemyGroupButtonClicked());

            enemyGroupButtonList.Add(enemyGroup);

            addButton.transform.position += addButtonRightShiftOffset;

        }

        public void ShowStartGrids()
        {
            StopShowStartGrids();
            showedStartGridList = pathPlanner.GetAvailableStartPoints();
            showStartGridsCoroutine = StartCoroutine(showStartGrids());
        }

        public void StopShowStartGrids()
        {
            if (showStartGridsCoroutine != null)
            {
                StopCoroutine(showStartGridsCoroutine);
                showStartGridsCoroutine = null;
                for (int i = 0; i < showedStartGridList.Count; i++)
                {
                    showedStartGridList[i].GetComponent<MapGrid>().ResetColor();
                }
                showedStartGridList = null;
            }
        }

        private IEnumerator showStartGrids()
        {
            while (true)
            {
                foreach (GameObject grid in showedStartGridList)
                {
                    grid.GetComponent<MapGrid>().SetColor(Color.red);
                }
                yield return new WaitForSeconds(0.3f);
                for (int i = 0; i < showedStartGridList.Count; i++)
                {
                    showedStartGridList[i].GetComponent<MapGrid>().ResetColor();
                }
                yield return new WaitForSeconds(0.3f);
            }
        }



        private void handleLeftClickWhenPlanPath()
        {
            GameObject mapGrid = getClickedMapGrid();
            if (mapGrid == null)
            {
                return;
            }
            if (pathPlanner.IsStartPointSelected() == false && map.GetComponent<Map>().StartPointList.Contains(mapGrid) == false)
            {
                showSelectStartPointFirstTextShortly();
                return;
            }

            if ((mapGrid.GetComponent<MapGrid>().type & MapGrid.MapGridType.Reachable) == 0)
            {
                ShowUnreachableTextShortly();
                return;
            }
            pathPlanner.OnMapGridIsClicked(mapGrid);
        }

        private void handleRightClick()
        {
            if (currentSelectedEnemyGroupButton != null)
            {
                ResetUI();
            }
        }

        private GameObject getClickedMapGrid()
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit, 100);
                if (isCollider)
                {
                    return hit.collider.gameObject;
                }
            }
            return null;
        }

        public void ShowUnreachableTextShortly()
        {
            unreachableText.SetActive(true);
            StartCoroutine(DelayFunc.Invoke(() =>
            {
               unreachableText.SetActive(false);
            }, 1.0f));
        }

        private void showSelectStartPointFirstTextShortly()
        {
            selectStartPointFirstText.SetActive(true);
            StartCoroutine(DelayFunc.Invoke(() =>
            {
               selectStartPointFirstText.SetActive(false);
            }, 1.0f));
        }

        public void ShowPath(List<GameObject> path)
        {
            StopShowPath();
            showedPath = path;
            if (path.Count == 0)
                return;
            showPathCoroutine = StartCoroutine(showPath());
        }

        public void StopShowPath()
        {
            if (showPathCoroutine != null)
            {
                StopCoroutine(showPathCoroutine);
                showPathCoroutine = null;
                for (int i = 0; i < showedPath.Count; i++)
                {
                    showedPath[i].GetComponent<MapGrid>().ResetColor();
                }
                showedPath = null;
            }
        }

        private IEnumerator showPath()
        {
            while (true)
            {
                if (showedPath.Count == 1)
                {
                    showedPath[0].GetComponent<MapGrid>().SetColor(Color.black);
                    yield return new WaitForSeconds(0.3f);
                    showedPath[0].GetComponent<MapGrid>().ResetColor();
                    yield return new WaitForSeconds(0.3f);
                }
                else if (showedPath.Count > 1) 
                {
                    float interval = 1.0f / (showedPath.Count);
                    foreach (GameObject grid in showedPath)
                    {
                        grid.GetComponent<MapGrid>().SetColor(Color.black);
                        yield return new WaitForSeconds(interval);
                        grid.GetComponent<MapGrid>().ResetColor();
                    }
                }
            }
        }

        public void ShowOkButton()
        {
            confirmButton.SetActive(true);
        }

        public GameObject GetCurrentSelectedEnemyGroupButton()
        {
            return currentSelectedEnemyGroupButton;
        }

        public void SetCurrentSelectedEnemyGroupButton(GameObject currentSelectedEnemyGroupButton)
        {
            this.currentSelectedEnemyGroupButton = currentSelectedEnemyGroupButton;
        }

        public void ResetUI()
        {
            StopAllCoroutines();
            confirmButton.SetActive(false);
            List<List<GameObject>> mapPlane = map.GetComponent<Map>().GetMapPlane();
            foreach(List<GameObject> row in mapPlane)
            {
                foreach(GameObject grid in row)
                {
                    grid.GetComponent<MapGrid>().ResetColor();
                }
            }
            foreach(GameObject btn in enemyGroupButtonList)
            {
                btn.GetComponent<EnemyGroupButton>().ResetPosition();
            }
            showedPath = null;
            showedStartGridList = null;
            showPathCoroutine = null;
            showStartGridsCoroutine = null;
            currentSelectedEnemyGroupButton = null;
            pathPlanner.StopPlanning();

            selectStartPointFirstText.SetActive(false);
            unreachableText.SetActive(false);
        }

        private void OnEnemyGroupButtonClicked()
        {
            GameObject btn = EventSystem.current.currentSelectedGameObject;
            if (btn == currentSelectedEnemyGroupButton)
            {
                return;
            }
            else
            {
                if (currentSelectedEnemyGroupButton != null)
                    ResetUI();
                currentSelectedEnemyGroupButton = btn;
                btn.GetComponent<EnemyGroupButton>().OnClicked();
            }
        }

    }
}