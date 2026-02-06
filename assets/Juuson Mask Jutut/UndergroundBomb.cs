using Godot;
using System;

public partial class UndergroundBomb : Sprite2D
{
	[Export]
	public Sprite2D explosion;
	
	[Export]
	public CompletionCondition[] defuseConditions;

	private bool active = true;
	private bool playerIsInside = false;

	private async void _on_area_2d_body_entered(Node2D body)
	{
		playerIsInside = true;
	}

	public override void _Ready() {
		if (defuseConditions != null) {
			foreach (var condition in defuseConditions)
			{
				condition.ConditionChanged += handleDefuseConditionChanged;
			}
		}
	}

    public override void _ExitTree()
    {
        base._ExitTree();
		if (defuseConditions != null)
		{
			foreach (var condition in defuseConditions)
			{
				condition.ConditionChanged -= handleDefuseConditionChanged;
			}
		}
	}

	private void handleDefuseConditionChanged(bool isCompleted)
	{
		if (!isCompleted) return;
		foreach (var condition in defuseConditions)
		{
			if (!condition.IsCompleted) return;
		}
		SetActive(false);
	}


	private void _on_area_2d_body_exited(Node2D body)
	{
		playerIsInside = false;
	}

	public void SetActive(bool state)
	{
		active = state;
	}
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (active && playerIsInside)
		{
			playerIsInside = false;
			Boom();
		}
	}
	private async void Boom()
	{
		GD.Print("boom");
		explosion.Visible = true;
		GlobalStateManager.Instance.EmitSignal(GlobalStateManager.SignalName.PlayerHit);
		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		explosion.Visible = false;
	}

}
