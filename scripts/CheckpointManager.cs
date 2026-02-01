using Godot;
using System;
using System.Collections.Generic;

public partial class CheckpointManager : Node2D
{
	private List<Checkpoint> _checkpoints = new List<Checkpoint>();
	private Player _player;
	private Mask _mask;
	private MainCamera _camera;
	
	private Vector2 _latestCheckpointPosition = Vector2.Zero;
	private bool _isRespawning = false;
	
	// Animation nodes
	private CanvasLayer _respawnAnimationLayer;
	private TextureRect _cursorSprite;
	private Tween _animationTween;

	private AudioStreamPlayer _AudioStreamPlayerLogoSpin;
	
	// Cursor texture path
	private const string CursorTexturePath = "res://assets/kursori.png";
	
	public override void _Ready()
	{
		// Find all checkpoints in the spawn_points group
		_checkpoints.Clear();
		foreach (Node node in GetTree().GetNodesInGroup("spawn_points"))
		{
			if (node is Checkpoint checkpoint)
			{
				_checkpoints.Add(checkpoint);
				// Connect to the checkpoint's signal
				checkpoint.CheckpointReached += OnCheckpointReached;
			}
		}
		GD.Print("Found checkpoints: ", _checkpoints.Count);
		
		// Get references to Player, Mask, and Camera
		_player = GetNode<Player>("/root/World/Player");
		_mask = GetNode<Mask>("/root/World/Mask");
		_camera = GetNode<MainCamera>("/root/World/Camera");
		_AudioStreamPlayerLogoSpin = GetNode<AudioStreamPlayer>("AudioStreamPlayerLogoSpin");
		
		// Set initial checkpoint position to the first checkpoint or player's start position
		if (_checkpoints.Count > 0)
		{
			_latestCheckpointPosition = _checkpoints[0].GlobalPosition;
		}
		else if (_player != null)
		{
			_latestCheckpointPosition = _player.GlobalPosition;
		}
		
		// Connect to GlobalStateManager's PlayerHit signal
		GlobalStateManager.Instance.PlayerHit += OnPlayerHit;
	}
	
	public override void _ExitTree()
	{
		// Disconnect signals when removed
		foreach (var checkpoint in _checkpoints)
		{
			if (IsInstanceValid(checkpoint))
			{
				checkpoint.CheckpointReached -= OnCheckpointReached;
			}
		}
		
		if (GlobalStateManager.Instance != null)
		{
			GlobalStateManager.Instance.PlayerHit -= OnPlayerHit;
		}
	}
	
	/// <summary>
	/// Called when the player reaches a checkpoint
	/// </summary>
	private void OnCheckpointReached(Vector2 position)
	{
		_latestCheckpointPosition = position;
		GD.Print("Checkpoint updated to: ", position);
	}
	
	/// <summary>
	/// Called when the player gets hit
	/// </summary>
	private void OnPlayerHit()
	{
		if (_isRespawning) return;
		
		GD.Print("Player hit! Starting respawn animation...");
		_isRespawning = true;
		PlayRespawnAnimation();
	}
	
