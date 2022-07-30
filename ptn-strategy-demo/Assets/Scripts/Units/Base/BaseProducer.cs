using SOScripts;
using UnityEngine;

namespace Units.Base
{
    public class BaseProducer: BaseBuilding
    {
        [SerializeField] public BaseForces forcesToProduce;
        
        public Node bannerNode;
        
        protected override void Awake()
        {
            base.Awake();
            SetBannerToNearestAvailableNode();
            
           
        }

        private void SetBannerToNearestAvailableNode()
        {
            if (GridSystem.Instance.TryGetNearestWalkableNode(transform.position,out Node node))
            {
                bannerNode = node;
            }
            else
            {
                Debug.Log("yoo");
            }
        }

        public void Produce()
        {
            ForcesFactory.Instance.OnSpawnForces?.Invoke(forcesToProduce,transform.position , bannerNode);
        }


    }
}