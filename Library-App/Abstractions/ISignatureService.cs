using Library_App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_App.Abstractions
{
    public interface ISignatureService
    {
        Task Create(Signature signature);
        ICollection<Signature> GetAll();

        Task<Signature> GetById(string id);

        Task Update(Signature signature);
        bool Exists(string id);
        Task DeleteById(string id);
    }
}
