using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtility
{
    // todo : y�� ���̳ʽ��� ��� ����� ������ �� �Ǳ� ������ ������ ����
    // note : ��� �켱������ ���߱� ���� z�������� ������ worldPosition�� ��ȯ
    public static Vector3 GetCorrectGridWorldPosition(Vector3 worldPosition) {
        return new Vector3(worldPosition.x, worldPosition.y, worldPosition.y * 4);
    }
}
