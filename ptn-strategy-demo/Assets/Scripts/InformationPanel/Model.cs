using SOScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace InformationPanel
{
    public class Model
    {
        public InformationPanelData ProcessInformationPanelData(BaseUnitData bd)
        {
            
            InformationPanelData panelData = new InformationPanelData();

            var normalizedHealth = bd.CurrentHealth / bd.MaxHealth;
            
            
            panelData.UnitSprite = bd.unitSprite;
            panelData.UnitText = bd.unitName;
            panelData.VisualizedHealth = normalizedHealth;
            return panelData;
        }
    }
    
    
    public struct InformationPanelData
    {
        public Sprite UnitSprite;
        public string UnitText;
        public float VisualizedHealth;

        
    }
}