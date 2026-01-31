using System;
using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 50.0f;
	[Export] SpriteFrames spriteFrames;
	[Export] public float PushingPower = 100.0f;

	private const string MoveRight = "move_right";
	private const string MoveLeft = "move_left";
	private const string MoveBack = "move_back";
	private const string MoveForward = "move_forward";
	private AudioStreamPlayer2D _walkingSFXplayer;
	private GlobalStateManager _stateManager;
	public int ModulationInterval { get; set; } = 10;
	private int _frameCounter = 0;

	public override void _Ready()
	{
		GD.Print("Player script is active!");
		this.SetAnimation(AnimationEnum.Idle);
		_stateManager = GetNode<GlobalStateManager>("/root/World");
	    _walkingSFXplayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D_Walking");
	}




	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Zero;
		direction.X = Input.GetActionStrength(MoveRight)
					  - Input.GetActionStrength(MoveLeft);

		direction.Y = Input.GetActionStrength(MoveBack)
			- Input.GetActionStrength(MoveForward);

		Velocity = direction.Normalized() * Speed;
		var velocityNormalized = Velocity.Normalized();
		var velocityNormalizedCombined = System.Math.Abs(velocityNormalized[0]) + System.Math.Abs(velocityNormalized[1]);
		if (!_walkingSFXplayer.Playing && velocityNormalizedCombined > 0)
		{
			_walkingSFXplayer.Play();
			//GD.Print("Audio Started");

		}
		else if (_walkingSFXplayer.Playing && velocityNormalizedCombined == 0)
		{
			_walkingSFXplayer.Stop();
			ResetAudioParams();
			//GD.Print("Audio Stopped");
		}

		if (velocityNormalizedCombined > 0)
		{
			_frameCounter++;
		}
		// Jos X framea on kulunut, randomisoidaan arvot
		if (_frameCounter >= ModulationInterval)
		{
			RandomizeAudioParams();
			_frameCounter = 0; // Nollataan laskuri
		}

		var pushedBodies = new System.Collections.Generic.HashSet<RigidBody2D>();
		if (MoveAndSlide())
		{
			for (int i = 0; i < GetSlideCollisionCount(); i++)
			{
				var collision = GetSlideCollision(i);
				var collider = collision.GetCollider();
				if (collider is RigidBody2D rigidBody2D && !pushedBodies.Contains(rigidBody2D))
				{
					rigidBody2D.ApplyForce(collision.GetNormal() * -PushingPower);
					pushedBodies.Add(rigidBody2D);
				}
			}
		}
	}

	private void RandomizeAudioParams()
	{
		// 1. PitchScale (Vaikuttaa korkeuteen JA nopeuteen)
		// 1.0 on normaali. Vaihtelu 0.9 - 1.1 on yleensä hyvä.
		float randomPitch = (float)GD.RandRange(0.9, 1.1);
		_walkingSFXplayer.PitchScale = randomPitch;

		// 2. VolumeDb (Voimakkuus)
		// 0 on normaali. Pieni vaihtelu esim. -2 ja 0 välillä tuo elävyyttä.
		float randomVol = (float)GD.RandRange(-2.0, 0.0);
		_walkingSFXplayer.VolumeDb = randomVol;

		_stateManager.PlayerPosition = GlobalPosition;
	}

	private void ResetAudioParams()
	{
		_walkingSFXplayer.PitchScale = 1.0f;
		_walkingSFXplayer.VolumeDb = 0.0f;
		_frameCounter = 0;
	}

	private void SetAnimation(AnimationEnum animationEnum)
	{
		// TODO:
		// Add animation controller functions to match what animation should play. If there is delay
		// this function would return late, where it should be handled here. 
		// For example, "ata", deal damage after animation finished.
		// Can also be handled with Godot inbuilt signals.
		// var animationController = GetNode<AnimationController>("AnimationController");
		// animationController.InitiateSpriteFrames(spriteFrames);
		// animationController.ChangeAnimation(animationEnum);
		
	}
}
