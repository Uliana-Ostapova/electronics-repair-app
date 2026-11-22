public enum RepairStatus //возможные заявки на ремонт
{
    New,
    Accepted,
    InProgress,
    Ready,
    Issued
}

public class RepairRequest
{
    public int Number { get; set; }

    public string ClientName { get; set; }

    public string Device { get; set; }

    public string ProblemDescription { get; set; }

    public RepairStatus Status { get; set; }

//создание новой заявки и её заполнение
    public RepairRequest(int number, string clientName, string device, string problemDescription)
    {
        Number = number;
        ClientName = clientName;
        Device = device;
        ProblemDescription = problemDescription;
        Status = RepairStatus.New; //изначально статус "новый"
    }
}