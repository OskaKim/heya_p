using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
using System.Linq;
using DataBase;
using System.Collections.ObjectModel;
namespace timeinfo
{
    public class TimeInfoModel
    {
        private Subject<DateTime> onUpdateGameTime = new Subject<DateTime>();
        public IObservable<DateTime> OnUpdateGameTime { get => onUpdateGameTime; }
        private DateTime currentGameTime;

        public TimeInfoModel(){
            Debug.Log("TimeInfoModel");
        }
        public void Tick()
        {
            var timeInfoDatabase = DataBase.MasterDataHolder.TimeInfoDatabase;
            currentGameTime = currentGameTime.AddSeconds(timeInfoDatabase.timeIntervalSpeed);
            onUpdateGameTime.OnNext(currentGameTime);
        }
    }
}