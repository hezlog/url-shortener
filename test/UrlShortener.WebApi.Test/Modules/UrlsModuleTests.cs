﻿using Moq;
using Nancy.Testing;
using NUnit.Framework;
using UrlShortener.Infrastructure.Data.Commands.Url;
using UrlShortener.Infrastructure.Data.Queries.Url;
using UrlShortener.WebApi.Lib;
using UrlShortener.WebApi.Lib.Authentication;
using UrlShortener.WebApi.Lib.Validators;
using UrlShortener.WebApi.Modules;

namespace UrlShortener.WebApi.Test.Modules
{
    public class UrlsModuleTests
    {
        protected Mock<GetAll> GetAllMock;
        protected Mock<GetByUrl> GetByUrlMock;
        protected Mock<CreateCommand> CreateMock;
        protected Mock<ExcludeCommand> ExcludeMock;
        protected Mock<UrlValidator> UrlValidatorMock;

        protected Browser Browser;

        [SetUp]
        public void SetUpBase()
        {
            GetAllMock = new Mock<GetAll>();
            GetByUrlMock = new Mock<GetByUrl>();
            CreateMock = new Mock<CreateCommand>();
            ExcludeMock = new Mock<ExcludeCommand>();
            UrlValidatorMock = new Mock<UrlValidator>();

            var configurable = new ConfigurableBootstrapper(with =>
            {
                with.Module<UrlsModule>();

                with.ApplicationStartup((container, pipelines) =>
                {
                    AutoMapperConfig.RegisterProfiles();
                });

                with.RequestStartup((container, pipelines, context) =>
                {
                    pipelines.OnError.AddItemToEndOfPipeline(HandlerError.Config);
                    context.CurrentUser = new UserIdentity
                    {
                        Claims = new[] { "admin" },
                        UserName = "admin"
                    };
                });

                with.Dependency(GetAllMock.Object);
                with.Dependency(GetByUrlMock.Object);
                with.Dependency(CreateMock.Object);
                with.Dependency(ExcludeMock.Object);
                with.Dependency(UrlValidatorMock.Object);
            });

            Browser = new Browser(configurable);

            SetUp();
        }

        public virtual void SetUp()
        {

        }
    }
}
