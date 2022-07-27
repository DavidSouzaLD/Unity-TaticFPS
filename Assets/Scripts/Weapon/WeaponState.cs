using System.Collections.Generic;

public class WeaponState : StateBase
{
    public WeaponState()
    {
        states = new List<State>()
            {
                new State("Safe"),
                new State("Combat"),
                new State("Aiming"),
                new State("Reloading"),
                new State("Firing"),
            };
    }
}
