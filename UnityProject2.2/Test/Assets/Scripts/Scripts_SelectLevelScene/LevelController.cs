using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private int selectedLevel = 1;//从1开始
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeLevel(int Level)
    {
        selectedLevel = Level;
    }

    public void startLevel()
    {
        string levelname="Level" + selectedLevel.ToString() + "Scene";
        if(SceneExist(levelname))
        SceneManager.LoadScene(levelname);
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
}
