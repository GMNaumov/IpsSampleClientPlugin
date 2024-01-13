using System;
using Intermech.Bars;
using Intermech.Interfaces.Plugins;

namespace IpsSampleClientPlugin
{
    public class Solution : IPackage
    {
        // Имя, отображаемое в описании модуля расширения 
        public string Name => "Мой первый плагин IPS";

        internal static IServiceProvider ipsServiseProvider;

        public void Load(IServiceProvider serviceProvider)
        {
            ipsServiseProvider = serviceProvider;

            BarManager barManager = ipsServiseProvider.GetService(typeof(BarManager)) as BarManager;

            MenuBar menuBar = barManager.MenuBar;

            // Имя, которое будет отображатся в главном меню IPS
            MenuItemBase itemSamples = new MenuBarItem("MenuItemBase");

            itemSamples.CommandName = "Command_sample";

            menuBar.Items.Add(itemSamples);
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
