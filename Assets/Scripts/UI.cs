using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;  // ���� TextMeshPro �����ռ�

public class LSystemUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider interactionsSlider;
    [SerializeField] private Slider angleSlider;
    [SerializeField] private Slider lengthSlider;

    [Header("Text UI Components")]
    [SerializeField] private TMP_Text interactionsText;  // ������ʾ interactions ֵ�� TextMeshPro
    [SerializeField] private TMP_Text angleText;         // ������ʾ angle ֵ�� TextMeshPro
    [SerializeField] private TMP_Text lengthText;        // ������ʾ length ֵ�� TextMeshPro
    [SerializeField] private TMP_Text axiomText;  // ������ʾ axiom �� TextMeshPro
    [SerializeField] private TMP_Text rulesText;  // ������ʾ rules �� TextMeshPro


    [Header("L-System Logic")]
    [SerializeField] private LSystemLogic lSystemLogic;

    [Header("Reset Button")]
    [SerializeField] private Button resetButton;  // ���ð�ť

    private void Start()
    {
        resetButton.onClick.AddListener(ResetTree);// �����ð�ť�¼�

        // ��ʼ������ֵ
        interactionsSlider.value = lSystemLogic.interactions;
        angleSlider.value = lSystemLogic.angle;
        lengthSlider.value = lSystemLogic.length;

        // ��ʼ����ʾ�ı�
        UpdateUI();

        // �󶨻����¼�
        interactionsSlider.onValueChanged.AddListener(OnSliderValueChanged);
        angleSlider.onValueChanged.AddListener(OnSliderValueChanged);
        lengthSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // ���� L-System ����
        lSystemLogic.interactions = Mathf.RoundToInt(interactionsSlider.value);
        lSystemLogic.angle = angleSlider.value;
        lSystemLogic.length = lengthSlider.value;

        // ���� UI ��ʾ
        UpdateUI();


        // ����������
        lSystemLogic.Generate();
    }

    private void UpdateUI()
    {
        // �����ı���ʾ��ǰ�Ļ���ֵ
        interactionsText.text = "Interactions: " + lSystemLogic.interactions.ToString();
        angleText.text = "Angle: " + lSystemLogic.angle.ToString("F2");  // ������λС��
        lengthText.text = "Length: " + lSystemLogic.length.ToString("F2"); // ������λС��

        // ���� axiom ��ʾ
        axiomText.text = "Axiom: " + lSystemLogic.axiom;

        // ���¹�����ʾ
        rulesText.text = "Rules:\n";
        foreach (var rule in lSystemLogic.rules)
        {
            rulesText.text += rule.Key + " -> " + rule.Value + "\n"; // ��ʾÿ�������ӳ��
        }
    }


    private void ResetTree()
    {
        // �ָ�Ĭ�ϲ���
        lSystemLogic.axiom = "X";  // ��ʼ�Ĺ��裨��ʼ���ţ�
        lSystemLogic.rules = new Dictionary<char, string> { { 'X', "F[+X]F[-X]FX" }, { 'F', "FF" } };  // Ĭ�Ϲ���
        lSystemLogic.interactions = 4;  // Ĭ�ϵ�������
        lSystemLogic.angle = 30f;  // Ĭ�ϽǶ�
        lSystemLogic.length = 5f;  // Ĭ�Ϸ�֧����

        // ����������
        lSystemLogic.Generate();

        UpdateUI();
    }
}