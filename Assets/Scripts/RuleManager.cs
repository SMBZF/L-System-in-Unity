using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // ���� TextMeshPro �����ռ�
using System.Linq;


public class RuleManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button[] ruleButtons; // 8 ����ť
    [SerializeField] private RawImage renderTarget; // ��ʾ���ε� RawImage
    [SerializeField] private Camera renderCamera; // ������Ⱦ���ε������
    [SerializeField] private LSystemLogic lSystemLogic; // L-System �ű�ʵ��

    [Header("Text UI Components")]
    [SerializeField] private TMP_Text axiomText;  // ������ʾ axiom �� TextMeshPro
    [SerializeField] private TMP_Text rulesText;  // ������ʾ rules �� TextMeshPro

    private List<LSystemRule> rules;

    private void Start()
    {
        // ��ʼ������
        rules = new List<LSystemRule>
        {
            new LSystemRule("F", new Dictionary<char, string> { { 'F', "F[+F]F[-F]F" } }),
            new LSystemRule("F", new Dictionary<char, string> { { 'F', "F[+F]F[-F][F]" } }),
            new LSystemRule("F", new Dictionary<char, string> { { 'F', "FF-[-F+F+F]+[+F-F-F]" } }),
            new LSystemRule("X", new Dictionary<char, string> { { 'X', "F[+X]F[-X]+X" }, { 'F', "FF" } }),
            new LSystemRule("X", new Dictionary<char, string> { { 'X', "F[+X][-X]FX" }, { 'F', "FF" } }),
            new LSystemRule("X", new Dictionary<char, string> { { 'X', "F-[[X]+X]+F[+FX]-X" }, { 'F', "FF" } }),
            new LSystemRule("X", new Dictionary<char, string> { { 'X', "F[*+X][/-X][+FX]" }, { 'F', "FF" } }),
            new LSystemRule("X", new Dictionary<char, string> { { 'X', "F-[[X]+X]+F[*X]/F" }, { 'F', "FF" } })
        };

        // �󶨰�ť�¼�
        for (int i = 0; i < ruleButtons.Length; i++)
        {
            int index = i; // ��������
            ruleButtons[i].onClick.AddListener(() => OnRuleButtonClicked(index));
        }
    }

    private void OnRuleButtonClicked(int index)
    {
        if (index < 0 || index >= rules.Count) return;

        // ���� L-System ����
        LSystemRule selectedRule = rules[index];
        lSystemLogic.axiom = selectedRule.axiom;
        lSystemLogic.rules = selectedRule.productions;

        // ����������
        lSystemLogic.Generate();

        // ���� UI ��ʾ
        UpdateUI();

        // ��Ⱦ�� RawImage
        RenderTree();
    }

    private void UpdateUI()
    {
        // ���� axiom �� rules ��ʾ
        axiomText.text = "Axiom: " + lSystemLogic.axiom;
        rulesText.text = "Rules: " + string.Join(", ", lSystemLogic.rules.Select(kv => kv.Key + " -> " + kv.Value));
    }

    private void RenderTree()
    {
        if (renderCamera.targetTexture == null)
        {
            RenderTexture rt = new RenderTexture(512, 512, 16);
            renderCamera.targetTexture = rt;
            renderTarget.texture = rt;
        }
        renderCamera.Render();

    }
}
