using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Paused,
    Playing,
    Lost,
    Won
}

public class GameManager : MonoBehaviour
{
    
    [SerializeField] Slingshot _slingshot;

    int _currentBirdIndex;
    List<GameObject> _Birds;

    List<GameObject> _Pigs;

    public static GameState CurrentGameState = GameState.Playing;


    [SerializeField] CinemachineScript _mainCamera;
    [SerializeField] float _birdResetDelay = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        _Birds = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird"));
        _Pigs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Pig"));
        _currentBirdIndex = _Birds.Count - 1;
        //_slingshot.enabled = false;
        _slingshot.BirdToThrow = _Birds[_currentBirdIndex];
        _mainCamera._followBird = _Birds[_currentBirdIndex];
        
    }

    private void Start()
    {
        AnimateBirdToSlingshot();
    }

    // Update is called once per frame
    void Update()
    {
        switch(CurrentGameState)
        {
            case GameState.Playing:
                if(_slingshot.slingshotState == SlingshotState.Pulling)
                {
                    //disable CameraMovement
                    _mainCamera._cameraState = CameraState.Disabled; 
                    break;
                }
                if (_slingshot.slingshotState == SlingshotState.Fired)
                    //&& nothing is moving )
                {
                    _slingshot.enabled = false;
                    _mainCamera._cameraState = CameraState.FollowBird;

                    //OnBirdImpact get event to start PrepareAnotherBird courutine
                    if (_Birds[_currentBirdIndex].TryGetComponent<MoveBird>(out MoveBird birdScript))
                    {
                        if(birdScript._birdState == BirdState.Stopped)
                        {
                            StartCoroutine(PrepareAnotherBird(_birdResetDelay));
                            _slingshot.slingshotState = SlingshotState.Idle;
                        }
                    }
                       

                }


                break;
            case GameState.Paused:



                break;
            case GameState.Won:


                break;
            case GameState.Lost:


                break;

        }

    }
    IEnumerator PrepareAnotherBird(float delay)
    {
        yield return new WaitForSeconds(delay);
        _currentBirdIndex--;
        MoveCameraToSlingshot();
        AnimateBirdToSlingshot();
        
    }

    void MoveCameraToSlingshot()
    {
        _mainCamera._followBird = _Birds[_currentBirdIndex];
        _mainCamera._cameraState = CameraState.BeforeIdle;
    }



    void AnimateBirdToSlingshot()
    {
        _slingshot.enabled = true;
        _slingshot.BirdToThrow = _Birds[_currentBirdIndex];
        StartCoroutine(_Birds[_currentBirdIndex].GetComponent<MoveBird>().MoveToSlingshot());
    }


}
