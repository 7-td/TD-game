using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Level3
{
    public class Enemy : MonoBehaviour
    {
        public enum EnemyType {
            Gold,
            Wood,
            Water,
            Soil,
            Fire
        }


        private List<GameObject> path;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetPath(List<GameObject> path)
        {
            this.path = path;
        }
    }
}
