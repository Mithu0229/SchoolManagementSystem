namespace SchoolManagementSystem.Application.School.Students.Helpers;

/// <summary>
/// Shared rules for the login user that is auto-created together with a student.
/// Used by <c>InsertStudentInfoCommandHandler</c> (manual save) and the Excel import
/// pre-validation so both paths always agree on the generated values.
/// </summary>
public static class StudentUserHelper
{
    /// <summary>
    /// Builds the login email for a student user: full name without spaces + "@gmail.com".
    /// Note: User.Email has a unique index, so two students with the same name collide.
    /// </summary>
    public static string BuildLoginEmail(string fullName)
        => fullName.Replace(" ", "") + "@gmail.com";
}
