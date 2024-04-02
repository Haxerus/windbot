using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindBot.Game.AI.Decks
{
    [Deck("Numeron", "AI_Numeron")]
    public class NumeronExecutor : DefaultExecutor
    {

        public class CardId
        {
            // Effect Monsters
            public const int LavaGolem = 00102380;
            public const int Gameciel = 55063751;
            public const int PlanetPathfinder = 97526666;
            public const int AshBlossom = 14558127;
            //public const int GhostMourner = 52038441;
            public const int GhostBelle = 73642296;
            public const int NumeronWall = 42352091;
            public const int EffectVeiler = 97268402;

            // Spells
            public const int Raigeki = 12580477;
            public const int LightningStorm = 14532163;
            public const int FeatherDuster = 18144506;
            public const int Terraforming = 73628505;
            public const int NumeronCalling = 77402960;
            public const int CosmicCyclone = 08267140;
            public const int LimiterRemoval = 23171610;
            public const int ForbiddenDroplet = 24299458;
            public const int NumeronNetwork = 41418852;

            // Imperm lol
            public const int Impermanence = 10045474;

            // Extra
            // Xyz
            public const int NC1_Sunya = 79747096;
            public const int N4_Catvari = 04019153;
            public const int N1_Ekam = 15232745;
            public const int N2_Dve = 42230449;
            public const int N3_Trini = 78625448;

            // Link
            public const int UnderworldGoddess = 98127546;
            public const int Avramax = 21887175;
            public const int AccesscodeTalker = 86066372;
            public const int Apollousa = 04280258;
            public const int Megaclops = 69073023;

            // Other Cards not in Deck
            public const int Kaiju_Dogoran = 93332803;
            public const int Kaiju_Gadarla = 36956512;
            public const int Kaiju_Jizukiru = 63941210;
            public const int Kaiju_Kumongous = 29726552;
            public const int Kaiju_Radian = 28674152;
            public const int Kaiju_MechaDogoran = 84769941;
            public const int Kaiju_MechaThunderKing = 29913783;
            public const int Kaiju_ThunderKing = 48770333;
        }

        private readonly int[] KaijuList = new[]
        {
            CardId.Kaiju_Dogoran,
            CardId.Kaiju_Gadarla,
            CardId.Kaiju_Jizukiru,
            CardId.Kaiju_Kumongous,
            CardId.Kaiju_Radian,
            CardId.Kaiju_MechaDogoran,
            CardId.Kaiju_MechaThunderKing,
            CardId.Kaiju_ThunderKing
        };

        private bool summonedPathfinder = false;
        private bool sentCallingToGY = false;
        private bool usedCallingEffect = false;

        public NumeronExecutor(GameAI ai, Duel duel) : base(ai, duel)
        {
            // Summons
            AddExecutor(ExecutorType.Summon, CardId.PlanetPathfinder, PlanetPathfinderSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.LavaGolem, LavaGolemSummon);
            AddExecutor(ExecutorType.SpSummon, CardId.Gameciel, GamecielSummon);

            // Field Searchers
            AddExecutor(ExecutorType.Activate, CardId.PlanetPathfinder, NeedToSearchField);
            AddExecutor(ExecutorType.Activate, CardId.NumeronWall, NeedToSearchField);

            AddExecutor(ExecutorType.Activate, CardId.NumeronNetwork, NumeronNetworkActivate);

            // Hand Traps
            AddExecutor(ExecutorType.Activate, CardId.AshBlossom, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.GhostBelle, DefaultGhostBelleAndHauntedMansion);

            // Imperm lol
            AddExecutor(ExecutorType.Activate, CardId.Impermanence, DefaultInfiniteImpermanence);

            AddExecutor(ExecutorType.Activate, CardId.CosmicCyclone, DefaultCosmicCyclone);
            
        }

        public override bool OnSelectHand()
        {
            // Always go second
            return false;
        }

        private bool LavaGolemSummon()
        {
            return Enemy.GetMonsterCount() >= 2 && !summonedPathfinder;
        }

        private bool GamecielSummon()
        {
            bool hasKaiju = false;
            foreach (var i in KaijuList)
            {
                if (Enemy.GetMonsters().ContainsCardWithId(i)) hasKaiju = true;
            }
            
            return !hasKaiju;
        }

        private bool PlanetPathfinderSummon()
        {
            if (Bot.HasInHand(CardId.LavaGolem)) {
                if (Bot.HasInHand(CardId.NumeronWall)
                || Bot.HasInHand(CardId.Terraforming)
                || Bot.HasInHand(CardId.NumeronNetwork))
                {
                    return false;
                }
            }

            summonedPathfinder = true;
            return true;
        }

        private bool NeedToSearchField()
        {
            return Bot.IsFieldEmpty() && !Bot.HasInHand(CardId.NumeronNetwork);
        }

        private bool NumeronNetworkActivate()
        {
            // Play from hand
            if (Bot.IsFieldEmpty() && Bot.HasInHand(CardId.NumeronNetwork)) return true;

            // Using Network's base effect
            if (!sentCallingToGY)
            {
                AI.SelectCard(CardId.NumeronCalling);
                sentCallingToGY = true;
                return true;
            }

            // Using Calling's inherited effect
            if (sentCallingToGY && !usedCallingEffect) {
                AI.SelectCard(CardId.NC1_Sunya, CardId.N2_Dve, CardId.N3_Trini, CardId.N4_Catvari);
                usedCallingEffect = true;
                return true;
            }

            return false;
        }
    }
}
