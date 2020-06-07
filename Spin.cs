using UnityEngine;

public class MathTab : MonoBehaviour
{
    public Vector3 spinSpeed;
    public float spinDuration = 1f;
    public AnimationCurve spinDecline;

    private float spin = 0f;

    [System.NonSerialized]
    public MathGate gate;
    [System.NonSerialized]
    public bool isCorrect = false;
    TextMesh textMesh;
    MeshRenderer _renderer;
    Transform trs;

    public void Initialize(string answer, bool isCorrect)
    {
        textMesh = GetComponentInChildren<TextMesh>();
        _renderer = GetComponent<MeshRenderer>();
        trs = transform;
        textMesh.text = answer;
        this.isCorrect = isCorrect;
    }

    private void Update()
    {
        if(spin > 0f)
        {
            spin = Mathf.MoveTowards(spin, 0f, Time.deltaTime / spinDuration);
            trs.Rotate(spinSpeed * spinDecline.Evaluate(1f - spin) * Time.deltaTime, Space.Self);
        }
    }

    public void SetMaterial(Material mat)
    {
        _renderer.sharedMaterial = mat;
    }

    public void SetAlpha(float alpha)
    {
        Color col = textMesh.color;
        col.a = alpha;
        textMesh.color = col;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gate.answered) return;
        if (other.GetComponentInParent<Runner>() == null) return;
        if (isCorrect)
        {
            gate.OnCorrectAnswer();
            spin = 1f;
        } else
        {
            gate.OnWrongAnswer();
        }
    }
}
