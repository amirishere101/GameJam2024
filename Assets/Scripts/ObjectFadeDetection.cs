using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFadeDetection : MonoBehaviour {
    public GameObject player;

    private void FixedUpdate() {
        transform.position = player.transform.position;
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("trigger enter " + Time.time);
        if(other.gameObject.tag == "Object"){
            Debug.Log("Object deteced " + Time.time);
            ObjectFader _fader = other.gameObject.GetComponent<ObjectFader>();
            if(_fader != null){
                Debug.Log("Object Fader deteced " + Time.time);
                _fader.doFade = true;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        Debug.Log("trigger enter " + Time.time);
        if(other.gameObject.tag == "Object"){
            Debug.Log("Object deteced " + Time.time);
            ObjectFader _fader = other.gameObject.GetComponent<ObjectFader>();
            if(_fader != null){
                Debug.Log("Object Fader deteced " + Time.time);
                _fader.doFade = true;
            }
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Object"){
            ObjectFader _fader = other.gameObject.GetComponent<ObjectFader>();
            if(_fader != null){
                _fader.doFade = false;
            }
        }
    }
}
