#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
using UnityScreenNavigator.Runtime.Core.Page;

public class TopPage : Page
{
    [SerializeField] Button? button;

    void Start()
    {
        Debug.Log("TopPage.Start");
        if (button == null)
        {
            Debug.LogError("Invalid");
            return;
        }
        Setup(button);
    }

    public ISubject<Unit> OnClick = new Subject<Unit>();

    void Setup(Button button)
    {
        button.OnClickAsObservable().Subscribe(_ =>
        {
            OnClick.OnNext(Unit.Default);
        }).AddTo(this);
    }
}
