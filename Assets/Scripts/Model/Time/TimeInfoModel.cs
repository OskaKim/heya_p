using UniRx;
using UnityEngine;
using System;
namespace timeinfo
{
    public class TimeInfoModel : BaseModel<TimeInfoModel>
    {
        private Subject<DateTime> onUpdateGameTime = new Subject<DateTime>();
        public IObservable<DateTime> OnUpdateGameTime { get => onUpdateGameTime; }
        private DateTime currentGameTime;
        public void Tick()
        {
            var timeInfoDatabase = DataBase.MasterDataHolder.TimeInfoDatabase;
            currentGameTime = currentGameTime.AddSeconds(timeInfoDatabase.timeIntervalSpeed);
            onUpdateGameTime.OnNext(currentGameTime);
        }
    }
}