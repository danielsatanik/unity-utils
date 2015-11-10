using UnityEngine;
using System;
using System.Collections;

namespace UnityUtils.Engine.Utilities.CustomTypes
{
    public class FuturePassable
    {
        public MonoBehaviour MonoBehaviour { get; set; }

        public IEnumerator Coroutine { get; set; }
    }

    public class Future<T>
    {
        T m_returnVal;
        bool m_unset = true;
        Exception m_exception;
        Coroutine m_coroutine;

        public void Wait()
        {
            while (m_unset)
            {
            }
        }

        public T Value
        {
            private set
            {
                m_unset = false;
                m_returnVal = value;
            }
            get
            {
                while (m_unset)
                {
                }
                if (m_exception != null)
                    throw m_exception;
                return m_returnVal;
            }
        }

        IEnumerator InternalRoutine(IEnumerator coroutine)
        {
            for (;;)
            {
                try
                {
                    if (!coroutine.MoveNext())
                        yield break;
                }
                catch (Exception e)
                {
                    m_exception = e;
                    yield break;
                }
                var yielded = coroutine.Current;
                if (yielded != null && yielded.GetType() == typeof(T))
                {
                    m_returnVal = (T)yielded;
                    yield break;
                }
                yield return coroutine.Current;
            }
        }

        public static implicit operator Future<T>(FuturePassable fp)
        {
            var future = new Future<T>();
            var enumerator = future.InternalRoutine(fp.Coroutine);
            future.m_coroutine = fp.MonoBehaviour.StartCoroutine(enumerator);
            return future;
        }
    }
}