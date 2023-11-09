using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Character Data")]
public class CharacterData : ScriptableObject
{
    public Sprite Repeat;
    public Sprite AdultRepeat;
    public Sprite ID;
    public Sprite Character;
    public Sprite GatherSeedsCharacter;
    public Sprite ChildGreenHouseNotice;
    public Sprite AdultGreenHouseNotice;
    public Sprite BullyMessage;
    public Sprite R3L2CharacterInfo;
    public Sprite R3L2CharacterSearching;
    public Sprite R3L2CharacterHappy;
    public Sprite R3L2CharacterWin;
    public Sprite R2L3Story1;
    public Sprite R2L3Story2;
    public Sprite R2L3Story3;
    public Sprite R3L6Dirty;
    public Sprite R3L6Clean;
    public Sprite R3L6Bed;
    public ChildAudioClips ChildAudio;
    public AdultAudioClips AdultAudio;
    public RuntimeAnimatorController Controller;

    public void SetAudioDictionaries()
    {
        ChildAudio.SetLevelDictionaries();
        AdultAudio.SetLevelDictionaries();
    }
}