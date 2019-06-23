using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level3
{
    public class RoundController : MonoBehaviour
    {
        private GameObject map;

        
        private enum RoundState
        {
            Ready,      //规划阶段
            Dispatch,   //派遣阶段(30s)
            Fight,      //战斗阶段
        } 

        private TowerBuilder towerBuilder;

        private int currentRound = 0;

        private RoundState roundState = RoundState.Ready;

        private UIController uiController;
        // Start is called before the first frame update
        void Start()
        {
            uiController = GetComponent<UIController>();
            map = GameObject.Find(ObjectNames.Map);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool IsInReadyStage()
        {
            return roundState == RoundState.Ready;
        }

        private void nextRound()
        {
            currentRound++;
        }


    }
}
