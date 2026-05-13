using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;

public static class ApiService
{
    public static string BASE_URL =
        "https://localhost:7191/";

    // =========================
    // GET
    // =========================
    public static IEnumerator Get(
        string endpoint,
        Action<string> onSuccess,
        Action<string> onError = null)
    {
        UnityWebRequest request =
            UnityWebRequest.Get(
                BASE_URL + endpoint
            );
        yield return request.SendWebRequest();
        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke(
                request.downloadHandler.text
            );
        }
        else
        {
            onError?.Invoke(request.error);
            Debug.LogError(request.error);
        }
    }

    // =========================
    // POST
    // =========================
    public static IEnumerator Post(
        string endpoint,
        string json,
        Action<string> onSuccess = null,
        Action<string> onError = null)
    {
        UnityWebRequest request =
            new UnityWebRequest(
                BASE_URL + endpoint,
                "POST"
            );
        byte[] bodyRaw =
            Encoding.UTF8.GetBytes(json);
        request.uploadHandler =
            new UploadHandlerRaw(bodyRaw);
        request.downloadHandler =
            new DownloadHandlerBuffer();
        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );
        yield return request.SendWebRequest();
        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke(
                request.downloadHandler.text
            );
        }
        else
        {
            onError?.Invoke(request.error);
            Debug.LogError(request.error);
        }
    }

    // =========================
    // PUT
    // =========================
    public static IEnumerator Put(
        string endpoint,
        string json,
        Action<string> onSuccess = null,
        Action<string> onError = null)
    {
        UnityWebRequest request =
            new UnityWebRequest(
                BASE_URL + endpoint,
                "PUT"
            );
        byte[] bodyRaw =
            Encoding.UTF8.GetBytes(json);
        request.uploadHandler =
            new UploadHandlerRaw(bodyRaw);
        request.downloadHandler =
            new DownloadHandlerBuffer();
        request.SetRequestHeader(
            "Content-Type",
            "application/json"
        );
        yield return request.SendWebRequest();
        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            Debug.Log("Request Body: " + json);
            Debug.Log("Response: " + request.downloadHandler.text); 
            onSuccess?.Invoke(
                request.downloadHandler.text
            );
        }
        else
        {
            onError?.Invoke(request.error);
            Debug.LogError(request.error);
        }
    }

    // =========================
    // DELETE
    // =========================
    public static IEnumerator Delete(
        string endpoint,
        Action onSuccess = null,
        Action<string> onError = null)
    {
        UnityWebRequest request =
            UnityWebRequest.Delete(
                BASE_URL + endpoint
            );
        request.downloadHandler =
            new DownloadHandlerBuffer();
        yield return request.SendWebRequest();
        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();
        }
        else
        {
            onError?.Invoke(request.error);
            Debug.LogError(request.error);
        }
    }
}