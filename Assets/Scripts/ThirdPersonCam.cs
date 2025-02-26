using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public Vector3 _offset;
    private Vector3 _currVelocity = Vector3.zero;
    [SerializeField] Transform target;
    [SerializeField] float smoothTime;
    [SerializeField] ObjectFader _fader;
    [SerializeField] GameObject player;

    void Start(){
        _offset = transform.position - target.position;
    }

    void Update(){
        Vector3 targetPosition = target.position + _offset;  
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currVelocity, smoothTime);

        // Vector3 dir = target.position - transform.position;
        // Ray ray = new Ray(new Vector3(transform.position.x, target.position.y, transform.position.z), dir);
        // RaycastHit hit;
        // Debug.DrawRay(new Vector3(transform.position.x, target.position.y, transform.position.z), dir, Color.red);

        // if(Physics.Raycast(ray, out hit)){
        //     if(hit.collider == null) {
        //         return;
        //     }
        //     if(hit.collider.gameObject.tag == "Player"){
        //         Debug.Log("Nothing between player and camera");
        //         if(_fader != null) {
        //             _fader.doFade = false;
        //         }
        //     } 
        // } else {
        //     Debug.Log("Object Detected");
        //     _fader = hit.collider.gameObject.GetComponent<ObjectFader>();
        //     if(_fader != null){
        //         _fader.doFade = true;
        //     } else {
        //         Debug.Log("Object has no ObjectFader componenet attached");

        //     }
        // }
    }
    
}
