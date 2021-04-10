using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kuhpik;

public class LoadingSystem : GameSystem, IIniting
{
    void IIniting.OnInit()
    {
        config.Init(config.GameValusConfigs);
    }
}
