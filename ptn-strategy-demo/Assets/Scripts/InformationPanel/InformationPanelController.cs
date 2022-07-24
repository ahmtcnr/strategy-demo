using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

namespace InformationPanel
{
    public class InformationPanelController : MonoBehaviour
    {
        private InformationPanelModel _informationPanelModel;
        [SerializeField] private InformationPanelViewer informationPanelViewer;

        private InformationPanelData _processedData;
        private void Awake()
        {
            _informationPanelModel = new InformationPanelModel();
        }

        private void OnEnable()
        {
            Actions.OnUnitSelected += OnClickUnit;
        }

        private void OnDisable()
        {
            Actions.OnUnitSelected -= OnClickUnit;
        }

        private void OnClickUnit(BaseUnitData bd)
        {
            _processedData = _informationPanelModel.ProcessInformationPanelData(bd);
            

            informationPanelViewer.VisualiseUnitData(_processedData);
        }
    }
}