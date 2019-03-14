using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

            if(DateTime.Today < dateOfBirth.AddYears(age))
                age--;
            return age;
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemPerPage, totalItems, totalPages);
            var formatter = new JsonSerializerSettings();
            formatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, formatter) );
            // indicates which headers can be exposed as part of the response by listing their names.
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}