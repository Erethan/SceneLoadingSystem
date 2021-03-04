using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Erethan.ScriptableSystems
{
    public abstract class ScriptableService<TBehaviour> : ScriptableObject, IService where TBehaviour : ScriptableServiceBehaviour
    {

        private TBehaviour InstantiateNewBehaviour() 
        {
            TBehaviour instance = new GameObject()
                .AddComponent<TBehaviour>();


            DontDestroyOnLoad(instance.gameObject);
            instance.gameObject.name = $"{typeof(TBehaviour)}";
            instance.Initialize();
            return instance;
        }

        private TBehaviour _controllerBehaviour;
        protected TBehaviour ControllerBehaviour
        {
            get
            {
                if (_controllerBehaviour == null)
                {
                    Startup();
                }
                return _controllerBehaviour;
            }
            set
            {
                _controllerBehaviour = value;
            }
        }


        public event Action InitializationComplete;
        public event Action FreeComplete;

        public virtual void Free()
        {
            if (ControllerBehaviour == null)
                return;
            Destroy(ControllerBehaviour.gameObject);
        }

        public virtual void Startup()
        {
            _controllerBehaviour = InstantiateNewBehaviour();
        }


    }
}