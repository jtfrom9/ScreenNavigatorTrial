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
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Foundation.AssetLoader;

public class PageManager : MonoBehaviour, IPageContainerCallbackReceiver
{
    [SerializeField] PageContainer? pageContainer;
    [SerializeField] ModalContainer? modalContainer;

    async void Start()
    {
        if (pageContainer == null || modalContainer == null)
        {
            Debug.LogError("invalid");
            return;
        }
        pageContainer.AddCallbackReceiver(this);

        await pageContainer.Push($"TopPage", playAnimation: false, loadAsync: true).Task;
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
                await pageContainer.Push("MainPage", playAnimation: true, loadAsync: true).Task;
            }).AddTo(this);
        }
        if(enterPage is MainPage) {
            var mainPage = (MainPage)enterPage;
            mainPage.OnShowModal.Subscribe(async _ => {
                if (modalContainer != null)
                {
                    await modalContainer.Push("SimpleModal", playAnimation: true, loadAsync: true).Task;
                }
            }).AddTo(this);
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
