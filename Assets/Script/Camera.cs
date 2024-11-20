using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform Player;  //�v���C���[��Transform���擾
    public float CameraSpeed = 0.125f; //�J�����Ǐ]�̑��x
    public float CameraPedX;
    public Vector3 offset; //�J�����ƃv���C���[�̈ʒu�I�t�Z�b�g

    [Header("�J����X���̈ړ������͈�")]
    public float MinX = 0f;  // X���̍ŏ��l�i���[�j
    public float MaxX = 10f;   // X���̍ő�l�i�E�[�j


    private void LateUpdate()
    {
        // �J������Y����Z���͌Œ肵�āAX���̂ݒǏ]
        Vector3 targetPosition = new Vector3(Player.position.x + offset.x, transform.position.y, transform.position.z);

        // X���͈̔͂𐧌�����
        CameraPedX = Mathf.Clamp(targetPosition.x, MinX, MaxX);

        // �X���[�Y�ɒǏ]�����邽�߂̕⊮����
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(CameraPedX, targetPosition.y, targetPosition.z), CameraSpeed);
        transform.position = smoothedPosition;
    }
}
