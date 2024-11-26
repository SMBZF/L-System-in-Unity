using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI; // 用于 RawImage

public class LSystemLogic : MonoBehaviour
{
    [Header("L-System Parameters")]
    public string axiom = "X"; // 初始 Axiom
    public Dictionary<char, string> rules; // 生产规则
    public int interactions = 4; // 迭代次数
    public float length = 5f; // 树枝长度
    public float angle = 30f; // 分叉角度
    public GameObject Branch;
    public Camera renderCamera; // 用于渲染的摄像机
    public RawImage rawImage; // 用于显示渲染结果的 RawImage
    public RenderTexture renderTexture; // 用于渲染的 RenderTexture

    public Transform treeParent;

    private Stack<TransformInfo> transformStack;
    private string currentString;

    private void Start()
    {
        Debug.Log("Start method called");  // 添加这个来验证 Start() 是否被调用

        // 初始化规则和堆栈
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
        transform.rotation = Quaternion.identity; // 重置旋转为默认值

        // 清空现有的树形结构
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        transformStack.Clear();
        currentString = axiom;

        // 重置树的位置，确保每次生成时树从相同的地方开始
        transform.position = Vector3.zero; // 或者你可以设置为你希望的初始位置

        // 构建 L-System 字符串
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

        // 根据字符串生成树形
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
                    transform.Rotate(Vector3.left * angle); // 围绕 X 轴正方向
                    break;
                case '/':
                    transform.Rotate(Vector3.up * angle); // 围绕 Y 轴正方向
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