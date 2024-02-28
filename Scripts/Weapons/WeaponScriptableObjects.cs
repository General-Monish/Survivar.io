using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObjects",menuName = "WeaponScriptableObjects/weapon")]
public class WeaponScriptableObjects : ScriptableObject
{
    public GameObject prefabs;
    public float damage;
    public float speed;
    public float coolDownDuration;
    float currentCoolDown;
    public int pierce;
}
