using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveBird : MonoBehaviour
{
    private TrailRenderer trailrenderer;
    



    private void Start()
    {
        trailrenderer = gameObject.GetComponent<TrailRenderer>();
    }
    private void OnMouseDown()
    {
        trailrenderer.enabled = false;
    }


    private void OnMouseUp()
    {
        trailrenderer.enabled = true;
    }


    public IEnumerator CoroutineAction()
    {
        trailrenderer.emitting = false;
        yield return StartCoroutine(WaitFor.Frames(5)); // wait for 5 frames
        trailrenderer.emitting = true;
    }






}




