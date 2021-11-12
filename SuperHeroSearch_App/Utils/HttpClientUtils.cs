using System;

namespace SuperHeroSearch_App.Utils
{
    public static class HttpClientUtils
    {
        public static string AssignRouteParameters(this string url, params (string, string)[] parameters)
        {
            var lowerurl = url.ToLower();

            foreach (var parameter in parameters)
            {
                (var parameterName, var parameterValue) = parameter;

                var paramToFind = "{" + parameterName.ToLower() + "}";

                lowerurl = lowerurl.Replace(paramToFind, parameterValue.ToLower());
            }

            if (lowerurl.Contains("{") || lowerurl.Contains("}")) throw new FormatException($"La url {lowerurl} contiene parametros sin asignar");

            return lowerurl;
        }
    }
}
