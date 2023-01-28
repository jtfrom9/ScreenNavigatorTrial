#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
using Cysharp.Threading.Tasks;

using UnityScreenNavigator.Runtime.Core.Page;
using UnityScreenNavigator.Runtime.Core.Sheet;

public class MainPage : Page
{
    [SerializeField]
    Button? backButton;

    [SerializeField]
    Button? showModelButton;

    [SerializeField]
    SheetContainer? sheetContainer;

    [SerializeField]
    Button? sheet1Button;
    [SerializeField]
    Button? sheet2Button;
    [SerializeField]
    Button? sheet3Button;

    void Start()
    {
        Debug.Log("MainPage.Start");

        if (backButton == null || showModelButton == null || sheetContainer == null || sheet1Button == null || sheet2Button==null || sheet3Button==null)
        {
            Debug.LogError("invalid");
            return;
        }
        setup(backButton, showModelButton);
        setupSheet(sheetContainer, sheet1Button, sheet2Button, sheet3Button).Forget();
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

        showModalButton.OnClickAsObservable().Subscribe(_ => {
            OnShowModal.OnNext(Unit.Default);

            sheetContainer?.Hide(false);
        }).AddTo(this);
    }

    async UniTask setupSheet(SheetContainer container, Button sheet1Button, Button sheet2Button , Button sheet3Button)
    {
        int sheet1 = 0;
        await container.Register("Sheet1", v => {
            Debug.Log($"registercb, sheet1 = {v.sheetId}");
            sheet1 = v.sheetId;
        });
        int sheet2 = 0;
        await container.Register("Sheet2", v =>
        {
            sheet2= v.sheetId;
        });
        int sheet3 = 0;
        await container.Register("Sheet3", v =>
        {
            sheet3 = v.sheetId;
        });

        sheet1Button.OnClickAsObservable().Subscribe(async _ =>
        {
            if (container.ActiveSheetId != sheet1)
            {
                await container.Show(sheet1, playAnimation: true).ToUniTask();
            }
        }).AddTo(this);
        sheet2Button.OnClickAsObservable().Subscribe(async _ =>
        {
            if (container.ActiveSheetId != sheet2)
            {
                await container.Show(sheet2, playAnimation: true).ToUniTask();
            }
        }).AddTo(this);
        sheet3Button.OnClickAsObservable().Subscribe(async _ =>
        {
            if (container.ActiveSheetId != sheet3)
            {
                await container.Show(sheet3, playAnimation: true).ToUniTask();
            }
        }).AddTo(this);

        await container.Show(sheet1, playAnimation: true).ToUniTask();
    }

    void OnDestroy()
    {
        Debug.Log("MainPager.OnDestroy");
    }
}
