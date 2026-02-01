using Godot;
using System;

public partial class MainMenuScript : Control
{
    public override void _Ready()
    {


        return;
    }

    public void OnButtonStartPressed(){
        GetTree().ChangeSceneToFile("res://scenes/BaseScene.tscn");
        GD.Print("Start pressed");
    }

    public void OnButtonOptionsPressed(){
        GD.Print("Options pressed");
    }

    public void OnButtonCreditsPressed(){
        GD.Print("Credits pressed");
    }

    public void OnButtonExitPressed(){
        GetTree().Quit();
        //GD.Print("Exit pressed");
    }
}
