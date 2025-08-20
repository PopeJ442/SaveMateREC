using System.ComponentModel.DataAnnotations;

public class UpdateUserViewModel
{
    public string Id { get; set; }

    
    public string Email { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

  
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }

    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}
