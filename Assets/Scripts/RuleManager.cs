using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // 导入 TextMeshPro 命名空间
using System.Linq;


public class RuleManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button[] ruleButtons; // 8 个按钮
    [SerializeField] private RawImage renderTarget; // 显示树形的 RawImage
    [SerializeField] private Camera renderCamera; // 用于渲染树形的摄像机
    [SerializeField] private LSystemLogic lSystemLogic; // L-System 脚本实例

    [Header("Text UI Components")]
    [SerializeField] private TMP_Text axiomText;  // 用来显示 axiom 的 TextMeshPro
    [SerializeField] private TMP_Text rulesText;  // 用来显示 rules 的 TextMeshPro

    private List<LSystemRule> rules;

    private void Start()
    {
        // 初始化规则
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

        // 绑定按钮事件
        for (int i = 0; i < ruleButtons.Length; i++)
        {
            int index = i; // 捕获索引
            ruleButtons[i].onClick.AddListener(() => OnRuleButtonClicked(index));
        }
    }

    private void OnRuleButtonClicked(int index)
    {
        if (index < 0 || index >= rules.Count) return;

        // 更新 L-System 参数
        LSystemRule selectedRule = rules[index];
        lSystemLogic.axiom = selectedRule.axiom;
        lSystemLogic.rules = selectedRule.productions;

        // 重新生成树
        lSystemLogic.Generate();

        // 更新 UI 显示
        UpdateUI();

        // 渲染到 RawImage
        RenderTree();
    }

    private void UpdateUI()
    {
        // 更新 axiom 和 rules 显示
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
