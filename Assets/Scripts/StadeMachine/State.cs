using UnityEngine;

public abstract class State 
{
    protected ManagerState _managerState;

    public abstract void EnterState(ManagerState manager);
    public abstract void ExitState(ManagerState manager);
    public abstract void Update(ManagerState manager);
}
