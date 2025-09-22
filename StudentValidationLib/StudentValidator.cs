namespace StudentValidationLib
{
    public static class StudentValidator
{
    public static bool ValidateFirstName(string firstName, out string error)
    {
        error = "";
        if (string.IsNullOrWhiteSpace(firstName))
        {
            error = "First name không được để trống";
            return false;
        }
        return true;
    }

    public static bool ValidateLastName(string lastName, out string error)
    {
        error = "";
        if (string.IsNullOrWhiteSpace(lastName))
        {
            error = "Last name không được để trống";
            return false;
        }
        return true;
    }

    public static bool ValidateEmail(string email, out string error)
    {
        error = "";
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            error = "Email không hợp lệ";
            return false;
        }
        return true;
    }

    public static bool ValidatePhone(string phone, out string error)
    {
        error = "";
        if (string.IsNullOrWhiteSpace(phone) || phone.Length < 9)
        {
            error = "Số điện thoại không hợp lệ";
            return false;
        }
        return true;
    }
}

}
