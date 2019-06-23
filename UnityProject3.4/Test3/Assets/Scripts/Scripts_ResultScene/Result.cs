using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    public GameObject defeat;
    public GameObject AttackerVectory;
    public GameObject DefenderVectory;
    public GameObject VectoryItem;
    public Text Number;
    public Text Point1;
    public Text Point2;
    public Text Comment;
    public List<GameObject> DefeatSentences;
    public List<GameObject> GreatSentences;
    public List<GameObject> GoodSentences;
    public List<GameObject> NormalSentences;
    public List<GameObject> BadSentences;

    // Start is called before the first frame update
    void Start()
    {
        switch(GeneralPool.player)
        {
            case 1:
                switch (GeneralPool.winner)
                {
                    case 1:
                        AttackerVectory.SetActive(true);
                        VectoryItem.SetActive(true);

                        Number.text = "10";
                        Point1.text = "50";
                        Point2.text = "50";

                        int total = int.Parse(Point1.text) + int.Parse(Point2.text);
                        if (total >= 85)
                        {
                            Comment.text = "优";
                            int index = Random.Range(0, 3);
                            GreatSentences[index].SetActive(true);
                        }
                        if (total<85 && total>=70)
                        {
                            Comment.text = "良";
                            int index = Random.Range(0, 1);
                            GoodSentences[index].SetActive(true);
                        }
                        if (total < 70 && total >= 50)
                        {
                            Comment.text = "中";
                            int index = Random.Range(0, 1);
                            NormalSentences[index].SetActive(true);
                        }
                        if (total < 50)
                        {
                            Comment.text = "差";
                            int index = Random.Range(0, 1);
                            NormalSentences[index].SetActive(true);
                        }
                        break;
                    case 2:
                        defeat.SetActive(true);
                        int index2 = Random.Range(0, 3);
                        DefeatSentences[index2].SetActive(true);
                        break;
                    default:
                        break;
                }
                break;
            case 2:
                switch (GeneralPool.winner)
                {
                    case 1:
                        defeat.SetActive(true);
                        int index2 = Random.Range(0, 3);
                        DefeatSentences[index2].SetActive(true);
                        break;
                    case 2:
                        DefenderVectory.SetActive(true);
                        VectoryItem.SetActive(true);

                        Number.text = "10";
                        Point1.text = "50";
                        Point2.text = "50";

                        int total = int.Parse(Point1.text) + int.Parse(Point2.text);
                        if (total >= 85)
                        {
                            Comment.text = "优";
                            int index = Random.Range(0, 3);
                            GreatSentences[index].SetActive(true);
                        }
                        if (total < 85 && total >= 70)
                        {
                            Comment.text = "良";
                            int index = Random.Range(0, 1);
                            GoodSentences[index].SetActive(true);
                        }
                        if (total < 70 && total >= 50)
                        {
                            Comment.text = "中";
                            int index = Random.Range(0, 1);
                            NormalSentences[index].SetActive(true);
                        }
                        if (total < 50)
                        {
                            Comment.text = "差";
                            int index = Random.Range(0, 1);
                            BadSentences[index].SetActive(true);
                        }                        
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackToSelectLevelScene()
    {
        SceneManager.LoadScene("SelectLevelScene");
    }
}
