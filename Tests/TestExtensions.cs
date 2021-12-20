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
