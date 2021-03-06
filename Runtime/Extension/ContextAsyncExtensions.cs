using System;
using UniModules.UniCore.Runtime.Extension;
using UniModules.UniGame.CoreModules.UniGame.AddressableTools.Runtime.Rx;

namespace UniModules.UniGame.CoreModules.UniGame.Context.Runtime.Extension
{
    using Cysharp.Threading.Tasks;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    
    public static class ContextAsyncExtensions 
    {
        public static async UniTask<TValue> ReceiveFirstAsync<TValue>(this IReadOnlyContext context)
        {
            if(context == null) return default;
            return await context.Receive<TValue>().AwaitFirstAsync(context.LifeTime);
        }
    
        public static async UniTask<TValue> ReceiveFirstAsync<TValue>(this IReadOnlyContext context,ILifeTime lifeTime)
        {
            if(context == null) return default;
            return await context.Receive<TValue>().AwaitFirstAsync(lifeTime);
        }
        
        public static async UniTask<TValue> ReceiveFirstAsync<TValue>(this IReadOnlyContext context,IObservable<TValue> observable)
        {
            if(context == null) return default;
            return await observable.AwaitFirstAsync(context.LifeTime);
        }
        
        public static async UniTask<TValue> ReceiveFirstAsync<TValue>(this ILifeTime lifeTime,IObservable<TValue> observable)
        {
            if(lifeTime == null) return default;
            return await observable.AwaitFirstAsync(lifeTime);
        }
        
    }
}
