using Godot;
using System;
using System.Collections.Generic;

public partial class MaskUI : HBoxContainer
{
	private TextureButton BasicMask;
	private TextureButton FlashliteMask;
	private TextureButton StrengthMask;
	private TextureButton XRayMask;

	public override void _Ready()
	{
		BasicMask = (TextureButton)GetNode("./Basic");
		FlashliteMask = (TextureButton)GetNode("./Flashlite");
		StrengthMask = (TextureButton)GetNode("./Strength");
		XRayMask = (TextureButton)GetNode("./XRay");

		GlobalStateManager.Instance.AvailableMasks.RegisterObserver(
			masks => UpdateAvailableMasks(masks));
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
				default:
					break;
			}
		}
	}

	public void OnClickBasicMask()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Basic);
		GD.Print("Clicked");
	}

	public void OnClickFlashlite()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Flashlite);
		GD.Print("Clicked");
	}

	public void OnClickStrength()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Strength);
		GD.Print("Clicked");
	}

	public void OnClickXRay()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.XRay);
		GD.Print("Clicked");
	}
}
