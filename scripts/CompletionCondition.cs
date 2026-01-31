using Godot;

public partial class CompletionCondition : Node
{
    
    public bool IsCompleted { get; private set;}

    public void MarkCompleted()
    {
        if (IsCompleted) return;
        IsCompleted = true;
        EmitSignal(SignalName.ConditionChanged, IsCompleted);
    }

    public void MarkUncompleted()
    {
        if (!IsCompleted) return;
        IsCompleted = false;
        EmitSignal(SignalName.ConditionChanged, IsCompleted);
    }

    [Signal]
	public delegate void ConditionChangedEventHandler(bool isCompleted);
}
