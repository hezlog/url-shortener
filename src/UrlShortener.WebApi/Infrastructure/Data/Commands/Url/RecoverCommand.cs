﻿using Simple.Data;
using UrlShortener.WebApi.Infrastructure.Exceptions;

namespace UrlShortener.WebApi.Infrastructure.Data.Commands.Url
{
    public class RecoverCommand
    {
        public virtual void Execute(int id)
        {
            var db = Database.OpenNamedConnection("db");

            Entities.Url entity = db.Urls.Get(id);

            if (entity == null)
            {
                throw new NotFoundException("Url {0} not found", id);
            }

            entity.Recover();

            db.Urls.Update(entity);
        }
    }
}