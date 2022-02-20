using System;
using Tribal.CreditLine.Api.Requests;
using Xunit;
using FluentValidation.TestHelper;
using Tribal.CreditLine.Api.Validations;
using Moq;
using Tribal.CreditLineRequests.Domain.CreditLines.Services;
using Tribal.CreditLineRequests.Api.Controllers;
using Tribal.CreditLine.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Tribal.CreditLineRequests.Services;
using Tribal.CreditLine.Data.Repositories;
using Tribal.CreditLine.Domain.CreditLineRequests.Models.Responses;
using Tribal.CreditLineRequests.Domain.CreditLines.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using Tribal.CreditLineRequests.Domain.CreditLines.Entities;
using System.Linq;
using System.IO;
using Tribal.CreditLine.Api.Middlewares;
using Tribal.CreditLine.Domain.Exceptions;
using FluentAssertions;
using System.Net;

namespace Tribal.CreditLine.Tests
{
    public class CreditLineRequestControllerTest
    {
        private CreditLineRequestValidator _validator;

        public CreditLineRequestControllerTest()
        {
            _validator = new CreditLineRequestValidator();
        }

        [Fact]
        public void Validators_should_have_error_when_invalid_founding_type()
        {
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "SMALL",
                CashBalance = 0,
                MonthlyRevenue = 0,
                RequestedCreditLine = 0,
                RequestedDate = DateTime.Now
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(cl => cl.FoundingType);
        }

        [Fact]
        public void Validators_should_have_error_when_no_founding_type()
        {
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                CashBalance = 0,
                MonthlyRevenue = 0,
                RequestedCreditLine = 0,
                RequestedDate = DateTime.Now
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(cl => cl.FoundingType);
        }

        [Fact]
        public void Validators_should_have_error_when_no_cash_balance()
        {
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "STARTUP",
                MonthlyRevenue = 5000,
                RequestedCreditLine = -100,
                RequestedDate = DateTime.Now
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(cl => cl.CashBalance);
        }

        [Fact]
        public void Validators_should_have_error_when_invalid_monthly_revenue()
        {
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "STARTUP",
                CashBalance = 1000,
                MonthlyRevenue = -10,
                RequestedCreditLine = 0,
                RequestedDate = DateTime.Now
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(cl => cl.MonthlyRevenue);
        }

        [Fact]
        public void Validators_should_have_error_when_no_monthly_revenue()
        {
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "STARTUP",
                CashBalance = 1000,
                RequestedCreditLine = 0,
                RequestedDate = DateTime.Now
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(cl => cl.MonthlyRevenue);
        }

        [Fact]
        public void Validators_should_have_error_when_invalid_requested_creditLine()
        {
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "STARTUP",
                CashBalance = 1000,
                MonthlyRevenue = 5000,
                RequestedCreditLine = -100,
                RequestedDate = DateTime.Now
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(cl => cl.RequestedCreditLine);
        }

        [Fact]
        public void Validators_should_have_error_when_no_requested_creditLine()
        {
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "STARTUP",
                CashBalance = 1000,
                MonthlyRevenue = 5000,
                RequestedDate = DateTime.Now
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(cl => cl.RequestedCreditLine);
        }

        [Fact]
        public void Validators_should_have_error_when_invalid_requested_date()
        {
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "STARTUP",
                CashBalance = 1000,
                MonthlyRevenue = 5000,
                RequestedCreditLine = -100,
                RequestedDate = DateTime.Now.AddDays(-1)
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(cl => cl.RequestedDate);
        }

        [Fact]
        public void Validators_should_have_error_when_no_requested_date()
        {
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "STARTUP",
                CashBalance = 1000,
                MonthlyRevenue = 5000,
                RequestedCreditLine = -100
            };

            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(cl => cl.RequestedDate);
        }

        [Fact]
        public async Task Create_should_return_rejected_when_sme_passed_as_startup()
        {
            // Arrange
            CreditLineRequestRepository repository = new CreditLineRequestRepository(CreateContext());
            CreditLineRequestService service = new CreditLineRequestService(repository);
            CreditLineRequestController controller = new CreditLineRequestController(service);

            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "SME",
                CashBalance = 900,
                MonthlyRevenue = 1000,
                RequestedCreditLine = 250, // Value is valid only in 'Startup' founding type
                RequestedDate = DateTime.Now
            };

