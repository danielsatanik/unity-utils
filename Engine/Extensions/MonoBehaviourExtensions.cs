using UnityEngine;
using System.Collections;
using UnityUtils.Engine.CustomTypes;

namespace UnityUtils.Engine.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static FuturePassable Async(this MonoBehaviour mb, IEnumerator coroutine)
        {
            return new FuturePassable
            {
                MonoBehaviour = mb,
                Coroutine = coroutine
            };
        }
    }
}