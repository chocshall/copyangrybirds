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


    //public IEnumerator CoroutineAction()
    //{
    //    trailrenderer.emitting = false;
    //    yield return WaitFor.Frames(5)); // wait for 5 frames
    //    trailrenderer.emitting = true;
    //}






    [SerializeField] CinemachineScript _cinemachineScript;
    [HideInInspector] public bool isFired;
  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFired)
        {
            _cinemachineScript.StopFollowing();
            isFired = false;
        }
            
    }
}




