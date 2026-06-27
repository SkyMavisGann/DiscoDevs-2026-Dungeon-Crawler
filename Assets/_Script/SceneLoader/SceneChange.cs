using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using TMPro;

using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine.UIElements;
using Unity.Collections.LowLevel.Unsafe;

public class SceneChange : MonoBehaviour
{
    public static SceneChange Instance { get; set; }
    public AssetReference SceneToLoad;
    public string DoorCode = "";
    public Transform DoorSpawnArea;
    public AsyncOperationHandle<SceneInstance> handle;
    public AsyncOperationHandle downloadDependencies;
    public float LoadPercent;
    public float loadTotal;

    public UnityEngine.UI.Slider sld_LoadingStatus;
    public TMP_Text txt_LoadingStatus;

    [SerializeField]
    private bool ActivateOnStart = false;

    private bool isLoading;

    private bool NeedsToTeleport = false;
    // Start is called before the first frame update
    void Start()
    {
        if (ActivateOnStart)
        {
            LoadScene();
        }
        if (DoorCode != "")
            MoveToDoor();
        

    }

    private void MoveToDoor()
    {
        if (DataHandlerPersistent.PersistentData == null)
        {
            Debug.LogError("To load a scene straight from assets you need to click on the dataHandlerPersistent scriptable object first to load it into memory, If done correctly you will see a debug message from its OnEnable. Otherwise load from preload scene.");
        }
        if (DataHandlerPersistent.PersistentData.CurrentDoorCode != "")
        {
            if (DataHandlerPersistent.PersistentData.CurrentDoorCode == DoorCode && !GameManager.Instance.Loading.activeSelf)
            {
                if (Player.Instance != null)
                {
                    Player.Instance.TeleportToPosition(DoorSpawnArea.position);
                    Player.Instance.RotatePlayer(DoorSpawnArea.transform.eulerAngles.y);
                } else
                {
                    NeedsToTeleport = true;
                } 
                DataHandlerPersistent.PersistentData.CurrentDoorCode = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (NeedsToTeleport && Player.Instance != null)
        {
            Player.Instance.TeleportToPosition(DoorSpawnArea.position);
            Player.Instance.RotatePlayer(DoorSpawnArea.transform.eulerAngles.y);
            NeedsToTeleport = false;
        }
        //Scene Load Handle (Fires After Dependencies)
        if (handle.IsValid())
        {
            LoadPercent = handle.GetDownloadStatus().Percent;
            if (sld_LoadingStatus != null)
            {
                sld_LoadingStatus.value = LoadPercent;
                txt_LoadingStatus.text = (LoadPercent * 100).ToString("F0") + "%";
            }
            
            Debug.Log("Download " + LoadPercent);

            if (handle.IsDone)
            {
                Addressables.Release(handle);
            }
        }

        //Dependecies Handle (Fires Before Scene Load)
        else if (downloadDependencies.IsValid())
        {
            LoadPercent = downloadDependencies.GetDownloadStatus().Percent;
            if (sld_LoadingStatus != null)
            {
                sld_LoadingStatus.value = LoadPercent;
                txt_LoadingStatus.text = (LoadPercent * 100).ToString("F0") + "%";
            }
                
            Debug.Log("Preload " + LoadPercent);

            if (downloadDependencies.IsDone)
            {
                Addressables.Release(downloadDependencies);
            }
        }
        

    }



    // Our Loading Process
    public async void LoadScene()
    {

        //Download our scene dependencies
        StartCoroutine(Preload());


        //Check if done, then release the handle
        if (!downloadDependencies.IsDone)
        {
            Debug.Log("Preload still occuring. Waiting for completion");
            await downloadDependencies.Task;
        }

        // Load the scene
        Debug.Log("Loading scene");
        if (DoorCode != "")
            DataHandlerPersistent.PersistentData.CurrentDoorCode = DoorCode;

        handle = Addressables.LoadSceneAsync(SceneToLoad, LoadSceneMode.Single);
        await handle.Task;
        FinishLoading();


    }
    void FinishLoading()
    {
        
    }

    // Download the dependencies for the scenes. This happens first before loading the scene
    public IEnumerator Preload()
    {
        Debug.Log("Starting Preload");
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(SceneToLoad);
        loadTotal = getDownloadSize.Result;
        Debug.Log("Package Size: " + (loadTotal / 1000000) + "mb"); // result returns the size in bytes.
        yield return getDownloadSize;

        if (getDownloadSize.Result > 0)
        {
            if (getDownloadSize.IsValid())
            {
                Addressables.Release(getDownloadSize);
            }

            downloadDependencies = Addressables.DownloadDependenciesAsync(SceneToLoad);
            yield return downloadDependencies;
        }

    }

    // Our function call to start loading the scene
    public void BeginSceneLoading()
    {
        Debug.Log("Start Scene Change");
        if (isLoading == false)
        {
            isLoading = true;

            //if (txt_LoadingStatus == null)
            //{
            //    txt_LoadingStatus = GameManager.Instance.Txt_LoadingStatus;
            //}

            //if (sld_LoadingStatus == null)
            //{
            //    sld_LoadingStatus = GameManager.Instance.Sld_LoadingStatus;
            //}

            GameManager.Instance.Loading.SetActive(true);
            //GameManager.Instance.Pnl_Dialogue.SetActive(false);
            //DialogueManager.Instance.DisplayStartDialoguePrompt(false);
            LoadScene();

        }
    }
}
