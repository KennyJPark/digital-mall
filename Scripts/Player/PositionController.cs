using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// USE TO SET POSITIONS OF ENTITIES LIKE THE PLAYER

public class PositionController : MonoBehaviour
//public class PositionController : Singleton<PositionController>
{
    public static PositionController instance { get; private set; }

    public static PlayerController Player;
    public static CashRegister CashRegister;
    void Awake()
    {

    }

    void Start()
    {

        Player = Mall.Instance.Player;
        CashRegister = CashRegister.instance;

        Assert.AreNotEqual(null, CashRegister);
        Assert.AreNotEqual(null, Player);


       // SetStorePositions();
    }

    public void FreezePlayer()
    {

    }

    public void SetStorePositions()
    {
        SendPlayerToRegister();
    }

    public void SetPosition(Transform obj, Vector3 dest, Vector2 directionFacing)
    {
        //Debug.Log("SETTING PLAYER POSITION TO CASH REGISTER");
        Player.gameObject.transform.position = dest;
        Player.gameObject.GetComponent<PlayerController>().SetLookDirection(directionFacing);
    }

    public void SendPlayerToRegister()
    {
        //Debug.Log("Register Vector: " + cashRegister.gameObject.transform.position);
        //Debug.Log("Player Vector: " + player.gameObject.transform.position);
        Vector3 posOffset = new Vector3();
        posOffset = CashRegister.gameObject.transform.position;
        posOffset.y = posOffset.y + 2.5f;
        Vector2 facing = new Vector2(0, -1);
        SetPosition(Player.gameObject.transform, posOffset, facing);
    }
}

/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace HappyHarvest
{
    [DefaultExecutionOrder(1000)]
    public class SpawnPoint : MonoBehaviour
    {
        public int SpawnIndex;

        private void OnEnable()
        {
            GameManager.Instance.RegisterSpawn(this);
        }

        private void OnDisable()
        {
            GameManager.Instance?.UnregisterSpawn(this);
        }

        public void SpawnHere()
        {
            var playerTransform = GameManager.Instance.Player.transform;
            
            playerTransform.position = transform.position;

            if (GameManager.Instance.MainCamera != null)
            {//some scene, like interior, may have fixed camera, so no need to change anything
                GameManager.Instance.MainCamera.Follow = playerTransform;
                GameManager.Instance.MainCamera.LookAt = playerTransform;
                GameManager.Instance.MainCamera.ForceCameraPosition(playerTransform.position, Quaternion.identity);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SpawnPoint))]
    public class SpawnPointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SpawnPoint[] transitions = GameObject.FindObjectsOfType<SpawnPoint>();
            var local = target as SpawnPoint;
            foreach (var transition in transitions)
            {
                if (transition == local)
                {
                    continue;
                }

                if (transition.SpawnIndex == local.SpawnIndex)
                {
                    EditorGUILayout.HelpBox(
                        $"Spawn Index need to be unique and this Spawn Index is already used by {transition.gameObject.name}",
                        MessageType.Error);
                }
            }
        }
    }
#endif
}
 */