using UnityEngine;

public class TreeRotationXController : MonoBehaviour
{
    // �� CameraRotation �����ñ�¶�� Inspector �����
    public Camera cameraToRotate; // ��ת�������
    public float rotationSpeed = 10f; // ��ת�ٶ�

    // �󶨵�����ؼ�
    public UnityEngine.UI.Slider rotationSlider;

    private void Start()
    {
        // ���ó�ʼֵ��ȷ������ֵ��Ӧ��ȷ����ת�Ƕ�
        if (rotationSlider != null)
        {
            rotationSlider.onValueChanged.AddListener(OnRotationSliderChanged);
        }
    }

    // ������ֵ�ı�ʱ����
    private void OnRotationSliderChanged(float value)
    {
        // �� Y ����ת����������߶Ȳ���
        RotateCameraAroundOrigin(value);
    }

    // �������Χ�� (0, 0, 0) Ϊ������ת�ķ���
    private void RotateCameraAroundOrigin(float rotationValue)
    {
        // �������ֵת��Ϊ�������ת�Ƕ�
        float rotationAngle = rotationValue * 360f;

        // ���������Ŀ��λ��
        Vector3 newPosition = new Vector3(
            Mathf.Sin(rotationAngle * Mathf.Deg2Rad) * 250f,
            130f, // ��������߶Ȳ���
            Mathf.Cos(rotationAngle * Mathf.Deg2Rad) * 250f
        );

        // ���������λ��
        cameraToRotate.transform.position = newPosition;

        // �������ʼ�ճ��� (0, 0, 0)
        cameraToRotate.transform.LookAt(Vector3.zero);

        // ȷ������� X ����תʼ��Ϊ 0������ԭʼ���ӽ���ת
        Vector3 eulerRotation = cameraToRotate.transform.rotation.eulerAngles;
        eulerRotation.x = 0; // �� X �����ת����Ϊ 0
        cameraToRotate.transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
