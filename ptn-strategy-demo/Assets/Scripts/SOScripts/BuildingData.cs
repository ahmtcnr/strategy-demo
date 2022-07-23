using UnityEngine;

namespace SOScripts
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Building", order = 0)]
    public class BuildingData : ScriptableObject
    {
        public Vector2Int buildingSize;
        public Sprite buildingSprite;
    }
}