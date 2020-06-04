﻿//------------------------------------------------------------
// ScrollCircleMaker v1.0
// Copyright © 2020 DaveAnt. All rights reserved.
// Homepage: https://dagamestudio.top/
// Github: https://github.com/DaveAnt/ScollCircleMaker
//------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIPlugs.ScrollCircleMaker
{
    public struct BoundaryInt
    {
        public short dir;//方向1或-1
        public int area;//最大显示区域高或宽
        public int length;//数据向上取整的大小
    }
    public abstract class BaseCircleHelper<T>
    {
        protected List<T> _dataSet;
        protected List<BaseItem<T>> _itemSet;
        protected Func<BaseItem<T>> _createItemFunc;
        protected Action _toLocationEvent;//动画结束回调

        protected ScrollRect _scrollRect;
        protected RectTransform _viewRect, _contentRect, _itemRect;

        protected ScrollCircleComponent _sProperty;
        protected GameObject _baseItem;

        public event Action toLocationEvent 
        {
            add {
                _toLocationEvent += value;
            }
            remove
            {
                _toLocationEvent -= value;
            }
        }

        public ScrollCircleComponent sProperty
        {
            get{
                return _sProperty;
            }
        }

        public int itemCount
        {
            get {
                if (_dataSet == null)
                    return 0;
                else
                    return _dataSet.Count;
            }
        }

        public abstract void OnStart(List<T> _tmpDataSet = null);//启动
        
        public virtual void OnDestroy()
        {
            _toLocationEvent = null;
            _createItemFunc = null;
            _scrollRect.onValueChanged.RemoveListener(OnRefreshHandler);
            _dataSet.Clear();
            _itemSet.Clear();
            GC.Collect();
        }
        public virtual void OnUpdate()
        {
            if (_itemSet == null) return;
            foreach (BaseItem<T> baseItem in _itemSet)
                baseItem.OnUpdate();
        }
        public virtual void OnSwitchSlide(bool state)
        {
            try{
                _scrollRect.enabled = state;
            }
            catch (Exception e){
                Debug.LogError("_scrollRect.enabled = ..." + e.Message);
            }
        }
        protected abstract void OnRefreshHandler(Vector2 v2);//刷新监听方式
        public abstract void DelItem(int itemIdx);//移除数据
        public abstract void DelItem(T data);//移除数据
        public abstract void AddItem(T data, int itemIdx = -1);//添加数据
        public abstract void UpdateItem(T data,int itemIdx);
        public abstract void ResetItems();//清空数据
        public abstract int GetLocation();//获取定位
        public abstract void ToLocation(int toSeat, bool isDrawEnable = true);//定位
        public abstract void ToTop(bool isDrawEnable = true);//置顶 true存在过程动画
        public abstract void ToBottom(bool isDrawEnable = true);//置底
    }
}
