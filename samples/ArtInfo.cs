using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/ArtInfo", order = 1)]
public class ArtInfo : ScriptableObject
{
    public string id;
    public ArtCategory category;
    public GameObject artPieceObject;
    public string creator;
    public Sprite heistListSprite;

    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}

public enum ArtCategory
{
    General,
    Digital,
    Crafts
}
