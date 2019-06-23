using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Level3
{
    public class MapGrid : MonoBehaviour
    {
        public enum MapGridType
        {
            Reachable = 1,
            Highland = 2,
            Door = 4,
            Buildable = 8
        }

        private GameObject tower = null;

        public MapGridType type = MapGridType.Reachable;

        public GameObject Top = null;
        public GameObject Down = null;
        public GameObject Left = null;
        public GameObject Right = null;

        public int X;
        public int Y;

        private Color color;

        // Start is called before the first frame update
        void Start()
        {
            color = GetComponent<MeshRenderer>().material.color;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetColor(Color color)
        {
            GetComponent<MeshRenderer>().material.color = color;
        }

        public void ResetColor()
        {
            GetComponent<MeshRenderer>().material.color = this.color;
        }

        public bool IsReachable()
        {
            return (type & MapGridType.Reachable) != 0;
        }
        public bool IsHighland()
        {
            return (type & MapGridType.Highland) != 0;
        }
        public bool IsDoor()
        {
            return (type & MapGridType.Door) != 0;
        }
        public bool IsBuildable()
        {
            return (type & MapGridType.Buildable) != 0;
        }

        public bool HasTower()
        {
            return tower != null;
        }
    }
}
