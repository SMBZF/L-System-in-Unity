using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;  // 导入 TextMeshPro 命名空间

public class LSystemUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider interactionsSlider;
    [SerializeField] private Slider angleSlider;
    [SerializeField] private Slider lengthSlider;

    [Header("Text UI Components")]
    [SerializeField] private TMP_Text interactionsText;  // 用来显示 interactions 值的 TextMeshPro
    [SerializeField] private TMP_Text angleText;         // 用来显示 angle 值的 TextMeshPro
    [SerializeField] private TMP_Text lengthText;        // 用来显示 length 值的 TextMeshPro
    [SerializeField] private TMP_Text axiomText;  // 用来显示 axiom 的 TextMeshPro
    [SerializeField] private TMP_Text rulesText;  // 用来显示 rules 的 TextMeshPro


    [Header("L-System Logic")]
    [SerializeField] private LSystemLogic lSystemLogic;

    [Header("Reset Button")]
    [SerializeField] private Button resetButton;  // 重置按钮

    private void Start()
    {
        resetButton.onClick.AddListener(ResetTree);// 绑定重置按钮事件

        // 初始化滑块值
        interactionsSlider.value = lSystemLogic.interactions;
        angleSlider.value = lSystemLogic.angle;
        lengthSlider.value = lSystemLogic.length;

        // 初始化显示文本
        UpdateUI();

        // 绑定滑块事件
        interactionsSlider.onValueChanged.AddListener(OnSliderValueChanged);
        angleSlider.onValueChanged.AddListener(OnSliderValueChanged);
        lengthSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // 更新 L-System 参数
        lSystemLogic.interactions = Mathf.RoundToInt(interactionsSlider.value);
        lSystemLogic.angle = angleSlider.value;
        lSystemLogic.length = lengthSlider.value;

        // 更新 UI 显示
        UpdateUI();


        // 重新生成树
        lSystemLogic.Generate();
    }

    private void UpdateUI()
    {
        // 更新文本显示当前的滑块值
        interactionsText.text = "Interactions: " + lSystemLogic.interactions.ToString();
        angleText.text = "Angle: " + lSystemLogic.angle.ToString("F2");  // 保留两位小数
        lengthText.text = "Length: " + lSystemLogic.length.ToString("F2"); // 保留两位小数

        // 更新 axiom 显示
        axiomText.text = "Axiom: " + lSystemLogic.axiom;

        // 更新规则显示
        rulesText.text = "Rules:\n";
        foreach (var rule in lSystemLogic.rules)
        {
            rulesText.text += rule.Key + " -> " + rule.Value + "\n"; // 显示每个规则的映射
        }
    }


    private void ResetTree()
    {
        // 恢复默认参数
        lSystemLogic.axiom = "X";  // 初始的公设（起始符号）
        lSystemLogic.rules = new Dictionary<char, string> { { 'X', "F[+X]F[-X]FX" }, { 'F', "FF" } };  // 默认规则
        lSystemLogic.interactions = 4;  // 默认迭代次数
        lSystemLogic.angle = 30f;  // 默认角度
        lSystemLogic.length = 5f;  // 默认分支长度

        // 重新生成树
        lSystemLogic.Generate();

        UpdateUI();
    }
}