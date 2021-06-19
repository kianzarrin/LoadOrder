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
    using ColossalFramework.PlatformServices;

    public class StatusButton : UIButton {
        const string bgSpriteHoveredName = "Hovered";
        const string bgSpritePressedName = "Pressed";

        public string AtlasName => $"{GetType().FullName}_{nameof(StatusButton)}_rev" + typeof(StatusButton).VersionOf();
        public const int SIZE = 80;

        public EntryData EntryData;

        public override void Awake() {
            try {
                base.Awake();
                isVisible = true;
                size = new Vector2(SIZE, SIZE);
                canFocus = false;
                name = nameof(StatusButton);
                SetupSprites();
                isVisible = false;
                isEnabled = true;
            } catch (Exception ex) { Log.Exception(ex); }
        }

        public override void OnDestroy() {
            this.SetAllDeclaredFieldsToNull();
            base.OnDestroy();
        }

        static UITextureAtlas atlas_;
        public void SetupSprites() {
            TextureUtil.EmbededResources = false;
            try {
                string[] spriteNames = new string[] {
                    nameof(SteamUtilities.IsUGCUpToDateResult.NotDownloaded) ,
                    nameof(SteamUtilities.IsUGCUpToDateResult.PartiallyDownloaded),
                    nameof(SteamUtilities.IsUGCUpToDateResult.OutOfDate),
                    bgSpriteHoveredName,
                    bgSpritePressedName,
                };
                atlas = atlas_ ??= TextureUtil.CreateTextureAtlas("Resources/Status.png" , AtlasName, spriteNames);

                hoveredBgSprite = bgSpriteHoveredName;
                pressedBgSprite = bgSpritePressedName;

            } catch (Exception ex) {
                Log.Exception(ex);
            }
        }

        public void SetStatus(SteamUtilities.IsUGCUpToDateResult status, string result) {
            LogCalled(status, result);
            isVisible = status != SteamUtilities.IsUGCUpToDateResult.OK;
            disabledFgSprite = focusedFgSprite = normalFgSprite = hoveredFgSprite = pressedFgSprite = status.ToString();
            tooltip = result;
        }

        protected override void OnClick(UIMouseEventParameter p) {
            p.Use();
            if(EntryData != null && EntryData.publishedFileId != PublishedFileId.invalid) {
                Settings.CheckSubsUtil.Instance.Resubscribe(EntryData.publishedFileId);
            }

            base.OnClick(p);
        }
    }
}