public class Program
{
    public static void Main()
    {
        //указывает путь к файлу в котором хранятся заявки
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "requests.json");

        //создание объекта для загрузки и сохранения заявок
        RequestStorage storage = new RequestStorage(filePath);

        //загрузка сохранённых заявок из файла
        List<RepairRequest> requests = storage.Load();

        //создание и передача заявок менеджеру
        RequestManager manager = new RequestManager(requests);

        bool isRunning = true;

        while (isRunning)
        {
            //главное меню
            Console.WriteLine();
            Console.WriteLine("=== УЧЁТ ЗАЯВОК НА РЕМОНТ ===");
            Console.WriteLine("1. Добавить заявку");
            Console.WriteLine("2. Показать все заявки");
            Console.WriteLine("3. Найти заявку по номеру");
            Console.WriteLine("4. Редактировать заявку");
            Console.WriteLine("5. Удалить заявку");
            Console.WriteLine("6. Изменить статус заявки");
            Console.WriteLine("0. Выход");
            Console.WriteLine("Выберите действие: ");

            string? choice = Console.ReadLine();

            switch (choice) //вызов метода в зависимости от выбора
            {
                case "1":
                    AddRequest(manager, storage);
                    break;
                case "2":
                    ShowRequests(manager);
                    break;
                case "3":
                    SearchRequest(manager);
                    break;
                case "4":
                    EditRequestInfo(manager, storage);
                    break;
                case "5":
                    DeleteRequestInfo(manager, storage);
                    break;
                case "6":
                    ChangeRequestStatus(manager, storage);
                    break;
                case "0":
                    isRunning = false;
                    Console.WriteLine("Программа завершена.");
                    break;
                default:
                    Console.WriteLine("Такого пункта в меню нет.");
                    break;
            }
        }
    }

    //добавление новой заявки
    private static void AddRequest(RequestManager manager, RequestStorage storage)
    {
        string clientName = ReadText("Введите имя клиента: ");
        string device = ReadText("Введите устройство: ");
        string problem = ReadText("Опишите неисправность: ");

        //передача данных менеджеру для создания заявки
        RepairRequest request = manager.AddRequest(
            clientName,
            device,
            problem
        );

        storage.Save(manager.GetRequests());

        Console.WriteLine("Заявка успешно добавлена.");
        Console.WriteLine("Номер заявки: " + request.Number);
    }

    //отображение всех заявок
    private static void ShowRequests(RequestManager manager)
    {
        List<RepairRequest> requests = manager.GetRequests(); //получили список от менеджера

        if (requests.Count == 0)
        {
            Console.WriteLine("Список заявок пуст.");
            return;
        }

        foreach (RepairRequest request in requests)
        {
            Console.WriteLine();
            Console.WriteLine("Номер: " + request.Number);
            Console.WriteLine("Клиент: " + request.ClientName);
            Console.WriteLine("Устройство: " + request.Device);
            Console.WriteLine("Неисправность: " + request.ProblemDescription);
            Console.WriteLine("Статус: " + GetStatusName(request.Status));
        }
    }

    //поиск заявки по номеру
    private static void SearchRequest(RequestManager manager)
    {
        Console.Write("Введите номер заявки: ");

        string? value = Console.ReadLine();

        if (!int.TryParse(value, out int number))
        {
            Console.WriteLine("Нужно ввести целое число.");
            return;
        }

        RepairRequest? request = manager.FindRequest(number);

        if (request == null)
        {
            Console.WriteLine("Заявка не найдена.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Номер: " + request.Number);
        Console.WriteLine("Клиент: " + request.ClientName);
        Console.WriteLine("Устройство: " + request.Device);
        Console.WriteLine("Неисправность: " + request.ProblemDescription);
        Console.WriteLine("Статус: " + GetStatusName(request.Status));
    }

    //редактирование сведений заявки
    private static void EditRequestInfo(RequestManager manager, RequestStorage storage)
    {
        Console.WriteLine("Введите номер заявки для редактирования: ");

        string? value = Console.ReadLine();

        //проверка правильный ли введённый номер
        if (!int.TryParse(value, out int number))
        {
            Console.WriteLine("Нужно ввести целое число.");
            return;
        }

        RepairRequest? request = manager.FindRequest(number);

        if (request == null)
        {
            Console.WriteLine("Заявка не найдена.");
            return;
        }

        Console.WriteLine("Введите новые данные заявки:");

        string clientName = ReadText("Имя клиента: ");
        string device = ReadText("Устройство: ");
        string problem = ReadText("Описание неисправность: ");

        //передача менеджеру и сохранение новых сведений
        manager.EditRequest(number, clientName, device, problem);
        storage.Save(manager.GetRequests());

        Console.WriteLine("Заявка успешно изменена.");
    }

    //удаление заявки
    private static void DeleteRequestInfo(RequestManager manager, RequestStorage storage)
    {
        Console.Write("Введите номер заявки для удаления: ");

        string? value = Console.ReadLine();

        if (!int.TryParse(value, out int number))
        {
            Console.WriteLine("Нужно ввести целое число.");
            return;
        }

        RepairRequest? request = manager.FindRequest(number);

        if (request == null)
        {
            Console.WriteLine("Заявка не найдена.");
            return;
        }

        Console.WriteLine("Вы действительно хотите удалить эту заявку?");
        Console.WriteLine("Клиент: " + request.ClientName);
        Console.WriteLine("Устройство: " + request.Device);
        Console.WriteLine("1. Да");
        Console.WriteLine("2. Нет");
        Console.Write("Ваш выбор: ");

        string? confirmation = Console.ReadLine();

        if (confirmation != "1")
        {
            Console.WriteLine("Удаление отменено.");
            return;
        }

        manager.DeleteRequest(number); //передача менеджеру номер заявки на удаление

        storage.Save(manager.GetRequests()); //обновление списка

        Console.WriteLine("Заявка успешно удалена.");
    }

    //изменение статуса заявки
    private static void ChangeRequestStatus(RequestManager manager, RequestStorage storage)
    {
        Console.Write("Введите номер заявки: ");

        string? value = Console.ReadLine();

        if (!int.TryParse(value, out int number))
        {
            Console.WriteLine("Нужно ввести целое число.");
            return;
        }

        RepairRequest? request = manager.FindRequest(number);

        if (request == null)
        {
            Console.WriteLine("Заявка не найдена.");
            return;
        }

        Console.WriteLine("Текущий статус: " + GetStatusName(request.Status));
        Console.WriteLine("Выберите новый статус:");
        Console.WriteLine("1. Новая");
        Console.WriteLine("2. Принята");
        Console.WriteLine("3. В работе");
        Console.WriteLine("4. Готова");
        Console.WriteLine("5. Выдана клиенту");
        Console.Write("Ваш выбор: ");

        string? statusChoice = Console.ReadLine();

        RepairStatus newStatus; //переменная для хранения выбранного статуса

        switch (statusChoice)
        {
            case "1":
                newStatus = RepairStatus.New;
                break;
            case "2":
                newStatus = RepairStatus.Accepted;
                break;
            case "3":
                newStatus = RepairStatus.InProgress;
                break;
            case "4":
                newStatus = RepairStatus.Ready;
                break;
            case "5":
                newStatus = RepairStatus.Issued;
                break;
            default:
                Console.WriteLine("Такого статуса нет.");
                return;
        }

        manager.ChangeStatus(number, newStatus);

        storage.Save(manager.GetRequests());

        Console.WriteLine("Статус заявки успешно изменён.");
    }

    //вывод текста
    private static string ReadText(string message)
    {
        while (true)
        {
            Console.Write(message);

            string? value = Console.ReadLine();

            if (value != null && value.Trim() != "")
            {
                return value.Trim(); //убираем пробелы в начале и конце введённой строки
            }

            Console.WriteLine("Поле не должно быть пустым.");
        }
    }

    //преобразование значений перечисления в русское название статуса
    private static string GetStatusName(RepairStatus status)
    {
        switch (status)
        {
            case RepairStatus.New:
                return "Новая";
            case RepairStatus.Accepted:
                return "Принята";
            case RepairStatus.InProgress:
                return "В работе";
            case RepairStatus.Ready:
                return "Готова";
            case RepairStatus.Issued:
                return "Выдана клиенту";
            default:
                return "Неизвестный статус";
        }
    }
}