using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InteractionSequenceManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector[] sequences;

    public void StartInteractionSequences()
    {
        StartCoroutine(BeginSequencesRoutine());
    }

    private IEnumerator BeginSequencesRoutine()
    {
        foreach (PlayableDirector sequence in sequences)
        {
            Debug.Log(sequence.name);
            Debug.Log(sequence.duration);
            sequence.Play();
            yield return new WaitForSeconds((float) sequence.duration);
        }
    }
}
