using Godot;
using System;
using System.Collections.Generic;

public partial class MaskUI : HBoxContainer
{
	private TextureButton BasicMask;
	private TextureButton FlashliteMask;
	private TextureButton StrengthMask;
	private TextureButton XRayMask;

	// Visual styling for selection state
	private static readonly Color SelectedColor = new Color(1f, 1f, 1f, 1f);      // Full brightness
	private static readonly Color UnselectedColor = new Color(0.5f, 0.5f, 0.5f, 0.7f); // Dimmed
	private static readonly Vector2 SelectedScale = new Vector2(1.2f, 1.2f);      // Slightly larger
	private static readonly Vector2 UnselectedScale = new Vector2(1f, 1f);        // Normal size

	public override void _Ready()
	{
		BasicMask = GetNode<TextureButton>("Basic");
		FlashliteMask = GetNode<TextureButton>("Flashlite");
		StrengthMask = GetNode<TextureButton>("Strength");
		XRayMask = GetNode<TextureButton>("XRay");

		// Connect button signals programmatically to ensure they work
		BasicMask.Pressed += OnClickBasicMask;
		FlashliteMask.Pressed += OnClickFlashlite;
		StrengthMask.Pressed += OnClickStrength;
		XRayMask.Pressed += OnClickXRay;

		// Set mouse filter to Stop to prevent click-through to game objects
		BasicMask.MouseFilter = Control.MouseFilterEnum.Stop;
		FlashliteMask.MouseFilter = Control.MouseFilterEnum.Stop;
		StrengthMask.MouseFilter = Control.MouseFilterEnum.Stop;
		XRayMask.MouseFilter = Control.MouseFilterEnum.Stop;

		GlobalStateManager.Instance.AvailableMasks.RegisterObserver(
			masks => UpdateAvailableMasks(masks));

		// Register observer for current mask selection
		GlobalStateManager.Instance.CurrentMask.RegisterObserver(
			mask => UpdateSelectedMask(mask));
	}

	public override void _ExitTree()
	{
		// Clean up signal connections
		BasicMask.Pressed -= OnClickBasicMask;
		FlashliteMask.Pressed -= OnClickFlashlite;
		StrengthMask.Pressed -= OnClickStrength;
		XRayMask.Pressed -= OnClickXRay;
	}

	private void UpdateAvailableMasks(List<MaskEnum> masks)
	{
		BasicMask.Visible = false;
		FlashliteMask.Visible = false;
		StrengthMask.Visible = false;
		XRayMask.Visible = false;

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
			}
		}

		// Refresh selection styling after visibility changes
		UpdateSelectedMask(GlobalStateManager.Instance.CurrentMask.Get());
	}

	private void UpdateSelectedMask(MaskEnum selectedMask)
	{
		// Apply visual styling to each button based on selection state
		SetButtonSelectionState(BasicMask, selectedMask == MaskEnum.Basic);
		SetButtonSelectionState(FlashliteMask, selectedMask == MaskEnum.Flashlite);
		SetButtonSelectionState(StrengthMask, selectedMask == MaskEnum.Strength);
		SetButtonSelectionState(XRayMask, selectedMask == MaskEnum.XRay);
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
}
