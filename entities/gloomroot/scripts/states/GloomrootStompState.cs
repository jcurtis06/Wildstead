using Godot;
using Wildstead.scripts.state;

namespace Wildstead.entities.gloomroot.scripts.states;

public class GloomrootStompState : IState<Gloomroot>
{
    private enum Phase
    {
        Jumping,
        Recovery
    };
    
    private Phase _currentPhase;
    private bool _isJumping;
    private float _jumpSpeed = 100;
    private Vector2 _jumpTarget;
    private Vector2 _jumpVelocity;
    private float _stompRadius = 16;
    private double _elapsedTime = 0f;
    private double _maxJumpTime = 1f;
    private float _recoveryTime = 1f;

    public void Enter(Gloomroot owner)
    {
        _jumpTarget = owner.Summoner.Position;
        _currentPhase = Phase.Jumping;
        _jumpVelocity = (owner.Summoner.Position - owner.Position).Normalized() * _jumpSpeed;
        owner.Velocity = _jumpVelocity;
        _elapsedTime = 0f;
    }

    public void Process(Gloomroot owner, double delta)
    {
        switch (_currentPhase)
        {
            case Phase.Jumping:
                PerformJump(owner, delta);
                break;
            case Phase.Recovery:
                PerformRecovery(owner, delta);
                break;
        }
    }

    private void PerformJump(Gloomroot owner, double delta)
    {
        owner.MoveAndSlide();

        if (owner.Position.DistanceTo(_jumpTarget) < 1)
        {
            owner.Velocity = Vector2.Zero;
            PerformStomp(owner);
            _currentPhase = Phase.Recovery;
        }
    }

    private void PerformRecovery(Gloomroot owner, double delta)
    {
        _elapsedTime += delta;

        if (_elapsedTime >= _recoveryTime)
        {
            owner.StateMachine.TransitionTo(new GloomrootStompState());
        }
    }

    private void PerformStomp(Gloomroot owner)
    {
        owner.StompParticles.Emitting = true;
        
        if (owner.Summoner.Position.DistanceTo(owner.Position) < _stompRadius)
        {
            owner.Summoner.TakeDamage(1);
        }
    }
    
    public void Exit(Gloomroot owner) {}
}