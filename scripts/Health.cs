using Godot;
using System;

public partial class Health : HBoxContainer
{
	private TextureRect health1;
	private TextureRect health2;
	private TextureRect health3;

	public override void _Ready()
	{
		health1 = (TextureRect)GetNode("./Health1");
		health2 = (TextureRect)GetNode("./Health2");
		health3 = (TextureRect)GetNode("./Health3");

		GlobalStateManager.Instance.Health.RegisterObserver(
			health => UpdateHealth(health));
	}

	private void UpdateHealth(int health)
	{
		health1.Visible = false;
		health2.Visible = false;
		health3.Visible = false;
		if (health >= 1)
			health1.Visible = true;
		if (health >= 2)
			health2.Visible = true;
		if (health >= 3)
			health3.Visible = true;
	}
}
