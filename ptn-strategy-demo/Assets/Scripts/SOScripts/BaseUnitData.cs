using UnityEngine;

namespace SOScripts
{
    public abstract class BaseUnitData : ScriptableObject
    {
        public Vector2Int unitSize;
        public Sprite unitSprite;
        public string unitName;
        
        [HideInInspector] public int CurrentHealth;
        public int MaxHealth;
    }
}