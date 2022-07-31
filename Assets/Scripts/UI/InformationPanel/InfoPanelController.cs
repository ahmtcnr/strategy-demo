using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

namespace InformationPanel
{
    public class InfoPanelController
    {
        private readonly InfoPanelModel _infoPanelModel;
        public InfoPanelController(InfoPanelModel infoPanelModel)
        {
            _infoPanelModel = infoPanelModel;
            
            Actions.OnUnitSelected += ProcessAndSetModelData;
            Actions.OnUnitDeselected += ClearPanel;
            
        }
        private void ProcessAndSetModelData(BaseUnit baseUnit)
        {
            _infoPanelModel.SelectedBaseUnit = baseUnit;
        }
        private void ClearPanel()
        {
            _infoPanelModel.SelectedBaseUnit = null;
        }
    }
}