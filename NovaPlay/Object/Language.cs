using System;
using System.Collections.Generic;
using MiNET;
using NovaPlay;
using log4net;

namespace NovaPlay.Object
{
    public class Language
    {

        public static Dictionary<string, Dictionary<string, string>> languages = new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, string> eng = new Dictionary<string, string>();
        public static Dictionary<string, string> rus = new Dictionary<string, string>();
        public Player player;
        public static ILog Logger = LogManager.GetLogger(typeof(Language));

        public static void InitEng()
        {
            var eng = new Dictionary<string, string>();
            eng.Add("login_f", " &cNLogin firstly");
            eng.Add("not_logged", " &cYou are not logged in");
            eng.Add("not_registered", " &cYou are not registered");
            eng.Add("no_perms", " &cYou dont have enough permissions");
            eng.Add("wrong_password", " &cWrong password");
            eng.Add("already_logged", " &You are already logged in");
            eng.Add("already_registered", " &cYou are already registered");
            eng.Add("logged_in", " &aYou have successfuly logged in");
            eng.Add("registered", " &aYou have successfuly registered");
            eng.Add("settings_save", " &aSucessfuly saved settings");
            languages.Add("english", eng);
            Logger.Error("[NovaPlay] Loaded eng");
        }

        public static void InitRus()
        {
            rus.Add("login_f", " &cВойди в аккаунт");
            rus.Add("not_logged", " &cТы не авторизован");
            rus.Add("not_registered", " &cТы не зарегистрирован");
            rus.Add("wrong_password", " &cНеправильный пароль");
            rus.Add("already_logged", " &cТы уже авторизирован");
            rus.Add("already_registered", " &cТы уже зарегистрирован");
            rus.Add("logged_in", " &aТы вошел в аккаунт");
            rus.Add("registered", " &aТы зарегистрировался");
            rus.Add("settings_save", " &aТы успешно сохранил настройки");
            languages.Add("russian", rus);
            Logger.Error("[NovaPlay] Loaded rus");
        }

        public static string Translate(string message, NovaPlayer player, params string[] args)
        {
            string arraydata = Language.languages[player.pData.GetLanguage()][message].Replace("&", "§");
            for (int i = 0; i < args.Length; ++i)
            {
                arraydata = arraydata.Replace("%" + i, args[i]);
            }
            return arraydata;
        }




    }
}
