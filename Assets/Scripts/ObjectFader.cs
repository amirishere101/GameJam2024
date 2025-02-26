using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed = 2f, fadeAmount = 0.5f;
    float originalOpacity;
    Material mat;
    public bool doFade = false;

    void Start() {
        mat = GetComponent<Renderer>().material;
        originalOpacity = mat.color.a;
    }

    void FixedUpdate(){
        if(doFade){
            FadeNow();
        } else {
            ResetFade();
        }
    }

    public void FadeNow(){
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
        mat.color = smoothColor;
    }

    public void ResetFade(){
        Color currentColor = mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed * Time.deltaTime));
        mat.color = smoothColor;
    }
}
