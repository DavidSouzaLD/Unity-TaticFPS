using System.Collections.Generic;

public class WeaponState : StateBase
{
    public WeaponState()
    {
        states = new List<State>()
            {
                new State("Safety"),
                new State("Aiming"),
                new State("Reloading"),
                new State("Firing"),
                new State("Drawing"),
                new State("Hiding"),
            };
    }
}
