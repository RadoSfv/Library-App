using Library_App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Abstractions
{
    public interface IOrderService
    {
        bool SetReturnDate(DateTime date, TakeBook book);
    }
}
