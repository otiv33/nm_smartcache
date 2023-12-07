using smartcache.API.Controllers;
using Orleans;
using Orleans.TestingHost;
using smartcache.API.Models;
using System.Text;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;


namespace smartcache.TESTING
{
    class SiloConfigurator : ISiloConfigurator
    {
        public void Configure(ISiloBuilder hostBuilder) => 
            hostBuilder.AddMemoryGrainStorage("nomniotest")
                       .UseInMemoryReminderService();
    }

    public class EmailsControllerTest
    {
        private TestCluster getCluster()
        {
            var builder = new TestClusterBuilder();
            builder.AddSiloBuilderConfigurator<SiloConfigurator>();
            var cluster = builder.Build();
            cluster.Deploy();
            return cluster;
        }

        // CheckEmail
        [Fact]
        public async Task CheckEmailInvalid()
        {
            EmailsController controller = new EmailsController(null);
            IResult res = await controller.CheckEmail("sad934ll@@w'edo'.saso02");
            Assert.IsType<BadRequest<string>>(res);
        }
        [Fact]
        public async void CheckEmailOk()
        {
            Email email = new Email("test", "mail.com");

            var cluster = getCluster();

            var grain = cluster.GrainFactory.GetGrain<IEmailsGrain>(email.Domain);
            await grain.AddEmail(email.LocalPart);

            EmailsController controller = new EmailsController(cluster.GrainFactory);
            var res = await controller.CheckEmail(email.LocalPart+"@"+email.Domain);
            cluster.StopAllSilos();
            Assert.IsType<Ok<string>>(res);
        }
        [Fact]
        public async void CheckEmailNotFound()
        {
            var cluster = getCluster();

            EmailsController controller = new EmailsController(cluster.GrainFactory);
            var res = await controller.CheckEmail("test@mail.com");
            cluster.StopAllSilos();
            Assert.IsType<NotFound<string>>(res);
        }

        // AddEmail
        [Fact]
        public async Task AddEmailInvalidAsync()
        {
            EmailsController controller = new EmailsController(null);
            IResult res = await controller.AddEmail("sad934ll@@w'edo'.saso02");
            Assert.IsType<BadRequest<string>>(res);
        }
        [Fact]
        public async void AddEmailOk()
        {
            var cluster = getCluster();

            EmailsController controller = new EmailsController(cluster.GrainFactory);
            var res = await controller.AddEmail("test@mail.com");
            cluster.StopAllSilos();
            Assert.IsType<Created<string>>(res);
        }
        [Fact]
        public async void AddEmailConflict()
        {
            Email email = new Email("test", "mail.com");

            var cluster = getCluster();

            var grain = cluster.GrainFactory.GetGrain<IEmailsGrain>(email.Domain);
            await grain.AddEmail(email.LocalPart);

            EmailsController controller = new EmailsController(cluster.GrainFactory);
            var res = await controller.AddEmail(email.LocalPart + "@" + email.Domain);
            cluster.StopAllSilos();
            Assert.IsType<Conflict<string>>(res);
        }

    }
}