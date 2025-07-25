using Interface;
using Manager;
using Service;
using UnityEngine;

public class TurnManager : ManagerBase<TurnManager>, IPauseable
{
    public bool IsPaused { get; set; }

    private int _turnCount;


    public void Pause()
    {
        
    }

    public void Resume()
    {
        
    }

    /// <summary>
    /// ƒ^[ƒ“‚ğˆê‚Âi‚ß‚é
    /// </summary>
    private void NextTurn()
    {
        _turnCount++;
    }

    public override void Initialize()
    {
        _turnCount = 0;
        
    }
}
