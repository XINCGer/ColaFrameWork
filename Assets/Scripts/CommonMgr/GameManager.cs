﻿//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColaFramework
{
    /// <summary>
    /// 游戏核心管理中心
    /// </summary>
    public class GameManager
    {

        private static GameManager instance;
        private bool init = false;

        /// <summary>
        /// Launcher的Obj
        /// </summary>
        private GameObject gameLauncherObj;

        /// <summary>
        /// 系统管理器
        /// </summary>
        private ModuleMgr moduleMgr;

        /// <summary>
        /// 场景/关卡管理器
        /// </summary>
        private SceneMgr sceneMgr;

        /// <summary>
        /// 资源管理器
        /// </summary>
        private ResourcesMgr resourceMgr;

        /// <summary>
        /// 音频管理器
        /// </summary>
        private AudioManager audioManager;

        /// <summary>
        /// 计时器管理器
        /// </summary>
        private TimerManager timerManager;

        /// <summary>
        /// 网络消息处理器
        /// </summary>
        private NetMessageCenter netMessageCenter;

        /// <summary>
        /// UI管理器
        /// </summary>
        private UIMgr uiMgr;

        private InputMgr inputMgr;

        private LuaClient luaClient;

        private GameManager()
        {

        }

        public static GameManager Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// 初始化游戏核心
        /// </summary>
        public void InitGameCore(GameObject gameObject)
        {
            init = false;

            //初始化各种管理器
            resourceMgr = ResourcesMgr.GetInstance();

            gameLauncherObj = gameObject;
            sceneMgr = gameObject.AddComponent<SceneMgr>();
            audioManager = AudioManager.Instance;
            timerManager = TimerManager.Instance;
            inputMgr = gameLauncherObj.AddComponent<InputMgr>();
            netMessageCenter = NetMessageCenter.Instance;

            GameStart();
        }

        /// <summary>
        /// 游戏模块开始运行入口
        /// </summary>
        public void GameStart()
        {
            audioManager.Init();
            timerManager.Init();

            //将lua初始化移动到这里，所有的必要条件都准备好以后再初始化lua虚拟机
            luaClient = gameLauncherObj.AddComponent<LuaClient>();

            init = true;
        }

        /// <summary>
        /// 模拟 Update
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (!init) return;
            resourceMgr.Update(deltaTime);
            timerManager.Update(deltaTime);
            audioManager.Update(deltaTime);
            netMessageCenter.Update(deltaTime);
        }

        /// <summary>
        /// 模拟 LateUpdate
        /// </summary>
        /// <param name="deltaTime"></param>
        public void LateUpdate(float deltaTime)
        {
            if (!init) return;
        }

        /// <summary>
        /// 模拟 FixedUpdate
        /// </summary>
        /// <param name="fixedDeltaTime"></param>
        public void FixedUpdate(float fixedDeltaTime)
        {
            if (!init) return;
        }

        public void OnApplicationQuit()
        {

        }

        public void OnApplicationPause(bool pause)
        {

        }

        public void OnApplicationFocus(bool focus)
        {

        }

        /// <summary>
        /// 获取系统管理器
        /// </summary>
        /// <returns></returns>
        public ModuleMgr GetModuleMgr()
        {
            if (null != moduleMgr)
            {
                return moduleMgr;
            }
            Debug.LogWarning("subSysMgr构造异常");
            return null;
        }

        /// <summary>
        /// 获取UI管理器
        /// </summary>
        /// <returns></returns>
        public UIMgr GetUIMgr()
        {
            if (null != uiMgr)
            {
                return uiMgr;
            }
            Debug.LogWarning("uiMgr构造异常");
            return null;
        }

        public SceneMgr GetSceneMgr()
        {
            if (null != sceneMgr)
            {
                return sceneMgr;
            }
            Debug.LogWarning("sceneMgr构造异常");
            return null;
        }

        public LuaClient GetLuaClient()
        {
            if(null != luaClient)
            {
                return luaClient;
            }
            Debug.LogWarning("luaClient构造异常");
            return null;
        }

        /// <summary>
        /// 退出游戏
        /// </summary>
        public void ApplicationQuit()
        {
            Application.Quit();
        }

    }
}

