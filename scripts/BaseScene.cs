using Godot;
using System;

public partial class BaseScene : Node
{
    
    public override void _EnterTree()
    {
        GlobalStateManager.Instance.RestartGame();
    }

}
