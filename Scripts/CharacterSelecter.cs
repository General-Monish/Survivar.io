using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelecter : MonoBehaviour
{
    public static CharacterSelecter Instance;
    public CharacterScriptableObjects characterData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning(" Exit " + this + " Deleted ");
            Destroy(gameObject);
        }
    }
  
    public static CharacterScriptableObjects GetData()
    {
        return Instance.characterData;
    }

    public void SelectCharacter(CharacterScriptableObjects character)
    {
        characterData = character;
    }

    public void DestroySingelton()
    {
        Instance = null;
        Destroy(gameObject);
    }
}
