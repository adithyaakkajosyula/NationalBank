namespace NationalBank.BackEnd.Authorization;

[AttributeUsage(AttributeTargets.Method)]
public class CustomAllowAnonymousAttribute : Attribute
{ }