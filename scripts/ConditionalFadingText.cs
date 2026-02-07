using System;
using Godot;

public partial class ConditionalFadingText : RichTextLabel
{

    [Export]
    public CompletionCondition Condition;

    [Export]
    public float FadeDurationSeconds = 1.0f;

    [Export]
    public int FontSize = 24;

    [Export]
    public string FontColor = "black";

    private Callable _freeCallable;

    public override void _Ready()
    {
        base._Ready();
        Hide();
        Condition.ConditionChanged += OnConditionChanged;
        _freeCallable = Callable.From(QueueFree);

        // make the text italic in bbcode
        Text = $"[i]{Text}[/i]";

        // // set font size in bbcode
        Text = $"[font_size={FontSize}]{Text}[/font_size]";

        // set color in bbcode
        Text = $"[color={FontColor}]{Text}[/color]";

    }

    private void OnConditionChanged(bool isCompleted)
    {
        if (isCompleted)
        {
            Show();
            FadeTextOut();
        }
        else
        {
            Hide();
        }
    }

    private void FadeTextOut()
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 0.0f, FadeDurationSeconds)
             .SetTrans(Tween.TransitionType.Linear)
             .SetEase(Tween.EaseType.InOut);
             tween.TweenCallback(_freeCallable);
    }

}
