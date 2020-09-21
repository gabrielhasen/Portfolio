using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class MinimapManager : MMPersistentSingleton<MinimapManager>
{
    public Sprite Background_Hidden;
    public Sprite Background_Visited;
    public Sprite Background_CurrentlyIn;
    public Sprite Icon_Bomb;
    public Sprite Icon_Health;
    public Sprite Icon_Door;
    public Sprite Icon_MiniBoss;
    public Sprite Icon_Boss;
    public Sprite Icon_QuestionMark;
    public Sprite Icon_StartChunk;
    public Sprite Icon_ExitChunk;
}
