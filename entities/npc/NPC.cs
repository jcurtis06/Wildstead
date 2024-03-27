using Godot;
using System;
using Godot.Collections;

public partial class NPC : Area2D
{
    [Export] public Array<string> Dialogues;
    [Export] public float PauseTime = 2.0f;

    private Timer _pauseTimer;
    private Label _dialogueLabel;
    
    private int _dialogueIndex = 0;

    public override void _Ready()
    {
        _pauseTimer = GetNode<Timer>("PauseTimer");
        _dialogueLabel = GetNode<Label>("DialogueLabel");
    }

    private void StartDialogue(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            _dialogueLabel.Text = Dialogues[_dialogueIndex];
            _pauseTimer.Start(PauseTime);
        }
    }
    
    private void StopDialogue(Node2D body)
    {
        GD.Print("StopDialogue");
        if (body.IsInGroup("Player"))
        {
            _pauseTimer.Stop();
            _dialogueLabel.Text = "";
            _dialogueIndex = 0;
        }
    }

    private void StartNextLine()
    {
        _dialogueIndex++;
        
        if (_dialogueIndex >= Dialogues.Count)
        {
            _dialogueIndex = 0;
        }
        
        _dialogueLabel.Text = Dialogues[_dialogueIndex];
        _pauseTimer.Start(PauseTime);
    }
}
