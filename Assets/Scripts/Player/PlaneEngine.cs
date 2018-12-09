using System.Collections;
using FlyBattle.Controllers;
using FlyBattle.Interface;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlaneEngine : MonoBehaviour, IEngine
{
    public float EnginePower = 1; //todo Черпать из БД

    [SerializeField] private AudioClip _run, _notRun;
    private AudioSource _source;

    private Rigidbody2D _rb2D;
    private Coroutine c_Run;

    private void Awake()
    {
        _source = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    public bool isStarted { get; private set; }

    public void StopEngine(float time = 2f, GameObject obj = null)
    {
        StartCoroutine(c_StopEngine(time, obj));
    }

    public void EnableEngine()
    {
        if (isStarted) DisableEngine();
        SoundController.ChangeAudioAndPlay(_source, _run);
        _rb2D.gravityScale = 0.0f;
        isStarted = true;
        c_Run = StartCoroutine(Run());
    }

    public void DisableEngine()
    {
        SoundController.ChangeAudioAndPlay(_source, _notRun);
        isStarted = false;
        StopCoroutine(c_Run);
        _rb2D.gravityScale = 1.0f;
    }

    private IEnumerator Run()
    {
        while (isStarted)
        {
            _rb2D.AddForce(transform.right * EnginePower);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator c_StopEngine(float time = 2f, GameObject obj = null)
    {
        DisableEngine();

        if (obj != null)
        {
            var layer = obj.layer;
            switch (layer)
            {
                case 15: // Потолок
                    var posY = obj.transform.position.y;
                    _rb2D.drag = 3f;
                    while (transform.position.y > posY - 3f) // Пока самолёт выше потолка минус ещё чутка
                    {
                        yield return null;
                    }

                    _rb2D.drag = 1f;
                    break;
                // Сюда добавлять варианты взаимодействия с объектами при отключении движка
            }
        }
        else
        {
            yield return new WaitForSeconds(time);
        }

        EnableEngine();
    }

    private void OnEnable()
    {
        EnableEngine();
    }

    private void OnDisable()
    {
        DisableEngine();
        StopAllCoroutines();
    }
}