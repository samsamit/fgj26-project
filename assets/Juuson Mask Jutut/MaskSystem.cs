using Godot;

public partial class MaskSystem : Node
{
    [Export] public SubViewport MaskViewport;
    [Export] public Sprite2D MaskCircleInViewport;   // the one inside MaskViewport
    [Export] public Node2D PlayerCircle;             // the real moving circle in the main world (or whatever follows player)
    [Export] public ShaderMaterial BoxesMaterial;    // shared material used by all boxes
    [Export] public Camera2D MaskCamera;

    public override void _Ready()
    {
        // Give shader the mask texture (SubViewportTexture)
        var tex = MaskViewport?.GetTexture();
        BoxesMaterial?.SetShaderParameter("mask_tex", tex);
    }

    public override void _Process(double delta)
    {
    var cam = GetViewport().GetCamera2D();

        // Keep the viewport circle aligned with the real circle (screen-space match)
        // If you have a Camera2D, this is still fine as long as both are in same canvas/screen space.
        Vector2 size = GetViewport().GetVisibleRect().Size; // Screen size
        MaskCircleInViewport.GlobalPosition =  PlayerCircle.GlobalPosition - MaskCamera.GlobalPosition + size / 2;
        MaskCircleInViewport.GlobalRotation = PlayerCircle.GlobalRotation;
        MaskCircleInViewport.GlobalScale    = PlayerCircle.GlobalScale;
    }

    public void SetActive(bool state)
    {
        MaskCircleInViewport.Visible = state;
    }
}
