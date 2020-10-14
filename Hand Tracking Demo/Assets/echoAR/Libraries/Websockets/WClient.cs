/**
 * Manager of connections with Websocket Server
 *
 * It class is singleton. You can add WClient component to many nodes and have one instance.
 * @property string url - link to websocket server (default ws://localhost:3000)
 *
 * Unity 16 and laters
 *
 * @category   WClient
 * @package    WClient
 * @author     Konstantin Ostrovsky <ska_live@mail.ru>
 * @copyright  2019 WebPanda inc.
 * @license    https://raw.githubusercontent.com/CatOstrovsky/websocket-nodejs-unity/master/LICENSE
 * @link       https://github.com/CatOstrovsky/websocket-nodejs-unity
 */

using WebSocketSharp;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class WClient : MonoBehaviour {

  public string url = "ws://console.echoar.xyz/message-endpoint"; //"ws://localhost:8080/message-endpoint";

    [Serializable]
  public class GameEvent : UnityEvent <string> {};
  private static WClient _eventManager;
  private Dictionary <string, GameEvent> _eventDictionary;

  private List<string> _queue;
  private static WebSocket ws = null;
  private static bool ended = false;
  private static bool needReconnect = false;

public enum EventType
{
    CONNECTED_TO_WS,
    CONNECTION_LOST,
    KEY,
    ADD_ENTRY,
    DELETE_ENTRY,
    DATA_POST_ALL,
    DATA_POST_ENTRY,
    DATA_REMOVE_ALL,
    DATA_REMOVE_ENTRY,
    SESSION_INFO
};

    public static WClient instance {
      get {
          if (!_eventManager) {
              _eventManager = FindObjectOfType (typeof (WClient)) as WClient;
              if (!_eventManager){
                 Debug.LogError ("There needs to be one active EventManger...");
              }else{
                _eventManager.Init ();
              }
          }
          return _eventManager;
      }
  }

  /**
   * Running when instance was created
   * @void
   */
  void Init () {
      if (_eventDictionary == null) {
        _queue = new List<string>();
        _eventDictionary = new Dictionary<string, GameEvent>();

        if (ws == null){
          ws = new WebSocket (url);
          ws.OnMessage += OnMessge;
          ws.OnClose += OnClose;
          ws.OnOpen += (s,e) => { instance._queue.Add(EventType.CONNECTED_TO_WS.ToString() + "|"); };
          ws.Connect ();
        }
      }
  }

  /**
   * Method for checking connection status. It's public method, you can use it in your projects :)
   * @bool (true - connected, false - disconnected)
   */
  public static bool IsConnected() {
    if (ws == null) return false;
    return ws.ReadyState != WebSocketState.Closed;
  }

  /**
   * Send message to ws server
   * @param key
   * @param value
   * @void
   */
  public static void Emit(string key, string value) {
    ws.Send(key + "|" + value);
  }

  /**
   * Send message to ws server
   * @param key
   * @param value
   * @void
   */
   public static void Emit(string[] key, string[] value)
   {
       string str = "";
       for (int i = 0; i < key.Length && i < value.Length; ++i)
            str += key[i] + "|" + value[i] + "/";
       ws.Send(str);
   }

  /**
   * Subscribe to event of ws server messages
   * @param eventName - name event
   * @param listener - callback method
   * @void
   */
  public static void On(string eventName, UnityAction <string> listener) {
    GameEvent thisEvent = null;
    if (instance._eventDictionary.TryGetValue (eventName, out thisEvent))
        thisEvent.AddListener (listener);
    else {
        thisEvent = new GameEvent ();
        thisEvent.AddListener (listener);
        instance._eventDictionary.Add (eventName, thisEvent);
    }
  }

  /**
   * Disable listener of ws server messages
   * @param eventName - event name
   * @param listener - listener method, which need disabled
   * @void
   */
  public static void Off(string eventName, UnityAction <string> listener) {
    if (_eventManager == null)
        return;
    GameEvent thisEvent = null;
    if (instance._eventDictionary.TryGetValue (eventName, out thisEvent))
        thisEvent.RemoveListener (listener);
  }

  /**
   * Call event in client
   * @void
   */
  public static void EmitEvent (string eventName, string param = null) {
      GameEvent thisEvent = null;
      if (instance._eventDictionary.TryGetValue (eventName, out thisEvent)){
        try {
          thisEvent.Invoke (param);
        }
        catch(Exception e)
        {
          Debug.Log(e);
        }
      }
  }

  /**
   * Called when client disconnected of ws server
   * implementation of https://github.com/sta/websocket-sharp#step-3
   * @void
   */
  private void OnClose(object sender, CloseEventArgs e) {
    instance._queue.Add(EventType.CONNECTION_LOST.ToString() + "|");
    needReconnect = true;
  }

  /**
   * Private method for repeat connect to server after time interval (2 seconds). Automatically called when connection lost
   * @void
   */
  public IEnumerator Reconnect()
  {
    yield return new WaitForSeconds(2f);
    if(WClient.ended != true && ws.ReadyState == WebSocketState.Closed) {
      ws.Connect();
      yield return new WaitForSeconds(1f);
      StartCoroutine(Reconnect());
    }
  }

  /**
   * On message resived from ws server
   * @void
   */
  private void OnMessge(object sender, MessageEventArgs e) {
    //if (e.Data == "ping") {
    //  ws.Send("pong");
    //} else{
      Debug.Log(e.Data);
      instance._queue.Add(e.Data);
    //}
  }

  /**
   * Disconnect of ws server when user close from application
   * @void
   */
  void OnApplicationQuit() {
    // Close connection when class destroyed
    WClient.ended = true;
    ws.Close();
  }

  /**
   * Shedule messages queue in loop and call events listeners.
   * Reconnect to ws server
   * @void
   */
  void Update() {
    if(instance._queue.Count > 0) {
      instance._queue.ForEach((string message) => {
          if (message != null && !message.Equals(""))
          {
              string[] messageArray = message.Split('|');
              EmitEvent(messageArray[0], message);
          }
      });
      instance._queue.Clear();
    }

    if (needReconnect == true) {
      needReconnect = false;
      StartCoroutine(Reconnect());
    }
  }

}
