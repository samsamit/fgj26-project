using Godot;
using System;

public partial class AnimationController : Node2D
{
	public AnimationEnum ActiveAnimation { get; private set; }
	private AnimatedSprite2D _animatedSprite2D;
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		ActiveAnimation = AnimationEnum.Idle;
	}

	public void InitiateSpriteFrames(SpriteFrames spriteFrames, float scale)
	{
		Scale = new Vector2(scale, scale);
		_animatedSprite2D.SpriteFrames = spriteFrames;
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
			case AnimationEnum.Push:
				ActiveAnimation = AnimationEnum.Push;
				_animatedSprite2D.Play(nameof(AnimationEnum.Push));
				break;
			default:
				GD.PrintErr("Animation Enum not defined");
				break;
		}

	}

	public void WhenAnimationChanged()
	{
		return;
	}

	public void HandleRotation(Vector2 direction, double delta, float rotationSpeed)
	{
		if (Rotation >= 2 * Math.PI)
		{
			Rotation -= (float)(2 * Math.PI);
		}
		if (Rotation <= -(2 * Math.PI))
		{
			Rotation += (float)(2 * Math.PI);
		}
		
		var wantedRotation = direction.Angle();
		var currentRotation = Rotation;
		var neededRotation = wantedRotation - currentRotation;

		if (neededRotation > Math.PI)
		{
			wantedRotation -= (float)(2 * Math.PI);
		}
		if (neededRotation < -Math.PI)
		{
			wantedRotation += (float)(2 * Math.PI);
		}

		Rotation = currentRotation + (wantedRotation - currentRotation) * (float)delta * rotationSpeed;
	}
}
