using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class About : MonoBehaviour
{

    public GameObject background;//背景图片
    public GameObject title;//小组名
    public GameObject mumber;//小组成员
    public GameObject thanks;//感谢
    public GameObject thankspic;//感谢图
    public GameObject back;//返回按钮
    public int timer;//计时器 单位 秒

    public IEnumerator showOne()
    {
        while (timer >= 0)
        {
            yield return new WaitForSeconds(1);
            timer++;
            if (timer == 1)
                title.SetActive(true);
            if (timer == 2)
                mumber.SetActive(true);
            if (timer == 3)
            {
                thanks.SetActive(true);
                thankspic.SetActive(true);
            }
            if (timer == 4)
            {
                back.SetActive(true);
                timer = 0;
                yield break;
            }
        }
    }

    public void show()
    {
        background.SetActive(true);
        StartCoroutine(showOne());
    }

    public void showBack()
    {
        background.SetActive(false);
        title.SetActive(false);
        mumber.SetActive(false);
        thanks.SetActive(false);
        thankspic.SetActive(false);
        back.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
