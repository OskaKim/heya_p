using UnityEngine;

public class FurnitureDecorateModel : BaseModel<FurnitureDecorateModel>
{
    public void DecorateFurniture(int furnitureId)
    {
        Debug.Log($"{furnitureId}의 데코");

        // todo : 마스터 데이터에서 가구를 특정해서 데코레이트 위치에 꾸밀 아이템을 배치
    }
}