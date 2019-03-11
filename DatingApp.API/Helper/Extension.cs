using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helper
{
    public static class Extension
    {
        //parameter with this HttpResponse to add extension method to HttpResponse
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*"); //allow any origin
        }

        public static int CalculateAge(this DateTime dateOfBirth){
            var age = DateTime.Now.Year - dateOfBirth.Year;

            if(DateTime.Today < dateOfBirth)
                age--;
            return age;
        }
    }
}