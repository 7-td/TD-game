using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class BasicInfo
{
    public enum elementAttr
    {
        NO = -1,
        Metal = 0,
        Grass = 1,
        Water = 2,
        Fire = 3,
        Ground = 4
    }
    public enum enemyStates
    {
        Stop = 0,//停止
        Walk = 1,//行走
        Attack = 2//攻击
    }

    public static string[] enemyKindsBasic ={
        "Soldier",
        "SuperiorSoldier",
        "Mage",
        "SuperiorMage"
    };

    public static string[] enemyKinds ={
        "SoldierMetal",
        "SoldierGrass",
        "SoldierWater",
        "SoldierFire",
        "SoldierGround",
        "SuperiorSoldierMetal",
        "SuperiorSoldierGrass",
        "SuperiorSoldierWater",
        "SuperiorSoldierFire",
        "SuperiorSoldierGround",
        "MageMetal",
        "MageGrass",
        "MageWater",
        "MageFire",
        "MageGround",
        "SuperiorMageMetal",
        "SuperiorMageGrass",
        "SuperiorMageWater",
        "SuperiorMageFire",
        "SuperiorMageGround"
    };

    public static string[] towerKindsBasic ={
        "ArrowTower",
        "GasMethodTower"
    };

    public static string[] towerKinds ={
        "ArrowTowerMetal",
        "ArrowTowerGrass",
        "ArrowTowerWater",
        "ArrowTowerFire",
        "ArrowTowerGround",
        "GasMethodTowerMetal",
        "GasMethodTowerGrass",
        "GasMethodTowerWater",
        "GasMethodTowerFire",
        "GasMethodTowerGround",
        "ElementFortTowerMetal",
        "ElementFortTowerGrass",
        "ElementFortTowerWater",
        "ElementFortTowerFire",
        "ElementFortTowerGround"
    };

    public static Dictionary<string, string> EngToCHS_Dict = new Dictionary<string, string>{
        {"ArrowTower","弓箭塔"},
        {"GasMethodTower","气法塔"},
        {"ElementFortTower","元素要塞"},
        {"Soldier","武夫"},
        {"SuperiorSoldier","武夫精兵"},
        {"Mage","术士"},
        {"SuperiorMage","术士精兵"},
        {"Metal","金"},
        {"Grass","木"},
        {"Water","水"},
        {"Fire","火"},
        {"Ground","土"},
    };


    // public static Dictionary<elementAttr, elementAttr> ElementAttackChart = new Dictionary<elementAttr, elementAttr>{
    //     {elementAttr.Metal,elementAttr.Grass},
    //     {elementAttr.Grass,elementAttr.Water},


    // };

    public static List<elementAttr> ElementAttackChart = new List<elementAttr>//相克关系
    {
        elementAttr.Metal,
        elementAttr.Grass,
        elementAttr.Ground,
        elementAttr.Water,
        elementAttr.Fire
    };

    public static List<elementAttr> ElementPromoteChart = new List<elementAttr>//相生关系
    {
        elementAttr.Metal,
        elementAttr.Water,
        elementAttr.Grass,
        elementAttr.Fire,
        elementAttr.Ground
    };

    public static float calcuteAttackPower(float basicAttackPower, elementAttr attackerEA, elementAttr targetEA)
    {
        float finalAttackPower = basicAttackPower;
        int attackerEA_index=ElementAttackChart.IndexOf(attackerEA);
        int targetEA_index=ElementAttackChart.IndexOf(targetEA);
        if(attackerEA_index==4&&targetEA_index==0)
        {
            finalAttackPower*=1.2f;
        }
        else
        {
            if(attackerEA_index+1==targetEA_index)
            {
                finalAttackPower*=1.2f;
            }
        }

        return finalAttackPower;
    }






}
