#if UNITY_EDITOR
#define IS_EDITOR
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    // シングルトン
    private static TouchManager _instance;
    private static TouchManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject(typeof(TouchManager).Name);
                _instance = obj.AddComponent<TouchManager>();
            }
            return _instance;
        }
    }

    private TouchManager _info = new TouchManager();
    private event System.Action<TouchManager> _began;
    private event System.Action<TouchManager> _moved;
    private event System.Action<TouchManager> _ended;

    // タッチ開始時のイベント
    public static event System.Action<TouchManager> Began
    {
        add
        {
            Instance._began += value;
        }
        remove
        {
            Instance._began -= value;
        }
    }

    // タッチ中のイベント
    public static event System.Action<TouchManager> Moved
    {
        add
        {
            Instance._moved += value;
        }
        remove
        {
            Instance._moved -= value;
        }
    }

    // タッチ終了時のイベント
    public static event System.Action<TouchManager> ended
    {
        add
        {
            Instance._ended += value;
        }
        remove
        {
            Instance._ended -= value;
        }
    }

    // 現在のタッチ状態
    private TouchState state
    {
        get {
#if IS_EDITOR
            // EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                return TouchState.Began;
            }
            else if (Input.GetMouseButton(0))
            {
                return TouchState.Moved;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                return TouchState.Ended;
            }
#else
            // EDITOR以外
            if (Input.touchCount > 0)
            {
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        return TouchState.Began;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        return TouchState.Moved;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        return TouchState.Ended;
                    default:
                        break;
                }
            }
#endif
            return TouchState.None;
        }
    }

    // タッチ状態
    private enum TouchState
    {
        None = 0,   // タッチなし
        Began = 1,  // タッチ開始
        Moved = 2,  // タッチ中
        Ended = 3,  // タッチ終了
    }

}

