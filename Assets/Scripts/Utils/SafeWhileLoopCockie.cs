using System;
using UnityEngine;

namespace Utils
{
    public static class InfiniteLoopProtector
    {
        public static SafeWhileLoopCookie RunCookie(int maxIterations = 100,
            SafeWhileLoopCookie.EInfiniteLoopBehavior behavior = SafeWhileLoopCookie.EInfiniteLoopBehavior.PauseEditor) => new SafeWhileLoopCookie(maxIterations, behavior);
    }
    
    public struct SafeWhileLoopCookie : IDisposable
    {
        private int _maxIterations;
        private readonly EInfiniteLoopBehavior _behavior;

        [Flags]
        public enum EInfiniteLoopBehavior
        {
            None = 0,
            ThrowException = 1 << 0,
            PauseEditor = 1 << 1,
            StopEditorPlayMode = PauseEditor | 1 << 2,
        }

        public SafeWhileLoopCookie(int maxIterations, EInfiniteLoopBehavior behavior)
        {
            _maxIterations = maxIterations;
            _behavior = behavior;
        }

        public bool NextLoop()
        {
            return --_maxIterations > 0;
        }

        public void Dispose()
        {
            if (_maxIterations <= 0)
            {
                if(_behavior.HasFlag(EInfiniteLoopBehavior.ThrowException))
                    throw new InfiniteLoopException();
                if (_behavior.HasFlag(EInfiniteLoopBehavior.PauseEditor))
                {
                    Debug.Break();
                    Debug.LogError("Infinite Loop");
                }
                if (_behavior.HasFlag(EInfiniteLoopBehavior.StopEditorPlayMode))
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#endif
                }
            }
        }
    }

    public class InfiniteLoopException : Exception
    {
    }
}