using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;


namespace DBS_AzureFunc;

public class FeeCalcService
{
    private readonly ILogger<FeeCalcService> _logger;

    public FeeCalcService(ILogger<FeeCalcService> logger)
    {
        _logger = logger;
    }

    [Function("FeeService")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string patientDOB = req.Query["PatientDob"];
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        int age = 0;

        if (DateOnly.TryParse(patientDOB, out DateOnly dob))
        {
            age = today.Year - dob.Year;
            if (today < dob.AddYears(age))
            {
                age--;
            }
        }
        else
        {
            return new BadRequestObjectResult("Invalid or missing Date of Birth data.");
        }


        if (!bool.TryParse(req.Query["PatientEnrolStat"], out bool isEnrolled))
        {
            return new BadRequestObjectResult("Invalid or missing enrolment status.");
        }

        decimal fee = 0.00m;



        if (isEnrolled)
        {
            if (age < 14)
            {
                fee = 0.00m;
            }
            else if (age >= 14 && age < 18)
            {
                fee = 9.00m;
            }
            else if (age >= 18 && age < 65)
            {
                fee = 18.00m;
            }
            else if (age >= 65)
            {
                fee = 25.00m;
            }
        }
        else if (!isEnrolled)
        {
            if (age <= 6)
            {
                fee = 0.00m;
            }
            else if (age > 6 && age < 14)
            {
                fee = 10.00m;
            }
            else if (age >= 14 && age < 18)
            {
                fee = 20.00m;
            }
            else if (age >= 18 && age < 65)
            {
                fee = 35.00m;
            }
            else if (age >= 65)
            {
                fee = 30.00m;
            }
        }


        return new OkObjectResult(new { bookingFee = fee });
    }

}