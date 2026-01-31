using Godot;

public partial class CompletionCondition : Node
{
    
    public bool IsCompleted { get; private set;}

    public void MarkCompleted()
    {
        if (IsCompleted) return;
        IsCompleted = true;
        EmitSignal(SignalName.ConditionCompleted);
    }

    [Signal]
	public delegate void ConditionCompletedEventHandler();
}
