using System;

internal class PauseState
{
    public event Action<bool> IsPausedChanged;

    private bool _isPaused;

    public bool IsPaused
    {
        get => _isPaused;
        set
        {
            if (_isPaused != value)
            {
                _isPaused = value;
                IsPausedChanged?.Invoke(value);
            }
        }
    }
}
