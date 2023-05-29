using UnityEngine;

public class Artillery : Unit
{
    public override UnitMapType GetUnitMapType()
    {
        return UnitMapType.Square;
    }

    protected override void AttackTarget(Transform target)
    {
        //throw new NotImplementedException();
    }
}
