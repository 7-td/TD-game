using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class Map : MonoBehaviour
    {

        public List<GameObject> StartPointList = new List<GameObject>();

        public List<GameObject> CityDoorList = new List<GameObject>();

        public List<GameObject> LeftTopHighlandList = new List<GameObject>();
        public List<GameObject> RightTopHighlandList = new List<GameObject>();
        public List<GameObject> LeftDownHighlandList = new List<GameObject>();
        public List<GameObject> RightDownHighlandList = new List<GameObject>();

        public List<GameObject> CityDoorFootRegion1 = new List<GameObject>();
        public List<GameObject> CityDoorFootRegion2 = new List<GameObject>();
        public List<GameObject> CityDoorFootRegion3 = new List<GameObject>();
        public List<GameObject> CityDoorFootRegion4 = new List<GameObject>();

        public List<List<GameObject>> CityDoorFootRegionList = new List<List<GameObject>>();

        public GameObject TowerBuilder;

        private const int Rows = 10;
        private const int Cols = 16;

        private List<List<GameObject>> mapPlane = new List<List<GameObject>>();

        private int[,] mapPlaneGridType = new int[10, 16] {
        {1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1 },
        {1,  1,  1,  1,  1,  1,  1,  9,  9,  1,  1,  1,  1,  1,  1,  1 },
        {1,  1,  10, 9,  1,  9,  9,  9,  9,  9,  9,  1,  9,  10, 1,  1 },
        {1,  1,  9,  1,  1,  9,  9,  14, 14, 9,  9,  1,  1,  9,  1,  1 },
        {1,  1,  1,  1,  9,  9,  14, 2,  2,  14, 9,  9,  1,  1,  1,  1 },
        {1,  1,  1,  1,  9,  9,  14, 2,  2,  14, 9,  9,  1,  1,  1,  1 },
        {1,  1,  9,  1,  1,  9,  9,  14, 14, 9,  9,  1,  1,  9,  1,  1 },
        {1,  1,  10, 9,  1,  9,  9,  9,  9,  9,  9,  1,  9,  10, 1,  1 },
        {1,  1,  1,  1,  1,  1,  1,  9,  9,  1,  1,  1,  1,  1,  1,  1 },
        {1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1 },
        };

        // 0起点 1，2,3，4城门脚区域 5,6,7,8高地区域 9城门 10不重要
        private int[,] mapPlaneRegion = new int[10, 16] {
        {0,   10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  0 },
        {10,  10,  10,  10,  10,  10,  10,  2,   2,   10,  10,  10,  10,  10,  10,  10 },
        {10,  10,  5,   5,   10,  1,    1,  2,   2,   2,   2,   10,  6,   6,   10,  10 },
        {10,  10,  5,   10,  10,  1,    1,  9,   9,   2,   2,   10,  10,  6,   10,  10 },
        {10,  10,  10,  10,  1,   1,    9,  10,  10,  9,   4,   4,   10,  10,  10,  10 },
        {10,  10,  10,  10,  1,   1,    9,  10,  10,  9,   4,   4,   10,  10,  10,  10 },
        {10,  10,  7,   10,  10,  3,    3,  9,   9,   4,   4,   10,  10,  8,   10,  10 },
        {10,  10,  7,   7,   10,  3,    3,  3,   3,   4,   4,   10,  8,   8,   10,  10 },
        {10,  10,  10,  10,  10,  10,  10,  3,   3,   10,  10,  10,  10,  10,  10,  10 },
        {0,   10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  10,  0 },
        };

        // Start is called before the first frame update
        void Start()
        {
            buildMap();
            fillLists();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void buildMap()
        {
            int k = 0;
            for (int i = 0; i < Rows; i++)
            {
                List<GameObject> row = new List<GameObject>();
                for (int j = 0; j < Cols; j++)
                {
                    row.Add(transform.GetChild(k).gameObject);
                    k++;
                }
                mapPlane.Add(row);
            }
            for (int i = 0; i < mapPlane.Count; i++)
            {
                for (int j = 0; j < mapPlane[i].Count; j++)
                {
                    mapPlane[i][j].AddComponent<MapGrid>();
                    MapGrid mapGrid = mapPlane[i][j].GetComponent<MapGrid>();

                    mapGrid.type = (MapGrid.MapGridType)mapPlaneGridType[i, j];

                    mapGrid.X = j;
                    mapGrid.Y = i;

                    if (i > 0)
                        mapGrid.Down = mapPlane[i - 1][j];
                    if (i < mapPlane.Count - 1)
                        mapGrid.Top = mapPlane[i + 1][j];
                    if (j > 0)
                        mapGrid.Left = mapPlane[i][j - 1];
                    if (j < mapPlane[i].Count - 1)
                        mapGrid.Right = mapPlane[i][j + 1];
                }
            }



        }

        private void fillLists()
        {
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Cols; j++)
                {
                    switch (mapPlaneRegion[i, j])
                    {
                        case 0:
                            StartPointList.Add(mapPlane[i][j]);
                            break;
                        case 1:
                            CityDoorFootRegion1.Add(mapPlane[i][j]);
                            break;
                        case 2:
                            CityDoorFootRegion2.Add(mapPlane[i][j]);
                            break;
                        case 3:
                            CityDoorFootRegion3.Add(mapPlane[i][j]);
                            break;
                        case 4:
                            CityDoorFootRegion4.Add(mapPlane[i][j]);
                            break;
                        case 5:
                            LeftTopHighlandList.Add(mapPlane[i][j]);
                            break;
                        case 6:
                            RightTopHighlandList.Add(mapPlane[i][j]);
                            break;
                        case 7:
                            LeftDownHighlandList.Add(mapPlane[i][j]);
                            break;
                        case 8:
                            RightDownHighlandList.Add(mapPlane[i][j]);
                            break;
                        case 9:
                            CityDoorList.Add(mapPlane[i][j]);
                            break;
                        default:
                            break;
                    }
                }
            }
            CityDoorFootRegionList.Add(CityDoorFootRegion1);
            CityDoorFootRegionList.Add(CityDoorFootRegion2);
            CityDoorFootRegionList.Add(CityDoorFootRegion3);
            CityDoorFootRegionList.Add(CityDoorFootRegion4);
        }


        public List<List<GameObject>> GetMapPlane()
        {
            return mapPlane;
        }

    }
}
