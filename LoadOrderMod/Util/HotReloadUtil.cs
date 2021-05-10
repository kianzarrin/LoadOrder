namespace LoadOrderMod.Util {
    using ColossalFramework;
    using ColossalFramework.UI;
    using KianCommons;
    using KianCommons.Plugins;
    using System;
    using System.Collections.Generic;
    using static ColossalFramework.Plugins.PluginManager;
    using static KianCommons.ReflectionHelpers;
    using UnityEngine;
    using ICities;
    using System.Reflection;
    using HarmonyLib;
    using System.Reflection.Emit;
    using ColossalFramework.Plugins;
    using System.Linq;

    public static class HotReloadUtil {
        static OptionsMainPanel optionsMainPanel_ => Singleton<OptionsMainPanel>.instance;
        public static List<UIComponent> m_Dummies => GetFieldValue(optionsMainPanel_, "m_Dummies") as List<UIComponent>;
        public static UIListBox m_Categories => GetFieldValue(optionsMainPanel_, "m_Categories") as UIListBox;
        public static UITabContainer m_CategoriesContainer => GetFieldValue(optionsMainPanel_, "m_CategoriesContainer") as UITabContainer;


        static TDelegate CreateDelegate<TDelegate>(object instance, string name) where TDelegate : Delegate {
            var method = GetMethod(instance.GetType(), name);
            return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), instance, method);
        }

        public static PluginsChangedHandler RefreshPlugins =>
            CreateDelegate<PluginsChangedHandler>(optionsMainPanel_, "RefreshPlugins");

        public static PropertyChangedEventHandler<int> OnCategoryChanged =>
            CreateDelegate<PropertyChangedEventHandler<int>>(optionsMainPanel_, "OnCategoryChanged");


        public static void DropCategory(string name) {
            LogCalled();
            if (name == null) throw new ArgumentNullException("name");
            int index = m_Categories.items.IndexOf(name);
            if (index < 0) return;
            int selectedIndex = m_Categories.selectedIndex;

            Log.Info("Dropping category :" + name);
            var category = m_Dummies.Find(c => c.name == name);
            m_Dummies.Remove(category);
            GameObject.DestroyImmediate(category);
            m_Categories.items = m_Categories.items.RemoveAt(index);

            m_Categories.selectedIndex = selectedIndex;
        }


        public static void AddCategory(PluginInfo p) {
            LogCalled();
            if (!p.isEnabled)
                return;
            if (p?.GetUserModInstance() is not IUserMod userMod)
                return;

            try {
                string name = p.name;
                MethodInfo mOnSettingsUI = userMod.GetType().GetMethod("OnSettingsUI", BindingFlags.Instance | BindingFlags.Public);
                if (mOnSettingsUI != null) {
                    Log.Info("Adding category :" + name);
                    UIComponent category = m_CategoriesContainer.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsScrollPanelTemplate"));
                    category.name = userMod.Name;
                    m_Dummies.Add(category);

                    mOnSettingsUI.Invoke(userMod, new object[] { new UIHelper(category.Find("ScrollContent")) });
                    m_Categories.items = m_Categories.items.AddToArray(name);
                    category.zOrder = m_Categories.items.Length - 1;
                }
            } catch (Exception ex) {
                Log.Exception(ex);
                
            }
        }


    }
}
