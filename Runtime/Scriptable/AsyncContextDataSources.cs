﻿namespace UniModules.UniGame.SerializableContext.Runtime.Scriptable
{
    using System.Collections.Generic;
    using Core.Runtime.Interfaces;
    using Cysharp.Threading.Tasks;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniCore.Runtime.Rx.Extensions;
    using UniModules.UniGame.Context.Runtime.Abstract;
    using UniRx;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AsyncSources", fileName = nameof(AsyncContextDataSources))]
    public class AsyncContextDataSources : 
        AsyncContextDataSource
    {
        [SerializeReference]
        public List<AsyncContextDataSource> sources = new List<AsyncContextDataSource>();

        protected override void OnActivate()
        {
            base.OnActivate();
            OnReset();
        }

        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            var taskList = ClassPool.Spawn<List<UniTask<IContext>>>();

            foreach (var t in sources) {
                taskList.Add(t.RegisterAsync(context));
            }
            
            await UniTask.WhenAll(taskList);
            
            taskList.Despawn();

            return context;
        }

        protected override void OnReset()
        {
            base.OnReset();
            foreach (var source in sources)
            {
                source.AddTo(LifeTime);
            }
        }
    }
}
