using Godot;

public partial class Player : CharacterBody2D
{
	public const float Speed = 50.0f;

	private const string MoveRight = "move_right";
	private const string MoveLeft = "move_left";
	private const string MoveBack = "move_back";
	private const string MoveForward = "move_forward";
	private AudioStreamPlayer2D _walkingSFXplayer;
	public int ModulationInterval { get; set; } = 10;
	private int _frameCounter = 0;
	
	public override void _Ready()
	{
		GD.Print("Player script is active!");
	}



	
	public override void _PhysicsProcess(double delta)
	{
		_walkingSFXplayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D_Walking");
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
			GD.Print("Audio Started");
			
		}
		else if (_walkingSFXplayer.Playing && velocityNormalizedCombined == 0)
		{
			_walkingSFXplayer.Stop();
			ResetAudioParams();
			GD.Print("Audio Stopped");
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
			MoveAndSlide();
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
	}

	private void ResetAudioParams()
	{
		_walkingSFXplayer.PitchScale = 1.0f;
		_walkingSFXplayer.VolumeDb = 0.0f;
		_frameCounter = 0;
	}

}
