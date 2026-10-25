using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace SimpleChat.Routing;

public class PositiveIntRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var routeValue))
        {
            return false;
        }

        if (routeValue == null)
        {
            return false;
        }

        int number;
        bool isParsed = int.TryParse(routeValue.ToString(), out number);

        if (isParsed == false)
        {
            return false;
        }

        if (number > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}