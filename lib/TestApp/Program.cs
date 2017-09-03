using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib_SMS;
namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SMSSender sender = new SMSSender("vladmir.klepach@gmail.com", "1qaZ2wsX", "name");
            var result = sender.Send("+380123456712", "Test message");
            switch (result)
            {
                case -1:
                    Console.WriteLine("Неправильный логин и / или пароль");
                    break;
                case -2:
                    Console.WriteLine("Неправильный формат XML");
                    break;
                case -3:
                    Console.WriteLine("Недостаточно кредитов на аккаунте пользователя");
                    break;
                case -4:
                    Console.WriteLine("Нет верных номеров получателей");
                    break;
                case -5:
                    Console.WriteLine("Ошибка подключения");
                    break;
                case -6:
                    break;
                case -7:
                    break;
                default:
                    Console.WriteLine("СМС в количестве: {0} отправлено!", result);
                    break;
            }
        }
    }
}
