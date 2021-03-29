using MVPStudio.Framework.Config;
using MVPStudio.Framework.Extensions;
using MVPStudio.Framework.Helps.Excel;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Talent.Automation.Model;
using Talent.Automation.Page;
using Talent.Automation.Page.Employer;
using Talent.Automation.Steps.BaseStep;
using TechTalk.SpecFlow;

namespace Talent.Automation.Steps.Employer
{
    [Binding]
    public class JobpageSteps:Base
    {
        private readonly ScenarioContext context;

        public string JobEndDate { get; private set; }
        public string JobStartDate { get; private set; }
        public string JobExpiryDate { get; private set; }
        public string PastEndDate { get; private set; }
        public string PastExpiryDate { get; private set; }

        public JobpageSteps(IWebDriver driver, ScenarioContext injectedContext) : base(driver)
        {
            context = injectedContext;
            ExcelUtil.SetDataSource("Job.xlsx");
        }

        //Scenario: Posting a new job with valid data
        [When(@"I post a new job")]
        public void WhenIPostANewJob()
        {
            try
            {
                Driver.WaitForPageLoaded("post");
                CurrentPage = GetInstance<PostJob>(Driver);
                CurrentPage.As<PostJob>().EnterJob();
            }
            catch(Exception)
            {
                throw;
            }
        }

        [Then(@"the job management page displays the newly added job")]
        public void ThenTheJobManagementPageDisplaysTheNewlyAddedJob()
        {
            Driver.WaitForPageLoaded("jobs");
            var row = ExcelUtil.DataSet.SelectSheet("ADD").GetRowByKey("Valid");
            Assert.AreEqual(CurrentPage.As<PostJob>().GetJobTitle(), row.GetValue("Title"));
        }

        //Scenario: Cancel posting a job
        [When(@"I cancel posting a new job")]
        public void WhenICancelPostingANewJob()
        {
            try
            {
                Driver.WaitForPageLoaded("post");
                CurrentPage = GetInstance<PostJob>(Driver);
                CurrentPage.As<PostJob>().EnterJob();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Then(@"I should be navigated to Dashboard page")]
        public void ThenIShouldBeNavigatedToDashboardPage()
        {
            Assert.That(CurrentPage.As<PostJob>().DashboardPageTitle().Contains("Dashboard", StringComparison.OrdinalIgnoreCase));
        }

        //Scenario: Posting a job outside the expected range for years of experience
        [When(@"I fill out a job post with a requirement of (.*) years of experience")]
        public void WhenIFillOutAJobPostWithARequirementOfYearsOfExperience(string YearsOfExperienceInput)
        {
            try
            {
                Driver.WaitForPageLoaded("post");
                CurrentPage = GetInstance<PostJob>(Driver);
                CurrentPage.As<PostJob>().ExperienceOutsideExpectedRange(YearsOfExperienceInput);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Then(@"The years of experience is converted to (.*)")]
        public void ThenTheYearsOfExperienceIsConvertedTo(string SavedYearsOfExperience)
        {
            Assert.AreEqual(SavedYearsOfExperience,CurrentPage.As<PostJob>().GetYears());
        }

        //Scenario: Posting a job without entering mandatory fields
        [When(@"I post a new job without entering any data")]
        public void WhenIPostANewJobWithoutEnteringAnyData()
        {
            CurrentPage = GetInstance<PostJob>(Driver);
            CurrentPage.As<PostJob>().SaveWithoutMandatoryField();
        }

       
        [Then(@"all the mandatory fields are highlighted in red with appropriate error messages")]
        public void ThenAllTheMandatoryFieldsAreHighlightedInRedWithAppropriateErrorMessages()
        {
            CurrentPage = GetInstance<PostJob>(Driver);
            // CurrentPage.As<PostJob>().AsserttMandatoryField();
            Assert.True(CurrentPage.As<PostJob>().AssertMandatoryField());

        }

        //Scenario: Posting a job with invalid date format
        [When(@"I fill out a job post with the following details:")]
        public void WhenIFillOutAJobPostWithTheFollowingDetails(Table table)
        {
            try
            {
                Driver.WaitForPageLoaded("post");
                CurrentPage = GetInstance<PostJob>(Driver);
                CurrentPage.As<PostJob>().DatesFormat(JobStartDate,JobEndDate,JobExpiryDate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Then(@"those date fields are not changed")]
        public void ThenThoseDateFieldsAreNotChanged()
        {
            Assert.AreEqual(CurrentPage.As<PostJob>().GetDateInvalidFormat(),"Please enter the end date");
        }

       // Scenario Outline: Posting a job with Past End date and Past Expiry date
        [When(@"I post a new job with following ExpiryDate")]
        public void WhenIPostANewJobWithFollowingExpiryDate(Table table)
        {
            try
            {
                Driver.WaitForPageLoaded("post");
                CurrentPage = GetInstance<PostJob>(Driver);
                CurrentPage.As<PostJob>().ExpiredDate(PastEndDate,PastExpiryDate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Then(@"the date was disable and could not save the details with expiry date")]
        public void ThenTheDateWasDisableAndCouldNotSaveTheDetailsWithExpiryDate()
        {
            Assert.AreEqual(CurrentPage.As<PostJob>().GetExpiredDate(), "Please enter the end date");
        }

    }
}
