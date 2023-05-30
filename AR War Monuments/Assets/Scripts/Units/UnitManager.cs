using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<UnitGroup> groups;
    [SerializeField] private List<Transform> groupParents;

    public List<Transform> GroupParents => groupParents;
    public List<UnitGroup> Groups => groups;

    public void SpawnUnitGroups()
    {
        for (int i = 0; i < groups.Count; i++)
        {
            UnitGroup group = groups[i];
            DestroyExistingObjects(groupParents[i]);
            Transform parent = groupParents[i];
            for (int j = 0; j < group.unitsInGroup.Count; j++)
            {
                var unitAmount = group.unitsInGroup[j];
                SpawnUnit(unitAmount, j, parent);
            }
        }
    }


    private void SpawnUnit(UnitAmount unitAmount, int index, Transform parent)
    {
        GameObject parentObj = new GameObject($"Units_{index}");
        parentObj.transform.SetParent(parent);
        parentObj.transform.localPosition = Vector3.zero;
        for (int i = 0; i < unitAmount.amount; i++)
        {
            Vector3 position = GetUnitPosition(unitAmount.formation, unitAmount.spaceBetweenUnits, i, unitAmount.amount);
            GameObject unit = Instantiate(unitAmount.unit).gameObject;
            unit.transform.localPosition = position + parent.position;
            unit.transform.SetParent(parentObj.transform);
        }
    }

    private Vector3 GetUnitPosition(UnitFormation formation, float spaceBetweenUnits, int index, int amount)
    {
        switch (formation)
        {
            case UnitFormation.Circle:
            {
                float angle = ((float)index / amount) * 360f * Mathf.Deg2Rad;
                return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spaceBetweenUnits; // spaceBetweenUnits = radius in this case
            }
            case UnitFormation.Square:
            {
                int amountPerRow = Mathf.CeilToInt(Mathf.Sqrt(amount));
                float initX = -(amountPerRow * spaceBetweenUnits) / 2f;
                float initY = initX;
                float x = spaceBetweenUnits * (index % amountPerRow), y = spaceBetweenUnits * (index / amountPerRow);
                return new Vector3(initX + x, 0, initY + y);
            }
            case UnitFormation.VerticalLine:
            {
                float initPos = -(amount * spaceBetweenUnits) / 2f;
                return new Vector3(0, 0, initPos + index * amount * spaceBetweenUnits);
            }
            case UnitFormation.HorizontalLine:
            default:
            {
                float initPos = -(amount * spaceBetweenUnits) / 2f;
                return new Vector3(initPos + index * amount * spaceBetweenUnits, 0, 0);
            }
          
           
        }
    }

    public void DestroyUnits()
    {
        foreach (Transform gTransform in groupParents)
        {
            DestroyExistingObjects(gTransform);
        }
    }
    
    private void DestroyExistingObjects(Transform groupParent)
    {
        for (int i = groupParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(groupParent.GetChild(i).gameObject);
        }
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (groups.Count == 0)
        {
            foreach (Transform parent in groupParents)
            {
                DestroyImmediate(parent.gameObject);
            }
            groupParents.Clear();
            return;
        }
        while (groupParents.Count < groups.Count)
        {
            GameObject newObject = new GameObject($"Group_{GroupParents.Count}");
            newObject.transform.SetParent(transform);
            groupParents.Add(newObject.transform);
        }

        while (groupParents.Count > groups.Count)
        {
            Transform toRemove = groupParents[groupParents.Count - 1];
            groupParents.RemoveAt(groupParents.Count - 1);
            UnityEditor.EditorApplication.delayCall+=()=>
            {
                DestroyImmediate(toRemove.gameObject);
            };
        }

        for (int i = 0; i < groups.Count; i++)
        {
            if(groupParents[i] == null)
            {
                GameObject newObject = new GameObject($"Group_{i}");
                newObject.transform.SetParent(transform);
                groupParents[i] = newObject.transform;
            }
        }
    }
#endif
    
}
