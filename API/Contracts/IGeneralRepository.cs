﻿using API.Models;
using API.ViewModels.Accounts;

namespace API.Contracts
{
    public interface IGeneralRepository<Tentity>
    {
        Tentity? Create(Tentity tentity);
        bool Update(Tentity tentity);
        bool Delete(Guid guid);

        IEnumerable<Tentity> GetAll();

        Tentity? GetByGuid(Guid guid);
    }
}