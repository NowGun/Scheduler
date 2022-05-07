using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Classes
{
    internal class SettingProgram
    {
        public void ChangeTheme()
        {
            var newTheme = Properties.Settings.Default.Theme == 1
                       ? WPFUI.Appearance.ThemeType.Light
                       : WPFUI.Appearance.ThemeType.Dark;

            if (Environment.OSVersion.Version.Build >= 22000)
            {
                WPFUI.Appearance.Theme.Apply(
           themeType: newTheme,
           backgroundEffect: WPFUI.Appearance.BackgroundType.Mica,
           updateAccent: true,
           forceBackground: false);
            }
            else
            {
                WPFUI.Appearance.Theme.Apply(
           themeType: newTheme,
           updateAccent: true,
           forceBackground: false);
            }
        }
        public string Hash(string password)
        {
            MD5 md5hasher = MD5.Create();
            return Convert.ToBase64String(md5hasher.ComputeHash(Encoding.Default.GetBytes(password)));
        }
    }
}
