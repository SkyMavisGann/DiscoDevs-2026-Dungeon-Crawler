using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SceneInstantiator : Singleton<SceneInstantiator>
{
    [Header("Parents")]
    public Transform EnviromentParent;
    public Transform SetupParent;
    public Transform CanvasParent;

    [Header("Objects")]
    public AssetReferenceGameObject RoomGeometry;
    public AssetReferenceGameObject Player;

    [HideInInspector]
    public GameObject RoomGeoInstance;
    [HideInInspector]
    public GameObject PlayerInstance;

    protected override void Awake()
    {
        base.Awake();

        if (RoomGeometry != null) RoomGeometry.InstantiateAsync(EnviromentParent).Completed += OnGeometryAddressableLoaded;
        if (Player != null) Player.InstantiateAsync(SetupParent).Completed += OnPlayerAddressableLoaded;


        //RoomGeometry.ReleaseInstance(RoomGeoInstance);
    }

    void OnGeometryAddressableLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            RoomGeoInstance = handle.Result;
        }
    }

    void OnPlayerAddressableLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            PlayerInstance = handle.Result;
        }
    }
}
