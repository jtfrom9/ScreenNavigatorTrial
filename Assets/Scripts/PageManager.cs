#nullable enable

using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
using Cysharp.Threading.Tasks;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityScreenNavigator.Runtime.Foundation.AssetLoader;

public class PageManager : MonoBehaviour, IPageContainerCallbackReceiver, IAssetLoader
{
    [SerializeField] PageContainer? pageContainer;
    [SerializeField] GameObject? topPagePrefab;
    [SerializeField] GameObject? mainPagePrefab;

    private int _controlId = 0;
    public AssetLoadHandle<T> Load<T>(string key) where T: UnityEngine.Object
    {
        var id = _controlId++;
        var handle = new AssetLoadHandle<T>(id);
        var setter = (IAssetLoadHandleSetter<T>)handle;

        if(key=="Prefabs/TopPage") {
            if (topPagePrefab is T)
            {
                var result = (topPagePrefab as T)!;
                setter.SetResult(result);
                setter.SetStatus(AssetLoadStatus.Success);
                setter.SetPercentCompleteFunc(() => 1.0f);
                setter.SetTask(Task.FromResult(result));
            } else {
                var exception = new InvalidOperationException($"Requested asset（Key: {key}）was not found.");
                setter.SetOperationException(exception);
            }
        } else
        {
            setter.SetOperationException(new InvalidOperationException($"Requested asset（Key: {key}）was not found."));
        }
        return handle;
    }

    public AssetLoadHandle<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        var id = _controlId++;
        var handle = new AssetLoadHandle<T>(id);
        var setter = (IAssetLoadHandleSetter<T>)handle;
        var tcs = new TaskCompletionSource<T>();

        if (key == "Prefabs/TopPage")
        {
            if (topPagePrefab is T)
            {
                var result = (topPagePrefab as T)!;
                setter.SetResult(result);
                setter.SetStatus(AssetLoadStatus.Success);
                setter.SetPercentCompleteFunc(() => 1.0f);
                setter.SetTask(Task.FromResult(result));
                tcs.SetResult(result);
                return handle;
            }
            else
            {
                var exception = new InvalidOperationException($"Requested asset（Key: {key}）was not found.");
                setter.SetOperationException(exception);
            }
        }
        else if (key == "Prefabs/MainPage")
        {
            if (mainPagePrefab is T)
            {
                var result = (mainPagePrefab as T)!;
                setter.SetResult(result);
                setter.SetStatus(AssetLoadStatus.Success);
                setter.SetPercentCompleteFunc(() => 1.0f);
                setter.SetTask(Task.FromResult(result));
                tcs.SetResult(result);
                return handle;
            }
            else
            {
                var exception = new InvalidOperationException($"Requested asset（Key: {key}）was not found.");
                setter.SetOperationException(exception);
            }
        }
        else
        {
            // setter.SetResult(null);
            setter.SetStatus(AssetLoadStatus.Failed);
            setter.SetOperationException(new InvalidOperationException($"Requested asset（Key: {key}）was not found."));
            // tcs.SetResult(null);
        }
        return handle;
    }

    public void Release(AssetLoadHandle handle)
    {

    }


    async void Start()
    {
        if (pageContainer == null || topPagePrefab==null)
        {
            Debug.LogError("invalid");
            return;
        }
        pageContainer.AddCallbackReceiver(this);
        pageContainer.AssetLoader = this;

        await pageContainer.Push($"Prefabs/TopPage", true, loadAsync: true).Task;
        //     await pageContainer.Push($"Prefabs/TopPage", true, loadAsync: true, onLoad: page =>
        //     {
        //         page.AddLifecycleEvent(initialize: UniTask.ToCoroutine(() => UniTask.FromResult<Page>(page)));
        //     });
        // }).Task;
    }

    // ページがPushされる直前に呼ばれる
    public void BeforePush(Page? enterPage, Page? exitPage)
    {
        Debug.Log($"BeforePush: {enterPage?.name ?? "N/A"}, {exitPage?.name ?? "N/A"}");
        if(pageContainer==null)
            return;

        if(enterPage is TopPage) {
            var topPage = (TopPage)enterPage;
            topPage.OnClick.Subscribe(async _ => {
                await pageContainer.Push("Prefabs/MainPage", true, loadAsync: true).Task;
            }).AddTo(this);
        }
        if(enterPage is MainPage) {
            var mainPage = (MainPage)enterPage;
        }
    }

    // ページがPushされた直後に呼ばれる
    public void AfterPush(Page enterPage, Page exitPage)
    {
        Debug.Log($"AfterPush: {enterPage?.name ?? "N/A"}, {exitPage?.name ?? "N/A"}");
    }
    // ページがPopされる直前に呼ばれる
    public void BeforePop(Page enterPage, Page exitPage)
    {
        Debug.Log($"BeforePop: {enterPage?.name ?? "N/A"}, {exitPage?.name ?? "N/A"}");
    }
    // ページがPopされた直後に呼ばれる
    public void AfterPop(Page enterPage, Page exitPage)
    {
        Debug.Log($"AfterPush: {enterPage?.name ?? "N/A"}, {exitPage?.name ?? "N/A"}");
    }
}
