/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at 	      *
* https://www.echoar.xyz/terms, or another agreement      	              *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using SimpleJSON;
using System.IO;
using System.Collections.Generic;
using AsImpL;
using Siccity.GLTFUtility;
using System.Globalization;
using UnityEngine.Video;

public class echoAR : MonoBehaviour
{

    // Your echoAR API key
    public string APIKey = "<YOUR_API_KEY>";
    private string serverURL;

    // echoAR Database
    static public Database dbObject;

    void Start()
    {
        // Debug logs control
        #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
        #else
            Debug.unityLogger.logEnabled = false;
        #endif
        
        // The echoAR server details
        serverURL = "https://console.echoar.xyz/query?key=" + APIKey;

        // Run the database query subroutine followed by assets download subroutine
        try
        {
            // Query database for all the entires
            StartCoroutine(QueryDatabase(serverURL));
            // What to query a single entry? Replace the above line with:
            // StartCoroutine(QueryDatabase(serverURL + "&entry=<ENTRY_ID>"));
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

    }

    IEnumerator QueryDatabase(string serverURL)
    {
        // Create a new request
        UnityWebRequest www = UnityWebRequest.Get(serverURL);

        Debug.Log("Querying database...");

        // Yield for the request
        yield return www.SendWebRequest();

        // Wait for the request to finish
        while (!www.isDone)
        {
            yield return null;
        }

        string json = "not found";
        // Check for errors
        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Parse response
            json = www.downloadHandler.text;

            // Handle repsonse
            Debug.Log("Query Response:");
            print(json);

            // Parse Database
            ParseDatabase(json);
        }

        // Cleanup
        www.disposeDownloadHandlerOnDispose = true;
        www.disposeUploadHandlerOnDispose = true;
        www.Dispose();
        www = null;

        // Check for valid response
        if (!json.ToLower().Contains("not found"))
        {
            // Start Websocket client
            StartCoroutine(WebsocketClient());
            // Download Assets
            StartCoroutine(DownloadAssets(serverURL));
        }
    }

    public void ParseDatabase(string json)
    {
        // Parse database
        if (!json.ToLower().Contains("not found"))
        {
            Debug.Log("Parsing database...");
            // Parse JSON
            var parsedJSON = JSON.Parse(json);
            // Create a Database object with and API key
            dbObject = new Database(parsedJSON["apiKey"].Value);
            // Get entries
            int i = 0;
            var entry = parsedJSON["db"][i];
            while (entry != null)
            {
                // Parse Entry
                ParseEntry(entry);
                // Continue to next entry
                entry = parsedJSON["db"][++i];
            }
            Debug.Log("Database parsed.");
        }
    }

