using UnityEngine;
using UnityEngine.Video;
using System.IO;
using System.Collections;

public class VideoLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoFileName; // Enter the video file name in the Inspector

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        StartCoroutine(PrepareAndPlayVideo());
    }

    IEnumerator PrepareAndPlayVideo()
    {
        string videoPath;

        #if UNITY_WEBGL
        videoPath = Application.streamingAssetsPath + "/" + videoFileName; // WebGL requires direct path
        #else
        videoPath = "file://" + Path.Combine(Application.streamingAssetsPath, videoFileName);
        #endif

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null; // Wait until the video is ready
        }

        videoPlayer.Play();
    }
}
