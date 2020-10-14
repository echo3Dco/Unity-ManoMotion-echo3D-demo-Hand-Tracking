using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#if !UniRxLibrary
using ObservableUnity = UniRx.Observable;
#endif

namespace UniRx
{
    using System.Threading;
#if !(UNITY_METRO || UNITY_WP8) && (UNITY_4_4 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
    // Fallback for Unity versions below 4.5
    using Hash = System.Collections.Hashtable;
    using HashEntry = System.Collections.DictionaryEntry;    
#else
    // Unity 4.5 release notes: 
    // WWW: deprecated 'WWW(string url, byte[] postData, Hashtable headers)', 
    // use 'public WWW(string url, byte[] postData, Dictionary<string, string> headers)' instead.
    using Hash = System.Collections.Generic.Dictionary<string, string>;
    using HashEntry = System.Collections.Generic.KeyValuePair<string, string>;
#endif

    public static partial class ObservableWWW
    {
        public static IObservable<string> Get(string url, Hash headers = null, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<string>((observer, cancellation) => FetchText(UnityWebRequest.Get(url), observer, progress, cancellation));
        }

        public static IObservable<byte[]> GetAndGetBytes(string url, Hash headers = null, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<byte[]>((observer, cancellation) => FetchBytes(UnityWebRequest.Get(url), observer, progress, cancellation));
        }
        public static IObservable<UnityWebRequest> GetWWW(string url, Hash headers = null, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => Fetch(UnityWebRequest.Get(url), observer, progress, cancellation));
        }

        public static IObservable<string> Post(string url, byte[] postData, IProgress<float> progress = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(postData);
            MyUploadHandler.contentType = "application/x-www-form-urlencoded";
            uwr.uploadHandler = MyUploadHandler;
            return ObservableUnity.FromCoroutine<string>((observer, cancellation) => FetchText(uwr, observer, progress, cancellation));
        }

        public static IObservable<string> Post(string url, byte[] postData, Hash headers, IProgress<float> progress = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(postData);
            MyUploadHandler.contentType = "application/x-www-form-urlencoded";
            uwr.uploadHandler = MyUploadHandler;
            foreach (KeyValuePair<string,string> header in headers) uwr.SetRequestHeader(header.Key, header.Value);
            return ObservableUnity.FromCoroutine<string>((observer, cancellation) => FetchText(uwr, observer, progress, cancellation));
        }

        public static IObservable<string> Post(string url, WWWForm content, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<string>((observer, cancellation) => FetchText(UnityWebRequest.Post(url, content), observer, progress, cancellation));
        }

