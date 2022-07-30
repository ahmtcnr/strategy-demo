using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

namespace InformationPanel
{
    public class InfoPanelController
    {
        private InfoPanelModel _infoPanelModel;

        // private InformationPanelData _processedData;
        
        
        

        public InfoPanelController(InfoPanelModel infoPanelModel)
        {
            _infoPanelModel = infoPanelModel;
            
            Actions.OnUnitSelected += ProcessAndSetModelData;
            Actions.OnDeselectUnit += ClearPanel;
            
        }
        private void ProcessAndSetModelData(BaseUnit baseUnit)
        {

            _infoPanelModel.SelectedBaseUnit = baseUnit;

            //informationPanelView.VisualiseUnitData(_processedData);


        }
        private void ClearPanel()
        {
            //informationPanelView.ClearPanel();
        }
        // private void Awake()
        // {
        //     _informationPanelModel = new InformationPanelModel();
        // }

        // private void OnEnable()
        // {
        //     Actions.OnUnitSelected += OnClickUnit;
        //     Actions.OnDeselectUnit += ClearPanel;
        // }
        //
        // private void OnDisable()
        // {
        //     Actions.OnUnitSelected -= OnClickUnit;
        //     Actions.OnDeselectUnit -= ClearPanel;
        // }


        
    }
}