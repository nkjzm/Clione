﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clione.Home
{
    /// <summary>
    /// Window のベースクラス
    /// </summary>
    public abstract class WindowBase : MonoBehaviour
    {
        /// <summary>
        /// 現在開かれている Screen
        /// </summary>
        public ScreenBase CurrentOpenScreen { get; private set; }

        public string CurrentOpenScreenPath => CurrentOpenScreen?.ScreenPath ?? string.Empty;

        public string WindowPath { get; private set; }

        /// <summary>
        /// これまで開いた Screen の Base クラス
        /// </summary>
        public Dictionary<string, ScreenBase> ScreenBaseList = new Dictionary<string, ScreenBase>();

        public void SetWindowPath(string path) => WindowPath = path;

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual IEnumerator InitializeEnumerator()
        {
            yield break;
        }

        #region Window Open

        /// <summary>
        /// Window が開かれる前の処理
        /// </summary>
        public virtual IEnumerator OnBeforeOpenWindowEnumerator()
        {
            yield break;
        }

        /// <summary>
        /// Window が開かられるときの処理
        /// </summary>
        public IEnumerator OnOpenWindowEnumerator(string nextScreenPath, string currentScreenPath)
        {
            if (currentScreenPath != nextScreenPath)
            {
                if (!ScreenBaseList.ContainsKey(nextScreenPath) || ScreenBaseList[nextScreenPath] == null)
                {
                    var prefab = Resources.Load<GameObject>(nextScreenPath);
                    ScreenBaseList.Add(nextScreenPath, Instantiate(prefab, this.transform).GetComponent<ScreenBase>());
                    yield return StartCoroutine(ScreenBaseList[nextScreenPath].InitializeEnumerator());
                }

                CurrentOpenScreen = ScreenBaseList[nextScreenPath];
                CurrentOpenScreen.gameObject.SetActive(true);
                CurrentOpenScreen.SetScreenPath(nextScreenPath);
            }

            yield return StartCoroutine(CurrentOpenScreen.OnBeforeOpenScreenEnumerator());
            yield return StartCoroutine(CurrentOpenScreen.OnOpenScreenEnumerator());
            yield return StartCoroutine(CurrentOpenScreen.OnAfterOpenScreenEnumerator());
        }

        /// <summary>
        /// Window が開かれたあとの処理
        /// </summary>
        public virtual IEnumerator OnAfterOpenWindowEnumerator()
        {
            yield break;
        }

        #endregion

        #region Window Close

        /// <summary>
        /// Window を閉じる前の処理
        /// </summary>
        public virtual IEnumerator OnBeforeCloseWindowEnumerator()
        {
            yield break;
        }

        /// <summary>
        /// Window を閉じるときの処理
        /// </summary>
        public IEnumerator OnCloseWindowEnumerator()
        {
            if (CurrentOpenScreen == null)
            {
                yield break;
            }

            yield return StartCoroutine(OnCloseScreenEnumerator());
        }

        /// <summary>
        /// Window を閉じたあとの処理
        /// </summary>
        public virtual IEnumerator OnAfterCloseWindowEnumerator()
        {
            yield break;
        }

        #endregion

        /// <summary>
        /// 現在開かれている Screen が閉じられる前の処理
        /// </summary>
        public IEnumerator OnCloseScreenEnumerator()
        {
            yield return StartCoroutine(CurrentOpenScreen.OnBeforeCloseScreenEnumerator());
            yield return StartCoroutine(CurrentOpenScreen.OnCloseScreenEnumerator());
            yield return StartCoroutine(CurrentOpenScreen.OnAfterCloseScreenEnumerator());
            CurrentOpenScreen.gameObject.SetActive(false);
        }
    }
}