using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Erethan.ScriptableSystems
{
    public interface IService
    {
        event Action InitializationComplete;
        event Action FreeComplete;

        void Startup();

        void Free();
    }
}

