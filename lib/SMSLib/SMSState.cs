using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SMS
{
    public enum SMSState
    {
        /// <summary>
        /// Отослано
        /// </summary>
        SENT,
        /// <summary>
        /// Не доставлено
        /// </summary>
        NOT_DELIVERED,
        /// <summary>
        /// Доставлено
        /// </summary>
        DELIVERED,                      
        /// <summary>
        /// Оператор не обслуживается
        /// </summary>
        NOT_ALLOWED, 
        /// <summary>
        /// Неверный адрес для доставки
        /// </summary>
        INVALID_DESTINATION_ADDRESS, 
        /// <summary>
        ///  Неправильное имя «От кого»
        /// </summary>
        INVALID_SOURCE_ADDRESS,
        /// <summary>
        /// Недостаточно кредитов
        /// </summary>
        NOT_ENOUGH_CREDITS,
        /// <summary>
        /// Произошла ошибка подключения
        /// </summary>
        ERROR 
    }
}
