using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[DefaultExecutionOrder(100)]
public class PlayerCamera : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        if (Mall.Instance.MainCamera == null)
        {
            Debug.Log("CAMERA INIT");
            Mall.Instance.MainCamera = cam;
            LookAtPlayer();
            FollowPlayer();
        }


    }

    public void LookAtPlayer()
    {
        Debug.Log("CAMERA -> LookAt");
        cam.m_LookAt = Mall.Instance.Player.transform;
        /*
        if (cinemachineCam.m_LookAt == null)
        {
            
        }
        */
    }
    public void FollowPlayer()
    {
        Debug.Log("CAMERA -> follow");
        cam.m_Follow = Mall.Instance.Player.transform;
        /*
        if (cinemachineCam.m_Follow == null)
        {
            
        }
        */
    }



}
