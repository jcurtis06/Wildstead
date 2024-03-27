using Godot;
using Wildstead.entities.scripts;

public partial class Gloomroot : Boss
{
    [Export] public float Speed = 50;
    [Export] public AnimationPlayer WeaponAnimation;
    [Export] public Node2D WeaponContainer;
    
    private GloomrootState _state = GloomrootState.Chase;
    
    public override void _Process(double delta)
    {
        base._Process(delta);
        
        if (Summoner == null)
        {
            return;
        }
        
        switch (_state)
        {
            case GloomrootState.Chase:
                Chase();
                break;
            case GloomrootState.Flee:
                break;
            case GloomrootState.Swing:
                Swing();
                break;
            case GloomrootState.Ranged:
                break;
        }
    }
    
    private void Chase()
    {
        if (Position.DistanceTo(Summoner.Position) < 50)
        {
            _state = GloomrootState.Swing;
            return;
        }
        
        var direction = (Summoner.Position - Position).Normalized();
        Velocity = direction * Speed;
        MoveAndSlide();
    }

    private void Swing()
    {
        WeaponContainer.LookAt(Summoner.GlobalPosition);
        WeaponAnimation.Play("swing");
    }

    private void AnimationFinished(string animation)
    {
        if (animation == "swing")
        {
            _state = GloomrootState.Chase;
        }
    }
}

enum GloomrootState
{
    Chase,
    Flee,
    Swing,
    Ranged,
}
