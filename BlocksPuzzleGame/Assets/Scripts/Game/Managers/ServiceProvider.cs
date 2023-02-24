using Data;
using Game.Core;
using Game.Core.GridSystem;
using System;
using System.Collections.Generic;

namespace Game.Managers
{
    public static class ServiceProvider
    {
        private static readonly Dictionary<Type, IProvidable> RegisterDictionary = new Dictionary<Type, IProvidable>();

        public static T GetManager<T>() where T : class, IProvidable
        {
            if (RegisterDictionary.ContainsKey(typeof(T)))
            {
                return (T)RegisterDictionary[typeof(T)];
            }

            return null;
        }

        public static T Register<T>(T target) where T : class, IProvidable
        {
            if (RegisterDictionary.ContainsKey(typeof(T)))
            {
                RegisterDictionary.Remove(typeof(T));
            }
            RegisterDictionary.Add(typeof(T), target);
            return target;
        }

        public static DataHandler GetDataHandler
        {
            get { return GetManager<DataHandler>(); }
        }
        public static ImageManager GetImageManager
        {
            get { return GetManager<ImageManager>(); }
        }
        public static BlocksVisualizer GetBlocksVisualizer
        {
            get { return GetManager<BlocksVisualizer>(); }
        }
        public static LevelManager GetLevelManager
        {
            get { return GetManager<LevelManager>(); }
        }
        public static BlocksManager GetBlocksManager
        {
            get { return GetManager<BlocksManager>(); }
        }
        public static GridObject GetGridObject
        {
            get { return GetManager<GridObject>(); }
        }
        public static ColliderDecider GetColliderDecider
        {
            get { return GetManager<ColliderDecider>(); }
        }

    }
}