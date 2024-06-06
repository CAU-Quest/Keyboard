using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    [Header("Prefab Settings")]
    public UnityEvent<string> onPress;
    public UnityEvent onRelease;

    [SerializeField] private string text;
    [SerializeField] private string capitalText;

    [SerializeField] private Vector3 fingerPosition;
    
    private Vector3 initialLocalPosition;
    private Vector3 initialColliderCenter;

    private GameObject _renderer;
    private GameObject presser;
    private BoxCollider collider;
    private AudioSource sound;
    private bool isPressed;
    private bool isCapital = false;
    private bool isCooldown = false;

    public float pressedHeight = 0.1f;
    public float heightThreshold = 0.05f;

    void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>().gameObject;
        collider = GetComponent<BoxCollider>();
        initialLocalPosition = _renderer.transform.localPosition;
        initialColliderCenter = collider.center;
        sound = GetComponent<AudioSource>();
        isPressed = false;
    }

    public void SetCapital(bool state)
    {
        isCapital = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finger") && !isPressed)
        {
            presser = other.gameObject;
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            ResetButton();
        }
    }

    void Update()
    {
        if (isPressed)
        {
            fingerPosition = transform.InverseTransformPoint(presser.transform.position);
            float newHeight = Mathf.Max(initialLocalPosition.y - pressedHeight, fingerPosition.y);
            newHeight = Mathf.Min(newHeight, initialLocalPosition.y);
            _renderer.transform.localPosition = new Vector3(initialLocalPosition.x, newHeight, initialLocalPosition.z);

            if (!isCooldown && newHeight <= initialLocalPosition.y - heightThreshold)
            {
                onPress.Invoke((isCapital) ? capitalText : text);
                sound.Play();
                StartCoroutine(KeyCooldown());
                isCooldown = true;
            }
        }
        else
        {
            _renderer.transform.localPosition = Vector3.Lerp(_renderer.transform.localPosition, initialLocalPosition, Time.deltaTime * 10);
        }

    }

    IEnumerator KeyCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        isCooldown = false;
    }

    private void ResetButton()
    {
        _renderer.transform.localPosition = initialLocalPosition;
        onRelease.Invoke();
        isPressed = false;
    }
}
