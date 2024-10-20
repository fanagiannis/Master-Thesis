using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Footsteps", menuName = "New ScriptableObject/Footsteps")]
public class FootstepsData : ScriptableObject
{
    private List<AudioClip> selectedList= new List<AudioClip>();
    [SerializeField]private List<AudioClip> footstepsDirt=new List<AudioClip>();
    [SerializeField]private List<AudioClip> footstepsStone=new List<AudioClip>();
    private int index;
    public AudioClip RandomSound()
    {
        selectedList=footstepsStone;
        index=Random.Range(0, selectedList.Count-1);
        return selectedList[index];
    }
}
