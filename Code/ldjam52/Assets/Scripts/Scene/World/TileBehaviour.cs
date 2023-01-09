using System;

using Assets.Scripts.Base;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

using UnityEngine;
using UnityEngine.Events;

public class TileBehaviour : MonoBehaviour
{
    public UnityEvent<TileBehaviour> OnClick = new UnityEvent<TileBehaviour>();

    private AudioSource audioSource;

    private GameObject floorGameObject;
    private GameObject naturalAreaGameObject;

    private Boolean isTileOwned;

    public FieldBehaviour FieldBehaviour;
    public FieldViewBehaviour FieldViewBehaviour;

    public Tile Tile { get; private set; }

    public void SetTile(Tile tile)
    {
        this.Tile = tile;

        if (this.FieldBehaviour != null)
        {
            this.FieldBehaviour.SetField(tile?.Field);
        }

        if (tile != default)
        {
            this.isTileOwned = tile.IsOwned;

            if (this.floorGameObject != null)
            {
                if (this.floorGameObject.TryGetComponent<MeshRenderer>(out var meshRenderer))
                {
                    if (meshRenderer.material != null)
                    {
                        meshRenderer.material.color = tile.Color.ToUnity();
                    }
                }
            }
        }
    }

    public void ShowFieldView()
    {
        if (this.FieldViewBehaviour != null)
        {
            this.FieldViewBehaviour.ViewField(this.FieldBehaviour);
        }
    }

    public void PlayEffect(String audioClipName)
    {
        var audioSource = GetAudioSource();

        var audioClip = GameFrame.Base.Resources.Manager.Audio.Get(audioClipName);

        audioSource.clip = audioClip;
        
        audioSource.Play();
    }

    private void Awake()
    {
        this.floorGameObject = transform.Find("Surface").gameObject;
        this.FieldBehaviour = transform.Find("Field").GetComponent<FieldBehaviour>();
        this.naturalAreaGameObject = transform.Find("NatureArea").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Tile != default)
        {
            if (this.Tile.IsOwned && !this.isTileOwned)
            {
                this.isTileOwned = this.Tile.IsOwned;

                this.FieldBehaviour.gameObject.SetActive(this.Tile.IsOwned);
                this.naturalAreaGameObject.SetActive(!this.Tile.IsOwned);
            }
        }
    }

    private AudioSource GetAudioSource()
    {
        if (this.audioSource == default)
        {
            this.audioSource = GetComponent<AudioSource>();
        }

        this.audioSource.volume = audioSource.volume = Core.Game.Options.EffectsVolume;

        return this.audioSource;
    }
}
