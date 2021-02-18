﻿using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Client
{
    public class NotifyOnDestroy : MonoBehaviour
    {
        public event Action<AssetReference, NotifyOnDestroy> Destroyed;
        public AssetReference AssetReference { get; set; }

        private void OnDestroy()
        {
            Destroyed?.Invoke(AssetReference, this);
        }
    }
}