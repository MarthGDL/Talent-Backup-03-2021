using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Talent.Automation.Model;
using Talent.Automation.Page;
using Talent.Automation.Page.EmployerAndRecruiter.Job;
using Talent.Automation.Page.Recruiter;
using Talent.Automation.Steps.BaseStep;
using TechTalk.SpecFlow;

namespace Talent.Automation.Steps.Recruiter
{
    [Binding]
    public sealed class JobPostSteps:Base
    {
        private JobPost NewJob = JobPost.CreateNewJob("Valid");

        public JobPostSteps(IWebDriver driver) : base(driver)
        {
            CurrentPage = GetInstance<DashboardPage>(Driver);
        }

        [Given(@"I am on Job post page")]
        public void GivenIAmOnJobPostPage()
        {
            CurrentPage = CurrentPage.As<DashboardPage>().GoToJobPostPage();
        }

        [When(@"I post a new job")]
        public void WhenIPostANewJob()
        {
            CurrentPage.As<JobPostPage>().EnterJobDetails(NewJob);
            CurrentPage = CurrentPage.As<JobPostPage>().ClickSave();
        }

        [When(@"I cancel posting a new job")]
        public void WhenICancelPostingANewJob()
        {
            CurrentPage = CurrentPage.As<JobPostPage>().ClickCancel();
        }

        [When(@"I fill out a job post with a requirement of '(.*)' years of experience")]
        public void WhenIFillOutAJobPostWithARequirementOfYearsOfExperience(string YearsOfExperience)
        {
            CurrentPage.As<JobPostPage>().ChangeYearsOfExperience(YearsOfExperience);
        }

        [When(@"I post a new job without entering any data")]
        public void WhenIPostANewJobWithoutEnteringAnyData()
        {
            CurrentPage = CurrentPage.As<JobPostPage>().ClickSaveAsPost();
        }

        [When(@"I post a new job with '(.*)' for '(.*)'")]
        public void WhenIPostANewJobWithFor(string text, string field)
        {

            switch (field)
            {
                case "Title":
                    NewJob.Title = text;
                    break;
                case "Summary":
                    NewJob.Summary = text;
                    break;
                case "Description":
                    NewJob.Description = text;
                    break;
            }
            CurrentPage.As<JobPostPage>().EnterJobDetails(NewJob);
            CurrentPage = CurrentPage.As<JobPostPage>().ClickSaveAsPost();
        }

        [When(@"I post a new job with a requirement of '(.*)' years of experience")]
        public void WhenIPostANewJobWithARequirementOfYearsOfExperience(string YearsOfExperience)
        {
            //Fills the JobPost form with valid details and changes the Years of Experience Field
            NewJob.YearsOfExperience = YearsOfExperience;
            CurrentPage.As<JobPostPage>().EnterJobDetails(NewJob);

            //Clicks save
            CurrentPage = CurrentPage.As<JobPostPage>().ClickSaveAsPost();
        }

        [When(@"I fill out a job post with the following details:")]
        public void WhenIFillOutAJobPostWithTheFollowingDetails(Table table)
        {
            NewJob.StartDate = table.Rows[0]["StartDate"];
            NewJob.EndDate = table.Rows[0]["EndDate"];
            NewJob.ExpiryDate = table.Rows[0]["ExpiryDate"];
            CurrentPage.As<JobPostPage>().EnterJobDetails(NewJob);
        }

        [Then(@"the job management page displays the newly added job")]
        public void ThenTheJobManagementPageDisplaysTheNewlyAddedJob()
        {
            Assert.IsTrue(CurrentPage.As<JobsPage>().ReturnFirstJobDesc().Contains(DateTime.Now.ToString("yyyy/MM/dd HH"), StringComparison.OrdinalIgnoreCase));
        }

        [Then(@"the job management page does not display the newly added job")]
        public void ThenTheJobManagementPageDoesNotDisplayTheNewlyAddedJob()
        {

            CurrentPage = CurrentPage.As<DashboardPage>().GotoJobsPage();
            Assert.IsFalse(CurrentPage.As<JobsPage>().ReturnFirstJobDesc().Contains(DateTime.Now.ToString("yyyy/MM/dd HH:mm"), StringComparison.OrdinalIgnoreCase));
        }

        [Then(@"the years of experience is converted to '(.*)'")]
        public void ThenTheYearsOfExperienceIsConvertedTo(string YearsOfExperience)
        {
            Assert.IsTrue(CurrentPage.As<JobPostPage>().GetYearsOfExperienceField()==YearsOfExperience);
        }

        [Then(@"the job is not posted")]
        public void ThenTheJobIsNotPosted()
        {
            if (CurrentPage.As<JobPostPage>().JobPostPageTitle()!="Post a Job - GTIO")
            {
                Assert.Fail("Browser should have remained on 'Post a Job - GTIO' but is in " + CurrentPage.As<JobPostPage>().JobPostPageTitle());
            }
        }

        [Then(@"all the mandatory fields are highlighted in red with appropriate error messages")]
        public void ThenAllTheMandatoryFieldsAreHighlightedInRedWithAppropriateErrorMessages()
        {

            Assert.Multiple(() =>
            {
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("Title"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("Summary"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("Description"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("Category"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("SubCategory"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("YearsOfExperience"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("QualificationTypes"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("VisaStatusTypes"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("JobTypes"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("Country"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("City"));
                Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckFieldAlert("EndDate"));
            });
        }

        [Then(@"the error message '(.*)' is displayed")]
        public void ThenTheErrorMessageIsDisplayed(string errorMessage)
        {
            Assert.IsTrue(CurrentPage.As<JobPostPage>().CheckAlertPopUp(errorMessage));
        }

        [Then(@"those date fields are not changed")]
        public void ThenThoseDateFieldsAreNotChanged()
        {
            Assert.Multiple(() =>
            {
                Assert.IsFalse(CurrentPage.As<JobPostPage>().StartDate.GetAttribute("value") == NewJob.StartDate);
                Assert.IsFalse(CurrentPage.As<JobPostPage>().EndDate.GetAttribute("value") == NewJob.EndDate);
                Assert.IsFalse(CurrentPage.As<JobPostPage>().ExpiryDate.GetAttribute("value") == NewJob.ExpiryDate);
            });
        }

    }
}
