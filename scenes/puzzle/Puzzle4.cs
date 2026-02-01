public partial class Puzzle4 : Puzzle
{
    public override string puzzleId => nameof(Puzzle4);

    public override void CompletePuzzle()
    {
        GlobalStateManager.Instance.EmitSignal(GlobalStateManager.SignalName.PuzzleCompleted, puzzleId);
    }
}
