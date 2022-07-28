using SOScripts;
using UnityEngine;

namespace Units.Base
{
    public class BaseProducer: BaseBuilding
    {
        [SerializeField] public BaseForces forcesToProduce;

        protected override void Awake()
        {
            base.Awake();
            SetBannerToNearestAvailableNode();
        }

        private void SetBannerToNearestAvailableNode()
        {
            if (GridSystem.Instance.TryGetNearestNode(transform.position,3,out Node node))
            {
                ((ProducerData)baseUnitData).bannerNode = node;
                Debug.Log(node.gridIndex);
            }
            else
            {
                Debug.Log("yoo");
            }
        }

        public void Produce()
        {
            //TODO: Implement produce method here
        }


    }
}