    public Entry ParseEntry(JSONNode entry)
    {
        // Create entry
        Entry entryObject = new Entry();
        entryObject.setId(entry["id"]);

        // Create target
        Target targetObject;
        var target = entry["target"];
        string targetType = target["type"];
        switch (targetType)
        {
            case "IMAGE_TARGET":
                ImageTarget imageTargetObject = new ImageTarget();
                imageTargetObject.setFilename(target["filename"]);
                imageTargetObject.setStorageID(target["storageID"]);
                imageTargetObject.setId(target["id"]);
                imageTargetObject.setType(Target.targetType.IMAGE_TARGET);
                targetObject = imageTargetObject;
                break;
            case "GEOLOCATION_TARGET":
                GeolocationTarget geolocationTargetObject = new GeolocationTarget();
                geolocationTargetObject.setCity(target["city"]);
                geolocationTargetObject.setContinent(target["continent"]);
                geolocationTargetObject.setCountry(target["country"]);
                geolocationTargetObject.setId(target["id"]);
                geolocationTargetObject.setLatitude(target["latitude"]);
                geolocationTargetObject.setLongitude(target["longitude"]);
                geolocationTargetObject.setPlace(target["place"]);
                geolocationTargetObject.setType(Target.targetType.GEOLOCATION_TARGET);
                targetObject = geolocationTargetObject;
                break;
            case "BRICK_TARGET":
                BrickTarget brickTargetObject = new BrickTarget();
                brickTargetObject.setId(target["id"]);
                brickTargetObject.setType(Target.targetType.BRICK_TARGET);
                targetObject = brickTargetObject;
                break;
            default:
                targetObject = new Target();
                break;
        }
        List<string> hologramsListObject = new List<string>();
        int j = 0;
        var hologramID = target["holograms"][j];
        while (hologramID != null)
        {
            hologramsListObject.Add(hologramID);
            hologramID = target["holograms"][++j];
        }
        targetObject.setHolograms(hologramsListObject);
        entryObject.setTarget(targetObject);

        // Create Hologram
        Hologram hologramObject;
        var hologram = entry["hologram"];
        string hologramType = hologram["type"];
        switch (hologramType)
        {
            case "IMAGE_HOLOGRAM":
                ImageHologram imageHologramObject = new ImageHologram();
                imageHologramObject.setFilename(hologram["filename"]);
                imageHologramObject.setId(hologram["id"]);
                imageHologramObject.setStorageID(hologram["storageID"]);
                imageHologramObject.setTargetID(hologram["targetID"]);
                imageHologramObject.setType(Hologram.hologramType.IMAGE_HOLOGRAM);
                imageHologramObject.setTarget(targetObject);
                hologramObject = imageHologramObject;
                break;
            case "VIDEO_HOLOGRAM":
                VideoHologram videoHologramObject = new VideoHologram();
                videoHologramObject.setFilename(hologram["filename"]);
                videoHologramObject.setId(hologram["id"]);
                videoHologramObject.setStorageID(hologram["storageID"]);
                videoHologramObject.setTargetID(hologram["targetID"]);
                videoHologramObject.setType(Hologram.hologramType.VIDEO_HOLOGRAM);
                videoHologramObject.setTarget(targetObject);
                hologramObject = videoHologramObject;
                break;
            case "ECHO_HOLOGRAM":
                EchoHologram echoHologramObject = new EchoHologram();
                echoHologramObject.setFilename(hologram["filename"]);
                echoHologramObject.setId(hologram["id"]);
                echoHologramObject.setEncodedEcho(hologram["encodedEcho"]);
                echoHologramObject.setTextureFilename(hologram["textureFilename"]);
                echoHologramObject.setTargetID(hologram["targetID"]);
                echoHologramObject.setType(Hologram.hologramType.ECHO_HOLOGRAM);
                echoHologramObject.setTarget(targetObject);
                List<string> videosListObject = new List<string>();

                j = 0;
                var videoID = hologram["vidoes"][j];
                while (videoID != null)
                {
                    videosListObject.Add(videoID);
                    hologramID = hologram["vidoes"][++j];
                }
                echoHologramObject.setVidoes(videosListObject);

                hologramObject = echoHologramObject;
                break;
            case "MODEL_HOLOGRAM":
                ModelHologram modelHologramObject = new ModelHologram();
                modelHologramObject.setEncodedFile(hologram["encodedFile"]);
                modelHologramObject.setFilename(hologram["filename"]);
                modelHologramObject.setId(hologram["id"]);
                modelHologramObject.setMaterialFilename(hologram["materialFilename"]);
                modelHologramObject.setMaterialStorageID(hologram["materialStorageID"]);
                modelHologramObject.setStorageID(hologram["storageID"]);
                modelHologramObject.setTargetID(hologram["targetID"]);
                var textureFilenames = hologram["textureFilenames"].AsArray;
                var textureStorageIDs = hologram["textureStorageIDs"].AsArray;
                for (j = 0; j < textureFilenames.Count; j++)
                {
                    modelHologramObject.addTexture(textureFilenames[j], textureStorageIDs[j]);
                }
                modelHologramObject.setType(Hologram.hologramType.MODEL_HOLOGRAM);
                modelHologramObject.setTarget(targetObject);
                // If applicable, update model hologram with .glb version
                if (entry["additionalData"]["glbHologramStorageID"] != null) {
                    modelHologramObject.setFilename(entry["additionalData"]["glbHologramStorageFilename"]);
                    modelHologramObject.setStorageID(entry["additionalData"]["glbHologramStorageID"]);
                }
                hologramObject = modelHologramObject;
                break;
            default:
                hologramObject = new Hologram();
                break;
        }
        entryObject.setHologram(hologramObject);

        // Create SDKs array
        bool[] sdksObject = new bool[9];
        var sdks = entry["sdks"].AsArray;
        for (j = 0; j < 9; j++)
        {
            sdksObject[j] = sdks[j];
        }
        entryObject.setSupportedSDKs(sdksObject);

        // Create Additional Data
        var additionalData = entry["additionalData"];
        foreach (var data in additionalData)
        {
            entryObject.addAdditionalData(data.Key, data.Value);
        }

        // Add entry to database
        dbObject.addEntry(entryObject);

        // Return
        return entryObject;
    }

