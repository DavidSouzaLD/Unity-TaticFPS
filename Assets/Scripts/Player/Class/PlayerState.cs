using System.Collections.Generic;

public class PlayerState : StateBase
{
    public PlayerState()
    {
        states = new List<State>()
            {
                new State("Walking"),
                new State("Running"),
                new State("Crouching"),
                new State("Jumping"),
                new State("GroundArea"),
                new State("GroundCollision"),
                new State("Covering"),
                new State("Sloping"),
                new State("Aiming"),
                new State("Graviting"),
            };
    }
}
