using System;
using System.Net;
using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 50.0f;
	[Export] SpriteFrames SpriteFrames;
	[Export] public float PushingPower = 100.0f;
	[Export] public float CharacterSpriteScaleMultiplier = 0.125f;
	[Export] public float CharacterRotationSpeed = 10.0f;

	private const string MoveRight = "move_right";
	private const string MoveLeft = "move_left";
	private const string MoveBack = "move_back";
	private const string MoveForward = "move_forward";

	private AudioStreamPlayer2D _walkingSFXplayer, _scrapingSFXPlayer;
	private AnimationController _animationController;
	private TileMapLayer _groundLayer;

	public int ModulationInterval { get; set; } = 20;
	private int _frameCounter = 0;

	// UUSI: Tallennetaan nykyisen tilen ID tähän
	private int _currentTileSourceId = -1;

	// Tilen ID:t vakioina selkeyden vuoksi
	private const int TileStone = 0;
	private const int TileWood = 2;

	public override void _Ready()
	{
		GD.Print("Player script is active!");
		InitializeAnimation(AnimationEnum.Idle);

		// Huom: Nämä GetNode polut ovat herkkiä hajoamaan jos scene-rakenne muuttuu.
		// Suosittelen käyttämään [Export]-muuttujia tai Scene Unique Nameja (%) tulevaisuudessa.
		_walkingSFXplayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D_Walking");
		_scrapingSFXPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D_BoxScraping");
		_groundLayer = GetNode<TileMapLayer>("../World/TileMapController/Ground");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Zero;
		direction.X = Input.GetActionStrength(MoveRight) - Input.GetActionStrength(MoveLeft);
		direction.Y = Input.GetActionStrength(MoveBack) - Input.GetActionStrength(MoveForward);

		Velocity = direction.Normalized() * Speed;
		var velocityNormalized = Velocity.Normalized();
		var velocityNormalizedCombined = System.Math.Abs(velocityNormalized.X) + System.Math.Abs(velocityNormalized.Y);

		if (velocityNormalizedCombined > 0)
		{
			_animationController.HandleRotation(direction, delta, CharacterRotationSpeed);
			if (!_walkingSFXplayer.Playing)
			{
				_walkingSFXplayer.Play();
			}
			if (_animationController.ActiveAnimation != AnimationEnum.Walk)
			{
				_animationController.ChangeAnimation(AnimationEnum.Walk);
			}

			// Tarkistetaan tile ja kasvatetaan laskuria vain liikkuessa
			CheckCurrentTile();
			_frameCounter++;
		}
		else if (_walkingSFXplayer.Playing) // Pysähtyminen
		{
			_walkingSFXplayer.Stop();
			ResetAudioParams();
		}
		if (velocityNormalizedCombined == 0 && _animationController.ActiveAnimation != AnimationEnum.Idle)
		{
			_animationController.ChangeAnimation(AnimationEnum.Idle);
		}
		
		// Jos X framea on kulunut, randomisoidaan arvot (materiaali huomioiden)
		if (_frameCounter >= ModulationInterval)
		{
			RandomizeAudioParams();
			_frameCounter = 0;
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
					if (!_scrapingSFXPlayer.Playing)
					{
						_scrapingSFXPlayer.Play();
					}
					if (_animationController.ActiveAnimation != AnimationEnum.Push && velocityNormalizedCombined > 0)
					{
						_animationController.ChangeAnimation(AnimationEnum.Push);
					}
				}
			}
		}
		else if (_scrapingSFXPlayer.Playing)
		{
			_scrapingSFXPlayer.Stop();
		}
	}

	public void CheckCurrentTile()
	{
		if (_groundLayer == null) return;

		// 1. Muunna pelaajan globaali sijainti TileMapin paikalliseksi
		Vector2 localPos = _groundLayer.ToLocal(GlobalPosition);

		// 2. Muunna paikallinen sijainti ruudukon koordinaateiksi
		Vector2I mapCoords = _groundLayer.LocalToMap(localPos);

		// 3. Hae ID ja tallenna se luokan muuttujaan
		int newSourceId = _groundLayer.GetCellSourceId(mapCoords);

		// Päivitetään vain jos se muuttui (debuggausta varten kätevä)
		if (newSourceId != _currentTileSourceId)
		{
			_currentTileSourceId = newSourceId;
			// GD.Print($"Materiaali vaihtui ID:hen: {_currentTileSourceId}");
		}
	}

	private void RandomizeAudioParams()
	{
		float basePitch = 1.0f;
		float baseVolume = 0.0f;

		switch (_currentTileSourceId)
		{
			case TileWood: // PUU (Lankkulattia)
						   // Lasketaan äänenkorkeutta, jotta saadaan "töminää" ja massan tuntua
				basePitch = 0.8f;

				// Puu soi usein kovempaa resonanssin takia, pidetään volume ennallaan tai nostetaan
				baseVolume = 0.0f;
				break;

			case TileStone: // ASFALTTI / KIVI
			default:
				// Nostetaan äänenkorkeutta -> "kovempi", "tiukempi", "napsahtavampi" ääni
				basePitch = 1.2f;

				// Lasketaan hieman volumea, koska korkeat äänet erottuvat muutenkin selkeästi
				// ja haluamme asfaltin olevan "lattea" verrattuna puun resonanssiin.
				baseVolume = -3.0f;
				break;
		}

		// Lisätään satunnaisuus (Randomness)
		// Pitch vaihtelee nyt basePitchin ympärillä +/- 10%
		float randomPitch = basePitch * (float)GD.RandRange(0.9, 1.1);
		_walkingSFXplayer.PitchScale = randomPitch;

		// Volume vaihtelee vähän (-2db ja 0db välillä suhteessa baseVolumeen)
		float randomVol = baseVolume + (float)GD.RandRange(-2.0, 0.0);
		_walkingSFXplayer.VolumeDb = randomVol;

		GlobalStateManager.Instance.PlayerPosition = GlobalPosition;
	}

	private void ResetAudioParams()
	{
		_walkingSFXplayer.PitchScale = 1.0f;
		_walkingSFXplayer.VolumeDb = 0.0f;
		_frameCounter = 0;
	}

	private void InitializeAnimation(AnimationEnum animationEnum)
	{
		_animationController = GetNode<AnimationController>("AnimationController");
		_animationController.InitiateSpriteFrames(SpriteFrames, CharacterSpriteScaleMultiplier);
		_animationController.ChangeAnimation(animationEnum);
		_animationController.GlobalRotation = GlobalRotation;
	}
}
