using Godot;
using System.Collections.Generic;

public partial class MaskUI : VBoxContainer
{
	private TextureButton BasicMask;
	private TextureButton FlashliteMask;
	private TextureButton StrengthMask;
	private TextureButton XRayMask;
	private Control PowerMinigame;
	private ProgressBar MaskPower;

	public override void _Ready()
	{
		BasicMask = (TextureButton)GetNode("./MaskContainer/Basic");
		FlashliteMask = (TextureButton)GetNode("./MaskContainer/Flashlite");
		StrengthMask = (TextureButton)GetNode("./MaskContainer/Strength");
		XRayMask = (TextureButton)GetNode("./MaskContainer/XRay");
		PowerMinigame = (Control)GetNode("./PowerMiniGame");
		MaskPower = (ProgressBar)GetNode("./MaskPower");

		GlobalStateManager.Instance.AvailableMasks.RegisterObserver(
			UpdateAvailableMasks);
		GlobalStateManager.Instance.MaskPower.RegisterObserver(
			UpdateMaskPower);
		GlobalStateManager.Instance.CurrentMask.RegisterObserver(SetMask);
		SetMask(GlobalStateManager.Instance.CurrentMask.Get());
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
	}

	private void UpdateMaskPower(float maskPower)
	{
		MaskPower.Value = maskPower;
	}

	public void OnClickBasicMask()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Basic);
	}

	public void OnClickFlashlite()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Flashlite);
	}

	public void OnClickStrength()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.Strength);
	}

	public void OnClickXRay()
	{
		GlobalStateManager.Instance.CurrentMask.Set(MaskEnum.XRay);
	}
}
