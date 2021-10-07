//#define USE_CUSTOM_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MouthScript : FacePartsBaseScript
{
    static Transform _MOUTH_ANCHOR;
    static Transform MOUTH_ANCHOR
    {
        get
        {
            if(_MOUTH_ANCHOR == null)
            {
                GameObject go = new GameObject("MOUTH_ANCHOR");
                _MOUTH_ANCHOR = go.transform;
            }
            return _MOUTH_ANCHOR;
        }
    }

    private float defaultVolume = 0.5f;

    [Header("Timer")]
    private float nextSoundTime = 0;


    [SerializeField] List<AudioClip> highSound = new List<AudioClip>();
    [SerializeField] List<AudioClip> lowSound = new List<AudioClip>();
    private AudioClip lastClip;
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage();
        }
    }




    void Start()
    {
        transform.SetParent(MOUTH_ANCHOR);
        Debug.Log(highSound.Count);
    }


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        PlaySound();
    }

    private void PlaySound()
    {
        if(Time.time > nextSoundTime && health >= 1)
        {

            AudioClip audio = GetRandomClip();
            audioSource.PlayOneShot(audio);

            nextSoundTime = Time.time + audio.length;
        }
        
    }


    private AudioClip GetRandomClip()
    {
        AudioClip clip;
        float ratio = SetVolume();
        Debug.Log("Ratio:" + ratio);

        int index;
        do
        {
            if (ratio >= 0.5) //Healthが50%以上
            {
                index = Random.Range(0, highSound.Count);
                clip = highSound[index];
            }
            else
            {
                index = Random.Range(0, lowSound.Count);
                clip = lowSound[index];
            }
        } while (clip == lastClip);
        

        lastClip = clip;

        return clip;
    }

    private float SetVolume()
    {
        float ratio = (float)health / cacheHealth;
        Debug.Log("R:" + ratio);

        audioSource.volume = 1 - defaultVolume * ratio;

        return ratio;
    }


#if USE_CUSTOM_EDITOR
    #region Custom Editor

#if UNITY_EDITOR

    [CustomEditor(typeof(MouthScript))]
    public class MouthEditor : Editor
    {
        bool folding = false;
        bool foldingLowClips = false;
        int deleteAt = 0;
        int deleteAtInLow = 0;

        float time;

        public override void OnInspectorGUI()
        {
            MouthScript mouth = target as MouthScript;

            EditorGUILayout.LabelField("HP / HP増加率");
            EditorGUILayout.BeginHorizontal();
            mouth.health = EditorGUILayout.IntField(mouth.health, GUILayout.Width(48));
            mouth.scale = EditorGUILayout.FloatField(mouth.scale, GUILayout.Width(48));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Dead Sound");
            mouth.deadSound = EditorGUILayout.ObjectField(mouth.deadSound, typeof(AudioClip), true) as AudioClip;

            EditorGUILayout.Space();
            time = EditorGUILayout.FloatField(mouth.nextSoundTime);
            
            EditorGUILayout.Space();

            #region High HP
            //AudioClip
            List<AudioClip> clips = mouth.highSound;

            
            if (folding = EditorGUILayout.Foldout(folding, "HP高"))
            {
                for(int i = 0; i < clips.Count; i++)
                {
                    clips[i] = EditorGUILayout.ObjectField(clips[i], typeof(AudioClip), true) as AudioClip;
                }

                AudioClip clip = EditorGUILayout.ObjectField("追加", null, typeof(AudioClip), true) as AudioClip;
                if (clip) clips.Add(clip);

                
            }

            EditorGUILayout.BeginHorizontal();
            
            deleteAt = EditorGUILayout.IntField(deleteAt, GUILayout.Width(48));
            if(GUILayout.Button(deleteAt + "番目を削除"))
            {
                if(clips.Count > deleteAt) clips.RemoveAt(deleteAt);

                deleteAt = 0;
            }
            EditorGUILayout.EndHorizontal();

            if(GUILayout.Button("HP高　クリア"))
            {
                clips.Clear();
            }

            #endregion

            EditorGUILayout.Space();

            #region Low HP

            List<AudioClip> lowClips = mouth.lowSound;
            

            if(foldingLowClips = EditorGUILayout.Foldout(foldingLowClips, "HP低"))
            {
                for(int i = 0; i < lowClips.Count; i++)
                {
                    lowClips[i] = EditorGUILayout.ObjectField(lowClips[i], typeof(AudioClip), true) as AudioClip;
                }

                AudioClip lowClip = EditorGUILayout.ObjectField("追加", null, typeof(AudioClip), true) as AudioClip;
                if (lowClip) lowClips.Add(lowClip);

                
            }

            EditorGUILayout.BeginHorizontal();
            deleteAtInLow = EditorGUILayout.IntField(deleteAtInLow, GUILayout.Width(48));
            if(GUILayout.Button(deleteAtInLow + "番目を削除"))
            {
                if (lowClips.Count > deleteAtInLow) lowClips.RemoveAt(deleteAtInLow);

                deleteAtInLow = 0;
            }
            EditorGUILayout.EndHorizontal();

            if(GUILayout.Button("HP低　クリア"))
            {
                lowClips.Clear();
            }


            mouth.highSound = clips;
            mouth.lowSound = lowClips;
            #endregion
        }
    }

#endif

    #endregion

#endif
}
