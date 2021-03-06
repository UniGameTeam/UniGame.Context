﻿using UnityEngine;

namespace UniModules.UniGame.Context.Scenarios {
    using System;
    using Core.Runtime.Interfaces;
    using Cysharp.Threading.Tasks;
    using SerializableContext.Runtime.Abstract;
    using SerializableContext.Runtime.Scenarios;
    using UniRx;

    [Serializable]
    public class DelayScenario : IAsyncContextCommand 
    {
        public float _delay = 2f;
        
        public async UniTask<AsyncStatus> ExecuteAsync(IContext value) {

            Debug.Log("DelayScenario START");
            
            await UniTask.Delay(TimeSpan.FromSeconds(_delay));
 
            Debug.Log("DelayScenario COMPLETE");
           
            return AsyncStatus.Succeeded;

        }
    }
}
