using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class LevelController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startLevel(int Level)
    {
        string levelname = "Level" + Level.ToString() + "Scene";
        if (SceneExist(levelname))
        {            
            switch(Level)
            {
                case 1:
                    GeneralPool.player = 2;
                    break;
                case 2:
                    GeneralPool.player = 2;
                    break;
                case 3:
                    GeneralPool.player = 1;
                    break;
            }
            SceneManager.LoadScene(levelname);
        }
        else
            Debug.Log("加载失败，关卡未制作");


    }

    bool SceneExist(string SceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string sp = SceneUtility.GetScenePathByBuildIndex(i);
            string nam = sp.Remove(0, sp.LastIndexOf('/') + 1);
            nam = nam.Remove(nam.LastIndexOf('.'));
            if (SceneName == nam)
            {
                return true;
            }
        }
        return false;
    }

    public void back()
    {
        SceneManager.LoadScene("StartScene");
    }


}