    IEnumerator DownloadAssets(string serverURL)
    {
        Debug.Log("Downloading assets...");

        // Iterate over all database entries
        foreach (Entry entry in dbObject.getEntries())
        {
            DownloadEntryAssets(entry, serverURL);

        }
        Debug.Log("Assets downloaded.");
        yield return null;
    }

    public void DownloadEntryAssets(Entry entry, string serverURL)
    {
        // Check if Unity is supported
        //if (entry.getSupportedSDKs()[Entry.SDKs.UNITY.ordinal()])
        if (true)
        {
            // Get hologram type
            Hologram.hologramType hologramType = entry.getHologram().getType();
            // Handle model hologram
            if (hologramType.Equals(Hologram.hologramType.MODEL_HOLOGRAM))
            {
                // Get model names and ID
                ModelHologram modelHologram = (ModelHologram) entry.getHologram();

                List<string> filenames = new List<string>();
                filenames.Add(modelHologram.getFilename());
                if (modelHologram.getMaterialFilename() != null) filenames.Add(modelHologram.getMaterialFilename());
                if (modelHologram.getTextureFilenames() != null) filenames.AddRange(modelHologram.getTextureFilenames());

                List<string> fileStorageIDs = new List<string>();
                fileStorageIDs.Add(modelHologram.getStorageID());
                if (modelHologram.getMaterialStorageID() != null) fileStorageIDs.Add(modelHologram.getMaterialStorageID());
                if (modelHologram.getTextureStorageIDs() != null) fileStorageIDs.AddRange(modelHologram.getTextureStorageIDs());

                // Import Options
                ImportOptions importOptions = ParseAdditionalData(entry.getAdditionalData());

                // Instantiate model based on type
                if (modelHologram.getFilename().EndsWith(".glb")){
                    // Instantiate model without downloading it
                    StartCoroutine(InstantiateModel(entry, filenames, importOptions));
                } else {
                    // Download model files and then instantiate
                    StartCoroutine(DownloadFiles(entry, serverURL, filenames, fileStorageIDs, importOptions));
                }
            // Handle video hologram
            } else if (hologramType.Equals(Hologram.hologramType.VIDEO_HOLOGRAM)) {

                // Get video
                VideoHologram videoHologram = (VideoHologram) entry.getHologram();

                // Create primitive plane for the video to appear on
                GameObject videoPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);

                // Set video plane position
                videoPlane.transform.parent = this.gameObject.transform;
                videoPlane.transform.position = this.gameObject.transform.position;

                // Set video plane size
                string value = "";
                float height = (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("height", out value)) ? float.Parse(value) * 0.01f : 1;
                float width = (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("width", out value)) ? float.Parse(value) * 0.01f : 2;

                // Scale video plane
                videoPlane.transform.localScale = new Vector3(width, height, height);

                // Attach VideoPlayer to video plane
                var videoPlayer = videoPlane.AddComponent<VideoPlayer>();
                videoPlayer.playOnAwake = false;                

                // Attach a CustomBehaviour Component
                videoPlane.AddComponent<CustomBehaviour>().entry = entry;

                // Set gameobject name to video name
                videoPlane.name = videoHologram.getFilename();

                // Set video URL
                videoPlayer.url = serverURL + "&file=" + videoHologram.getStorageID();
                
                // Play video
                videoPlayer.Play();

                // Mute
                if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("mute", out value) && value.Equals("true")) {
                    for (ushort i = 0; i < videoPlayer.controlledAudioTrackCount; ++i)
                        videoPlayer.SetDirectAudioMute(i, true);
                }
            // Handle image hologram
            } else if (hologramType.Equals(Hologram.hologramType.IMAGE_HOLOGRAM)) {

                // Get image
                ImageHologram imageHologram = (ImageHologram) entry.getHologram();

                // Create primitive plane for the image to appear on
                GameObject imagePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);

                // Set image plane position
                imagePlane.transform.parent = this.gameObject.transform;
                imagePlane.transform.position = this.gameObject.transform.position;

                // Set image plane size
                string value = "";
                float height = (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("height", out value)) ? float.Parse(value) * 0.01f : 1;
                float width = (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("width", out value)) ? float.Parse(value) * 0.01f : 1;

