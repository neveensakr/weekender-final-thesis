using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InteractionSequenceManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector[] sequences;
    [SerializeField] private PlayableDirector elevatorScene;

    public void StartInteractionSequences()
    {
        StartCoroutine(BeginSequencesRoutine());
    }

    private IEnumerator BeginSequencesRoutine()
    {
        foreach (PlayableDirector sequence in sequences)
        {
            sequence.Play();
            yield return new WaitForSeconds((float) sequence.duration);
        }
        elevatorScene.Play();
    }
}
