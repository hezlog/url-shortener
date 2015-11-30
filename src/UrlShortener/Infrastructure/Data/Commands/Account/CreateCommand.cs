﻿using Simple.Data;

namespace UrlShortener.Infrastructure.Data.Commands.Account
{
    public class CreateCommand
    {
        public virtual int Execute(Entities.Account entity)
        {
            entity.HashPassword();

            var db = Database.Open();

            var inserted = db.Accounts.Insert(entity);

            return inserted.Id;
        }
    }
}