                // Scale image plane
                imagePlane.transform.localScale = new Vector3(width, height, height);

                // Set gameobject name to image name
                imagePlane.name = imageHologram.getFilename();

                // Set image URL
                string imageURL = (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("compressedImageStorageID", out value)) ? 
                    serverURL + "&file=" + value :
                    serverURL + "&file=" + imageHologram.getStorageID();

                // Download image file and then instantiate
                StartCoroutine(DownloadandInitiateImage(entry, imageURL, imagePlane));
            }
        }
    }

    ImportOptions ParseAdditionalData(Dictionary<string, string> additionalData)
    {
        ImportOptions importOptions = new ImportOptions();
        // Set position
        string x, y, z;
        float xFloat = 0, yFloat = 0, zFloat = 0;
        if (additionalData.TryGetValue("x", out x)) xFloat = float.Parse(x, CultureInfo.InvariantCulture);
        if (additionalData.TryGetValue("y", out y)) yFloat = float.Parse(y, CultureInfo.InvariantCulture);
        if (additionalData.TryGetValue("z", out z)) zFloat = float.Parse(z, CultureInfo.InvariantCulture);
        importOptions.localPosition = new Vector3(xFloat, yFloat, zFloat);
        // Set scale
        string scale;
        float scaleFloat = 1;
        if (additionalData.TryGetValue("scale", out scale)) scaleFloat = float.Parse(scale, CultureInfo.InvariantCulture);
        importOptions.localScale = Vector3.one * scaleFloat;
        // Set rotation
        string xAngle, yAngle, zAngle;
        float xAngleFloat = 0, yAngleFloat = 0, zAngleFloat = 0;
        if (additionalData.TryGetValue("xAngle", out xAngle)) xAngleFloat = float.Parse(xAngle, CultureInfo.InvariantCulture);
        if (additionalData.TryGetValue("yAngle", out yAngle)) yAngleFloat = float.Parse(yAngle, CultureInfo.InvariantCulture);
        if (additionalData.TryGetValue("zAngle", out zAngle)) zAngleFloat = float.Parse(zAngle, CultureInfo.InvariantCulture);
        importOptions.localEulerAngles = new Vector3(90 + xAngleFloat, 180 + yAngleFloat, 0 + zAngleFloat);
        // Return
        return importOptions;
    }

    IEnumerator DownloadandInitiateImage(Entry entry, string imageURL, GameObject imagePlane)
    {
        // Get texture
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();
        // Get renderer
        MeshRenderer meshRenderer = imagePlane.GetComponent<MeshRenderer>();
        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            meshRenderer.material.color = Color.white;
        } else {
            // Set texture
            Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;            
            // Set image as texture
            meshRenderer.material.color = Color.white;
            meshRenderer.material.mainTexture = texture;
        }
        // Attach a CustomBehaviour Component
        imagePlane.AddComponent<CustomBehaviour>().entry = entry;
    }

    IEnumerator DownloadFiles(Entry entry, string serverURL, List<string> filenames, List<string> fileStorageIDs, ImportOptions importOptions)
    {

        for (int i = 0; i < filenames.Count; ++i)
        {
            // Check for invalid files
            if (string.IsNullOrEmpty(filenames[i]) || string.IsNullOrEmpty(fileStorageIDs[i])) continue;

            // Create a new request
            UnityWebRequest www = UnityWebRequest.Get(serverURL + "&file=" + fileStorageIDs[i]);

            Debug.Log("Downloading file " + filenames[i] + " (" + fileStorageIDs[i] + ")...");

            // Yield for the request
            yield return www.SendWebRequest();

            // Wait for the request to finish
            while (!www.isDone)
            {
                yield return null;
            }

            // Check for errors
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Parse response
                byte[] bytes = www.downloadHandler.data;

                // Handle repsonse
                System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + filenames[i], bytes);

                Debug.Log("File " + filenames[i] + " (" + fileStorageIDs[i] + ") downloaded and stored in " + Application.persistentDataPath);
            }

            // Cleanup
            www.disposeDownloadHandlerOnDispose = true;
            www.disposeUploadHandlerOnDispose = true;
            www.Dispose();
            www = null;
        }

        // Instantiate model
        StartCoroutine(InstantiateModel(entry, filenames, importOptions));
        yield return null;
    }

    IEnumerator InstantiateModel(Entry entry, List<string> filenames, ImportOptions importOptions)
    {
        Debug.Log("Instantiating model " + filenames[0]);

        // Refresh assets in editor
        #if UNITY_EDITOR
                if (Application.isEditor) UnityEditor.AssetDatabase.Refresh();
        #endif

        // Set shader
        string shader = null;
        if (entry.getAdditionalData() != null) entry.getAdditionalData().TryGetValue("shader", out shader);
        #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            // In mobile, if shader isn't set then force a lagacy shader
            if (shader == null) shader = "Legacy Shaders/Diffuse";
        #endif

        // Import model
        string filepath = Application.persistentDataPath + "/" + filenames[0];
        string extension = Path.GetExtension(filepath).ToLower();
        // Load file by extension
        if (extension == ".glb") {
            GameObject result = new GameObject();
            result.name = filenames[0];
            var glb = result.AddComponent<GLTFast.GlbAsset>();
            glb.shader = shader;
            glb.url = serverURL + "&file=" + ((ModelHologram) entry.getHologram()).getStorageID();
            result.AddComponent<CustomBehaviour>().entry = entry;
            // Set game object parent and position
            result.transform.parent = this.gameObject.transform;
            result.transform.position = this.gameObject.transform.position;
            #if UNITY_EDITOR
                // Caculate render times
                glb.onLoadComplete += calcRenderTime;
                renderIndex = filenames[0];
                calcRenderTime(false, null);
            #endif
        }
        else if (extension == ".gltf") {
            GameObject result = Importer.LoadFromFile(filepath, shader);
            result.name = filenames[0];
            result.AddComponent<CustomBehaviour>().entry = entry;
            // Set game object parent and position
            result.transform.parent = this.gameObject.transform;
            result.transform.position = this.gameObject.transform.position;
        }
        else if (extension == ".obj") {
            // Check how many files were uploaded to console
            int numFiles = filenames.Count;
            List<string> texNames = new List<string>();
            if (numFiles >= 3) {
                texNames = filenames.GetRange(2, numFiles-2);
            }
            gameObject.AddComponent<ObjectImporter>().ImportModelAsync(entry, Path.GetFileNameWithoutExtension(filenames[0]), filepath, texNames, null, importOptions, shader);
        }
        
        yield return null;
    }

