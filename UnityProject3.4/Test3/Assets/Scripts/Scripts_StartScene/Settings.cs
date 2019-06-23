using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]

public class Settings : MonoBehaviour
{
    public GameObject background;//背景图片
    public GameObject Slider1;//背景音乐滑块
    public GameObject Slider2;//音效滑块
    public GameObject back;//返回按钮
    public GameObject dropdown;//下拉选单
    public AudioSource BGM;//背景音乐
    public Slider BGMSlider;//背景音乐滑块
    public Slider SoundEffectSlider;//音效滑块
    public Dropdown drop;//下拉选单
    private int lastIndex;//下拉选单的选择项

    public void show()
    {
        background.SetActive(true);
        Slider1.SetActive(true);
        Slider2.SetActive(true);
        back.SetActive(true);
        dropdown.SetActive(true);
    }

    public void showback()
    {
        background.SetActive(false);
        Slider1.SetActive(false);
        Slider2.SetActive(false);
        back.SetActive(false);
        dropdown.SetActive(false);
    }

    public void BGMControl()
    {
        BGM.volume = BGMSlider.value;
    }

    public void SoundEffectControl()
    {

    }

    public void ResolutionControl()
    {
        //print("     value:" + drop.value);
        if (drop.value == 0)
        {
            Resolution[] resolutions = Screen.resolutions;//获取设置当前屏幕分辩率
            //print(resolutions[0].height + " " + resolutions[0].width);
            //Debug.Log(resolutions[0].height + " " + resolutions[0].width);
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);//设置当前分辨率
            Screen.fullScreen = true;
        }
        if(drop.value==1)
        {
            Screen.SetResolution(1080, 720, false);
        }
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
