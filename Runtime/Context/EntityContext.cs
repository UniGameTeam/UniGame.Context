﻿namespace UniModules.UniGame.Context.Runtime.Context 
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.DataFlow;
    using Core.Runtime.DataFlow.Interfaces;
    using Core.Runtime.Interfaces;
    using global::UniGame.UniNodes.NodeSystem.Runtime.Connections;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniRx;

    [Serializable]
    public class EntityContext : 
        IManagedBroadcaster<IMessagePublisher>,
        IDisposableContext
    {
        private TypeData           data;
        private LifeTimeDefinition lifeTimeDefinition;
        private TypeDataBrodcaster broadcaster;
        private int                id;

        public EntityContext() 
        {
            //context data container
            data = new TypeData();
            
            //context lifetime
            lifeTimeDefinition = new LifeTimeDefinition();
            broadcaster        = new TypeDataBrodcaster();

            id = Unique.GetId();
        }

        #region connection api

        public int BindingsCount => broadcaster.Count;

        public void Break(IMessagePublisher connection) {
            broadcaster.Remove(connection);
        }

        public IDisposable Broadcast(IMessagePublisher connection) {
            if(ReferenceEquals(connection, this))
                return Disposable.Empty;
            
            var disposable = broadcaster.Broadcast(connection);
            return disposable;
        }

        #endregion

        #region public properties

        public int Id => id;

        public ILifeTime LifeTime => lifeTimeDefinition.LifeTime;

        public bool HasValue => data.HasValue;

        #endregion

        #region public methods

        public bool Contains<TData>() => data.Contains<TData>();

        public virtual TData Get<TData>() => data.Get<TData>();

        public bool Remove<TData>() =>  data.Remove<TData>();

        public void RemoveSilent<TData>() => data.RemoveSilent<TData>();

        public void Release() {
            data.Release();
            broadcaster.Release();
            lifeTimeDefinition.Release();
        }

        public virtual void Dispose() {
            Release();
            this.Despawn();
        }

        #region rx

        public void Publish<T>(T message)
        {
            CheckLifeTimeValue(message);
            
            data.Publish(message);
            broadcaster.Publish(message);
        }

        public IObservable<T> Receive<T>() {
            return data.Receive<T>();
        }

        #endregion

        #endregion

        #region private methods


        private void CheckLifeTimeValue<T>(T value)
        {
            var lifeTime = value.GetLifeTime();
            
            if (lifeTime.IsTerminatedLifeTime())
                return;
            
            lifeTime.AddCleanUpAction(() =>
            {
                var valueData = Get<T>();
                if (ReferenceEquals(valueData,value))
                    Remove<T>();
            });
        }
        
        #endregion
        
        
        #region Unity Editor Api

#if UNITY_EDITOR

        public IReadOnlyDictionary<Type, IValueContainerStatus> EditorValues => data.EditorValues;

#endif

        #endregion
    }
}