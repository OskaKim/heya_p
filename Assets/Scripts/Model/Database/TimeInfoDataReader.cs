using UnityEngine;
using System;
namespace DataBase
{
    public class TimeInfoDataReader : MonoBehaviour
    {
        public void setup(out TimeInfoDataEntity timeInfoDataEntity)
        {
            var timeInfoDataJson = Utility.FileIOUtility.LoadJsonFile<TimeInfoDataJson>("SaveFile/Test", "timeInfoData");
            timeInfoDataEntity.currentTime = DateTime.FromOADate(timeInfoDataJson.currentTime);
            timeInfoDataEntity.timeIntervalSpeed = timeInfoDataJson.timeIntervalSpeed;
        }
    }
}