using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public static class Actions 
{
    
    public static Action<BaseUnitData> OnBuildingUIClick;

    public static Action<BaseBuilding> OnBuildingClick;

    public static Action<BaseBuilding> OnBuildSuccess;
    
    public static Action OnDeselectBuilding;
    
    public static Action<BaseUnit> OnUnitSelected;

    public static Action OnUnitDeselected;




    
    
    
    #region InputActions

    public static Action OnLeftClick;
    public static Action OnRightClick;
    public static Action OnSpaceBarDown;

    #endregion

}
