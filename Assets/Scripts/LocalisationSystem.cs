using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalisationSystem {
    public enum Language {
        English,
        French,
        Spanish,
        German,
        Dutch,
        Russian,
        Thai,
        Japan,
        Portueguese,
        Chinese,
        Italian,
        Indonesian,
        Turkish,
        Korean
    }


    public static Language language;
    private static Dictionary<string, string> localisedEN;
    private static Dictionary<string, string> localisedFR;
    private static Dictionary<string, string> localisedSP;
    private static Dictionary<string, string> localisedGM;
    private static Dictionary<string, string> localisedDU;
    private static Dictionary<string, string> localisedRU;
    private static Dictionary<string, string> localisedTH;
    private static Dictionary<string, string> localisedJP;
    private static Dictionary<string, string> localisedPO;
    private static Dictionary<string, string> localisedCN;
    private static Dictionary<string, string> localisedIT;
    private static Dictionary<string, string> localisedIN;
    private static Dictionary<string, string> localisedTK;
    private static Dictionary<string, string> localisedKO;
    public static bool isInit;
    

    public static void SetLocalisedLanguage(string selectedLangauge) {
        switch (selectedLangauge) {
            case "English":
                language = Language.English;
                break;
            case "French":
                language = Language.French;
                break;
            case "Spanish":
                language = Language.Spanish;
                break;
            case "German":
                language = Language.German;
                break;
            case "Dutch":
                language = Language.Dutch;
                break;
            case "Russian":
                language = Language.Russian;
                break;
            case "Thai":
                language = Language.Thai;
                break;
            case "Japan":
                language = Language.Japan;
                break;
            case "Portueguese":
                language = Language.Portueguese;
                break;
            case "Chinese":
                language = Language.Chinese;
                break;
            case "Italian":
                language = Language.Italian;
                break;
            case "Indonesian":
                language = Language.Indonesian;
                break;
            case "Turkish":
                language = Language.Turkish;
                break;
            case "Korean":
                language = Language.Korean;
                break;
        }
    }

    public static void Init() {
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();
        localisedEN = csvLoader.GetDictionaryValues("en");
        localisedFR = csvLoader.GetDictionaryValues("fr");
        localisedSP = csvLoader.GetDictionaryValues("sp");
        localisedGM = csvLoader.GetDictionaryValues("gm");
        localisedDU = csvLoader.GetDictionaryValues("du");
        localisedRU = csvLoader.GetDictionaryValues("ru");
        localisedTH = csvLoader.GetDictionaryValues("th");
        localisedJP = csvLoader.GetDictionaryValues("jp");
        localisedPO = csvLoader.GetDictionaryValues("po");
        localisedCN = csvLoader.GetDictionaryValues("cn");
        localisedIT = csvLoader.GetDictionaryValues("it");
        localisedIN = csvLoader.GetDictionaryValues("in");
        localisedTK = csvLoader.GetDictionaryValues("tk");
        localisedKO = csvLoader.GetDictionaryValues("ko");
        isInit = true;
    }

    public static string GetLocalisedValue(string key) {
        if (!isInit) { Init(); }
        string value = key;
        switch (language) {
            case Language.English:
                localisedEN.TryGetValue(key, out value);
                break;
            case Language.French:
                localisedFR.TryGetValue(key, out value);
                break;
            case Language.Spanish:
                localisedSP.TryGetValue(key, out value);
                break;
            case Language.German:
                localisedGM.TryGetValue(key, out value);
                break;
            case Language.Dutch:
                localisedDU.TryGetValue(key, out value);
                break;
            case Language.Russian:
                localisedRU.TryGetValue(key, out value);
                break;
            case Language.Thai:
                localisedTH.TryGetValue(key, out value);
                break;
            case Language.Japan:
                localisedJP.TryGetValue(key, out value);
                break;
            case Language.Portueguese:
                localisedPO.TryGetValue(key, out value);
                break;
            case Language.Chinese:
                localisedCN.TryGetValue(key, out value);
                break;
            case Language.Italian:
                localisedIT.TryGetValue(key, out value);
                break;
            case Language.Indonesian:
                localisedIN.TryGetValue(key, out value);
                break;
            case Language.Turkish:
                localisedTK.TryGetValue(key, out value);
                break;
            case Language.Korean:
                localisedKO.TryGetValue(key, out value);
                break;
        }
        return value;
    }
}