        public static IObservable<string> Post(string url, WWWForm content, Hash headers, IProgress<float> progress = null)
        {
            var contentHeaders = content.headers;
            UnityWebRequest uwr = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(content.data);
            MyUploadHandler.contentType = "application/x-www-form-urlencoded";
            uwr.uploadHandler = MyUploadHandler;
            foreach (KeyValuePair<string,string> header in MergeHash(contentHeaders, headers)) uwr.SetRequestHeader(header.Key, header.Value);
            return ObservableUnity.FromCoroutine<string>((observer, cancellation) => FetchText(uwr, observer, progress, cancellation));
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, byte[] postData, IProgress<float> progress = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(postData);
            MyUploadHandler.contentType = "application/x-www-form-urlencoded";
            uwr.uploadHandler = MyUploadHandler;
            return ObservableUnity.FromCoroutine<byte[]>((observer, cancellation) => FetchBytes(uwr, observer, progress, cancellation));
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, byte[] postData, Hash headers, IProgress<float> progress = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(postData);
            MyUploadHandler.contentType = "application/x-www-form-urlencoded";
            uwr.uploadHandler = MyUploadHandler;
            foreach (KeyValuePair<string,string> header in headers) uwr.SetRequestHeader(header.Key, header.Value);
            return ObservableUnity.FromCoroutine<byte[]>((observer, cancellation) => FetchBytes(uwr, observer, progress, cancellation));
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, WWWForm content, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<byte[]>((observer, cancellation) => FetchBytes(UnityWebRequest.Post(url, content), observer, progress, cancellation));
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, WWWForm content, Hash headers, IProgress<float> progress = null)
        {
            var contentHeaders = content.headers;
            UnityWebRequest uwr = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(content.data);
            MyUploadHandler.contentType = "application/x-www-form-urlencoded";
            uwr.uploadHandler = MyUploadHandler;
            foreach (KeyValuePair<string,string> header in MergeHash(contentHeaders, headers)) uwr.SetRequestHeader(header.Key, header.Value);
            return ObservableUnity.FromCoroutine<byte[]>((observer, cancellation) => FetchBytes(uwr, observer, progress, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, byte[] postData, IProgress<float> progress = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(postData);
            MyUploadHandler.contentType = "application/x-www-form-urlencoded";
            uwr.uploadHandler = MyUploadHandler;
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => Fetch(uwr, observer, progress, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, byte[] postData, Hash headers, IProgress<float> progress = null)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(postData);
            MyUploadHandler.contentType = "application/x-www-form-urlencoded";
            uwr.uploadHandler = MyUploadHandler;
            foreach (KeyValuePair<string,string> header in headers) uwr.SetRequestHeader(header.Key, header.Value);
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => Fetch(uwr, observer, progress, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, WWWForm content, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => Fetch(UnityWebRequest.Post(url, content), observer, progress, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, WWWForm content, Hash headers, IProgress<float> progress = null)
        {
            var contentHeaders = content.headers;
            UnityWebRequest uwr = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw MyUploadHandler = new UploadHandlerRaw(content.data);
            MyUploadHandler.contentType = "application/x-www-form-urlencoded";
            uwr.uploadHandler = MyUploadHandler;
            foreach (KeyValuePair<string,string> header in MergeHash(contentHeaders, headers)) uwr.SetRequestHeader(header.Key, header.Value);
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => Fetch(uwr, observer, progress, cancellation));
        }

        public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, int version, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<AssetBundle>((observer, cancellation) => FetchAssetBundle(UnityWebRequestAssetBundle.GetAssetBundle(url, (uint)version, 0), observer, progress, cancellation));
        }

        public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, int version, uint crc, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<AssetBundle>((observer, cancellation) => FetchAssetBundle(UnityWebRequestAssetBundle.GetAssetBundle(url, (uint)version, crc), observer, progress, cancellation));
        }

        // over Unity5 supports Hash128
#if !(UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_4 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
        public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, Hash128 hash128, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<AssetBundle>((observer, cancellation) => FetchAssetBundle(UnityWebRequestAssetBundle.GetAssetBundle(url, hash128, 0), observer, progress, cancellation));
        }

        public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, Hash128 hash128, uint crc, IProgress<float> progress = null)
        {
            return ObservableUnity.FromCoroutine<AssetBundle>((observer, cancellation) => FetchAssetBundle(UnityWebRequestAssetBundle.GetAssetBundle(url, hash128, crc), observer, progress, cancellation));
        }
#endif

        // over 4.5, Hash define is Dictionary.
        // below Unity 4.5, WWW only supports Hashtable.
        // Unity 4.5, 4.6 WWW supports Dictionary and [Obsolete]Hashtable but WWWForm.content is Hashtable.
        // Unity 5.0 WWW only supports Dictionary and WWWForm.content is also Dictionary.
#if !(UNITY_METRO || UNITY_WP8) && (UNITY_4_5 || UNITY_4_6 || UNITY_4_7)
        static Hash MergeHash(Hashtable wwwFormHeaders, Hash externalHeaders)
        {
            var newHeaders = new Hash();
            foreach (DictionaryEntry item in wwwFormHeaders)
            {
                newHeaders[item.Key.ToString()] = item.Value.ToString();
            }
            foreach (HashEntry item in externalHeaders)
            {
                newHeaders[item.Key] =  item.Value;
            }
            return newHeaders;
        }
#else
        static Hash MergeHash(Hash wwwFormHeaders, Hash externalHeaders)
        {
            foreach (HashEntry item in externalHeaders)
            {
                wwwFormHeaders[item.Key] = item.Value;
            }
            return wwwFormHeaders;
        }
