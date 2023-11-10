
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Entities.Models;


namespace Repository.Extensions;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees(
        this IQueryable<Employee> employees,
        uint minAge,
        uint maxAge)
    {
        return employees.Where(e =>
            e.Age >= minAge && e.Age <= maxAge);
    }


    public static IQueryable<Employee> Search(
        this IQueryable<Employee> employees,
        string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return employees;
        }

        var lowerCaseTerm = searchTerm
            .Trim()
            .ToLower();

        return employees.Where(e =>
            e.Name!.ToLower().Contains(lowerCaseTerm));
    }


    public static IQueryable<Employee> Sort(
        this IQueryable<Employee> employees,
        string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            return employees.OrderBy(e => e.Name);
        }

        string[] orderParams = orderByQueryString
            .Trim()
            .Split(',');

        PropertyInfo[] propertyInfos = typeof(Employee)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var orderQueryBuilder = new StringBuilder();

        foreach (string? param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                continue;
            }

            string propertyFromQueryName = param.Split(" ")[0];

            PropertyInfo? objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            if (objectProperty == null)
            {
                continue;
            }

            string direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
        }

        string orderQuery = orderQueryBuilder
            .ToString()
            .TrimEnd(',', ' ');
        if (string.IsNullOrWhiteSpace(orderQuery))
        {
            return employees.OrderBy(e => e.Name);
        }

        return employees.OrderBy(orderQuery);
    }
}
