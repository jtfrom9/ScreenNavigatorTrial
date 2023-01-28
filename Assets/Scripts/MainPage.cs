#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityScreenNavigator.Runtime.Core.Modal;

public class MainPage : Page
{
    [SerializeField]
    Button? backButton;

    [SerializeField]
    Button? showModelButton;

    void Start()
    {
        Debug.Log("MainPage.Start");

        if (backButton == null || showModelButton==null)
        {
            Debug.LogError("invalid");
            return;
        }
        setup(backButton, showModelButton);
    }

    public ISubject<Unit> OnShowModal = new Subject<Unit>();

    void setup(Button backButton, Button showModalButton)
    {
        PageContainer pageContainer = PageContainer.Of(transform);

        backButton.OnClickAsObservable().Subscribe(_ =>
        {
            // OnBack.OnNext(Unit.Default);
            pageContainer.Pop(playAnimation: true);
        }).AddTo(this);

        showModalButton.OnClickAsObservable().Subscribe(async _ => {
            // await ModalContainer.Of(transform).Push("SimpleModal", playAnimation: true, loadAsync: true).Task;
            OnShowModal.OnNext(Unit.Default);
        }).AddTo(this);
    }

    void OnDestroy()
    {
        Debug.Log("MainPager.OnDestroy");
    }
}
