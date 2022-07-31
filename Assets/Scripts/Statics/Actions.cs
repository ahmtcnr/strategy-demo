using System;
using SOScripts;
public static class Actions
{
    #region BuildingActions

    public static Action<BaseUnitData> OnBuildingUIClick;

    public static Action<BaseBuilding> OnBuildingClick;

    public static Action<BaseBuilding> OnBuildSuccess;

    public static Action OnDeselectBuilding;

    #endregion

    #region UnitSelection

    public static Action<BaseUnit> OnUnitSelected;

    public static Action OnUnitDeselected;

    public static Action OnLeftClickGround;

    #endregion

    #region InputActions

    public static Action OnLeftClick;
    public static Action OnRightClick;
    public static Action OnSpaceBarDown;

    #endregion
}