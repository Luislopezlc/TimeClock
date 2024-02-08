using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Management.Services
{
    public static class ErrorMessages
    {
        public static Dictionary<string, List<string>> AddMessageError(string nameError, string valueError)
        {
            var response = new Dictionary<string, List<string>>();
            var listMessage = new List<string>();
            listMessage.Add(valueError);
            response.Add(nameError, listMessage);

            return response;
        }
    }
}