#if UNITY_EDITOR
    static public string renderIndex = "";
    static Dictionary<string, float> renderTimes = new Dictionary<string, float>();
    static public void calcRenderTime(bool someBooleanValue, GameObject someGameObject)
    {
        // Check if started redering
        float time;
        if (renderTimes.TryGetValue(renderIndex, out time)){
            // Output render time
            Debug.Log("Render time of " + renderIndex + " is " + (Time.time - time));
        } else {
            // Store render time start
            renderTimes.Add(renderIndex, Time.time);
        }
    }
#endif

    IEnumerator WebsocketClient()
    {
        // Instantiate a websocket
        this.gameObject.AddComponent<WClient>();
        // Set up listeners
        WClient.On(WClient.EventType.CONNECTED_TO_WS.ToString(), (string arg0) => {
            WClient.Emit(WClient.EventType.KEY.ToString(), APIKey);
        });
        WClient.On(WClient.EventType.CONNECTION_LOST.ToString(), (string arg0) => {
            
        });
        WClient.On(WClient.EventType.ADD_ENTRY.ToString(), (string message) => {
            // Parse new entry
            Entry entry = ParseEntry(JSON.Parse(message));
            // Download and instantiate content
            DownloadEntryAssets(entry, serverURL);
        });
        WClient.On(WClient.EventType.DELETE_ENTRY.ToString(), (string message) => {
            // Parse data
            string[] messageArray = message.Split('|');
            string eventType = messageArray[0];
            string entryID = messageArray[1];
            GameObject gameObjectToDestroy = null;
            // Find entry and destroy content
            foreach (CustomBehaviour cb in FindObjectsOfType<CustomBehaviour>())
            {
                if (cb.entry.getId().Equals(entryID))
                {
                    // Remove entry
                    dbObject.getEntries().Remove(cb.entry);
                    // Destroy game object
                    gameObjectToDestroy = cb.gameObject;
                    break;
                }
            }
            if (gameObjectToDestroy != null) Destroy(gameObjectToDestroy);
        });
        WClient.On(WClient.EventType.DATA_POST_ALL.ToString(), (string message) => {
            // Parse data
            string[] messageArray = message.Split('|');
            string eventType = messageArray[0];
            string dataKey = messageArray[1];
            string dataValue = messageArray[2];
            // Find entry
            foreach (Entry entry in dbObject.getEntries())
            {
                // Update or add key
                entry.removeAdditionalData(dataKey);
                entry.addAdditionalData(dataKey, dataValue);
            }
        });
        WClient.On(WClient.EventType.DATA_POST_ENTRY.ToString(), (string message) => {
            // Parse data
            string[] messageArray = message.Split('|');
            string eventType = messageArray[0];
            string entryID = messageArray[1];
            string dataKey = messageArray[2];
            string dataValue = messageArray[3];
            // Find entry
            foreach (Entry entry in dbObject.getEntries())
            {
                if (entry.getId().Equals(entryID))
                {
                    // Update key
                    entry.removeAdditionalData(dataKey);
                    entry.addAdditionalData(dataKey, dataValue);
                    break;
                }
            }
        });
        WClient.On(WClient.EventType.DATA_REMOVE_ALL.ToString(), (string message) => {
            // Parse data
            string[] messageArray = message.Split('|');
            string eventType = messageArray[0];
            string dataKey = messageArray[1];
            // Find entry
            foreach (Entry entry in dbObject.getEntries())
            {
                // Remove key
                entry.removeAdditionalData(dataKey);
            }
        });
        WClient.On(WClient.EventType.DATA_REMOVE_ENTRY.ToString(), (string message) => {
            // Parse data
            string[] messageArray = message.Split('|');
            string eventType = messageArray[0];
            string entryID = messageArray[1];
            string dataKey = messageArray[2];
            // Find entry
            foreach (Entry entry in dbObject.getEntries())
            {
                if (entry.getId().Equals(entryID))
                {
                    // Remove key
                    entry.removeAdditionalData(dataKey);
                    break;
                }
            }
        });
        WClient.On(WClient.EventType.SESSION_INFO.ToString(), (string session) => {
            Debug.Log("Session is: " + session);
        });
        yield return null;
    }
	
	public IEnumerator UploadModel(byte[] objFile, string objFileName, byte[] mtlFile, string mtlFileName)
    {
        return UploadModel(objFile, objFileName, mtlFile, mtlFileName, (List<byte[]>) null, (List<string>) null);
    }

    public IEnumerator UploadModel(byte[] objFile, string objFileName, byte[] mtlFile, string mtlFileName, byte[] pngFile, string pngFileName)
    {
        return UploadModel(objFile, objFileName, mtlFile, mtlFileName, new List<byte[]> {pngFile}, new List<string> {pngFileName});
    }
    
    public IEnumerator UploadModel(byte[] objFile, string objFileName, byte[] mtlFile, string mtlFileName, List<byte[]> pngFiles, List<string> pngFileNames)
    {
        // Create form
        WWWForm form = new WWWForm();
        // Set form data
        form.AddField("key",  APIKey);      // API Key
        form.AddField("target_type", 2);    // Target type is SURFACE
        form.AddField("hologram_type", 2);  // Hologram type is MODEL
        // Set form files
        form.AddBinaryData("file_model", objFile, objFileName, null);   // .obj file
        form.AddBinaryData("file_model", mtlFile, mtlFileName, null);   // .mtl file
        if (pngFiles != null && pngFileNames != null)
			for (int i = 0; i < pngFileNames.Count; i++)                    // Texture files
            	form.AddBinaryData("file_model", pngFiles[i], pngFileNames[i], null);
        // Send request
        UnityWebRequest www = UnityWebRequest.Post("https://console.echoAR.xyz/upload", form);
        yield return www.SendWebRequest();
        // Check for errors
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }

    public IEnumerator UpdateEntryData(string entryID, string dataKey, string dataValue)
    {
        // Create form
        WWWForm form = new WWWForm();
        // Set form data
        form.AddField("key", APIKey);        // API Key
        form.AddField("entry", entryID);     // Entry ID
        form.AddField("data", dataKey);      // Key
        form.AddField("value", dataValue);   // Value
        UnityWebRequest www = UnityWebRequest.Post("https://console.echoar.xyz/post", form);
        // Send request
        yield return www.SendWebRequest();
        // Check for errors
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Data update complete!");
        }
    }
} 