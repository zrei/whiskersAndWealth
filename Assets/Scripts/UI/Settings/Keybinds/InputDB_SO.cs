using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InputDB_SO", menuName = "ScriptableObjects/Input/InputDB_SO")]
public class InputDB_SO : ScriptableObject
{
    public List<InputMapBindsSO> InputMapBindsSO;
}