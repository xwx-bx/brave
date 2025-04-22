using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string effectDescription;

    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("Item Effect Executed");
    }
}
