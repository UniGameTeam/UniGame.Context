﻿using UniCore.Runtime.ProfilerTools;
using UniModules.UniCore.Runtime.Common;
using UniModules.UniCore.Runtime.ObjectPool.Runtime;

namespace UniModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using Context.Runtime.Context;
    using Core.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniModules.UniContextData.Runtime.Entities;
    using UniModules.UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Assets/ContextContainerAsset", fileName = nameof(ContextContainerAsset))]
    public class ContextContainerAsset :
        TypeContainerAssetSource<IContext>
    {
        [SerializeField] private bool _createDefaultOnLoad = false;

        private DisposableAction _disposableAction;
        
        protected override void OnActivate()
        {
            base.OnActivate();

            if (_createDefaultOnLoad)
            {
                SetValue(new EntityContext());
            }

            this.Do(OnContextUpdated).Subscribe().AddTo(LifeTime);
        }

        private void OnContextUpdated(IContext context)
        {
            if (context == null)
                return;

            _disposableAction?.Complete();
            _disposableAction = ClassPool.Spawn<DisposableAction>();
            _disposableAction.Initialize(() => SetValue(null));
            
            context.LifeTime.AddDispose(_disposableAction);
            context.LifeTime.AddCleanUpAction(() => GameLog.Log($"CONTEXT CONTAINER {name} CONTEXT FINISHED", Color.red));

            GameLog.Log($"CONTEXT CONTAINER {name} CONTEXT VALUE UPDATE {context}", Color.red);
        }

        protected override void ResetValue()
        {
            base.ResetValue();
            _disposableAction?.Complete();
            SetValue(null);
        }
    }
}