// Note: This file was automatically generated using Tools/GenerateEnums.
// No changes should be made to this file by hand. Instead, the relevant json
// files should be modified and then GenerateEnums should be used to regenerate
// the various ID enums.
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace UAlbion.Base
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SystemText : ushort
    {
        PartyPortrait_XLifeMana = 1,
        Combat_Attack = 2,
        Combat_Move = 3,
        Combat_UseMagic = 4,
        PartyPopup_MakeLeader = 5,
        PartyPopup_TalkTo = 6,
        MapPopup_TalkTo = 7,
        MapPopup_Examine = 8,
        MapPopup_Manipulate = 9,
        MapPopup_UseItem = 10,
        PartyPopup_UseMagic = 11,
        MapPopup_TravelOnFoot = 12,
        MapPopup_Rest = 13,
        Inv3_CombatPositions = 14,
        MapPopup_Environment = 15,
        InvPopup_Drop = 16,
        InvPopup_LearnSpell = 17,
        InvPopup_Examine = 18,
        InvPopup_Drink = 19,
        InvPopup_ActivateSpell = 20,
        InvPopup_Read = 21,
        Inv_LeaveCharacterScreen = 22,
        Item_X = 23,
        ItemType_Armor = 24,
        ItemType_Helmet = 25,
        ItemType_Shoes = 26,
        ItemType_Shield = 27,
        ItemType_CloseRangeWeapon = 28,
        ItemType_LongRangeWeapon = 29,
        ItemType_Ammo = 30,
        ItemType_Document = 31,
        ItemType_SpellScroll = 32,
        ItemType_Drink = 33,
        ItemType_Amulet = 34,
        ItemType_BreastPin = 35,
        ItemType_Ring = 36,
        ItemType_Valuable = 37,
        ItemType_Tool = 38,
        ItemType_Key = 39,
        ItemType_Normal = 40,
        ItemType_MagicalItem = 41,
        ItemType_SpecialItem = 42,
        ItemType_Transportation = 43,
        ItemType_Lockpick = 44,
        ItemType_Torch = 45,
        ItemType_ = 46,
        ItemType_2 = 47,
        Inv_Backpack = 48,
        Gold_NNGold = 49,
        Gold_NRations = 50,
        Gold_ThrowAway = 51,
        Gold_Gold = 52,
        Gold_TakeHowMuchGold = 53,
        Gold_Rations = 54,
        Inv_CharacterScreen1 = 55,
        Inv_CharacterScreen2 = 56,
        Inv_CharacterScreen3 = 57,
        Inv_I = 58,
        Inv_II = 59,
        Inv_III = 60,
        MsgBox_Yes = 61,
        MsgBox_No = 62,
        InvMsg_ReallyThrowThisItemAway = 63,
        InvMsg_ThisItemIsCursed = 64,
        MsgBox_OK = 65,
        InvMsg_CannotBeUnequippedDuringCombat = 66,
        InvMsg_TakeHowManyItems = 67,
        InvMsg_CannotBeUnequippedDuringCombat2 = 68,
        InvMsg_YouCannotEquipThisItem = 69,
        Inv3_SwapXAndX = 70,
        PartyPopup_CharacterScreen = 71,
        Lock_Lock = 72,
        Lock_PickTheLock = 73,
        Lock_OpenTheLock = 74,
        MapPopup_Take = 75,
        Meta_He = 76,
        Meta_She = 77,
        Meta_ItNominative = 78,
        Meta_HimAccusative = 79,
        Meta_HerAccusative = 80,
        Meta_ItAccusative = 81,
        Meta_His = 82,
        Meta_Her = 83,
        Meta_Its = 84,
        Meta_Man = 85,
        Meta_Woman = 86,
        Meta_Creature = 87,
        Magic_CastSpellOnWhichMember = 88,
        Magic_CastSpellOnWhichMonster = 89,
        Magic_CastSpellOnWhichRowOfMonsters = 90,
        Magic_CastSpellOnWhoseItem = 91,
        Magic_CastSpellOnWhichItem = 92,
        Magic_XCastsX = 93,
        Magic_ThisIsNotAMagicalItem = 94,
        Magic_ItemHasNoChargesLeft = 95,
        Class_Pilot = 96,
        Class_Scientist = 97,
        Class_Warrior = 98,
        Class_DjiKasMage = 99,
        Class_Druid = 100,
        Class_EnlightenedOne = 101,
        Class_Technician = 102,
        Class_OquloKamulos = 104,
        Class_Warrior2 = 105,
        Misc_IsUnconscious = 128,
        Misc_CanBeUsedBy = 129,
        Misc_DoesntSpeakTheRightLanguage = 130,
        MapPopup_Map = 131,
        InvPopup_Activate = 132,
        InvMsg_ThisItemIsAlreadyActivated = 133,
        Magic_CastSpellOnWhatTacticalSquare = 134,
        Magic_XCastsXWithX = 135,
        Misc_PointHasBeenMarked = 168,
        Automap_RiddleMouth = 169,
        Automap_Teleporter = 170,
        Automap_Spinner = 171,
        Automap_Trap = 172,
        Automap_TrapDoor = 173,
        Automap_Special = 174,
        Automap_Monster = 175,
        Automap_ClosedDoor = 176,
        Automap_OpenDoor = 177,
        Automap_Merchant = 178,
        Automap_Tavern = 179,
        Automap_ClosedChest = 180,
        Automap_Exit = 181,
        Automap_OpenChest = 182,
        Automap_TrashHeap = 183,
        Automap_Person = 184,
        InvMsg_HasNoChargesLeft = 185,
        Automap_Event = 186,
        InvMsg_WrongSpellArea = 187,
        InvMsg_NoOneKnowsThisSpellClass = 188,
        InvMsg_ThisSpellIsAlreadyKnown = 189,
        InvMsg_ThisSpellsLevelIsTooHigh = 190,
        InvMsg_XLearnedTheSpell = 191,
        InvMsg_ItemIsBroken = 192,
        InvMsg_ThrowHowManyItemsAway = 193,
        InvMsg_ThisIsAVitalItem = 194,
        InvMsg_ThisItemIsBroken = 195,
        InvMsg_ThisCharacterHasTheWrongClassForThisItem = 196,
        InvMsg_ThisCharacterHasTheWrongGenderForThisItem = 197,
        InvMsg_NotEnoughHandsFree = 198,
        Combat_Flee = 199,
        Combat_UseMagicItem = 200,
        Combat_AdvanceParty = 201,
        MsgBox_EnterNumber = 202,
        Combat_StartRound = 203,
        Spell0_0_ThornSnare = 204,
        Spell0_1_DELETED = 205,
        Spell0_2_DELETED = 206,
        Spell0_3_Hurry = 207,
        Spell0_4_ViewOfLife = 208,
        Spell0_5_FrostSplinter = 209,
        Spell0_6_FrostCrystal = 210,
        Spell0_7_FrostAvalanche = 211,
        Spell0_8_LightHealing = 212,
        Spell0_9_BlindingSpark = 213,
        Spell0_10_BlindingRay = 214,
        Spell0_11_BlindingStorm = 215,
        Spell0_12_SleepSpores = 216,
        Spell0_13_ThornTrap = 217,
        Spell0_14_RemoveTrap = 218,
        Spell0_15_DELETED = 219,
        Spell0_16_HealIntoxication = 220,
        Spell0_17_HealBlindness = 221,
        Spell0_18_HealPoisoning = 222,
        Spell0_19_Fungification = 223,
        Spell0_20_Light = 224,
        Spell1_0_Regeneration = 234,
        Spell1_1_MapView = 235,
        Spell1_2_Lifebringer = 236,
        Spell1_3_Teleporter = 237,
        Spell1_4_Healing = 238,
        Spell1_5_QuickWithdrawal = 239,
        Spell1_6_DELETED = 240,
        Spell1_7_DELETED = 241,
        Spell1_8_GoddesssWrath = 242,
        Spell1_9_Irritation = 243,
        Spell1_10_Recuperation = 244,
        Spell2_0_Berserk = 264,
        Spell2_1_BanishDemon = 265,
        Spell2_2_BanishDemons = 266,
        Spell2_3_DemonExodus = 267,
        Spell2_4_SmallFireball = 268,
        Spell2_5_MagicShield = 269,
        Spell2_6_Healing = 270,
        Spell2_7_Boasting = 271,
        Spell2_8_Shock = 272,
        Spell2_9_Panic = 273,
        Spell2_10_DELETED = 274,
        Spell3_0_Fireball = 294,
        Spell3_1_LightningStrike = 295,
        Spell3_2_FireRain = 296,
        Spell3_3_Thunderbolt = 297,
        Spell3_4_FireHail = 298,
        Spell3_5_Thunderstorm = 299,
        Spell3_6_LightningTrap = 300,
        Spell3_7_BigLightningTrap = 301,
        Spell3_8_LightningMine = 302,
        Spell3_9_BigLightningMine = 303,
        Spell3_10_StealLife = 304,
        Spell3_11_StealMagic = 305,
        Spell3_12_PersonalProtection = 306,
        Spell3_13_KamulossGaze = 307,
        Spell3_14_RemoveTrap = 308,
        Spell5_0_Panic = 354,
        Spell5_1_PoisonBreeze = 355,
        Spell5_2_Irritation = 356,
        Spell5_3_PlagueBreeze = 357,
        Magic_NobodyIsTired = 414,
        CombatMsg_XTriggeredATrap = 415,
        Options_CombatDetailLevel = 416,
        Options_CombatTextDelay = 417,
        Misc_DeathEndsTheJourneyEveryoneInThePartyIsFatallyWoundedTheyDieSoonAfterwards = 418,
        Magic_XWasNotAbleToDeflectTheSpell = 419,
        CombatEffect_XHasBeenMagicallyAccelerated = 420,
        Shop_ExitMerchantScreen = 421,
        Misc_ExitScreen = 422,
        CombatEffect_XHasGoneBerserk = 423,
        SpellSelect_CannotBeCastHereAndNow = 424,
        SpellSelect_NeedsNSpellPoints = 425,
        PartyPopup_DoesNotKnowAnySpellsYet = 426,
        SpellSelect_NotStrongEnoughToCastThisSpell = 427,
        SpellSelect_NeedsNSPAndNLP = 428,
        Magic_TeleportWho = 429,
        Magic_TeleportWhere = 430,
        MapPopup_UseWhichItem = 431,
        MapPopup_ThisItemDoesntWorkHere = 432,
        MapPopup_ThisWordDoesntWorkHere = 433,
        CombatMsg_LongRangeWeaponHasNoAmmo = 434,
        CombatMsg_AttackWhom = 435,
        CombatMsg_NoTargetsForAttack = 436,
        CombatMsg_NoTargetsInRange = 437,
        Combat_CannotAttack = 438,
        Combat_CannotCauseDamage = 439,
        Combat_UnableToMove = 440,
        Combat_CannotGetAway = 441,
        CombatMsg_MoveWhere = 442,
        CombatMsg_NowhereToMove = 443,
        CombatMsg_MoveWasBlocked = 444,
        CombatMsg_XIsMoving = 445,
        CombatMsg_XIsFleeing = 446,
        CombatMsg_XIsAttackingXWithX = 447,
        CombatMsg_XCannotBeHurt = 448,
        CombatMsg_AttackRepelled = 449,
        CombatMsg_XMakesCriticalHit = 450,
        CombatMsg_XReceivesNDamage = 451,
        CombatMsg_NoDamageDone = 452,
        CombatMsg_XMissesHisVictim = 453,
        CombatMsg_XMisses = 454,
        CombatMsg_XHasUsedUpHisAmmunition = 455,
        CombatMsg_XAttacksX = 456,
        Race_Terran = 457,
        Race_Iskai = 458,
        Race_Celt = 459,
        Race_KengetKamulos = 460,
        Race_DjiCantos = 461,
        Race_Mahino = 462,
        Race_Decadent = 463,
        Race_Umajo = 464,
        Race_Monster = 472,
        Combat_Observe = 489,
        MapPopup_CannotCarryThatMuch = 490,
        MapPopup_NoSpaceLeft = 491,
        Dialog_WhatDoYouKnowAbout = 492,
        Dialog_WhatDoYouKnowAboutThisItem = 493,
        Dialog_WouldYouLikeToJoinUs = 494,
        Dialog_IWouldLikeYouToLeaveUs = 495,
        Dialog_ItsBeenNiceTalkingToYou = 496,
        Dialog_OfferWhichItem = 497,
        Dialog_ICannotSayAnythingAboutThis = 498,
        Dialog_ICantTellYouAnythingAboutThisTom = 499,
        Dialog_ImNotInterestedInThisItem = 500,
        Dialog_WhatsYourProfession = 501,
        Dialog_ThanksButIThinkIdRatherNotJoinYou = 502,
        Dialog_AlrightIllLeave = 503,
        Dialog_Farewell = 504,
        Dialog_SeeYouTom = 505,
        Dialog_Greetings = 506,
        Dialog_HiTomWhatsUp = 507,
        MsgBox_EnterWord = 508,
        Dialog_ThePartySeemsToBeComplete = 509,
        Dialog_IDontUnderstandThat = 510,
        Dialog_IDontUnderstandThatTom = 511,
        Dialog_ThisIsAVitalItem = 512,
        PartyPopup_CannotTalkRightNow = 513,
        MapPopup_Person = 514,
        Chest_ExitChestScreen = 515,
        Gold_TakeHowManyRations = 516,
        Chest_YouCantPutItBack = 517,
        Item_PutDownX = 518,
        Item_SwapXWithX = 519,
        Item_ThisSpaceIsOccupied = 520,
        Item_Add = 521,
        Door_ExitDoorScreen = 522,
        Lock_LeaderPickedTheLock = 523,
        Lock_ThisLockCannotBePicked = 524,
        Lock_LeaderTriggeredATrapButManagedToEvadeIt = 525,
        Lock_LeaderTriggeredATrap = 526,
        Lock_LeaderOpenedTheLock = 527,
        Lock_LeaderPickedTheLockWithALockpick = 528,
        Lock_ThisIsNotTheRightKey = 529,
        Lock_YouCannotOpenTheLockWithThisItem = 530,
        Chest_Chest = 531,
        InvMsg_ReallyThrowTheseItemsAway = 532,
        MapPopup_TooFarAway = 533,
        MapPopup_TooFarAwayToTalkTo = 534,
        MapPopup_TooFarAwayToTouch = 535,
        MapPopup_Blocked = 536,
        MapPopup_TooFarAway2 = 537,
        MapPopup_Blocked2 = 538,
        Lock_LeaderCannotPickThisLock = 539,
        MapPopup_ThisPersonIsAsleep = 540,
        MapPopup_ThisPersonSpeaksALanguageLeaderDoesntUnderstand = 541,
        PartySelect_SomebodyMUSTLeadTheParty = 542,
        PartyPopup_CannotLeadThePartyNow = 543,
        MapPopup_ThesePeopleDoNotSpeakTheSameLanguage = 544,
        PartySelect_ThisPersonCannotLeadTheParty = 545,
        PartySelect_WhoShouldLeadThePartyNow = 546,
        Combat_EndCombat = 547,
        Combat_ExitCombatScreen = 548,
        Combat_ItemsLeftBehind = 549,
        Combat_DoYouWantToLeaveTheRestOfTheItems = 550,
        Combat_YouveLeftAVitalItem = 551,
        PostCombat_NoOneGainedAnyExperience = 552,
        PostCombat_EverybodyInThePartyGainsNExperiencePoints = 553,
        Shop_ThereAreNoTrainingPointsLeft = 554,
        Shop_ThereIsNothingICanTeachYouNow = 555,
        Shop_ThePartyDoesNotHaveEnoughGold = 556,
        Shop_ByHowManyPointsDoYouWantToImproveYourSkill = 557,
        Shop_ThatllBeNNCoinsDoYouWantToBuyIt = 558,
        Shop_LeaderTakesIntensiveTraining = 559,
        Shop_ThisPersonDoesntNeedHealing = 560,
        Shop_HowManyLifePointsDoYouWantMeToHeal = 561,
        Shop_XsLifePointsHaveBeenRestored = 562,
        Shop_XsCursedItemsHaveBeenRemoved = 563,
        Shop_WhoDoYouWantMeToHeal = 564,
        Shop_WhichItemDoYouWantMeToExamine = 565,
        Shop_ThisItemHasAlreadyBeenExamined = 566,
        Shop_TheItemHasBeenExaminedInformationIsNowAvailable = 567,
        Shop_ThisItemIsAlreadyFullyCharged = 568,
        Shop_ThisItemCannotBeRechargedAnymore = 569,
        Shop_WhichItemDoYouWantToBeRecharged = 570,
        Shop_RestoreHowManyCharges = 571,
        Shop_ThisItemHasBeenChargedWithMagicalEnergy = 572,
        Shop_Merchant = 573,
        Shop_Buy = 574,
        InvPopup_Sell = 575,
        Shop_BuyHowManyItems = 576,
        Shop_TheItemsHaveBeenBought = 577,
        Item_X_NGold = 578,
        Shop_ThisIsTooExpensive = 579,
        Item_X_NGold2 = 580,
        Shop_XHasNoSpaceLeft = 581,
        Shop_TheMerchantHasNoStorageRoomLeft = 582,
        Shop_SellHowManyItems = 583,
        Shop_TheItemsHaveBeenSold = 584,
        Shop_IllGiveYouNNCoinsForThatDoYouWantToSellIt = 585,
        Shop_GoldAll = 586,
        Shop_TotalGoldNdN = 587,
        Shop_ThisIsAUselessItem = 588,
        Shop_HowManyRationsDoYouWantToBuy = 589,
        Shop_TheRationsHaveBeenBought = 590,
        Shop_ThisItemIsNotBroken = 591,
        Shop_RepairWhichItem = 592,
        Shop_TheItemHasBeenRepaired = 593,
        PartyPortrait_GiveXToX = 594,
        PartyPortrait_GiveGoldToX = 595,
        PartyPortrait_GiveFoodToX = 596,
        PartyPortrait_XHasNoSpaceLeft = 597,
        PartyPortrait_XCannotCarryThatMuchGold = 598,
        PartyPortrait_XCannotCarryThatManyRations = 599,
        InvMsg_WrongClass = 600,
        Magic_ThisSpellOnlyWorksIn3DMaps = 601,
        MapPopup_ItsTooDangerousHere = 602,
        Shop_LeaderAndHisCompanionsAreLedToTheirRoomsWhereTheySpendAPeacefulNight = 603,
        Rest_NobodyInThePartyIsTired = 604,
        Rest_ThePartyRestsTillDawn = 605,
        Rest_ThePartyRestsForEightHours = 606,
        Rest_XCannotRecuperateHeHasNoFoodLeft = 607,
        InvPopup_Use = 608,
        Shop_HealWhichCondition = 609,
        Condition_Unconscious = 610,
        Condition_Poisoned = 611,
        Condition_Ill = 612,
        Condition_Exhausted = 613,
        Condition_Paralyzed = 614,
        Condition_Fleeing = 615,
        Condition_Intoxicated = 616,
        Condition_Blind = 617,
        Condition_Panicking = 618,
        Condition_Asleep = 619,
        Condition_Insane = 620,
        Condition_Irritated = 621,
        Shop_XIsHealed = 626,
        Shop_NoOneInThisPartyNeedsHealing = 627,
        CombatEffect_XHasBeenBlinded = 628,
        Shop_NoOneInYourPartyCanLearnMyKindOfMagic = 629,
        CombatEffect_XIsNotBlindAnymore = 630,
        Magic_XManagedToPartiallyDeflectTheSpell = 631,
        Shop_LearnWhichSpell = 632,
        Inv1_NYearsOldRaceClassLevelN = 633,
        Inv1_LifePoints = 634,
        Inv1_SpellPoints = 635,
        Attrib_Strength = 636,
        Attrib_Intelligence = 637,
        Attrib_Dexterity = 638,
        Attrib_Speed = 639,
        Attrib_Stamina = 640,
        Attrib_Luck = 641,
        Attrib_MagicResistance = 642,
        Attrib_MagicTalent = 643,
        Skill_CloseRangeCombat = 644,
        Skill_LongRangeCombat = 645,
        Skill_CriticalHit = 646,
        Skill_Lockpicking = 647,
        Inv2_Attributes = 648,
        Inv2_Skills = 649,
        Attrib_STR = 650,
        Attrib_INT = 651,
        Attrib_DEX = 652,
        Attrib_SPD = 653,
        Attrib_STA = 654,
        Attrib_LUC = 655,
        Attrib_MR = 656,
        Attrib_MT = 657,
        Skill_CLO = 658,
        Skill_LON = 659,
        Skill_CRI = 660,
        Skill_LP = 661,
        Inv3_Conditions = 662,
        Inv3_Languages = 663,
        Lang_Terran = 664,
        Lang_Iskai = 665,
        Lang_Celtic = 666,
        Inv3_ChangeCombatPositions = 667,
        Inv1_ExperiencePoints = 668,
        Inv1_TrainingPoints = 669,
        Inv3_TemporarySpells = 670,
        SpellClass_Attack = 671,
        SpellClass_Protection = 672,
        SpellClass_AntiMagic = 673,
        Inv3_NHours = 674,
        Inv3_PositionX = 675,
        Examine1_Weight = 676,
        Examine1_Type = 677,
        Inv_DamageN = 678,
        Inv_ProtectionN = 679,
        Inv_WeightNKg = 680,
        Inv_CarriedWeightNdOfNdG = 681,
        Examine1_Damage = 682,
        Examine1_Protection = 683,
        Examine1_More = 684,
        Examine1_NG = 685,
        Examine2_SkillTax1 = 686,
        Examine2_SkillTax2 = 687,
        Examine2_None = 688,
        Examine2_LPMaximumBonus = 689,
        Examine2_SPMaximumBonus = 690,
        Examine2_AttributeBonus = 691,
        Examine2_SkillBonus = 692,
        Examine2_Spell = 693,
        Examine2_Enchantments = 694,
        Examine1_MoreInformation = 695,
        MsgBox_CloseWindow = 696,
        MapPopup_MainMenu = 697,
        Combat_Combat = 698,
        Magic_XSpellDoesNotHitAnybody = 699,
        Misc_XIsCarryingTooMuch = 700,
        MainMenu_MainMenu = 701,
        PartyPopup_Toggle = 702,
        MainMenu_NewGame = 703,
        MainMenu_ContinueGame = 704,
        MainMenu_LoadGame = 705,
        MainMenu_SaveGame = 706,
        MainMenu_Options = 707,
        MainMenu_ViewIntro = 708,
        MainMenu_Credits = 709,
        MainMenu_QuitGame = 710,
        MainMenu_DoYouReallyWantToQuit = 711,
        MainMenu_WhichSavedGameDoYouWantToLoad = 712,
        MainMenu_SaveOnWhichPosition = 713,
        MainMenu_DoYouReallyWantToOverwriteThisSavedGame = 714,
        MainMenu_PleaseEnterANameForThisSavedGame = 715,
        MainMenu_DoYouReallyWantToStartANewGame = 716,
        MainMenu_EmptyPosition = 717,
        MainMenu_NoSavedGamesFound = 718,
        Shop_ThisPersonDoesNotCarryAnyCursedItems = 719,
        InvMsg_WhoShouldDrinkThis = 720,
        Misc_XWasLuckyAndEscapedTheTrap = 721,
        Magic_ThisMonsterCannotBeTeleported = 722,
        Magic_TheTeleportTargetIsAlreadyOccupied = 723,
        MapPopup_Wait = 724,
        MapPopup_WaitForHowManyHours = 725,
        Magic_XUsedLifePointsForMagic = 726,
        MapPopup_ReallyRest = 727,
        Gold_ThrowHowMuchGoldAway = 728,
        Gold_ReallyThrowTheGoldAway = 729,
        Gold_ThrowHowManyRationsAway = 730,
        Gold_ReallyThrowTheRationsAway = 731,
        Magic_XManagedToCompletelyDeflectTheSpell = 732,
        Misc_EveryoneIsGettingTired = 733,
        Misc_EverybodyIsExhausted = 734,
        Misc_Attack = 735,
        PartyPopup_MellthasCannotSpeak = 736,
        CombatMsg_XIsBroken = 737,
        CombatEffect_XMagicalAccelerationIsOver = 738,
        CombatEffect_XIsNoLongerBerserk = 739,
        CombatEffect_XIsFrozen = 740,
        CombatEffect_XIsNotFrozenAnymore = 741,
        Options_MusicVolume = 742,
        Options_FXVolume = 743,
        Options_3DWindowSize = 744,
        Shop_NoneOfYouCarriesAnyCursedItems = 745,
        Shop_WhoseCursedItemsDoYouWantMeToRemove = 746,
        Combat_DoNothing = 747,
        Misc_XHasReachedLevelNLifePointsNSpellPointsNAdditionalTrainingPointsNAttacksPerRoundN = 749,
        Misc_XHasReachedLevelNLifePointsNAdditionalTrainingPointsNAttacksPerRoundN = 750,
        Misc_XHasReachedTheMaximumCharacterLevel = 751,
        Shop_YouCannotSellBrokenThings = 752,
        InvPopup_XHasNoSpaceLeft = 753,
        SpecialItem_TheCompassHasBeenActivated = 754,
        SpecialItem_TheMonsterEyeHasBeenActivated = 755,
        Combat_PartyHasAlreadyAdvanced = 756,
        SpecialItem_TheClockHasBeenActivated = 757,
        PartyPopup_ThisBackpackIsNotAvailableNow = 758,
        Meta_HimDative = 759,
        Misc_IsTooHurtToBeHealed = 760,
        InvPopup_NotDuringCombat = 761,
        Misc_XIsSickAndHeFeelsHisBodyBeingPermanentlyDamaged = 762,
        Condition_XIsUnconscious = 763,
        Condition_XHasBeenPoisoned = 764,
        Condition_XIsIll = 765,
        Condition_XIsExhausted = 766,
        Condition_XIsUnableToMove = 767,
        Condition_XHasFled = 768,
        Condition_XIsIntoxicated = 769,
        Condition_XHasBeenBlinded = 770,
        Condition_XIsPanicking = 771,
        Condition_XIsAsleep = 772,
        Condition_XHasGoneInsane = 773,
        Condition_XIsIrritated = 774,
        Magic_ThisSpellHasNoEffectOnX = 775,
        MainMenu_NotEnoughRoomOnYourHardDrive = 776,
        Meta_HerDative = 777,
        Meta_ItDative = 778,
    }
}
#pragma warning restore CA1707 // Identifiers should not contain underscores