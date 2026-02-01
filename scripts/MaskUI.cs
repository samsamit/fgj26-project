using System;
using Godot;
using System.Collections.Generic;

public partial class MaskUI : VBoxContainer
{
	private TextureButton BasicMask;
	private TextureButton FlashliteMask;
	private TextureButton StrengthMask;
	private TextureButton XRayMask;
	private TextureButton SlowMask;
	private Control PowerMinigame;
	private ProgressBar MaskPower;

	// Visual styling for selection state
	private static readonly Color SelectedColor = new Color(1f, 1f, 1f, 1f);      // Full brightness
	private static readonly Color UnselectedColor = new Color(0.5f, 0.5f, 0.5f, 0.7f); // Dimmed
	private static readonly Vector2 SelectedScale = new Vector2(1.2f, 1.2f);      // Slightly larger
	private static readonly Vector2 UnselectedScale = new Vector2(1f, 1f);        // Normal size

	public override void _Ready()
	{
		BasicMask = GetNode<TextureButton>("./MaskContainer/Basic");
		FlashliteMask = GetNode<TextureButton>("./MaskContainer/Flashlite");
		StrengthMask = GetNode<TextureButton>("./MaskContainer/Strength");
		XRayMask = GetNode<TextureButton>("./MaskContainer/XRay");
		SlowMask = GetNode<TextureButton>("./MaskContainer/Slow");
		PowerMinigame = (Control)GetNode("./PowerMiniGame");
		MaskPower = (ProgressBar)GetNode("./MaskPower");

		// Connect button signals programmatically to ensure they work
		BasicMask.Pressed += OnClickBasicMask;
		FlashliteMask.Pressed += OnClickFlashlite;
		StrengthMask.Pressed += OnClickStrength;
		XRayMask.Pressed += OnClickXRay;
		SlowMask.Pressed += OnClickSlow;

		// Set mouse filter to Stop to prevent click-through to game objects
		BasicMask.MouseFilter = MouseFilterEnum.Stop;
		FlashliteMask.MouseFilter = MouseFilterEnum.Stop;
		StrengthMask.MouseFilter = MouseFilterEnum.Stop;
		XRayMask.MouseFilter = MouseFilterEnum.Stop;
		SlowMask.MouseFilter = MouseFilterEnum.Stop;

		GlobalStateManager.Instance.AvailableMasks.RegisterObserver(
			UpdateAvailableMasks);
		GlobalStateManager.Instance.MaskPower.RegisterObserver(
			UpdateMaskPower);
		GlobalStateManager.Instance.CurrentMask.RegisterObserver(SetMask);
		SetMask(GlobalStateManager.Instance.CurrentMask.Get());
	}

	public override void _ExitTree()
	{
		// Clean up signal connections
		BasicMask.Pressed -= OnClickBasicMask;
		FlashliteMask.Pressed -= OnClickFlashlite;
		StrengthMask.Pressed -= OnClickStrength;
		XRayMask.Pressed -= OnClickXRay;
		SlowMask.Pressed -= OnClickSlow;
	}

	private void UpdateAvailableMasks(List<MaskEnum> masks)
	{
		BasicMask.Visible = false;
		FlashliteMask.Visible = false;
		StrengthMask.Visible = false;
		XRayMask.Visible = false;
		SlowMask.Visible = false;

		foreach (var mask in masks)
		{
			switch (mask)
			{
				case MaskEnum.Basic:
					BasicMask.Visible = true;
					break;
				case MaskEnum.Flashlite:
					FlashliteMask.Visible = true;
					break;
				case MaskEnum.Strength:
					StrengthMask.Visible = true;
					break;
				case MaskEnum.XRay:
					XRayMask.Visible = true;
					break;
				case MaskEnum.Slow:
					SlowMask.Visible = true;
					break;
				default:
					throw new ArgumentException("Unknown enum value: " + nameof(mask));
			}
		}

		// Refresh selection styling after visibility changes
		SetMask(GlobalStateManager.Instance.CurrentMask.Get());
	}

	public void SetMask(MaskEnum mask)
	{
		if (mask == MaskEnum.Strength)
		{
			PowerMinigame.Visible = true;
			PowerMinigame.ProcessMode = ProcessModeEnum.Always;
			MaskPower.Visible = true;
		}
		else
		{
			PowerMinigame.Visible = false;
			PowerMinigame.ProcessMode = ProcessModeEnum.Disabled;
			foreach (var child in PowerMinigame.GetChildren())
			{
				PowerMinigame.RemoveChild(child);
				child.QueueFree();
			}
			MaskPower.Visible = false;
		}

		// Apply visual styling to each button based on selection state
		SetButtonSelectionState(BasicMask, mask == MaskEnum.Basic);
		SetButtonSelectionState(FlashliteMask, mask == MaskEnum.Flashlite);
		SetButtonSelectionState(StrengthMask, mask == MaskEnum.Strength);
		SetButtonSelectionState(XRayMask, mask == MaskEnum.XRay);
		SetButtonSelectionState(SlowMask, mask == MaskEnum.Slow);
	}

	private void SetButtonSelectionState(TextureButton button, bool isSelected)
	{
		if (isSelected)
		{
			button.Modulate = SelectedColor;
			button.Scale = SelectedScale;
		}
		else
		{
			button.Modulate = UnselectedColor;
			button.Scale = UnselectedScale;
		}
	}

	private void UpdateMaskPower(float maskPower)
	{
		MaskPower.Value = maskPower;
	}

	private void OnClickBasicMask()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Basic);
	}

	private void OnClickFlashlite()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Flashlite);
	}

	private void OnClickStrength()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Strength);
	}

	private void OnClickXRay()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.XRay);
	}

	private void OnClickSlow()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Slow);
	}
}
