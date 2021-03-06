﻿using System;
using System.Collections;
using UnityEngine;

namespace Clione.Home
{
    public interface ISceneManager
    {
        MonoBehaviour Mono { get; }
        string CurrentSceneName { get; }
        string CurrentWindowPath { get; }
        string CurrentScreenPath { get; }
        bool IsLoadingScene { get; }
        void InitializeSetUp();
        IEnumerator LoadSceneEnumerator(string loadSceneName, object param = null, Action onComplete = null, Action onFail = null);
        IEnumerator LoadWindowEnumerator(string loadWindowPath, string loadScreenPath, Action onComplete = null);
        IEnumerator LoadScreenEnumerator(string loadScreenPath, Action onComplete = null);
    }
}