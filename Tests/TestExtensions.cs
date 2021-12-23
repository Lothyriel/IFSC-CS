using Domain;
using Domain.Business;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Tests
{
    public static class TestExtensions
    {
        public static string GetToString(this byte[] byteArray)
        {
            return Encoding.Unicode.GetString(byteArray);
        }
    }
}
