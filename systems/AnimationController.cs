using Godot;
using System;
using System.Runtime.CompilerServices;

// This controller doesn't care what animations should be played. The parent calling the functions should.


public partial class AnimationController : Node2D
{
	private SpriteFrames _spriteFrames;
	public AnimationEnum ActiveAnimation { get; private set; }
	private AnimatedSprite2D _animatedSprite2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		ActiveAnimation = AnimationEnum.Idle;
		_animatedSprite2D.AnimationChanged += WhenAnimationChanged;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void InitiateSpriteFrames(SpriteFrames spriteFrames)
	{
		_spriteFrames = spriteFrames;
	}

	public void ChangeAnimation(AnimationEnum animationEnum)
	{
		if (ActiveAnimation == animationEnum)
		{
			return;
		}

		switch (animationEnum)
		{
			case AnimationEnum.Idle:
				ActiveAnimation = AnimationEnum.Idle;
				_animatedSprite2D.Play(nameof(AnimationEnum.Idle));
				break;
			case AnimationEnum.Walk:
				ActiveAnimation = AnimationEnum.Walk;
				_animatedSprite2D.Play(nameof(AnimationEnum.Walk));
				break;
			case AnimationEnum.Run:
				ActiveAnimation = AnimationEnum.Run;
				_animatedSprite2D.Play(nameof(AnimationEnum.Run));
				break;
			default:
				Console.WriteLine("Animation Enum not defined");
				break;
		}
		
	}

	public void WhenAnimationChanged() {
		return;
	}
}
