﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 参与排序的元素
/// </summary>
public class UISorter
{
    public UIBase ui;
    public int moveTop;
    public int inedx;

    public UISorter(UIBase ui, int moveTop, int index)
    {
        this.ui = ui;
        this.moveTop = moveTop;
        this.inedx = index;
    }
}

/// <summary>
/// UI排序管理器
/// </summary>
public class UISorterMgr : ISorter
{
    private int minSortIndex = 0;
    private int maxSortIndex = 0;
    private List<UISorter> uiSortList;
    private List<Canvas> canvasSortList;

    public UISorterMgr(int minIndex, int maxIndex)
    {
        minSortIndex = minIndex;
        maxSortIndex = maxIndex;
        uiSortList = new List<UISorter>();
        canvasSortList = new List<Canvas>();
    }

    /// <summary>
    /// UI排序管理器构造器,序号越大，界面越靠上
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="sortIndex"></param>
    /// <returns></returns>
    public int SortIndexSetter(GameObject panel, int sortIndex)
    {
        if (null == panel)
        {
            Debug.LogWarning("参与排序的ui不能为空！");
            return 0;
        }
        canvasSortList.Clear();
        var canvasList = panel.GetComponentsInChildren<Canvas>(true);
        for (int i = 0; i < canvasList.Length; i++)
        {
            canvasSortList.Add(canvasList[i]);
        }
        canvasSortList.Sort((x, y) => { return x.sortingOrder.CompareTo(y.sortingOrder); });

        for (int i = 0; i < canvasSortList.Count; i++)
        {
            canvasSortList[i].sortingOrder = sortIndex;
            //DropDown组件关闭按钮的层级为DropDown层级减一，所以多加一个间隔
            sortIndex += 2;
        }
        return sortIndex + 2;
    }

    public int SortTagIndexSetter(GameObject panel, int sortIndex)
    {
        if (null == panel)
        {
            Debug.LogWarning("参与排序的ui不能为空！");
            return 0;
        }

        var sortTag = panel.GetComponent<SorterTag>();
        if (null == sortTag)
        {
            return sortIndex;
        }
        sortTag.SetSorter(sortIndex + 1);
        sortIndex = sortTag.GetSorter();
        return sortIndex;
    }

    public int SortTag3DSetter(GameObject model, int z)
    {
        if (null == model)
        {
            return 0;
        }
        if (!model.activeSelf)
        {
            return z;
        }
        var sortTag = model.GetComponent<SorterTag>();
        if (null == sortTag)
        {
            return z;
        }
        int space3D = sortTag.Space3D;
        sortTag.SetSpace3D(z);
        z += space3D;
        return z;
    }

    public void MovePanelToTop(UIBase ui)
    {
        int index;
        if (IsContainSorter(ui, out index))
        {
            uiSortList[index].moveTop = 1;
            ReSortPanels();
        }
        else
        {
            Debug.LogWarning(string.Format("UISortMgr中不包含 {0},MoveToTop-UI面板失败！", ui.Name));
        }
    }

    public void ReSortPanels()
    {
        throw new System.NotImplementedException();
    }

    public void AddPanel(UIBase ui, UILevel uiLevel)
    {
        if (null == ui)
        {
            Debug.LogWarning("添加到UISortMgr中的ui不能为空！");
            return;
        }

        int index;
        if (IsContainSorter(ui, out index))
        {
            Debug.LogWarning(string.Format("{0}已经在uiSortList中添加过了！", ui.Name));
            return;
        }
        uiSortList.Add(new UISorter(ui, 0, 0));
        ReSortPanels();
    }

    public void RemovePanel(UIBase ui)
    {
        if (null == ui)
        {
            Debug.LogWarning("UISortMgr中待移除的ui不能为空！");
            return;
        }

        int index;
        if (!IsContainSorter(ui, out index))
        {
            Debug.LogWarning(string.Format("UISortMgr中不包含 {0},移除UI面板失败！", ui.Name));
            return;
        }
        uiSortList.RemoveAt(index);
        ReSortPanels();
    }

    /// <summary>
    /// 判断一个ui是否在uiSortList中，并返回索引
    /// </summary>
    /// <param name="ui"></param>
    /// <returns></returns>
    private bool IsContainSorter(UIBase ui, out int index)
    {
        if (null != uiSortList)
        {
            for (int i = 0; i < uiSortList.Count; i++)
            {
                if (uiSortList[i].ui == ui)
                {
                    index = i;
                    return true;
                }
            }
        }
        index = -1;
        return false;
    }
}
