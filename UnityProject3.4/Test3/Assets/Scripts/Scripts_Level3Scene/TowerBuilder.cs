using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Level3
{
    public class TowerBuilder : MonoBehaviour
    {
        private Dictionary<string, GameObject> towerPrefabList = new Dictionary<string, GameObject>();

        public List<GameObject> TowerPrefabList;

        private GameObject map;

        // Start is called before the first frame update
        void Start()
        {
            makePrefabDictionary();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void makePrefabDictionary()
        {
            foreach (GameObject prefab in TowerPrefabList)
            {
                GameObject tower = Instantiate(prefab);
                towerPrefabList.Add(tower.GetComponent<Tower>().name, prefab);
            }
        }

        private void buildTower(string name, GameObject grid)
        {
            GameObject tower = Instantiate(towerPrefabList[name]);
            tower.transform.parent = map.transform;
            tower.transform.position = grid.transform.position;
        }

        public void BuildTowersForRound(int round)
        {
            List<GameObject> gridList = new List<GameObject>();

            if (round == 1)
            {
                buildTower(ObjectNames.GasMethodTowerFire, map.GetComponent<Map>().LeftTopHighlandList[0]);
                buildTower(ObjectNames.ArrowTowerFire, map.GetComponent<Map>().LeftTopHighlandList[1]);
                buildTower(ObjectNames.ArrowTowerFire, map.GetComponent<Map>().LeftTopHighlandList[2]);

                buildTower(ObjectNames.GasMethodTowerMetal, map.GetComponent<Map>().RightTopHighlandList[1]);
                buildTower(ObjectNames.ArrowTowerMetal, map.GetComponent<Map>().RightTopHighlandList[0]);
                buildTower(ObjectNames.ArrowTowerMetal, map.GetComponent<Map>().RightTopHighlandList[2]);

                buildTower(ObjectNames.GasMethodTowerGrass, map.GetComponent<Map>().LeftDownHighlandList[1]);
                buildTower(ObjectNames.ArrowTowerGrass, map.GetComponent<Map>().LeftDownHighlandList[0]);
                buildTower(ObjectNames.ArrowTowerGrass, map.GetComponent<Map>().LeftDownHighlandList[2]);

                buildTower(ObjectNames.GasMethodTowerGround, map.GetComponent<Map>().RightDownHighlandList[2]);
                buildTower(ObjectNames.ArrowTowerGround, map.GetComponent<Map>().RightDownHighlandList[0]);
                buildTower(ObjectNames.ArrowTowerGround, map.GetComponent<Map>().RightDownHighlandList[1]);
            }
            else if (round >= 3 && round <= 25)
            {

            }
            else
            {
                Debug.LogError("Error!");
            }
            
        }

        private List<GameObject> getRegionOfLeastTowerAndCount(out int count)
        {
            List<GameObject> ret = null;
            List<List<GameObject>> cityDoorFootRegionList = map.GetComponent<Map>().CityDoorFootRegionList;
            Dictionary<int, List<GameObject>> towerCounter = new Dictionary<int, List<GameObject>>();
            for(int i = 0; i < cityDoorFootRegionList.Count; i++)
            {
                int counter = 0;
                List<GameObject> region = cityDoorFootRegionList[i];
                foreach (GameObject grid in region)
                {
                    counter += grid.GetComponent<MapGrid>().HasTower() ? 1 : 0;
                }
                towerCounter.Add(counter, region);
            }
            int max = 0;
            foreach(var item in towerCounter)
            {
                if (ret == null)
                {
                    ret = item.Value;
                    max = item.Key;
                }
                else if (item.Key > max)
                {
                    max = item.Key;
                    ret = item.Value;
                }
            }
            count = max;
            return ret;
        }

        public void SaveEnemyGroupInfo()
        {

        }

    }
}
