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

            // Получаем ссылку на главное меню IPS
            MenuBar menuBar = barManager.MenuBar;

            // Имя, которое будет отображатся в главном меню IPS
            MenuItemBase menuItem = new MenuBarItem("Пример элемента меню");

            // Уникальное имя элемента, по которому будет выполняться поиск методом FindMenuBar()
            menuItem.CommandName = "MenuElementSample";

            // Создаём новую кнопку элемента меню, которая будет отображаться в кастомном меню
            MenuButtonItem menuButton = new MenuButtonItem("Пример кнопки в меню");

            // Уникальное имя кнопки, по которому будет выполняться поиск 
            menuButton.CommandName = "MenuButtonSample";

            // Добавляем кнопку в элемент меню
            menuItem.Items.Add(menuButton);

            // Добавляем новое меню в главное меню IPS
            menuBar.Items.Add(menuItem);
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }
    }
}