	/// <summary>
	/// Plays the respawn animation with the cursor image rotating and scaling to fill the screen
	/// </summary>
	private void PlayRespawnAnimation()
	{
		_player.canMove = false;
		_AudioStreamPlayerLogoSpin.Play();

		// Create the CanvasLayer for the animation (renders on top of everything)
		_respawnAnimationLayer = new CanvasLayer();
		_respawnAnimationLayer.Layer = 100; // High layer to be on top
		AddChild(_respawnAnimationLayer);
		
		// Create a ColorRect as background that fades in
		var background = new ColorRect();
		background.Color = new Color(0, 0, 0, 0); // Start transparent
		background.SetAnchorsPreset(Control.LayoutPreset.FullRect);
		_respawnAnimationLayer.AddChild(background);
		
		// Load the cursor texture
		var cursorTexture = GD.Load<Texture2D>(CursorTexturePath);
		if (cursorTexture == null)
		{
			GD.PushError("Failed to load cursor texture: ", CursorTexturePath);
			CompleteRespawn();
			return;
		}
		
		// Create a Control to center the sprite
		var centerControl = new Control();
		centerControl.SetAnchorsPreset(Control.LayoutPreset.FullRect);
		_respawnAnimationLayer.AddChild(centerControl);
		
		// Create the cursor sprite as a TextureRect
		_cursorSprite = new TextureRect();
		_cursorSprite.Texture = cursorTexture;
		_cursorSprite.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		_cursorSprite.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
		
		// Set initial size (small)
		var viewportSize = GetViewport().GetVisibleRect().Size;
		float initialSize = 32f;
		_cursorSprite.Size = new Vector2(initialSize, initialSize);
		
		// Position at center of screen
		_cursorSprite.Position = (viewportSize / 2) - (_cursorSprite.Size / 2);
		_cursorSprite.PivotOffset = _cursorSprite.Size / 2;
		
		centerControl.AddChild(_cursorSprite);
		
		// Create and configure the tween animation
		_animationTween = CreateTween();
		_animationTween.SetParallel(true);
		
		// Calculate final size to fill screen (with some extra to ensure full coverage)
		float maxDimension = Mathf.Max(viewportSize.X, viewportSize.Y) * 2f;
		
		// Animation duration (2.5 seconds as specified)
		float duration = 2.5f;
		
		// Animate scale (size expansion)
		_animationTween.TweenMethod(
			Callable.From<float>((scale) => {
				if (_cursorSprite != null && IsInstanceValid(_cursorSprite))
				{
					float newSize = Mathf.Lerp(initialSize, maxDimension, scale);
					_cursorSprite.Size = new Vector2(newSize, newSize);
					_cursorSprite.Position = (viewportSize / 2) - (_cursorSprite.Size / 2);
					_cursorSprite.PivotOffset = _cursorSprite.Size / 2;
				}
			}),
			0f, 1f, duration
		).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
		
		// Animate rotation (multiple full rotations)
		float totalRotations = 3f; // 3 full rotations
		_animationTween.TweenMethod(
			Callable.From<float>((rotation) => {
				if (_cursorSprite != null && IsInstanceValid(_cursorSprite))
				{
					_cursorSprite.Rotation = rotation * Mathf.Pi * 2 * totalRotations;
				}
			}),
			0f, 1f, duration
		).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Sine);
		
		// Fade in the background
		_animationTween.TweenProperty(background, "color", new Color(0, 0, 0, 1), duration * 0.8f)
			.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
		
		// Set up completion callback
		_animationTween.SetParallel(false);
		_animationTween.TweenCallback(Callable.From(CompleteRespawn));
	}
	
	/// <summary>
	/// Called when the respawn animation completes. Teleports entities and cleans up.
	/// </summary>
	private void CompleteRespawn()
	{
		GD.Print("Respawn animation complete. Teleporting to: ", _latestCheckpointPosition);
		
		// Teleport Player, Mask, and Camera to the checkpoint
		if (_player != null)
		{
			_player.SpawnAt(_latestCheckpointPosition);
			_player.canMove = true;
		}
		
		if (_mask != null)
		{
			_mask.SpawnAt(_latestCheckpointPosition);
		}
		
		if (_camera != null)
		{
			_camera.GlobalPosition = _latestCheckpointPosition;
		}
		
		// Clean up the animation layer
		if (_respawnAnimationLayer != null && IsInstanceValid(_respawnAnimationLayer))
		{
			_respawnAnimationLayer.QueueFree();
			_respawnAnimationLayer = null;
		}
		
		_cursorSprite = null;
		_animationTween = null;
		_isRespawning = false;
	}
	
	/// <summary>
	/// Gets the current latest checkpoint position
	/// </summary>
	public Vector2 GetLatestCheckpointPosition()
	{
		return _latestCheckpointPosition;
	}
	
	/// <summary>
	/// Manually sets the checkpoint position (for testing or special cases)
	/// </summary>
	public void SetCheckpointPosition(Vector2 position)
	{
		_latestCheckpointPosition = position;
	}
}
