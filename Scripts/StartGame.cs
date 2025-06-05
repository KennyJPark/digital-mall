using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannelSO
    _onNewGameButton = default;
    private void Start()
    {
        _onNewGameButton.OnEventRaised += StartNewGame;
    }
    private void OnDestroy()
    {
        _onNewGameButton.OnEventRaised -= StartNewGame;
    }
    private void StartNewGame()
    {
        // load level logic here…
    }
}