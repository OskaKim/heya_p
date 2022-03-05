using System;
using System.Collections.Generic;
using System.Reflection;

public class ModelInfoHolder
{
    /// <summary>
    /// 사용중인 모델의 참조 개수가 카운트 되어있음
    /// </summary>
    /// <typeparam name="Type">모델 타입</typeparam>
    /// <typeparam name="int">참조되고 있는 컨트롤러의 개수</typeparam>
    /// <returns></returns>
    private Dictionary<Type, int> usingModelCounts = new Dictionary<Type, int>();

    public void AddModel<T>(out T modelInstance)
    {
        Type modelType = typeof(T);

        int usingCount = 0;
        if (usingModelCounts.TryGetValue(modelType, out usingCount))
        {
            // 이미 사용중인 모델은 카운트를 +1
            usingModelCounts[modelType] = ++usingCount;
        }
        else
        {
            // 처음으로 사용되는 모델
            usingModelCounts.Add(modelType, 1);
        }

        // someModel -> baseModel -> singleton
        Type singleton = typeof(T).BaseType.BaseType;

        PropertyInfo propertyInfo = singleton.GetProperty("instance");

        try
        {
            modelInstance = (T)propertyInfo.GetValue(null);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}