using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

public static class Actions 
{
    
    public static Action<BuildingData> OnBuildingUIClick;//Data olarak gönder

    public static Action<BaseBuilding> OnBuildingClick;

    public static Action<BaseBuilding> OnBuildSuccess;
    
    public static Action OnDeselectBuilding;

    public static Action OnLeftClick;



}
