// Copyright (c) Improbable Worlds Ltd, All Rights Reserved
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// =========== 
using System;
using System.Collections;
using Improbable.Assets;
using UnityEngine;
using Random = System.Random;

namespace Improbable.Unity.Assets
{
    class ExponentialBackoffRetryAssetLoader : MonoBehaviour, IAssetLoader<AssetBundle>
    {
        [Range(0, 100)]
        public int MaxRetries = 3;

        [Range(250, 10000)]
        public int StartBackoffTimeout = 250;

        public IAssetLoader<AssetBundle> AssetLoader { get; set; }
        private static readonly Random Random = new Random();

        public void LoadAsset(string prefabName, Action<AssetBundle> onAssetLoaded, Action<Exception> onError)
        {
            var retryContext = new RetryContext(this, MaxRetries, StartBackoffTimeout, prefabName, onAssetLoaded, onError);
            AssetLoader.LoadAsset(prefabName, onAssetLoaded, retryContext.Retry);
        }

        private void LoadAssetRetry(RetryContext retryContext, float backoffSeconds)
        {
            StartCoroutine(ScheduleRetry(retryContext, backoffSeconds));
        }

        private IEnumerator ScheduleRetry(RetryContext retryContext, float backoffSeconds)
        {
            yield return new WaitForSeconds(backoffSeconds);
            if (AssetLoader != null && retryContext != null)
            {
                AssetLoader.LoadAsset(retryContext.PrefabName, retryContext.OnAssetLoaded, onError: retryContext.Retry);
            }
            else
            {
                var errorMessage = string.Format("Suspicious condition in ExponentialBackoffRetryAssetLoader: AssetLoader is {0}, retryContext is {1}", 
                    AssetLoader == null ? "null" : "not null", retryContext == null ? "null" : "not null");

                Debug.LogErrorFormat(errorMessage);
                
                // Try again, why not?
                if (retryContext != null)
                {
                    retryContext.Retry(new Exception(errorMessage));
                }
            }
        }

        private class RetryContext
        {
            private int retriesLeft;
            private int nextBackoffTimeout;
            private readonly Action<Exception> onError;
            private readonly ExponentialBackoffRetryAssetLoader backoffRetryAssetLoader;
            public readonly Action<AssetBundle> OnAssetLoaded;
            public readonly string PrefabName;

            public RetryContext(ExponentialBackoffRetryAssetLoader backoffRetryAssetLoader, int retriesLeft, int nextBackoffTimeout, string prefabName, Action<AssetBundle> onAssetLoaded, Action<Exception> onError)
            {
                this.retriesLeft = retriesLeft;
                this.nextBackoffTimeout = nextBackoffTimeout;
                this.onError = onError;
                this.backoffRetryAssetLoader = backoffRetryAssetLoader;
                OnAssetLoaded = onAssetLoaded;
                PrefabName = prefabName;
            }

            public void Retry(Exception exception)
            {
                if (retriesLeft <= 0)
                {
                    onError(exception);
                }
                else
                {
                    --retriesLeft;
                    var backoffSeconds = Random.Next(nextBackoffTimeout) / 1000f;
                    nextBackoffTimeout *= 2;
                    Debug.LogFormat("Loading asset {0} failed with exception {1}\nRetrying after {2}s. Retries left {3}.", PrefabName, exception.Message, backoffSeconds, retriesLeft);
                    backoffRetryAssetLoader.LoadAssetRetry(this, backoffSeconds);
                }
            }
        }
    }
}
