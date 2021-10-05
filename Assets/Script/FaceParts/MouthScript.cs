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

    private static int volume = -5;
    // Start is called before the first frame update
    [SerializeField] AudioMixer mixer;
    private AudioSource SE;
    private AudioClip SECLIP;

    [SerializeField] List<AudioClip> highSound = new List<AudioClip>();
    [SerializeField] List<AudioClip> lowSound = new List<AudioClip>();

    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage();
        }
    }


    private void Volume(float a)
    {
        switch (a)
        {
            case 0.8f:
                volume += 2;
                break;

            case 0.6f:
                volume ++;
                break;

            case 0.4f:
                volume --;
                break;
        }
        mixer.SetFloat("SE", volume);
    }


    void Start()
    {
        transform.SetParent(MOUTH_ANCHOR);

        SE = GetComponent<AudioSource>();
        mixer.SetFloat("SE", volume);

        SetCache();
    }
    //private float cacheTime = 0;
    // Update is called once per frame


    public override void TakeDamage()
    {
        base.TakeDamage();

        SE.PlayOneShot(SECLIP);
        if (Mathf.Approximately(health, cacheHealth * 0.8f)) Volume(0.8f);
        else if (Mathf.Approximately(health, cacheHealth * 0.6f)) Volume(0.6f);
        else if (Mathf.Approximately(health, cacheHealth * 0.4f)) Volume(0.4f);

        
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        SE.PlayOneShot(SECLIP);
        if (Mathf.Approximately(health, cacheHealth * 0.8f)) Volume(0.8f);
        else if (Mathf.Approximately(health, cacheHealth * 0.6f)) Volume(0.6f);
        else if (Mathf.Approximately(health, cacheHealth * 0.4f)) Volume(0.4f);

    }


    #region Custom Editor

#if UNITY_EDITOR

    [CustomEditor(typeof(MouthScript))]
    public class MouthEditor : Editor
    {
        bool folding = false;
        bool foldingLowClips = false;
        int deleteAt = 0;
        int deleteAtInLow = 0;

        public override void OnInspectorGUI()
        {
            MouthScript mouth = target as MouthScript;

            EditorGUILayout.LabelField("HP / HP増加率");
            EditorGUILayout.BeginHorizontal();
            mouth.health = EditorGUILayout.IntField(mouth.health, GUILayout.Width(48));
            mouth.scale = EditorGUILayout.FloatField(mouth.scale, GUILayout.Width(48));
            EditorGUILayout.EndHorizontal();


            //AudioMixer
            AudioMixer mixer = mouth.mixer;
            mixer = EditorGUILayout.ObjectField(mixer, typeof(AudioMixer), true) as AudioMixer;

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
            #endregion
        }
    }

#endif

#endregion
}
