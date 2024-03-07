using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObjects",menuName = "WeaponScriptableObjects/weapon")]
public class WeaponScriptableObjects : ScriptableObject
{
    [SerializeField]
    GameObject prefabs;
    public GameObject Prefabs { get => prefabs; private set => prefabs = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField]
    float coolDownDuration;
    public float CoolDownDuration { get => coolDownDuration; private set => coolDownDuration = value; }

    [SerializeField]
    int pierce;
    public int Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    int level; // not meant to modifiyin game but only for the editor
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; // the prefab of next level means after upgrading 
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }


    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }
    
    [SerializeField]
     string description;
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon;private set => icon = value; }

}
