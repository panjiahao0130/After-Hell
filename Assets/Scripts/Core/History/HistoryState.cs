using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace History
{
    [System.Serializable]
    public class HistoryState
    {
        public DialogueData dialogue;
        public List<CharacterData> characters;
        public List<AudioTrackData> audio;
        public List<AudioSFXData> sfx;
        public List<GraphicData> graphics;

        public static HistoryState Capture()
        {
            HistoryState state = new HistoryState();
            state.dialogue = DialogueData.Capture();
            state.characters = CharacterData.Capture();
            state.audio = AudioTrackData.Capture();
            state.sfx = AudioSFXData.Capture();
            state.graphics = GraphicData.Capture();

            return state;
        }

        public void Load()
        {
            DialogueData.Apply(dialogue);
            CharacterData.Apply(characters);
            AudioTrackData.Apply(audio);
            AudioSFXData.Apply(sfx);
            GraphicData.Apply(graphics);
        }
    }
}