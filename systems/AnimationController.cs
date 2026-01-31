using Godot;
using System;

// This controller doesn't care what animations should be played. The parent calling the functions should.
public partial class AnimationController : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var walkSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ChangeAnimation(string animationName, string animationPath)
	{
		throw new NotImplementedException();
	}
}
