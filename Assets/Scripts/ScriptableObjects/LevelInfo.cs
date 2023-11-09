using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Info")]
public class LevelInfo : ScriptableObject
{
    public int Level;
    public int AmountToCollect;
}