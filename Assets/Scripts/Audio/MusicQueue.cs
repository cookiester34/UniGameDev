using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MusicQueue
{
    private List<Music> tracks;


    public MusicQueue(List<Music> tracks)
    {
        this.tracks = tracks;
    }

    // The actual coroutine to repeat playbacks.
    public IEnumerator LoopMusic(MonoBehaviour player, float delay, System.Action<Music> playFunction)
    {
        while (true)
        {
            yield return player.StartCoroutine(Run(RandomizeList(tracks), delay, playFunction));
        }
    }

    // Plays the next track after current finishes.
    public IEnumerator Run(List<Music> tracks, float delay, System.Action<Music> playFunction)
    {
        foreach(Music track in tracks)
        {
            playFunction(track);

            yield return new WaitForSeconds(track.source.clip.length + delay);
        }
    }

    // Shuffles the list of tracks.
    public List<Music> RandomizeList(List<Music> list)
    {
        List<Music> copy = new List<Music>(list);

        int i = copy.Count;
        while (i > 1)
        {
            i--;

            // Gets random index and assigns its track to a randomTrack.
            int j = Random.Range(0, i + 1);
            Music randomTrack = copy[j];

            // Assigns the random track to the last index.
            copy[j] = copy[i];
            copy[i] = randomTrack;
        }

        return copy;
    }
}
