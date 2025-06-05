using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1000)]

public class SpawnPoint : MonoBehaviour
{
    public int SpawnIndex;

    private void OnEnable()
    {
        Debug.Log("Spawn Enabled");
        Mall.Instance.RegisterSpawn(this);
    }

    private void OnDisable()
    {
        Mall.Instance?.UnregisterSpawn(this);
    }

    public void SpawnHere()
    {
        var playerTransform = Mall.Instance.Player.transform;

        playerTransform.position = transform.position;

        /*
        if (Mall.Instance.MainCamera != null)
        {//some scene, like interior, may have fixed camera, so no need to change anything
            Mall.Instance.MainCamera.Follow = playerTransform;
            Mall.Instance.MainCamera.LookAt = playerTransform;
            Mall.Instance.MainCamera.ForceCameraPosition(playerTransform.position, Quaternion.identity);
        }
        */

    }
}
