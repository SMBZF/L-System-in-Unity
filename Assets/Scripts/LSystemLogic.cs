using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI; // ���� RawImage

public class LSystemLogic : MonoBehaviour
{
    [Header("L-System Parameters")]
    public string axiom = "X"; // ��ʼ Axiom
    public Dictionary<char, string> rules; // ��������
    public int interactions = 4; // ��������
    public float length = 5f; // ��֦����
    public float angle = 30f; // �ֲ�Ƕ�
    public GameObject Branch;
    public Camera renderCamera; // ������Ⱦ�������
    public RawImage rawImage; // ������ʾ��Ⱦ����� RawImage
    public RenderTexture renderTexture; // ������Ⱦ�� RenderTexture

    public Transform treeParent;

    private Stack<TransformInfo> transformStack;
    private string currentString;

    private void Start()
    {
        Debug.Log("Start method called");  // ����������֤ Start() �Ƿ񱻵���

        // ��ʼ������Ͷ�ջ
        transformStack = new Stack<TransformInfo>();
        rules = new Dictionary<char, string>
    {
        { 'X', "F[+X]F[-X]FX" },
        { 'F', "FF" }
    };
        Generate();
    }


    public void Generate()
    {
        transform.rotation = Quaternion.identity; // ������תΪĬ��ֵ

        // ������е����νṹ
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        transformStack.Clear();
        currentString = axiom;

        // ��������λ�ã�ȷ��ÿ������ʱ������ͬ�ĵط���ʼ
        transform.position = Vector3.zero; // �������������Ϊ��ϣ���ĳ�ʼλ��

        // ���� L-System �ַ���
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < interactions; i++)
        {
            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }
            currentString = sb.ToString();
            sb = new StringBuilder();
        }

        // �����ַ�����������
        foreach (char c in currentString)
        {
            switch (c)
            {
                case 'F':
                    GenerateBranch();
                    break;
                case '+':
                    transform.Rotate(Vector3.back * angle);
                    break;
                case '*':
                    transform.Rotate(Vector3.left * angle); // Χ�� X ��������
                    break;
                case '/':
                    transform.Rotate(Vector3.up * angle); // Χ�� Y ��������
                    break;
                case '-':
                    transform.Rotate(Vector3.forward * angle);
                    break;
                case '[':
                    transformStack.Push(new TransformInfo
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;
                case ']':
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;
            }
        }
    }


    private void GenerateBranch()
    {

        Vector3 initialPosition = transform.position;
        transform.Translate(Vector3.up * length);
        GameObject branch = Instantiate(Branch, transform);
        LineRenderer lineRenderer = branch.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, initialPosition);
        lineRenderer.SetPosition(1, transform.position);
    }

}