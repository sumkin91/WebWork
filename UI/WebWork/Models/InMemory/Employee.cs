namespace WebWork.Models.InMemory;

public class Employee
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Patronymic { get; set; }

    public int Age { get; set; }

    public override string ToString() => $"(id: {Id}) {LastName} {FirstName} {Patronymic} - {Age} age";

}

