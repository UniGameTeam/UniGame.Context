﻿namespace UniModules.UniGame.Context.GameSceneContext.Runtime {
    using System;

    [Serializable]
    public struct SceneInfo {
        public int         handle;
        public string      name;
        public string      path;
        public bool        isActive;
        public SceneStatus status;
    }
}