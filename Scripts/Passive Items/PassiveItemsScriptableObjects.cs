using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PassiveItemsScriptableObjects",menuName = "PassiveItemsScriptableObjects/PassiveItems")]
public class PassiveItemsScriptableObjects : ScriptableObject
{
    [SerializeField]
    float multiPlier;
    public float Multiplier { get => multiPlier; private set => value = multiPlier; }

    [SerializeField]
    int level; // not meant to modifiyin game but only for the editor
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; // the prefab of next level means after upgrading 
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }
}
