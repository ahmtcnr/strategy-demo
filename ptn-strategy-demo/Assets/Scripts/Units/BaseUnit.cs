using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public class BaseUnit : MonoBehaviour,ISelectable
{
    
    [SerializeField] public BaseUnitData baseUnitData;

    [SerializeField] protected Transform spriteParent;
    
    
    
    public void OnClickAction()
    {
        Actions.OnUnitSelected?.Invoke(baseUnitData);
    }
}