#endif

        static IEnumerator Fetch(UnityWebRequest www, IObserver<UnityWebRequest> observer, IProgress<float> reportProgress, CancellationToken cancel)
        {
            using (www)
            {
                yield return www.SendWebRequest();
                if (reportProgress != null)
                {
                    while (!www.isDone && !cancel.IsCancellationRequested)
                    {
                        try
                        {
                            reportProgress.Report(www.downloadProgress);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            yield break;
                        }
                        yield return null;
                    }
                }
                else
                {
                    if (!www.isDone)
                    {
                        yield return www;
                    }
                }

                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (reportProgress != null)
                {
                    try
                    {
                        reportProgress.Report(www.downloadProgress);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        yield break;
                    }
                }

                if (!string.IsNullOrEmpty(www.error))
                {
                    observer.OnError(new WWWErrorException(www, www.downloadHandler.text));
                }
                else
                {
                    observer.OnNext(www);
                    observer.OnCompleted();
                }
            }
        }

        static IEnumerator FetchText(UnityWebRequest www, IObserver<string> observer, IProgress<float> reportProgress, CancellationToken cancel)
        {
            using (www)
            {
                yield return www.SendWebRequest();

                if (reportProgress != null)
                {
                    while (!www.isDone && !cancel.IsCancellationRequested)
                    {
                        try
                        {
                            reportProgress.Report(www.downloadProgress);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            yield break;
                        }
                        yield return null;
                    }
                }
                else
                {
                    if (!www.isDone)
                    {
                        yield return www;
                    }
                }

                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (reportProgress != null)
                {
                    try
                    {
                        reportProgress.Report(www.downloadProgress);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        yield break;
                    }
                }

                if (!string.IsNullOrEmpty(www.error))
                {
                    observer.OnError(new WWWErrorException(www, www.downloadHandler.text));
                }
                else
                {
                    observer.OnNext(www.downloadHandler.text);
                    observer.OnCompleted();
                }
            }
        }

        static IEnumerator FetchBytes(UnityWebRequest www, IObserver<byte[]> observer, IProgress<float> reportProgress, CancellationToken cancel)
        {
            using (www)
            {
                yield return www.SendWebRequest();

                if (reportProgress != null)
                {
                    while (!www.isDone && !cancel.IsCancellationRequested)
                    {
                        try
                        {
                            reportProgress.Report(www.downloadProgress);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            yield break;
                        }
                        yield return null;
                    }
                }
                else
                {
                    if (!www.isDone)
                    {
                        yield return www;
                    }
                }

                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (reportProgress != null)
                {
                    try
                    {
                        reportProgress.Report(www.downloadProgress);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        yield break;
                    }
                }

                if (!string.IsNullOrEmpty(www.error))
                {
                    observer.OnError(new WWWErrorException(www, www.downloadHandler.text));
                }
                else
                {
                    observer.OnNext(www.downloadHandler.data);
                    observer.OnCompleted();
                }
            }
        }

        static IEnumerator FetchAssetBundle(UnityWebRequest www, IObserver<AssetBundle> observer, IProgress<float> reportProgress, CancellationToken cancel)
        {
            using (www)
            {
                yield return www.SendWebRequest();
                
                if (reportProgress != null)
                {
                    while (!www.isDone && !cancel.IsCancellationRequested)
                    {
                        try
                        {
                            reportProgress.Report(www.downloadProgress);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            yield break;
                        }
                        yield return null;
                    }
                }
                else
                {
                    if (!www.isDone)
                    {
                        yield return www;
                    }
                }

                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (reportProgress != null)
                {
                    try
                    {
                        reportProgress.Report(www.downloadProgress);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        yield break;
                    }
                }

                if (!string.IsNullOrEmpty(www.error))
                {
                    observer.OnError(new WWWErrorException(www, ""));
                }
                else
                {
                    observer.OnNext(DownloadHandlerAssetBundle.GetContent(www));
                    observer.OnCompleted();
                }
            }
        }
    }

    public class WWWErrorException : Exception
    {
        public string RawErrorMessage { get; private set; }
        public bool HasResponse { get; private set; }
        public string Text { get; private set; }
        public System.Net.HttpStatusCode StatusCode { get; private set; }
        public System.Collections.Generic.Dictionary<string, string> ResponseHeaders { get; private set; }
        public UnityWebRequest WWW { get; private set; }

        // cache the text because if www was disposed, can't access it.
        public WWWErrorException(UnityWebRequest www, string text)
        {
            this.WWW = www;
            this.RawErrorMessage = www.error;
            this.ResponseHeaders = www.GetResponseHeaders();
            this.HasResponse = false;
            this.Text = text; 

            var splitted = RawErrorMessage.Split(' ', ':');
            if (splitted.Length != 0)
            {
                int statusCode;
                if (int.TryParse(splitted[0], out statusCode))
                {
                    this.HasResponse = true;
                    this.StatusCode = (System.Net.HttpStatusCode)statusCode;
                }
            }
        }

        public override string ToString()
        {
            var text = this.Text;
            if (string.IsNullOrEmpty(text))
            {
                return RawErrorMessage;
            }
            else
            {
                return RawErrorMessage + " " + text;
            }
        }
    }
}