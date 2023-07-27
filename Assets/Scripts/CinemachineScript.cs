using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

public class CinemachineScript : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _main;
    [SerializeField] CinemachineVirtualCamera _startingShot;

    [SerializeField] float _startingShotDelay = 2;
    [SerializeField] float _birdCamResetDelay = 2;

    [SerializeField] Transform _followObject;
    [SerializeField] Transform _mousePan;

    private MoveBird birdScript;

    //Vector3 Difference;
    Vector2 _lastMousePos;
    

    /// after delay change _strtingShot priority to below 10 +++
    // clamp X - axis borders and lock y axis
    // after shot follow bird until collision then stop
    // create camera movement by x- axis

    // Start is called before the first frame update
    void Start()
    {
        _main.Follow = _followObject;
        //ResetCamera = Camera.main.transform.position;
        birdScript = _followObject.GetComponent<MoveBird>();
    }

    bool drag;
    // Update is called once per frame
    void Update()
    {
        _startingShotDelay -= Time.deltaTime;

        if (_startingShotDelay <= 0.0f)
        {
            ChangeStartShotPriority();
        }
        
        if(!birdScript.isAiming && !birdScript.isFired)
        {
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

        if (birdScript.isAiming || birdScript.isFired)
        {
            _main.Follow = _followObject;
        }

    }

    void ChangeStartShotPriority()
    {
        _startingShot.m_Priority = 0;
    }

    public void StopFollowing()
    {
        _main.Follow = null;
        StartCoroutine(ResetCameraAfterDelay(_birdCamResetDelay));

    }

    IEnumerator ResetCameraAfterDelay(float count)
    {
        yield return new WaitForSeconds(count);
        _main.Follow = _followObject;
    }

    

    

}
