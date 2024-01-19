using Intermech;
using Intermech.Bars;
using IPSClientNavigator = Intermech.Navigator;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using System;
using System.Windows.Forms;

namespace IpsSampleClientPlugin
{
    public class Solution : IPackage
    {
        // Имя, отображаемое в описании модуля расширения 
        public string Name => "Пример клиентского расширения IPS";

        internal static IServiceProvider ipsServiseProvider;


        public void Load(IServiceProvider serviceProvider)
        {
            ipsServiseProvider = serviceProvider;

            ShowMessageInOutputView();

            AddNewMenuWithButtons();

            AddNewButtonInExistsMenu();

            AddNewButtonInExistsMenuWithChilds();
        }

        /// <summary>
        /// Пример вывода сообщения в окне "Вывод" главного окна IPS
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void ShowMessageInOutputView()
        {
            
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

            // Создавние новой кнопки с передачей в конструктор обработчика события нажатия
            MenuButtonItem menuButton = new MenuButtonItem("Пример кнопки в меню #2",
                    new EventHandler(ShowHelloMessage));

            menuButton.CommandName = "MenuButtonSampleTwo";

            menuItem.Items.Add(menuButton);
        }

        /// <summary>
        /// Пример добавления элемента в существующее меню IPS, с "дочерними" кнопками
        /// </summary>
        private void AddNewButtonInExistsMenuWithChilds()
        {
            BarManager barManager = ipsServiseProvider.GetService(typeof(BarManager)) as BarManager;

            MenuBar menuBar = barManager.MenuBar;

            // Поиск существующего элемента меню по значению свойства CommandName
            MenuItemBase menuItem = menuBar.FindMenuBar("MenuElementSample");

            // Создавние новой кнопки с передачей в конструктор обработчика события нажатия
            MenuButtonItem menuButton = new MenuButtonItem("Пример кнопки в меню #3");

            menuButton.CommandName = "MenuButtonSampleWithGroup";

            // Определяет, будет ли элемент начинать новую группу в меню
            menuButton.BeginGroup = true;

            MenuButtonItem menuButtonChildOne = new MenuButtonItem("Пример кнопки подменю #1");
            
            // Определяем обработчик события нажатия на созданную кнопку
            menuButtonChildOne.Click += new EventHandler(CreateNewIpsObject);
            

            MenuButtonItem menuButtonChildTwo = new MenuButtonItem("Пример кнопки подменю #2");

            menuButton.Items.Add(menuButtonChildOne);
            menuButton.Items.Add(menuButtonChildTwo);

            menuItem.Items.Add(menuButton);
        }

        /// <summary>
        /// Метод интерфейса IPackage. 
        /// В новых версиях IPS не используется, оставлен для совместимости.
        /// </summary>
        public void Unload()
        {

        }

        /// <summary>
        /// Пример создания нового объекта
        /// </summary>
        private static void CreateNewIpsObject(object sender, EventArgs eventArgs)
        {
            // Получаем из контейнера сервисов ссылку на сервис для создания новых объектов IPS
            IObjectCreatorService ipsCreatorService = ApplicationServices.Container.GetService(typeof(IObjectCreatorService)) as IObjectCreatorService;

            // Вызов мастера создания новых объектов с получением идентификатора версии создаваемого объекта
            long createdObjectId = ipsCreatorService.CreateObjectDialog();

            // Выходим, если новый объект не создан
            if (createdObjectId == Intermech.Consts.UnknownObjectId || createdObjectId == -1)
            {
                return;
            }

            // Получаем из контейнера сервисов ссылку на сервис уведомлений IPS
            // NB: приведены два примера для получения ссылки - "из документации" (закомментирован) и найденный с помощью подсказок Visual Studio. Второй симпатишнее что ли...
            // INotificationService ipsNotificationService = ApplicationServices.Container.GetService(typeof(INotificationService)) as INotificationService;
            INotificationService ipsNotificationService = ApplicationServices.Container.GetService<INotificationService>();
            
            // Создаём уведомление о том, что новый объект создан
            // Очень увлекательно поизучать статический класс NotificationEventNames
            DBObjectsEventArgs dBObjectsEventArgs = new DBObjectsEventArgs(NotificationEventNames.ObjectsCreated, createdObjectId);

            // Уведомление для клиента IPS о создании нового объекта
            ipsNotificationService.FireEvent(null, dBObjectsEventArgs);

            // Создаём дескриптор нового окна IPS
            IPSClientNavigator.DBObjects.Descriptor descriptor = new IPSClientNavigator.DBObjects.Descriptor(createdObjectId);

            // Открываем созданный объект в новом окне навигатора IPS
            IPSClientNavigator.Utils.OpenNewWindow(descriptor, null);
        }

        /// <summary>
        /// Простой вывод окна WinForms
        /// Очень помогает, когда нужно проверить, работает ли что-то, или нет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ShowHelloMessage(object sender, EventArgs e)
        {
            MessageBox.Show("Это сообщение вызвано обработчиком события IPS");
        }
    }
}
