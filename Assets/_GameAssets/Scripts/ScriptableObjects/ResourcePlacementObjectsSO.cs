using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName ="ResourceData", menuName = "SOs/ResourcePlacementObjectData")]
public class ResourcePlacementObjectsSO : ScriptableObject
{
    public Enums.CardTypes ResourceType;
    public GameObject ObjectPrefab;
    public GameObject GhostPrefab;
    public int ResourceRange;
}
