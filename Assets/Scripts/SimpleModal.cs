#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
using UnityScreenNavigator.Runtime.Core.Modal;

public class SimpleModal : Modal
{
    [SerializeField]
    Button? button;

    void Start()
    {
        Debug.Log("SimpleModal.Start");
        if(button==null) {
            Debug.Log("Invalid");
            return;
        }
        button.OnClickAsObservable().Subscribe(async _ => {
            await ModalContainer.Of(transform).Pop(playAnimation: true).Task;
        }).AddTo(this);
    }
}
