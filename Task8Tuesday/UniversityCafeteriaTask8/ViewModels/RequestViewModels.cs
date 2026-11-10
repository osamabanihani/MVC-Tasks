namespace UniversityCafeteriaTask8.ViewModels;

public class RequestLineInput
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateRequestViewModel
{
    // Three simple rows are enough for this beginner assignment.
    public List<RequestLineInput> Items { get; set; } = new()
    {
        new(), new(), new()
    };
}

public class EditRequestLine
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
}

public class EditRequestViewModel
{
    public int Id { get; set; }
    public List<EditRequestLine> Items { get; set; } = new();
}
