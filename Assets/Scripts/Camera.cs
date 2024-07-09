using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    // cameras
    [SerializeField] private CinemachineVirtualCamera followPlayerCamera;
    [SerializeField] private CinemachineVirtualCamera bossFightCamera;
    private CinemachineVirtualCamera activeCamera;

    // variables
    [SerializeField] private Player player;
    [SerializeField] private Vector3 bossFightCameraPosition;


    // Start is called before the first frame update
    void Start()
    {
        activeCamera = followPlayerCamera;
        followPlayerCamera.Priority = 10;
        bossFightCamera.Priority = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.checkFightingState())
        {
            bossFightCamera.transform.position = bossFightCameraPosition;
            switchCamera(bossFightCamera);
        }
    }

    private void switchCamera(CinemachineVirtualCamera newCamera)
    {
        if (newCamera != activeCamera)
        {
            activeCamera.Priority = 5;
            newCamera.Priority = 10;
            activeCamera = newCamera;
        }
    }
}
