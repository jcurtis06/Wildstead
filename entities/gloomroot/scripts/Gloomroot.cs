using Godot;
using Wildstead.entities.gloomroot.scripts.states;
using Wildstead.entities.scripts;
using Wildstead.scripts.state;

public partial class Gloomroot : Boss
{
    [Export] public float Speed = 50;
    [Export] public GpuParticles2D StompParticles;
    
    public StateMachine<Gloomroot> StateMachine;

    public override void _Ready()
    {
        StateMachine = new StateMachine<Gloomroot>(this);
        StateMachine.TransitionTo(new GloomrootStompState());
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Summoner == null) return;
        
        StateMachine.Process(delta);
    }
}
