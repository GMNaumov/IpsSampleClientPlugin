using Intermech.Bars;
using Intermech.Interfaces.Plugins;
using System;
using System.Windows.Forms;

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

            AddNewMenuWithButtons();

            AddNewButtonInExistsMenu();
        }

        /// <summary>
        /// Пример добавления нового элемента главного меню IPS с кнопкой
        /// </summary>
        private void AddNewMenuWithButtons()
        {
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

            // Подсказка при наведении на кнопку
            menuButton.ToolTipText = "Эта кнопка добавлена при помощи клиентского плагина";

            // Добавляем кнопку в элемент меню
            menuItem.Items.Add(menuButton);

            // Добавляем новое меню в главное меню IPS
            menuBar.Items.Add(menuItem);
        }

        /// <summary>
        /// Пример добавления кнопки в существующее меню IPS, с обработчиком события нажатия на кнопку
        /// </summary>
        private void AddNewButtonInExistsMenu()
        {
            BarManager barManager = ipsServiseProvider.GetService(typeof(BarManager)) as BarManager;

            MenuBar menuBar = barManager.MenuBar;

            // Поиск существующего элемента меню по значению свойства CommandName
            MenuItemBase menuItem = menuBar.FindMenuBar("MenuElementSample");

            if (menuItem != null)
            {
                MessageBox.Show("Элемент меню найден!");

                MenuButtonItem menuButton = new MenuButtonItem("Пример кнопки в меню #3",
                    new EventHandler(ShowHelloMessage));

                menuButton.CommandName = "MenuButtonSampleTwo";

                menuItem.Items.Add(menuButton);
            }
            else
            {
                MessageBox.Show("Элемент меню не найден...");
            }
        }

        /// <summary>
        /// Метод интерфейса IPackage. 
        /// В новых версиях IPS не используется, оставлен для совместимости.
        /// </summary>
        public void Unload()
        {

        }

        private static void ShowHelloMessage(object sender, EventArgs e)
        {
            MessageBox.Show("Это сообщение вызвано обработчиком события IPS");
        }
    }
}
