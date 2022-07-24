using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using UnityEngine;

namespace InformationPanel
{
    public class Controller : MonoBehaviour
    {
        private Model model;
        [SerializeField] private View view;

        private void Awake()
        {
            model = new Model();
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
            InformationPanelData processedData = new InformationPanelData();
            processedData = model.ProcessInformationPanelData(bd);

            view.VisualiseUnitData(processedData);
        }
    }
}