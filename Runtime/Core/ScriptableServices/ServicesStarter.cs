using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Erethan.ScriptableSystems;

public class ServicesStarter : MonoBehaviour
{
    [SerializeField] private IService[] _services;

    [SerializeField] private UnityEvent _servicesRunning;

    private void Start()
    {
        foreach (var service in _services)
        {
            service.Startup();
        }
    }


}
