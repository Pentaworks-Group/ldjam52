using System;
using System.Collections.Generic;

namespace Assets.Scripts.Constants
{
    public static class SceneNames
    {
        public const String MainMenu = "MainMenuScene";
        public const String GameMode = "GameModeScene";
        public const String SavedGames = "SaveGameScene";
        public const String Options = "OptionsScene";
        public const String World = "WorldScene";
        public const String Credits = "CreditsScene";
        public const String TestField = "FieldTestScene";
        public const String Shop = "SeedShopScene";


        public static List<String> scenes = new() { MainMenu, SavedGames, Options, Credits, World };
        public static List<String> scenesDevelopment = new() { TestField, Shop, GameMode};
    }
}
