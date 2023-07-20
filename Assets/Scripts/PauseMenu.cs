using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private readonly PauseState _pauseState = new PauseState();

    private Rect _rect =
        new Rect(0.0f, 0.0f, Screen.width, Screen.height);

    private void Start()
    {
        _pauseState.IsPausedChanged += PauseStateOnIsPausedChanged;
        PauseStateOnIsPausedChanged(_pauseState.IsPaused);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseState.IsPaused = !_pauseState.IsPaused;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (_pauseState.IsPaused)
            {
                Quit();
            }
        }
    }

    private void OnGUI()
    {
        if (_pauseState.IsPaused)
        {
            _rect.width = Screen.width;
            _rect.height = Screen.height;
            if (GUI.Button(_rect, Texture2D.blackTexture, GUI.skin.box))
            {
                _pauseState.IsPaused = false;
            }
        }
    }

    private void PauseStateOnIsPausedChanged(bool value)
    {
        playerController.enabled = !value;

        Cursor.visible = value;
        Cursor.lockState =
            value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif
    }
}
