using UnityEngine;

public class TreeRotationXController : MonoBehaviour
{
    // 将 CameraRotation 的引用暴露在 Inspector 面板中
    public Camera cameraToRotate; // 旋转的摄像机
    public float rotationSpeed = 10f; // 旋转速度

    // 绑定到滑块控件
    public UnityEngine.UI.Slider rotationSlider;

    private void Start()
    {
        // 设置初始值，确保滑块值对应正确的旋转角度
        if (rotationSlider != null)
        {
            rotationSlider.onValueChanged.AddListener(OnRotationSliderChanged);
        }
    }

    // 当滑块值改变时调用
    private void OnRotationSliderChanged(float value)
    {
        // 绕 Y 轴旋转，保持相机高度不变
        RotateCameraAroundOrigin(value);
    }

    // 控制相机围绕 (0, 0, 0) 为轴心旋转的方法
    private void RotateCameraAroundOrigin(float rotationValue)
    {
        // 将滑块的值转换为相机的旋转角度
        float rotationAngle = rotationValue * 360f;

        // 计算相机的目标位置
        Vector3 newPosition = new Vector3(
            Mathf.Sin(rotationAngle * Mathf.Deg2Rad) * 250f,
            130f, // 保持相机高度不变
            Mathf.Cos(rotationAngle * Mathf.Deg2Rad) * 250f
        );

        // 更新相机的位置
        cameraToRotate.transform.position = newPosition;

        // 保持相机始终朝向 (0, 0, 0)
        cameraToRotate.transform.LookAt(Vector3.zero);

        // 确保相机的 X 轴旋转始终为 0，保持原始的视角旋转
        Vector3 eulerRotation = cameraToRotate.transform.rotation.eulerAngles;
        eulerRotation.x = 0; // 将 X 轴的旋转设置为 0
        cameraToRotate.transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
