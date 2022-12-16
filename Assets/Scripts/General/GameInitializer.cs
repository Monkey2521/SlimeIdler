using System.Collections;
using UnityEngine;

public class GameInitializer : MonoBehaviour, IGameOverHandler
{
    [SerializeField] private Player _player;
    [SerializeField] private MainMenu _mainMenu;

    private void OnEnable()
    {
        EventBus.Subscribe(this);
    }
    
    private void OnDisable()
    {
        EventBus.Unsubscribe(this);
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _player.Initialize();

        EventBus.Publish<IGameStartHandler>(handler => handler.OnGameStart());
    }

    public void OnGameOver()
    {
        _mainMenu.ShowPopupMessage("Enemy upgrades downgrade at 3 levels. Try again!");

        StartCoroutine(WaitReplay());
    }

    private IEnumerator WaitReplay()
    {
        yield return new WaitForSecondsRealtime(3);

        Initialize();
    }
}
