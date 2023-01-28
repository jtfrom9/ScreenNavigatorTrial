#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
using UnityScreenNavigator.Runtime.Core.Page;

public class MainPage : Page
{
    [SerializeField]
    Button? backButton;

    void Start()
    {
        Debug.Log("MainPage.Start");

        if (backButton == null)
        {
            Debug.LogError("invalid");
            return;
        }
        setup(backButton);
    }

    void setup(Button backButton)
    {
        PageContainer pageContainer = PageContainer.Of(transform);

        backButton.OnClickAsObservable().Subscribe(_ =>
        {
            // OnBack.OnNext(Unit.Default);
            pageContainer.Pop(false);
        }).AddTo(this);
    }

    void OnDestroy()
    {
        Debug.Log("MainPager.OnDestroy");
    }
}
