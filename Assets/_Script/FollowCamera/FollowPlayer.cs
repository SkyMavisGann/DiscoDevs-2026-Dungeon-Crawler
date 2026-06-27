
using System.Collections.Generic;

using UnityEngine;


public class FollowPlayer : MonoBehaviour
{
    public Camera cam;
    public Transform player;

    [Header("Follow Speeds")]
    public float FollowingMoveLerpSpeed = 5.0f;
    public float FollowingRotationLerpSpeed = 5.0f;
    public float FollowingScaleLerpSpeed = 5.0f;

    [Header("Transitional Speeds")]
    public float TransitioningMoveLerpSpeed = 5.0f;
    public float TransitioningRotationLerpSpeed = 5.0f;
    public float TransitioningScaleLerpSpeed = 5.0f;

    public Vector3 offset;

    private List<CameraFollowBox> followBoxes = new List<CameraFollowBox>();
    private List<CameraStationaryBox> stationaryBoxes = new List<CameraStationaryBox>();

    private Transform StationaryCameraTransform;
    private float stationaryCameraSize;

    private float originalCameraSize;

    private bool collectBoxesOnce = true;

    private bool JustLeftStationBox = false;
    private bool wasInStationaryBox = false;
    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
        originalCameraSize = cam.orthographicSize;
        if (offset == Vector3.zero && player != null)
        {
            offset = transform.position - player.transform.position;
        }
    }

    private void OnceAfterLoad()
    {
        followBoxes = new List<CameraFollowBox>(FindObjectsOfType<CameraFollowBox>());
        stationaryBoxes = new List<CameraStationaryBox>(FindObjectsOfType<CameraStationaryBox>());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (SceneInstantiator.Instance.RoomGeoInstance != null && collectBoxesOnce)
        {
            OnceAfterLoad();
            collectBoxesOnce = false;
        }

        //determine which box you are in
        bool inFollowBox = false;
        bool inStationaryBox = false;
        foreach (CameraFollowBox box in followBoxes)
        {
            if (box.PlayerInside)
            {
                inFollowBox = true;
            }
        }
        int highestPri = -1;
        foreach (CameraStationaryBox box in stationaryBoxes)
        {
            if ( box.PlayerInside)
            {
                inStationaryBox = true;

                if (box.priority > highestPri)
                {
                    StationaryCameraTransform = box.CameraPositionHere;
                    stationaryCameraSize = box.size;
                    highestPri = box.priority;
                }
                
            }
        }



        if (Player.Instance != null && Player.Instance.CurrentController != null)
        {
            player = Player.Instance.CurrentController.transform;

            //this behaviior is because you want to transition at a transition speed instead of a moving speed
            if (wasInStationaryBox && inFollowBox && !inStationaryBox)
            {
                JustLeftStationBox = true;
                wasInStationaryBox = false;
            }

            if (JustLeftStationBox && !inStationaryBox)
            {

                Vector3 DesiredPos = player.transform.position + offset;
                transform.position = Vector3.Lerp(transform.position, DesiredPos, TransitioningMoveLerpSpeed * Time.deltaTime);

                Quaternion desiredRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, TransitioningRotationLerpSpeed * Time.deltaTime);

                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, originalCameraSize, TransitioningScaleLerpSpeed * Time.deltaTime);

                float dist = Mathf.Abs(Vector3.Distance(transform.position, DesiredPos));

                //this causes that little lock in the camera, its when its really close so then it needs to go to follow behavior box;
                if ((dist < 0.1f) && Mathf.Abs(cam.orthographicSize - originalCameraSize) < 0.1f)
                {
                    JustLeftStationBox = false;
                }
            } 
            else if (inStationaryBox)
            {
                //these are lerpings to the correct position, heres in when you enter a stay box,
                Vector3 DesiredPos = StationaryCameraTransform.position;
                transform.position = Vector3.Lerp(transform.position, DesiredPos, TransitioningMoveLerpSpeed * Time.deltaTime);

                Quaternion desiredRotation = StationaryCameraTransform.rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, TransitioningRotationLerpSpeed * Time.deltaTime);

                float desiredSize = stationaryCameraSize;
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredSize, TransitioningScaleLerpSpeed * Time.deltaTime);
                wasInStationaryBox = true;
            } 
            else if (inFollowBox)
            {
                //this behavior only runs when your detransition from the stay box is done and you're in a ofllow box

                Vector3 DesiredPos = player.transform.position + offset;
                transform.position = Vector3.Lerp(transform.position, DesiredPos, FollowingMoveLerpSpeed * Time.deltaTime);

                Quaternion desiredRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, FollowingRotationLerpSpeed * Time.deltaTime);

                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, originalCameraSize, FollowingScaleLerpSpeed * Time.deltaTime);

                
            }
        }
    }
}
