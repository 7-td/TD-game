using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Level3 {

    public class EnemyGroupButton : MonoBehaviour
    {

        private Vector3 position;

        private Vector3 enemyGroupButtonUpShiftOffset = new Vector3(0, 20, 0);

        private UIController uiController;
        private RoundController roundController;
        private PathPlanner pathPlanner;

        // Start is called before the first frame update
        void Start()
        {
            position = transform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpShift()
        {
            transform.position += enemyGroupButtonUpShiftOffset;
        }

        public void ResetPosition()
        {
            transform.position = position;
        }

        public void DoInit(UIController uiController, RoundController roundController, PathPlanner pathPlanner)
        {
            this.uiController = uiController;
            this.roundController = roundController;
            this.pathPlanner = pathPlanner;
        }

        public void OnClicked()
        {
            UpShift();
            if (GetComponent<EnemyGroup>().IsReady())
            {
                uiController.ShowPath(GetComponent<EnemyGroup>().GetPath());
                Debug.Log("showing path");
            }
            else
            {
                uiController.ShowStartGrids();
                pathPlanner.Plan();
            }
        }
    }
}