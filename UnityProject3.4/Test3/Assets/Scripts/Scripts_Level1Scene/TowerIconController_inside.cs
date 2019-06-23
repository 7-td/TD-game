using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerIconController_inside : MonoBehaviour
{
    private Transform elementChooseButton;
    private Transform TowerScrollViewContent;
    private Image image;

    Color shallowGray = new Color(0.4f, 0.4f, 0.4f);
    // Start is called before the first frame update
    void Start()
    {
        elementChooseButton = transform.Find("ElementChooseButtons");
        TowerScrollViewContent = this.transform.parent;
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchElementChooseButton()
    {
        Debug.Log(TowerScrollViewContent.childCount);
        for (int i = 0; i < TowerScrollViewContent.childCount; i++)
        {
            Transform iconframeTrans = TowerScrollViewContent.GetChild(i);
            Transform elementChooseButtonTrans = iconframeTrans.Find("ElementChooseButtons").transform;
            if (elementChooseButtonTrans.Equals(this.elementChooseButton))
            { }
            else
            {
                elementChooseButtonTrans.localScale = Vector3.zero;
                iconframeTrans.GetComponent<Image>().color = Color.white;

            }
        }
        elementChooseButton.localScale = (elementChooseButton.localScale == Vector3.zero) ? Vector3.one : Vector3.zero;
        image.color = (image.color == Color.white) ? shallowGray : Color.white;
    }


}
