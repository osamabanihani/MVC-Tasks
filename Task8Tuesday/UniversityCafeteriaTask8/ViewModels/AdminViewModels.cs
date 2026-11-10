namespace UniversityCafeteriaTask8.ViewModels;

public class UserRowViewModel
{
    public string Id { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
}

public class EditUserRoleViewModel : UserRowViewModel { }
