using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBarrierManager : MMPersistentSingleton<MagicBarrierManager>,
                                    MMEventListener<CorgiEngineEvent>
{
    private GameObject Barrier1;
    private GameObject Barrier2;

    private void Start()
    {
        Barrier1 = transform.GetChild(0).gameObject;
        Barrier2 = transform.GetChild(1).gameObject;

        Barrier1.SetActive(true);
        Barrier2.SetActive(true);
    }

    public void DisableBarrier()
    {
        if (Barrier1.activeSelf) {
            DisableFirstBarrier();
        }
        else {
            DisableSecondBarrier();
        }
    }

    private void DisableFirstBarrier()
    {
        Barrier1.SetActive(false);
    }

    private void DisableSecondBarrier()
    {
        Barrier2.SetActive(false);
    }

    void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
    }
    void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
    }

    public virtual void OnMMEvent(CorgiEngineEvent engineEvent)
    {
        switch (engineEvent.EventType) {
            case CorgiEngineEventTypes.LevelStart:
                Start();
                break;
        }
    }
}
