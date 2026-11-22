using System.Collections.Specialized;
using System.ComponentModel;

public class RequestManager
{
    private List<RepairRequest> _requests;

    public int RequestCount //возвращение текущего количества заявок
    {
        get { return _requests.Count; }
    }

    public RequestManager(List<RepairRequest> requests)
    {
        _requests = requests;
    }

    public RepairRequest AddRequest(string clientName, string device, string problemDescription) //создание заявки, присваивание номера и добавление в список
    {
        int number = 1;

        foreach (RepairRequest request in _requests)
        {
            if (request.Number >= number)
            {
                number = request.Number + 1;
            }
        }

        RepairRequest newRequest = new RepairRequest(
            number,
            clientName,
            device,
            problemDescription
        );

        _requests.Add(newRequest);

        return newRequest;
    }

    public RepairRequest? FindRequest(int number) //ищет заявку по номеру
    {
        foreach (RepairRequest request in _requests)
        {
            if (request.Number == number)
            {
                return request;
            }
        }

        return null;
    }

    public List<RepairRequest> GetRequests() //возвращает копию списка
    {
        return new List<RepairRequest>(_requests);
    }

    public bool EditRequest(int number, string clientName, string device, string problemDescription) //изменение сведений о заявке
    {
        RepairRequest? request = FindRequest(number);

        if (request == null)
        {
            return false;
        }

        request.ClientName = clientName;
        request.Device = device;
        request.ProblemDescription = problemDescription;

        return true;
    }

    public bool DeleteRequest(int number) //удаление заявки
    {
        RepairRequest? request = FindRequest(number);

        if (request == null)
        {
            return false;
        }

        return _requests.Remove(request);
    }

    public bool ChangeStatus(int number, RepairStatus status) //изменение статуса
    {
        RepairRequest? request = FindRequest(number);

        if (request == null)
        {
            return false;
        }

        request.Status = status;

        return true;
    }
}