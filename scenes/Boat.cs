using Godot;

public partial class Boat : RigidBody2D
{

    public enum BoatState
    {
        Broken,
        Sunk,
        Fixed
    }


    private Sprite2D _spriteNode;

    [Export]
    public BoatState _currentState = BoatState.Broken;

    public override void _Ready()
    {
        _spriteNode = GetNode<Sprite2D>("Sprite2D");
        SetState(_currentState);

        GlobalStateManager.Instance.PuzzleCompleted += OnPuzzleCompleted;
    }

    public override void _ExitTree()
    {
        GlobalStateManager.Instance.PuzzleCompleted -= OnPuzzleCompleted;
    }

    public void SetState(BoatState state)
    {
        GD.Print("Setting boat state to: " + state);
        _currentState = state;
        switch (state)
        {
            case BoatState.Broken:
                Show();
                _spriteNode.Texture = GD.Load<Texture2D>("res://assets/venerikki.png");
                break;
            case BoatState.Sunk:
                Hide();
                break;
            case BoatState.Fixed:
                Show();
                Rotation = 3.089232776f;
                _spriteNode.Texture = GD.Load<Texture2D>("res://assets/venekorjattu.png");
                break;
        }
    }

    public void RepairBoat()
    {
        SetState(BoatState.Fixed);
    }

    public void SinkBoat()
    {
        SetState(BoatState.Sunk);
    }

    private void OnPuzzleCompleted(string puzzleId)
    {
        if (puzzleId == nameof(Puzzle1))
        {
            SinkBoat();
        }
        else if (puzzleId == nameof(FinalPuzzle))
        {
            RepairBoat();
        }
    }
}
