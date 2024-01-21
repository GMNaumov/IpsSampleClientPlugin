using Intermech;
using Intermech.Bars;
using IPSClientNavigator = Intermech.Navigator;
using IPSToolbar = Intermech.Bars.ToolBar;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Plugins;
using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Drawing;

namespace IpsSampleClientPlugin
{
    public class Solution : IPackage
    {
        // Имя, отображаемое в описании модуля расширения 
        public string Name => "Пример клиентского расширения IPS";

        // Определяем GUID-ы элементов, которые будут созданы в коде клиентского плагина
        private static readonly Guid toolBarGuid = new Guid("{7244C45A-2AC3-4566-8A8F-60361A8284F6}");

        internal static IServiceProvider ipsServiseProvider;


        public void Load(IServiceProvider serviceProvider)
        {
            ipsServiseProvider = serviceProvider;

            ShowMessageInOutputView();

            AddNewMenuWithButtons();

            AddNewButtonInExistsMenu();

            AddNewButtonInExistsMenuWithChilds();

            CreateNewToolBar();
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

            MenuButtonItem menuButtonChildOne = new MenuButtonItem("Cоздание нового объекта с диалогом IPS");

            // Определяем обработчик события нажатия на созданную кнопку
            menuButtonChildOne.Click += new EventHandler(CreateNewIpsObjectWithDialogWindow);

            MenuButtonItem menuButtonChildTwo = new MenuButtonItem("Создание нового ПКИ без окна диалога");
            menuButtonChildTwo.Click += new EventHandler(CreateNewObject);

            menuButton.Items.Add(menuButtonChildOne);
            menuButton.Items.Add(menuButtonChildTwo);

            menuItem.Items.Add(menuButton);
        }

        /// <summary>
        /// Пример создания пользовательской панели инструментов
        /// </summary>
        private void CreateNewToolBar()
        {
            // Получение ссылки на менеджер меню, панелей и инструментов IPS
            BarManager barManager = ApplicationServices.Container.GetService<BarManager>();

            // Получаем ссылку на сервис значков для элементов интерфейса IPS
            INamedImageList images = ApplicationServices.Container.GetService<INamedImageList>();

            // Создаём новую панель инструментов
            // Алиас IPSToolbar используется для исключения конфликтов с классом ToolBar стандартной библиотеки Windows.Forms
            IPSToolbar toolBar = new IPSToolbar();
            
            // Определяем имя элемента, которое будет использоваться в поиске по элементам управления IPS
            toolBar.Name = "IPSSampleClient.NewToolbar";

            // Определяем текст, который будет отображаться в клиенте IPS
            toolBar.Text = "Пример клиентского плагина IPS";

            toolBar.ImageList = images.ImageList;
            
            //Присваиваем новой панели инструментов Guid
            // Самый надёжный вариант - прибить гвоздями readonly-поле класса
            toolBar.Guid = toolBarGuid;

            // Скрываем кнопку "Добавить/удалить кнопки"
            toolBar.AddRemoveButtonsVisible = false;

            // Разрешаем вертикальный докинг
            toolBar.AllowVerticalDock = true;

            // Разрешаем горизонтальный докинг
            toolBar.AllowHorizontalDock = true;

            // Отображаем только полные меню
            toolBar.FullMenus = true;

            // Определяем размер "непридоченной" панели инструментов
            toolBar.MinimumFloatingSize = new Size(250, 30);

            // Определяем размер "придоченной" панели инструментов
            toolBar.Size = new Size(400, 30);

            // Создаём новую кнопку для кастомной панели инструментов
            ButtonItem buttonItem = new ButtonItem();
            buttonItem.CommandName = "ToolBarButtonSample";
            buttonItem.Text = "Кнопка #1";
            buttonItem.ToolTipText = "Пример пользовательской кнопки на панели инструментов";
            buttonItem.Click += new EventHandler(ShowHelloMessage);

            // Добавляем кнопку на панель инструментов
            // Метод AddRange() позволяет добавить несколько кнопок, для добавления одной кнопки можно использовать Add()
            toolBar.Items.AddRange(new ToolbarItemBase[]
                {
                    buttonItem
                }
            );

            // Добавляем новую панель инструментов
            barManager.AddToolbar(toolBar);
            toolBar.Parent = barManager.FindSuitableContainer(DockStyle.Top);

            // Определение параметров докинга новой панели инструментов
            toolBar.DockLine = 2;
            toolBar.DockOffset = 0;
            toolBar.Location = new Point(0, 0);

            toolBar.Hidden = false;
            toolBar.Visible = true;
        }

        /// <summary>
        /// Пример создания нового объекта при помощи стандартного диалогового окна IPS
        /// </summary>
        private void CreateNewIpsObjectWithDialogWindow(object sender, EventArgs eventArgs)
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
        /// Пример создания нового объекта конкретного типа ("Прочие изделия") без вызова диалогового окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void CreateNewObject(object sender, EventArgs eventArgs)
        {
            // Получаем идентификатор версии нового объекта - ДО создания самого объекта
            long createdPkiId = Consts.NavigatorUndefinedObjectID;

            // Обращение к базе данных IPS
            using (SessionKeeper keeper = new SessionKeeper())
            {
                // Получаем из метаданных идентификатор объекта "Прочие изделия"
                int pkiObjectType = MetaDataHelper.GetObjectTypeID(SystemGUIDs.objtypeOtherProducts);

                // Получаем коллекцию объектов определённого типа (типа "Прочие изделия")
                IDBObjectCollection pkiCollection = keeper.Session.GetObjectCollection(pkiObjectType);

                // Создаём "болванку" нового объекта типа "Прочие изделия"
                IDBObject createdPki = pkiCollection.Create();

                // Ищем у создаваемого объекта атрибут
                // Возможен поиск по: а) имени, б) псевдониму, в) идентификатору IPS, г) GUID-у
                IDBAttribute createdPkiAttribute = createdPki.Attributes.FindByGUID(new Guid(SystemGUIDs.attributeName));

                // Если атрибут найден - задаём ему значение
                if (createdPkiAttribute != null)
                {
                    createdPkiAttribute.Value = $"Пример нового объекта ПКИ";
                }

                // Создаём "полноценный" объект IPS из болванки. Если в метод передано значение true - болванка будет удалена
                // Вторая перегрузка данного метода позволяет определять - будет ли взят на редактирование созданный объект, или нет
                createdPki.CommitCreation(true);

                // Получаем "правильный" идентификатор версии нового объекта, присвоенный IPS
                createdPkiId = createdPki.ObjectID;
            }

            if (createdPkiId == Consts.UnknownObjectId || createdPkiId == -1)
            {
                MessageBox.Show("В процессе создания объекта ПКИ возникла ошибка");
            }
            else
            {
                MessageBox.Show($"Успешно создан новый объект ПКИ c идентификатором: {createdPkiId}");
            }
        }

        /// <summary>
        /// Простой вывод окна WinForms
        /// Очень помогает, когда нужно проверить, работает ли что-то, или нет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHelloMessage(object sender, EventArgs e)
        {
            MessageBox.Show("Это сообщение вызвано обработчиком события IPS");
        }

        /// <summary>
        /// Метод интерфейса IPackage. 
        /// В новых версиях IPS не используется, оставлен для совместимости.
        /// </summary>
        public void Unload()
        {

        }
    }
}
