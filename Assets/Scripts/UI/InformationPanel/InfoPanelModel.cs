using System;
using SOScripts;
using TMPro;
using Units.Base;
using UnityEngine;
using UnityEngine.UI;


namespace InformationPanel
{
    public class InfoPanelModel
    {
        private BaseUnit _selectedBaseUnit;

        public BaseUnit SelectedBaseUnit
        {
            set
            {
                if (!Equals(_selectedBaseUnit, value))
                {
                    _selectedBaseUnit = value;
                    OnBaseUnitChanged?.Invoke(_selectedBaseUnit);
                }
            }
        }

        public event Action<BaseUnit> OnBaseUnitChanged;
    }
}