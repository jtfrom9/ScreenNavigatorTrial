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

[CreateAssetMenu(fileName = "InspectorAssetLoader", menuName = "Resource Loader/Inspector Asset Loader")]
public class InspectorAssetLoader : AssetLoaderObject, IAssetLoader
{
    [SerializeField] GameObject? topPagePrefab;
    [SerializeField] GameObject? mainPagePrefab;
    [SerializeField] GameObject? modalPrefab;

    private int _controlId = 0;
    public override AssetLoadHandle<T> Load<T>(string key)
    {
        var id = _controlId++;
        var handle = new AssetLoadHandle<T>(id);
        var setter = (IAssetLoadHandleSetter<T>)handle;

        if (key == "Prefabs/TopPage")
        {
            if (topPagePrefab is T)
            {
                var result = (topPagePrefab as T)!;
                setter.SetResult(result);
                setter.SetStatus(AssetLoadStatus.Success);
                setter.SetPercentCompleteFunc(() => 1.0f);
                setter.SetTask(Task.FromResult(result));
            }
            else
            {
                var exception = new InvalidOperationException($"Requested asset（Key: {key}）was not found.");
                setter.SetOperationException(exception);
            }
        }
        else
        {
            setter.SetOperationException(new InvalidOperationException($"Requested asset（Key: {key}）was not found."));
        }
        return handle;
    }

    public override AssetLoadHandle<T> LoadAsync<T>(string key)
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
        else if (key == "SimpleModal")
        {
            if (modalPrefab is T)
            {
                var result = (modalPrefab as T)!;
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

    public override void Release(AssetLoadHandle handle)
    {
    }
}