            // Act
            IActionResult response = await controller.Post(model);

            var excpetedResult = CreditLineRequestResponse.CreateFromStatus(CreditLineRequestsStatus.REJECTED);
            var excpetedResultStr = JsonConvert.SerializeObject(excpetedResult);
            ObjectResult responseResult = response as ObjectResult;
            var actualStr = JsonConvert.SerializeObject(responseResult.Value);

            // Assert
            Assert.Equal(excpetedResultStr, actualStr);
        }

        [Fact]
        public async Task Create_should_return_429_on_accepted_3_times_rule()
        {
            // Arrange
            CreditLineRequestRepository repository = new CreditLineRequestRepository(CreateContext());
            CreditLineRequestService service = new CreditLineRequestService(repository);
            CreditLineRequestController controller = new CreditLineRequestController(service);

            ErrorHandlerMiddleware middleware = null;
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "SME",
                CashBalance = 900,
                MonthlyRevenue = 1000,
                RequestedCreditLine = 200,
                RequestedDate = DateTime.Now
            };

            // Act
            for (int i = 0; i < 3; i++)
            {
                await controller.Post(model);

                if (i == 2)
                {
                    middleware = new ErrorHandlerMiddleware(async (innerHttpContext) =>
                    {
                        await controller.Post(model);
                    });
                }
            }

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            await middleware.Invoke(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var objResponse = JsonConvert.DeserializeObject<ExceptionResponseModel>(streamText);

            //Assert
            objResponse
            .Should()
            .BeEquivalentTo(new ExceptionResponseModel { Message = "Too many requests" });

            context.Response.StatusCode
            .Should()
            .Be((int)HttpStatusCode.TooManyRequests);
        }

        [Fact]
        public async Task Create_should_return_429_on_rejected_30_seconds_rule()
        {
            // Arrange
            CreditLineRequestRepository repository = new CreditLineRequestRepository(CreateContext());
            CreditLineRequestService service = new CreditLineRequestService(repository);
            CreditLineRequestController controller = new CreditLineRequestController(service);

            ErrorHandlerMiddleware middleware = null;
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "SME",
                CashBalance = 900,
                MonthlyRevenue = 1000,
                RequestedCreditLine = 1000,
                RequestedDate = DateTime.Now
            };

            // Act
            for (int i = 0; i < 2; i++)
            {
                if (i == 1)
                {
                    middleware = new ErrorHandlerMiddleware(async (innerHttpContext) =>
                    {
                        await controller.Post(model);
                    });
                    continue;
                }

                var resp = await controller.Post(model);
            }

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            await middleware.Invoke(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var objResponse = JsonConvert.DeserializeObject<ExceptionResponseModel>(streamText);

            //Assert
            objResponse
            .Should()
            .BeEquivalentTo(new ExceptionResponseModel { Message = "Too many requests" });

            context.Response.StatusCode
            .Should()
            .Be((int)HttpStatusCode.TooManyRequests);
        }

        [Fact]
        public async Task Create_should_return_400_after_4_fails()
        {
            // Arrange
            CreditLineRequestRepository repository = new CreditLineRequestRepository(CreateContext());
            CreditLineRequestService service = new CreditLineRequestService(repository);
            CreditLineRequestController controller = new CreditLineRequestController(service);

            ErrorHandlerMiddleware middleware = null;
            CreditLineRequestModel model = new CreditLineRequestModel
            {
                FoundingType = "SME",
                CashBalance = 900,
                MonthlyRevenue = 1000,
                RequestedCreditLine = 1000,
                RequestedDate = DateTime.Now
            };

            // Act
            for (int i = 0; i < 4; i++)
            {
                await controller.Post(model);

                if (i == 3)
                {
                    middleware = new ErrorHandlerMiddleware(async (innerHttpContext) =>
                    {
                        await controller.Post(model);
                    });
                }
                Task.Delay(30000).Wait();
            }

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            await middleware.Invoke(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var objResponse = JsonConvert.DeserializeObject<ExceptionResponseModel>(streamText);

            //Assert
            objResponse
            .Should()
            .BeEquivalentTo(new ExceptionResponseModel { Message = "A sales agent will contact you" });

            context.Response.StatusCode
            .Should()
            .Be((int)HttpStatusCode.BadRequest);
        }

        private static DataContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            return new DataContext(optionsBuilder);
        }
    }
}
