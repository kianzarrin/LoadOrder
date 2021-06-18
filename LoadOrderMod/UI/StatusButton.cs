namespace LoadOrderMod.UI {
    extern alias Injections;
    using Injections.LoadOrderInjections;
    using ColossalFramework;
    using ColossalFramework.UI;
    using KianCommons;
    using KianCommons.UI;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using static KianCommons.ReflectionHelpers;
    
    public class StatusButton : UIButton {
        static string bgSpriteHovered = "Hovered";
        static string bgSpritePressed = "Pressed";

        public string AtlasName => $"{GetType().FullName}_{nameof(StatusButton)}_rev" + typeof(StatusButton).VersionOf();
        public const int SIZE = 80;

        public override void Awake() {
            try {
                base.Awake();
                isVisible = true;
                size = new Vector2(SIZE, SIZE);
                canFocus = false;
                name = nameof(StatusButton);
                SetupSprites();
                isVisible = false;
            } catch (Exception ex) { Log.Exception(ex); }
        }

        public override void OnDestroy() {
            this.SetAllDeclaredFieldsToNull();
            base.OnDestroy();
        }

        static UITextureAtlas atlas_;
        public UITextureAtlas SetupSprites() {
            TextureUtil.EmbededResources = false;
            try {
                string[] spriteNames = new string[] {
                    nameof(SteamUtilities.IsUGCUpToDateResult.NotDownloaded) ,
                    nameof(SteamUtilities.IsUGCUpToDateResult.PartiallyDownloaded),
                    nameof(SteamUtilities.IsUGCUpToDateResult.OutOfDate),
                    bgSpriteHovered,
                    bgSpritePressed,
                };
                atlas_ ??= TextureUtil.CreateTextureAtlas("Resources/Status.png" , AtlasName, spriteNames);
                return atlas = atlas_;
            } catch (Exception ex) {
                Log.Exception(ex);
                return TextureUtil.Ingame;
            }
        }

        public void SetStatus(SteamUtilities.IsUGCUpToDateResult status, string result) {
            isVisible = status == SteamUtilities.IsUGCUpToDateResult.OK;
            if (isVisible) {
                normalFgSprite = hoveredFgSprite = pressedFgSprite = status.ToString();
            }
            tooltip = result;
        }
    }
}