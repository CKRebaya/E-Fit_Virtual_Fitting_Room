using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    public Slider slider;
    public GameObject sphere;
    
    public float scale = 0f;
    // Start is called before the first frame update
    public void ScaleObject(){
        scale = slider.value;
    }

    public void UpdateScaleViaButton(bool increment){
        if(increment){
            scale+=0.1f;
        }else{
            scale-=0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        slider.value = scale;
        sphere.transform.localScale = new Vector3(scale, scale, scale);
    }
}
