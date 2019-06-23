using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{

    public class PathPlanner : MonoBehaviour
    {
        private GameObject map;

        private List<GameObject> availableStartPoints;

        private RoundController roundController;

        private UIController uiController;

        private bool planning = false;

        private List<GameObject> path = new List<GameObject>();

        private List<GameObject> previewedPath = null;

        // Start is called before the first frame update
        void Start()
        {
            map = GameObject.Find(ObjectNames.Map);
            availableStartPoints = map.GetComponent<Map>().StartPointList;
            roundController = GetComponent<RoundController>();
            uiController = GetComponent<UIController>();
        }

        public void Plan()
        {
            planning = true;
        }

        public List<GameObject> GetAvailableStartPoints()
        {
            return availableStartPoints;
        }

        public bool IsPlanning()
        {
            return planning;
        }

        public void StopPlanning()
        {
            planning = false;
            path.Clear();
        }




        public void OnMapGridIsClicked(GameObject mapGrid)
        {
            if (path.Count == 0)
            {
                path.Add(mapGrid);
                uiController.ShowOkButton();
                uiController.StopShowStartGrids();
            }
            else if (path[path.Count - 1] != mapGrid)
            {
                List<GameObject> newPath = findShortestPath(path[path.Count - 1], mapGrid);
                if (newPath == null)
                {
                    uiController.ShowUnreachableTextShortly();
                    return;
                }
                for (int i = 1; i < newPath.Count; i++)
                {
                    path.Add(newPath[i]);
                }
            }
            List<GameObject> showedPath = new List<GameObject>(path);
            showedPath.RemoveAt(showedPath.Count - 1);
            uiController.ShowPath(showedPath);
        }

        private List<GameObject> findShortestPath(GameObject from, GameObject to)
        {
            if (from == to)
                return new List<GameObject>() { from };
            HashSet<GameObject> visited = new HashSet<GameObject>();
            Queue<GameObject> q = new Queue<GameObject>();
            Hashtable preGrids = new Hashtable();
            q.Enqueue(from);
            visited.Add(from);
            while (q.Count > 0)
            {
                GameObject obj = q.Dequeue();
                if (obj == to)
                    break;
                MapGrid mapGrid = obj.GetComponent<MapGrid>();
                GameObject top = mapGrid.Top;
                GameObject right = mapGrid.Right;
                GameObject down = mapGrid.Down;
                GameObject left = mapGrid.Left;
                if (top != null && visited.Contains(top) == false && top.GetComponent<MapGrid>().IsReachable()) 
                {
                    q.Enqueue(top);
                    visited.Add(top);
                    preGrids.Add(top, obj);
                }
                if (right != null && visited.Contains(right) == false && right.GetComponent<MapGrid>().IsReachable())
                {
                    q.Enqueue(right);
                    visited.Add(right);
                    preGrids.Add(right, obj);
                }
                if (down != null && visited.Contains(down) == false && down.GetComponent<MapGrid>().IsReachable())
                {
                    q.Enqueue(down);
                    visited.Add(down);
                    preGrids.Add(down, obj);
                }
                if (left != null && visited.Contains(left) == false && left.GetComponent<MapGrid>().IsReachable())
                {
                    q.Enqueue(left);
                    visited.Add(left);
                    preGrids.Add(left, obj);
                }
            }
            if (preGrids.Contains(to) == false)
                return null;
            List<GameObject> path = new List<GameObject>();
            GameObject currentObject = to;
            while (currentObject != from)
            {
                path.Add(currentObject);
                currentObject = (GameObject)preGrids[currentObject];
            }
            path.Add(from);
            path.Reverse();
            return path;
        }

        public void PreviewPathTo(GameObject to)
        {
            if (previewedPath != null)
            {
                for (int i = 0; i < previewedPath.Count; i++)
                    previewedPath[i].GetComponent<MapGrid>().ResetColor();

            }
            previewedPath = findShortestPath(path[path.Count - 1], to);
            if (previewedPath != null)
            {
                if (previewedPath.Count > 0)
                    previewedPath[0].GetComponent<MapGrid>().SetColor(Color.black);
                for (int i = 1; i < previewedPath.Count; i++)
                    previewedPath[i].GetComponent<MapGrid>().SetColor(Color.gray);
            }
        }

        public void FinishPlanning(EnemyGroup enemyGroup)
        {
            enemyGroup.SetPath(path);
            StopPlanning();
        }

        public bool IsStartPointSelected()
        {
            return path.Count > 0;
        }
    }
}
