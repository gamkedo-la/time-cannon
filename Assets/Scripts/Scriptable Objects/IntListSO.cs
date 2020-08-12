using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntListSO", menuName = "ScriptableObject/IntListSO", order = 3)]
public class IntListSO : ScriptableObject
{
    public List<int> value;
}
