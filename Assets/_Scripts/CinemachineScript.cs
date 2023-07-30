using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public enum CameraState
{
    BeforeIdle,
    Idle,
    FollowBird,
    Disabled
}


public class CinemachineScript : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _main;
    [SerializeField] CinemachineVirtualCamera _startingShot;

    [SerializeField] float _startingShotDelay = 2;
    [SerializeField] float _birdCamResetDelay = 2;

    public CameraState _cameraState = CameraState.BeforeIdle;
    public GameObject _followBird;
    //[SerializeField] Transform _followBird;
    [SerializeField] Transform _mousePan;
    Vector3 _mousePanStartingPos;

    //private MoveBird birdScript;

    //Vector3 Difference;
    Vector2 _lastMousePos;

    //bool _isCameraPanAllowed = false;
    

    /// after delay change _strtingShot priority to below 10 +++
    // clamp X - axis borders and lock y axis
    // after shot follow bird until collision then stop
    // create camera movement by x- axis

    // Start is called before the first frame update
    void Start()
    {
        //_main.Follow = _followBird.transform;
        StartCoroutine(ChangeStartShotPriorityAfterDelay(_startingShotDelay));
        _mousePanStartingPos = _mousePan.transform.position;
        //ResetCamera = Camera.main.transform.position;
        //birdScript = _followBird.GetComponent<MoveBird>();
    }

    
    // Update is called once per frame
    void LateUpdate()
    {
        

        switch(_cameraState)
        {
            case(CameraState.BeforeIdle):
                _mousePan.transform.position = _mousePanStartingPos;
                _main.Follow = _mousePan.transform;
                _cameraState = CameraState.Idle;
                break;

            case (CameraState.Idle):
                CameraPan();
                break;

            case (CameraState.FollowBird):
                _main.Follow = _followBird.transform;
                break;
            case (CameraState.Disabled):
                _main.Follow = null;
                break;

        }
        
       

    }

    void CameraPan()
    {
        bool drag = false;

        Vector3 inputDir = new Vector3(0, 0, 0);
        if (Input.GetMouseButtonDown(0))
        {
            drag = true;
            _lastMousePos = Input.mousePosition;
            _main.Follow = _mousePan;
        }
        if (Input.GetMouseButtonUp(0))
        {
            drag = false;

        }

        if (drag)
        {
            Vector2 Difference = (Vector2)Input.mousePosition - _lastMousePos;

            inputDir.x = Difference.x;

            _lastMousePos = Input.mousePosition;
        }

        Vector3 moveDir = _mousePan.right * inputDir.x * -1;

        float moveSpeed = 1f;
        _mousePan.position += moveDir * moveSpeed * Time.deltaTime;
        _mousePan.position = Vector3.right * Mathf.Clamp(_mousePan.position.x, -5, 16);
    }


    IEnumerator ChangeStartShotPriorityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _startingShot.m_Priority = 0;
        _cameraState = CameraState.Idle;
    }

    //public void StopFollowing()
    //{
    //    _main.Follow = null;
    //    StartCoroutine(ResetCameraAfterDelay(_birdCamResetDelay));

    //}

    //IEnumerator ResetCameraAfterDelay(float count)
    //{
    //    yield return new WaitForSeconds(count);
    //    _main.Follow = _followBird.transform;
    //}

    

    

}
