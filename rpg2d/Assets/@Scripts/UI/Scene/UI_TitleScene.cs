using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Define;

public class UI_TitleScene : UI_Base
{
    #region Enum
    private enum GameObjects
    {
        StartButton,
    }

    private enum Texts
    {
        StatusText,
        StartText,
    }

    #endregion

    #region DownloadState
    public enum EState
    {
        None = 0,
        CalculatingSize,
        NothingToDownload,
        AskingDownload,
        Downloading,
        DownloadFinished
    }

    Downloader _downloader;
    DownloadProgressStatus progressInfo;
    ESizeUnits _eSizeUnit;
    long curDownloadedSizeInUnit;
    long totalSizeInUnit;

    private EState _currentState = EState.None;

    public EState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            UpdateUI();
        }
    }
    #endregion

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));

        GetObject((int)GameObjects.StartButton).BindEvent(() =>
        {
            Debug.Log("OnClickStartButton");
        });

        GetObject((int)GameObjects.StartButton).gameObject.SetActive(false);

        _downloader = gameObject.GetOrAddComponent<Downloader>();
        _downloader.DownloadLabel = "Preload";
        return true;
    }

    IEnumerator Start()
    {

        yield return _downloader.StartDownload((events) =>
        {
            events.Initialized += OnInitialized;
            events.CatalogUpdated += OnCatalogUpdated;
            events.SizeDownloaded += OnSizeDownloaded;
            events.ProgressUpdated += OnProgress;
            events.Finished += OnFinished;
        });
    }

    private void UpdateUI()
    {
        switch(CurrentState)
        {
            case EState.Downloading:
                GetText((int)Texts.StatusText).text = $"다운로드 중...{progressInfo.totalProgress * 100.0f}%";
                break;
            case EState.DownloadFinished:
                GetObject((int)GameObjects.StartButton).gameObject.SetActive(true);
                GetText((int)Texts.StatusText).text = $"에셋 로드 시작...";

                Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
                {
                    GetText((int)Texts.StatusText).text = $"로딩중 : {key} {count}/{totalCount}";
                       
                    if (count == totalCount)
                    {
                        GetText((int)Texts.StatusText).text = $"로딩 완료";
                        GetObject((int)GameObjects.StartButton).gameObject.SetActive(true);
                        //GetObject((int)GameObjects.ArtTestSceneButton).SetActive(true);
                        //Managers.Data.Init();
                        //Managers.Game.Init();
                        Managers.Resource.Instantiate("Hero");
                        TestDOAnimation();
                    }
                });

                break;
        }
    }

    private void TestDOAnimation()
    {
        Sequence seq = DOTween.Sequence();

        Transform tf = GetText((int)Texts.StartText).rectTransform;
        tf.localScale = Vector3.one;

        seq.Append(tf.DOScale(1.5f, 0.8f))
            .Append(tf.DOScale(1.0f, 0.8f))
            .SetLoops(-1);
            
    }

    private void OnInitialized()
    {
        _downloader.GoNext();
    }

    private void OnCatalogUpdated()
    {
        _downloader.GoNext();
    }

    private void OnSizeDownloaded(long size)
    {
        Debug.Log($"다운로드 사이즈 다운로드 완료 ! : {Util.GetConvertedByteString(size, ESizeUnits.KB)} ({size}바이트)");

        if (size == 0)
        {
            CurrentState = EState.DownloadFinished;
        }
        else
        {
            _eSizeUnit = Util.GetProperByteUnit(size);
            totalSizeInUnit = Util.ConvertByteByUnit(size, _eSizeUnit);

            CurrentState = EState.AskingDownload;

            //TODO 일단 묻지않고 바로 다운로드
            CurrentState = EState.Downloading;
            _downloader.GoNext();
        }
    }
    private void OnProgress(DownloadProgressStatus newInfo)
    {
        bool changed = this.progressInfo.downloadedBytes != newInfo.downloadedBytes;

        progressInfo = newInfo;

        if (changed)
        {
            UpdateUI();

            curDownloadedSizeInUnit = Util.ConvertByteByUnit(newInfo.downloadedBytes, _eSizeUnit);
        }
    }

    private void OnFinished(bool isSuccess)
    {
        CurrentState = EState.DownloadFinished;
        _downloader.GoNext();
    }
}
